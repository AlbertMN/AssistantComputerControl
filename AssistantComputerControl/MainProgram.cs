﻿/*
 * AssistantComputerControl
 * Made by Albert MN.
 * Updated: v1.4.3, 21-04-2021
 * 
 * Use:
 * - Main class. Starts everything.
 */

using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Net;
using System.Linq;
using System.Threading;
using Sentry;
using System.Configuration;
using System.Xml;
using NLog;
using Microsoft.Win32.TaskScheduler;

namespace AssistantComputerControl {
    class MainProgram {
        public const string softwareVersion = "1.4.3",
            releaseDate = "2021-04-21 00:33:00", //YYYY-MM-DD H:i:s - otherwise it gives an error
            appName = "AssistantComputerControl",

            sentryToken = "super_secret";

        static public bool debug = true,
            unmuteVolumeChange = true,
            isCheckingForUpdate = false,

            testingAction = false,
            aboutVersionAwaiting = false,
            hasAskedForSetupAgain = false,
            hasStarted = false,
            reopenSettingsOnClose = false;

        public TestStatus currentTestStatus = TestStatus.ongoing;
        public enum TestStatus {
            success,
            error,
            ongoing
        }

        private static FileSystemWatcher watcher;

        static public string currentLocationFull = Assembly.GetEntryAssembly().Location,
            currentLocation = Path.GetDirectoryName(currentLocationFull),
            dataFolderLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AssistantComputerControl"),
            shortcutLocation = Path.Combine(dataFolderLocation, "shortcuts"),
            logFilePath = Path.Combine(dataFolderLocation, "log.txt"),
            actionModsPath = Path.Combine(dataFolderLocation, "mods"),
            startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup),
            messageBoxTitle = appName;

        private static SysTrayIcon sysIcon;
        public static TestActionWindow testActionWindow;
        private static SettingsForm settingsForm = null;
        public static GettingStarted gettingStarted = null;
        public static UpdateProgress updateProgressWindow;

        //Start main function
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        [STAThread]
        static void Main(string[] args) {
            Console.WriteLine("Log location; " + logFilePath);
            CheckSettings();

            var config = new NLog.Config.LoggingConfiguration();
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = logFilePath };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");         
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);         
            NLog.LogManager.Configuration = config;

            void ActualMain() {
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                //Upgrade settings
                if (Properties.Settings.Default.UpdateSettings) {
                    /* Copy old setting-files in case the Evidence type and Evidence Hash has changed (which it does sometimes) - easier than creating a whole new settings system */
                    try {
                        Configuration accConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
                        string currentFolder = new DirectoryInfo(accConfiguration.FilePath).Parent.Parent.FullName;
                        string[] directories = Directory.GetDirectories(new DirectoryInfo(currentFolder).Parent.FullName);

                        foreach (string dir in directories) {
                            if (dir != currentFolder.ToString()) {
                                var directoriesInDir = Directory.GetDirectories(dir);
                                foreach (string childDir in directoriesInDir) {
                                    string checkPath = Path.Combine(currentFolder, Path.GetFileName(childDir));
                                    if (!Directory.Exists(checkPath)) {
                                        string checkFile = Path.Combine(childDir, "user.config");
                                        if (File.Exists(checkFile)) {
                                            bool xmlHasError = false;
                                            try {
                                                XmlDocument xml = new XmlDocument();
                                                xml.Load(checkFile);

                                                xml.Validate(null);
                                            } catch {
                                                xmlHasError = true;
                                                DoDebug("XML document validation failed (is invalid): " + checkFile);
                                            }

                                            if (!xmlHasError) {
                                                Directory.CreateDirectory(checkPath);
                                                File.Copy(checkFile, Path.Combine(checkPath, "user.config"), true);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    } catch (Exception e) {
                        Console.WriteLine("Error getting settings from older versions of ACC; " + e.Message);
                    }
                    /* End "copy settings" */

                    try {
                        Properties.Settings.Default.Upgrade();
                        Properties.Settings.Default.UpdateSettings = false;
                        Properties.Settings.Default.Save();
                    } catch {
                        DoDebug("Failed to upgrade from old settings file.");
                    }

                    Console.WriteLine("Upgraded settings to match last version");
                }

                if (Properties.Settings.Default.LastUpdated == DateTime.MinValue) {
                    Properties.Settings.Default.LastUpdated = DateTime.Now;
                }

                //Translator
                string tempDir = Path.Combine(currentLocation, "Translations");
                if (Directory.Exists(tempDir)) {
                    Translator.translationFolder = Path.Combine(currentLocation, "Translations");
                    Translator.languagesArray = Translator.GetLanguages();
                } else {
                    MessageBox.Show("Missing the translations folder. Reinstall the software to fix this issue.", messageBoxTitle);
                }

                string lang = Properties.Settings.Default.ActiveLanguage;
                if (Array.Exists(Translator.languagesArray, element => element == lang)) {
                    DoDebug("ACC running with language \"" + lang + "\"");

                    Translator.SetLanguage(lang);
                } else {
                    DoDebug("Invalid language chosen (" + lang + ")");

                    Properties.Settings.Default.ActiveLanguage = "English";
                    Translator.SetLanguage("English");
                }
                //End translator
                sysIcon = new SysTrayIcon();

                Properties.Settings.Default.TimesOpened += 1;
                Properties.Settings.Default.Save();

                SetupDataFolder();
                if (File.Exists(logFilePath)) {
                    try {
                        File.WriteAllText(logFilePath, string.Empty);
                    } catch {
                        // Don't let this being DENIED crash the software
                        Console.WriteLine("Failed to empty the log");
                    }
                } else {
                    Console.WriteLine("Trying to create log");
                    CreateLogFile();
                }

                //Check if software already runs, if so kill this instance
                var otherACCs = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(currentLocationFull));
                if (otherACCs.Length > 1) {
                    //Try kill the _other_ process instead
                    foreach (Process p in otherACCs) {
                        if (p.Id != Process.GetCurrentProcess().Id) {
                            try {
                                p.Kill();
                                DoDebug("Other ACC instance was running. Killed it.");
                            } catch {
                                DoDebug("Could not kill other process of ACC; access denied");
                            }
                        }
                    }
                }

                Application.EnableVisualStyles();

                DoDebug("[ACC begun (v" + softwareVersion + ")]");

                if (Properties.Settings.Default.CheckForUpdates) {
                    if (HasInternet()) {
                        new Thread(() => {
                            new SoftwareUpdater().Check();
                        }).Start();
                    } else {
                        DoDebug("Couldn't check for new update as PC does not have access to the internet");
                    }
                }

                //On console close: hide NotifyIcon
                Application.ApplicationExit += new EventHandler(OnApplicationExit);
                handler = new ConsoleEventDelegate(ConsoleEventCallback);
                SetConsoleCtrlHandler(handler, true);

                //Create shortcut folder if doesn't exist
                if (!Directory.Exists(shortcutLocation)) {
                    Directory.CreateDirectory(shortcutLocation);
                }
                if (!File.Exists(Path.Combine(shortcutLocation, @"example.txt"))) {
                    //Create example-file
                    try {
                        using (StreamWriter sw = File.CreateText(Path.Combine(shortcutLocation, @"example.txt"))) {
                            sw.WriteLine("This is an example file.");
                            sw.WriteLine("If you haven't already, make your assistant open this file!");
                        }
                    } catch {
                        DoDebug("Could not create or write to example file");
                    }
                }

                //Delete all old action files
                if (Directory.Exists(CheckPath())) {
                    DoDebug("Deleting all files in action folder");
                    foreach (string file in Directory.GetFiles(CheckPath(), "*." + Properties.Settings.Default.ActionFileExtension)) {
                        int timeout = 0;

                        if (File.Exists(file)) {
                            while (ActionChecker.FileInUse(file) && timeout < 5) {
                                timeout++;
                                Thread.Sleep(500);
                            }
                            if (timeout >= 5) {
                                DoDebug("Failed to delete file at " + file + " as file appears to be in use (and has been for 2.5 seconds)");
                            } else {
                                try {
                                    File.Delete(file);
                                } catch (Exception e) {
                                    DoDebug("Failed to delete file at " + file + "; " + e.Message);
                                }
                            }
                        }
                    }
                    DoDebug("Old action files removed - moving on...");
                }

                //SetupListener();
                watcher = new FileSystemWatcher() {
                    Path = CheckPath(),
                    NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                        | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                    Filter = "*." + Properties.Settings.Default.ActionFileExtension,
                    EnableRaisingEvents = true
                };
                watcher.Changed += new FileSystemEventHandler(new ActionChecker().FileFound);
                watcher.Created += new FileSystemEventHandler(new ActionChecker().FileFound);
                watcher.Renamed += new RenamedEventHandler(new ActionChecker().FileFound);
                watcher.Deleted += new FileSystemEventHandler(new ActionChecker().FileFound);
                watcher.Error += delegate { DoDebug("Something wen't wrong"); };

                DoDebug("\n[" + messageBoxTitle + "] Initiated. \nListening in: \"" + CheckPath() + "\" for \"." + Properties.Settings.Default.ActionFileExtension + "\" extensions");

                sysIcon.TrayIcon.Icon = Properties.Resources.ACC_icon_light;

                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true);
                if (Registry.GetValue(key.Name + @"\AssistantComputerControl", "FirstTime", null) == null) {
                    SetStartup(true);

                    key.CreateSubKey("AssistantComputerControl");
                    key = key.OpenSubKey("AssistantComputerControl", true);
                    key.SetValue("FirstTime", false);

                    Properties.Settings.Default.HasCompletedTutorial = true;
                    Properties.Settings.Default.Save();

                    ShowGettingStarted();

                    DoDebug("Starting setup guide");
                } else {
                    if (!Properties.Settings.Default.HasCompletedTutorial) {
                        ShowGettingStarted();
                        DoDebug("Didn't finish setup guide last time, opening again");
                    }
                }
                SetRegKey("ActionFolder", CheckPath());
                SetRegKey("ActionExtension", Properties.Settings.Default.ActionFileExtension);

                testActionWindow = new TestActionWindow();

                //If newly updated
                if (Properties.Settings.Default.LastKnownVersion != softwareVersion) {
                    //Up(or down)-grade, display version notes
                    DoDebug("ACC has been updated");

                    if (Properties.Settings.Default.LastKnownVersion != "" && new System.Version(Properties.Settings.Default.LastKnownVersion) < new System.Version("1.4.3")) {
                        //Had issues before; fixed now
                        DoDebug("Upgraded to 1.4.3, fixed startup - now starting with Windows");

                        try {
                            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                            rk.DeleteValue(appName, false);
                        } catch {
                            DoDebug("Failed to remove old start with win run");
                        }

                        SetStartup(true);
                    }

                    Properties.Settings.Default.LastUpdated = DateTime.Now;
                    if (gettingStarted != null) {
                        DoDebug("'AboutVersion' window awaits, as 'Getting Started' is showing");
                        aboutVersionAwaiting = true;
                    } else {
                        Properties.Settings.Default.LastKnownVersion = softwareVersion;
                        new NewVersion().Show();
                    }
                    Properties.Settings.Default.Save();
                }

                //Check if software starts with Windows
                if (!ACCStartsWithWindows())
                    sysIcon.AddOpenOnStartupMenu();

                /* 'Evalufied' user feedback implementation */
                if ((DateTime.Now - Properties.Settings.Default.LastUpdated).TotalDays >= 7 && Properties.Settings.Default.TimesOpened >= 7
                    && gettingStarted == null
                    && !Properties.Settings.Default.HasPromptedFeedback) {
                    //User has had the software/update for at least 7 days, and has opened the software more than 7 times - time to ask for feedback
                    //(also the "getting started" window is not showing)
                    if (HasInternet()) {
                        try {
                            WebRequest request = WebRequest.Create("https://evalufied.dk/");
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            if (response == null || response.StatusCode != HttpStatusCode.OK) {
                                DoDebug("'Evalufied' is down - won't show faulty feedback window");
                            } else {
                                DoDebug("Showing 'User Feedback' window");
                                Properties.Settings.Default.HasPromptedFeedback = true;
                                Properties.Settings.Default.Save();
                                new UserFeedback().Show();
                            }
                        } catch {
                            DoDebug("Failed to check for 'Evalufied'-availability");
                        }
                    } else {
                        DoDebug("No internet connection, not showing user feedback window");
                    }
                }

                //Action mods implementation
                ActionMods.CheckMods();
                TaskSchedulerSetup();

                hasStarted = true;
                SystemEvents.SessionSwitch += new SessionSwitchEventHandler(SystemEvents_SessionSwitch); //On wake up from sleep
                Application.Run();
            }

            if (sentryToken != "super_secret") {
                //Tracking issues with Sentry.IO - not forked from GitHub (official version)
                bool sentryOK = false;
                try {
                    if (Properties.Settings.Default.UID != "") {
                        Properties.Settings.Default.UID = Guid.NewGuid().ToString();
                        Properties.Settings.Default.Save();
                    }

                    if (Properties.Settings.Default.UID != "") {
                        SentrySdk.ConfigureScope(scope => {
                            scope.User = new Sentry.Protocol.User {
                                Id = Properties.Settings.Default.UID
                            };
                        });
                    }

                    using (SentrySdk.Init(sentryToken)) {
                        sentryOK = true;
                    }
                } catch {
                    //Sentry failed. Error sentry's side or invalid key - don't let this stop the app from running
                    DoDebug("Sentry initiation failed");
                    ActualMain();
                }

                if (sentryOK) {
                    try {
                        using (SentrySdk.Init(sentryToken)) {
                            DoDebug("Sentry initiated");
                            ActualMain();
                        }
                    } catch {
                        ActualMain();
                    }
                }
            } else {
                //Code is (most likely) forked - skip issue tracking
                ActualMain();
            }
        }
        //End main function

        //If woken up from sleep, make sure the sleep 'computerAction' is deleted
        static void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e) {
            if (e.Reason == SessionSwitchReason.SessionUnlock) {
                //Unlock. Clear folder.
                DoDebug("Woken from sleep / lock, clearing action folder");
                new CleanupService().Start();
            }
        }

        public static void TaskSchedulerSetup () {
            //Create "Task Scheduler" service; cleanup ACC on startup, log on, workstation unlock
            try {
                using (TaskService ts = new TaskService()) {
                    var ps1File = Path.Combine(MainProgram.currentLocation, "ExtraCleanupper.ps1");

                    TaskDefinition td = ts.NewTask();
                    td.Principal.LogonType = TaskLogonType.S4U;
                    td.RegistrationInfo.Author = "Albert MN. | AssistantComputerControl";
                    td.RegistrationInfo.Description = "AssistantComputerControl cleanup - clears the action folder to prevent the same action being executed twice";
                    td.Triggers.Add(new BootTrigger());
                    td.Triggers.Add(new LogonTrigger());
                    td.Triggers.Add(new SessionStateChangeTrigger { StateChange = TaskSessionStateChangeType.SessionUnlock });
                    td.Actions.Add(new ExecAction("powershell.exe", $"-WindowStyle Hidden -file \"{ps1File}\" \"{Path.Combine(MainProgram.CheckPath(), "*")}\" \"*.{Properties.Settings.Default.ActionFileExtension}\"", null));

                    //Register the task in the root folder
                    ts.RootFolder.RegisterTaskDefinition(@"AssistantComputerControl cleanup", td);
                }
            } catch {
                DoDebug("Failed to create / update Task Scheduler service");
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args) {
            Exception e = (Exception)args.ExceptionObject;
            string errorLogLoc = Path.Combine(dataFolderLocation, "error_log.txt");

            string subKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows NT\CurrentVersion";
            RegistryKey thekey = Registry.LocalMachine;
            RegistryKey skey = thekey.OpenSubKey(subKey);

            string windowsVersionName = skey.GetValue("ProductName").ToString();
            string rawWindowsVersion = Environment.OSVersion.ToString();

            int totalExecutions = 0;
            foreach (int action in Properties.Settings.Default.TotalActionsExecuted) {
                totalExecutions += action;
            }
            if (File.Exists(errorLogLoc))
                try {
                    File.Delete(errorLogLoc);
                } catch {
                    DoDebug("Failed to delete error log");
                }

            if (!File.Exists(errorLogLoc)) {
                try {
                    using (var tw = new StreamWriter(errorLogLoc, true)) {
                        tw.WriteLine("OS;");
                        tw.WriteLine("- " + windowsVersionName);
                        tw.WriteLine("- " + rawWindowsVersion);
                        tw.WriteLine();
                        tw.WriteLine("ACC info;");
                        tw.WriteLine("- Version; " + softwareVersion + ", " + releaseDate);
                        tw.WriteLine("- UID; " + Properties.Settings.Default.UID);
                        tw.WriteLine("- Running from; " + currentLocationFull);
                        tw.WriteLine("- Start with Windows; " + (ACCStartsWithWindows() ? "[Yes]" : "[No]"));
                        tw.WriteLine("- Check for updates; " + (Properties.Settings.Default.CheckForUpdates ? "[Yes]" : "[No]"));
                        tw.WriteLine("- In beta program; " + (Properties.Settings.Default.BetaProgram ? "[Yes]" : "[No]"));
                        tw.WriteLine("- Has completed setup guide; " + (Properties.Settings.Default.HasCompletedTutorial ? "[Yes]" : "[No]"));
                        tw.WriteLine("- Check path; " + CheckPath());
                        tw.WriteLine("- Check extension; " + Properties.Settings.Default.ActionFileExtension);
                        tw.WriteLine("- Actions executed; " + totalExecutions);
                        tw.WriteLine("- Assistant type; " + "[Google Assistant: " + Properties.Settings.Default.AssistantType[0] + "] [Alexa: " + Properties.Settings.Default.AssistantType[1] + "] [Unknown: " + Properties.Settings.Default.AssistantType[1] + "]");
                        tw.WriteLine();

                        tw.WriteLine(e);
                        tw.Close();
                    }
                } catch {
                    //Caught exception when trying to log exception... *sigh*
                }
            }

            File.AppendAllText(errorLogLoc, DateTime.Now.ToString() + ": " + e + Environment.NewLine);
            if (debug) {
                Console.WriteLine(e);
            }
            MessageBox.Show("A critical error occurred. The developer has been notified and will resolve this issue ASAP! Try and start ACC again, and avoid whatever just made it crash (for now) :)", "ACC | Error");
        }

        public static bool CheckSettings() {
            //Thanks to https://stackoverflow.com/a/18905791/4880538
            var isReset = false;

            try {
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
            } catch (ConfigurationErrorsException ex) {
                string filename = string.Empty;
                if (!string.IsNullOrEmpty(ex.Filename)) {
                    filename = ex.Filename;
                } else {
                    if (ex.InnerException is ConfigurationErrorsException innerEx && !string.IsNullOrEmpty(innerEx.Filename)) {
                        filename = innerEx.Filename;
                    }
                }

                if (!string.IsNullOrEmpty(filename)) {
                    if (File.Exists(filename)) {
                        var fileInfo = new System.IO.FileInfo(filename);
                        var watcher
                             = new FileSystemWatcher(fileInfo.Directory.FullName, fileInfo.Name);
                        File.Delete(filename);
                        isReset = true;
                        if (File.Exists(filename)) {
                            watcher.WaitForChanged(System.IO.WatcherChangeTypes.Deleted);
                        }
                    }
                }
            }

            return isReset;
        }

        private static bool CreateLogFile() {
            try {
                using (var tw = new StreamWriter(logFilePath, true)) {
                    tw.WriteLine(string.Empty);
                    tw.Close();
                }
            } catch (Exception e) {
                Console.WriteLine("Could not create/clear log file; " + e.Message);
                return false;
            }

            return true;
        }

        public static void SetupListener() {
            watcher.Path = CheckPath();
            watcher.Filter = "*." + Properties.Settings.Default.ActionFileExtension;
            TaskSchedulerSetup();
            DoDebug("Listener modified");
        }

        public static void SetCheckFolder(string setTo) {
            if (!String.IsNullOrEmpty(setTo)) {
                Console.WriteLine(setTo);
                if (!Directory.Exists(setTo) && setTo != null) {
                    string[] splitted1 = setTo.Split('\\');
                    string[] splitted2 = setTo.Split('\\');

                    string[] splitted3 = setTo.Split('/');
                    string[] splitted4 = setTo.Split('/');

                    if (splitted1[splitted1.Length - 1] == "AssistantComputerControl" || splitted2[splitted2.Length - 1] == "AssistantComputerControl" ||
                        splitted3[splitted3.Length - 1] == "AssistantComputerControl" || splitted4[splitted4.Length - 1] == "AssistantComputerControl") {
                        try {
                            Directory.CreateDirectory(setTo);
                        } catch {
                            MessageBox.Show(Translator.__("invalid_path", "general"), MainProgram.messageBoxTitle);
                            return;
                        }
                    } else {
                        MessageBox.Show(Translator.__("invalid_path", "general"), MainProgram.messageBoxTitle);
                        return;
                    }
                }

                Console.WriteLine(setTo);

                SetRegKey("ActionFolder", setTo);

                Properties.Settings.Default.ActionFilePath = setTo;
                Properties.Settings.Default.Save();

                SetRegKey("ActionFolder", setTo);

                SetupListener();
                DoDebug("Check folder updated (" + CheckPath() + ")");
            } else {
                MessageBox.Show(Translator.__("invalid_path", "general"), MainProgram.messageBoxTitle);
            }
        }

        public static void SetCheckExtension(string setTo) {
            SetRegKey("ActionExtension", setTo);

            Properties.Settings.Default.ActionFileExtension = setTo;
            Properties.Settings.Default.Save();

            SetRegKey("ActionExtension", Properties.Settings.Default.ActionFileExtension);

            SetupListener();
        }

        public static void SetRegKey(string theKey, string setTo) {
            if (setTo != null) {
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true);
                if (Registry.GetValue(key.Name + "\\AssistantComputerControl", theKey, null) == null) {
                    key.CreateSubKey("AssistantComputerControl");
                }
                key = key.OpenSubKey("AssistantComputerControl", true);
                key.SetValue(theKey, setTo);
            } else {
                DoDebug("Invalid value (is null)");
            }
        }

        //On application exit event
        private static void OnApplicationExit(object sender, EventArgs e) {
            sysIcon.HideIcon();
        }
        private static bool ConsoleEventCallback(int eventType) {
            if (eventType == 2) {
                //Closing console
                sysIcon.HideIcon();
            }
            return false;
        }

        private static ConsoleEventDelegate handler;
        private delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
        public static void Exit() {
            DoDebug("Exiting");
            sysIcon.HideIcon();
            Application.Exit();
            //Application.Exit doesn't close the application on update...?
            Environment.Exit(1);
        }

        public static void SetStartup(bool status, bool setThroughSoftware = false) {
            try {
                if (status) {
                    //Create "Task Scheduler" service; run ACC on startup & log on, added by Shelby Marvell
                    try {
                        using (TaskService ts = new TaskService()) {
                            TaskDefinition td = ts.NewTask();
                            td.Principal.LogonType = TaskLogonType.InteractiveToken;
                            td.Principal.RunLevel = TaskRunLevel.Highest;
                            td.RegistrationInfo.Author = "Albert MN. | AssistantComputerControl";
                            td.RegistrationInfo.Description = "AssistantComputerControl startup - Runs ACC on reboot/login";
                            td.Triggers.Add(new LogonTrigger());
                            td.Actions.Add(new ExecAction(Application.ExecutablePath, null, null));

                            //Register the task in the root folder
                            ts.RootFolder.RegisterTaskDefinition(@"AssistantComputerControl startup", td);
                        }
                    } catch {
                        DoDebug("Failed to create / update Task Scheduler startup service");
                    }
                } else {
                    //Create "Task Scheduler" service; run ACC on startup & log on, added by Shelby Marvell
                    try {
                        using (TaskService ts = new TaskService()) {
                            // Register the task in the root folder
                            ts.RootFolder.DeleteTask(@"AssistantComputerControl startup");
                        }
                    } catch {
                        DoDebug("Failed to create / update Task Scheduler startup service");
                    }
                }

            } catch {
                DoDebug("Failed to start ACC with Windows");
                if (!setThroughSoftware) {
                    MessageBox.Show("Failed to make ACC start with Windows", messageBoxTitle);
                }
            }
        }

        public static bool ACCStartsWithWindows() {
            try {
                using (TaskService ts = new TaskService()) {
                    return ts.GetTask(@"AssistantComputerControl startup") != null;
                }
            } catch (Exception e) {
                DoDebug("Something went wrong with TaskService, checking if ACC starts with Windows; " + e.Message);
            }

            return false;
        }

        public static bool HasInternet() {
            try {
                using (var client = new WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204")) {
                    return true;
                }
            } catch {
                return false;
            }
        }

        public static bool ApplicationInstalled(string c_name) {
            string displayName;

            string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey);
            if (key != null) {
                foreach (RegistryKey subkey in key.GetSubKeyNames().Select(keyName => key.OpenSubKey(keyName))) {
                    displayName = subkey.GetValue("DisplayName") as string;
                    if (displayName != null && displayName.Contains(c_name)) {
                        return true;
                    }
                }
                key.Close();
            }

            registryKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
            key = Registry.LocalMachine.OpenSubKey(registryKey);
            if (key != null) {
                foreach (RegistryKey subkey in key.GetSubKeyNames().Select(keyName => key.OpenSubKey(keyName))) {
                    displayName = subkey.GetValue("DisplayName") as string;
                    if (displayName != null && displayName.Contains(c_name)) {
                        return true;
                    }
                }
                key.Close();
            }
            return false;
        }

        private static bool PathIsFile(string path) {
            if (path == null) throw new ArgumentNullException("path");
            path = path.Trim();

            if (Directory.Exists(path))
                return true;

            if (File.Exists(path))
                return false;

            // neither file nor directory exists. guess intention

            // if has trailing slash then it's a directory
            if (new[] { "\\", "/" }.Any(x => path.EndsWith(x)))
                return true; // ends with slash

            // if has extension then its a file; directory otherwise
            return string.IsNullOrWhiteSpace(Path.GetExtension(path));
        }

        public static void DefaultPathIssue() {
            //Path is program root - most likely an error, alert user
            DialogResult dialogResult = MessageBox.Show("It seems the path to the cloud service wasn't set correctly. Choose \"Yes\" to go through the setup again. If this doesn't work, try restarting the ACC software.", "Whoops, problem!", MessageBoxButtons.YesNo);
            DoDebug(dialogResult.ToString());

            if (dialogResult == DialogResult.Yes) {
                Properties.Settings.Default.HasCompletedTutorial = false;
                Properties.Settings.Default.ActionFilePath = "";
                Properties.Settings.Default.Save();

                if (gettingStarted != null) {
                    gettingStarted.Close();
                }

                ShowGettingStarted();
            }
        }

        public static string CheckPath() {
            string path = currentLocation;

            if (Properties.Settings.Default.ActionFilePath != "") {
                //Custom path set
                if (Directory.Exists(Properties.Settings.Default.ActionFilePath)) {
                    path = Properties.Settings.Default.ActionFilePath;
                }
            } else {
                if ((Properties.Settings.Default.HasCompletedTutorial && gettingStarted is null && !hasAskedForSetupAgain)) {
                    //Cloud service path not found & no custom filepath, go through setup again?
                    hasAskedForSetupAgain = true;
                    var msgBox = MessageBox.Show(Translator.__("no_cloudservice_chosen", "general"), "[ERROR] No folder specified | AssistantComputerControl", MessageBoxButtons.YesNo);
                    if (msgBox == DialogResult.Yes) {
                        ShowGettingStarted();
                    }
                }
            }

            if (hasStarted && gettingStarted is null && path == currentLocation && !hasAskedForSetupAgain) {
                DoDebug("Did it here");

                hasAskedForSetupAgain = true;
                DefaultPathIssue();
            }

            return Path.HasExtension(path) ? Path.GetDirectoryName(path) : path;
        }

        private static readonly NLog.Logger _log_ = NLog.LogManager.GetCurrentClassLogger();
        public static void DoDebug(string str) {
            _log_.Debug(str);
            if (debug) {
                Console.WriteLine(str);
            }
        }

        static private void SetupDataFolder() {
            if (!Directory.Exists(dataFolderLocation)) {
                Directory.CreateDirectory(dataFolderLocation);
            }
        }

        public static void ShowSettings() {
            if (settingsForm is null) {
                //New instance
                settingsForm = new SettingsForm();
                settingsForm.Show();

                settingsForm.FormClosed += delegate { if (reopenSettingsOnClose) { ShowSettings(); reopenSettingsOnClose = false; } };
                settingsForm.FormClosing += delegate { settingsForm = null; };
            } else {
                //Focus
                settingsForm.Focus();
            }
        }

        public static void ShowGettingStarted(int startTab = 0) {
            if (gettingStarted is null) {
                //New instance
                gettingStarted = new GettingStarted(startTab);
                gettingStarted.Show();

                gettingStarted.FormClosing += delegate { settingsForm = null; GettingStarted.WebBrowserHandler.stopCheck = true; };
            } else {
                //Focus
                gettingStarted.Focus();
            }
        }

        public static bool IsValidPath(string path, bool allowRelativePaths = false) {
            bool isValid = true;

            try {
                string fullPath = Path.GetFullPath(path);

                if (allowRelativePaths) {
                    isValid = Path.IsPathRooted(path);
                } else {
                    string root = Path.GetPathRoot(path);
                    isValid = string.IsNullOrEmpty(root.Trim(new char[] { '\\', '/' })) == false;
                }
            } catch {
                isValid = false;
            }

            return isValid;
        }
    }
}