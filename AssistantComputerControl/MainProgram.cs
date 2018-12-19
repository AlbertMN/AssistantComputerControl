/*
 * AssistantComputerControl
 * Made by Albert MN.
 * Updated: v1.1.4, 15-11-2018
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
using Newtonsoft.Json;
using System.Linq;
using System.Threading;
using Sentry;

namespace AssistantComputerControl {
    class MainProgram {
        public const string softwareVersion = "1.1.4",
            releaseDate = "2018-12-13 20:30:00", //YYYY-MM-DD H:i:s - otherwise it gives an error
            appName = "AssistantComputerControl";
        static public bool debug = true,
            unmuteVolumeChange = true,
            isCheckingForUpdate = false,

            testingAction = false,
            aboutVersionAwaiting = false,
            hasAskedForSetupAgain = false;

        public TestStatus currentTestStatus = TestStatus.ongoing;
        public enum TestStatus {
            success,
            error,
            ongoing
        }

        private static FileSystemWatcher watcher;

        static public string currentLocationFull = Assembly.GetEntryAssembly().Location,
            defaultActionFolder = CheckPath(),

            currentLocation = Path.GetDirectoryName(currentLocationFull),
            dataFolderLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AssistantComputerControl"),
            shortcutLocation = Path.Combine(dataFolderLocation, "shortcuts"),
            logFilePath = Path.Combine(dataFolderLocation, "log.txt"),
            errorMessage = "",
            startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup),
            messageBoxTitle = appName;

        private static SysTrayIcon sysIcon = new SysTrayIcon();
        public static TestActionWindow testActionWindow = new TestActionWindow();
        private static SettingsForm settingsForm = null;
        public static GettingStarted gettingStarted = null;
        public static UpdateProgress updateProgressWindow;


        //Start main function
        [STAThread]
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        static void Main(string[] args) {
            if (AnalyticsSettings.sentryToken != "super_secret") {
                //Tracking issues with Sentry.IO - not forked from GitHub (official version)
                bool sentryOK = false;
                try {
                    if (Properties.Settings.Default.UID != "") {
                        SentrySdk.ConfigureScope(scope => {
                            scope.User = new Sentry.Protocol.User {
                                Id = Properties.Settings.Default.UID
                            };
                        });
                    }

                    using (SentrySdk.Init(AnalyticsSettings.sentryToken)) {
                        sentryOK = true;
                    }
                } catch {
                    //Sentry failed. Error sentry's side or invalid key - don't let this stop the app from running
                    DoDebug("Sentry initiation failed");
                    ActualMain();
                }

                if (sentryOK) {
                    using (SentrySdk.Init(AnalyticsSettings.sentryToken)) {
                        DoDebug("Sentry initiated");
                        ActualMain();
                    }
                }
            } else {
                //Code is (most likely) forked - skip issue tracking
                ActualMain();
            }

            void ActualMain() {
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                SetupDataFolder();
                if (File.Exists(logFilePath))
                    File.WriteAllText(logFilePath, string.Empty);
                else
                    CreateLogFile();

                //Check if software already runs, if so kill this instance
                var otherACCs = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(currentLocationFull));
                if (otherACCs.Length > 1) {
                    //DoDebug("ACC is already running, killing this proccess");
                    //MessageBox.Show("ACC is already running.", "Already running | " + messageBoxTitle + "");
                    //Process.GetCurrentProcess().Kill();

                    //Try kill the _other_ process instead
                    foreach (Process p in otherACCs) {
                        if (p.Id != Process.GetCurrentProcess().Id) {
                            p.Kill();
                            DoDebug("Other ACC instance was running. Killed it.");
                        }
                    }
                }

                Application.EnableVisualStyles();

                DoDebug("[ACC begun (v" + softwareVersion + ")]");
                AnalyticsSettings.SetupAnalytics();

                if (Properties.Settings.Default.CheckForUpdates) {
                    if (HasInternet()) {
                        new Thread(() => {
                            new ACC_Updater().Check();
                        }).Start();
                    } else {
                        DoDebug("Couldn't check for new update as PC does not have access to the internet");
                    }
                }

                //On console close: hide NotifyIcon
                Application.ApplicationExit += new EventHandler(OnApplicationExit);
                handler = new ConsoleEventDelegate(ConsoleEventCallback);
                SetConsoleCtrlHandler(handler, true);

                //Check if software starts with Windows
                if (!Properties.Settings.Default.StartWithWindows)
                    sysIcon.AddOpenOnStartupMenu();

                //Create shortcut folder if doesn't exist
                if (!Directory.Exists(shortcutLocation)) {
                    Directory.CreateDirectory(shortcutLocation);
                }
                if (!File.Exists(Path.Combine(shortcutLocation, @"example.txt"))) {
                    //Create example-file
                    using (StreamWriter sw = File.CreateText(Path.Combine(shortcutLocation, @"example.txt"))) {
                        sw.WriteLine("This is an example file.");
                        sw.WriteLine("If you haven't already, make your assistant open this file!");
                    }
                }

                //Delete all old action files
                if (Directory.Exists(CheckPath())) {
                    foreach (string file in Directory.GetFiles(CheckPath(), "*." + Properties.Settings.Default.ActionFileExtension)) {
                        ClearFile(file);
                    }
                }

                //SetupListener();
                watcher = new FileSystemWatcher() {
                    Path = CheckPath(),
                    NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                        | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                    Filter = "*." + Properties.Settings.Default.ActionFileExtension,
                    EnableRaisingEvents = true
                };
                watcher.Changed += new FileSystemEventHandler(ActionChecker.FileFound);
                watcher.Created += new FileSystemEventHandler(ActionChecker.FileFound);

                DoDebug("\n[" + messageBoxTitle + "] Initiated. \nListening in: \"" + CheckPath() + "\" for \"." + Properties.Settings.Default.ActionFileExtension + "\" extensions");

                sysIcon.TrayIcon.Icon = Properties.Resources.ACC_icon;

                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true);
                if (Registry.GetValue(key.Name + "\\AssistantComputerControl", "FirstTime", null) == null) {
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

                if (gettingStarted is null && !Properties.Settings.Default.AnalyticsInformed) {
                    //"Getting started" not shown but user hasn't been told about analytics gathering yet
                    ShowGettingStarted(3);
                }

                //If newly updated
                if (Properties.Settings.Default.LastKnownVersion != softwareVersion) {
                    //Up(or down)-grade, display version notes
                    if (gettingStarted != null) {
                        DoDebug("'AboutVersion' window awaits, as 'Getting Started' is showing");
                        aboutVersionAwaiting = true;
                    } else {
                        Properties.Settings.Default.LastKnownVersion = softwareVersion;
                        new NewVersion().Show();
                        Properties.Settings.Default.Save();
                    }
                }

                Application.Run();
            }
        }
        //End main function


        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args) {
            Exception e = (Exception)args.ExceptionObject;
            string errorLogLoc = Path.Combine(dataFolderLocation, "error_log.txt");

            string subKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows NT\CurrentVersion";
            RegistryKey thekey = Registry.LocalMachine;
            RegistryKey skey = thekey.OpenSubKey(subKey);

            string windowsVersionName = skey.GetValue("ProductName").ToString();
            string rawWindowsVersion = Environment.OSVersion.ToString();

            int totalExecutions = 0;
            foreach(int action in Properties.Settings.Default.TotalActionsExecuted) {
                totalExecutions += action;
            }
            if (File.Exists(errorLogLoc))
                try {
                    File.Delete(errorLogLoc);
                } catch {
                    DoDebug("Failed to delete error log");
                }

            if (!File.Exists(errorLogLoc)) {
                using (var tw = new StreamWriter(errorLogLoc, true)) {
                    tw.WriteLine("OS;");
                    tw.WriteLine("- " + windowsVersionName);
                    tw.WriteLine("- " + rawWindowsVersion);
                    tw.WriteLine();
                    tw.WriteLine("ACC info;");
                    tw.WriteLine("- Version; " + softwareVersion + ", " + releaseDate);
                    tw.WriteLine("- UID; " + Properties.Settings.Default.UID);
                    tw.WriteLine("- Running from; " + currentLocationFull);
                    tw.WriteLine("- Start with Windows; " + (Properties.Settings.Default.StartWithWindows ? "[Yes]" : "[No]"));
                    tw.WriteLine("- Analytics; " + (Properties.Settings.Default.SendAnonymousAnalytics ? "[Yes]" : "[No]"));
                    tw.WriteLine("- Check for updates; " + (Properties.Settings.Default.CheckForUpdates ? "[Yes]" : "[No]"));
                    tw.WriteLine("- In beta program; " + (Properties.Settings.Default.BetaProgram ? "[Yes]" : "[No]"));
                    tw.WriteLine("- Has completed setup guide; " + (Properties.Settings.Default.HasCompletedTutorial ? "[Yes]" : "[No]"));
                    tw.WriteLine("- Check path; " + CheckPath());
                    tw.WriteLine("- Check extension; " + Properties.Settings.Default.ActionFileExtension);
                    tw.WriteLine("- Has Dropbox; " + (GetDropboxFolder() == "" ? "[No]" : "[Yes]"));
                    tw.WriteLine("- Actions executed; " + totalExecutions);
                    tw.WriteLine("- Assistant type; " + "[Google Assistant: " + Properties.Settings.Default.AssistantType[0] + "] [Alexa: " + Properties.Settings.Default.AssistantType[1] + "] [Unknown: " + Properties.Settings.Default.AssistantType[1] + "]");
                    tw.WriteLine();

                    tw.WriteLine(e);
                    tw.Close();
                }
            }

            File.AppendAllText(errorLogLoc, DateTime.Now.ToString() + ": " + e + Environment.NewLine);
            if (debug) {
                Console.WriteLine(e);
            }
            MessageBox.Show("A critical error occurred. The developer has been notified and will resolve this issue ASAP! Try and start ACC again, and avoid whatever just made it crash (for now) :)", "ACC | Error");
        }

        private static void CreateLogFile() {
            using (var tw = new StreamWriter(logFilePath, true)) {
                tw.WriteLine(string.Empty);
                tw.Close();
            }
        }

        public static void SetupListener() {
            watcher.Path = CheckPath();
            watcher.Filter = "*." + Properties.Settings.Default.ActionFileExtension;
            DoDebug("Listener modified");
        }

        public static void SetCheckFolder(string setTo) {
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
                        MessageBox.Show("Path not valid", "ACC");
                        return;
                    }
                } else {
                    MessageBox.Show("Path not valid", "ACC");
                    return;
                }
            }

            Console.WriteLine(setTo);

            SetRegKey("ActionFolder", setTo);

            Properties.Settings.Default.ActionFilePath = setTo;
            Properties.Settings.Default.Save();

            SetupListener();
            DoDebug("Check folder updated (" + CheckPath() + ")");
        }
        public static void SetCheckExtension(string setTo) {
            SetRegKey("ActionExtension", setTo);

            Properties.Settings.Default.ActionFileExtension = setTo;
            Properties.Settings.Default.Save();

            SetupListener();
        }

        public static void SetRegKey(string theKey, string setTo) {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true);
            if (Registry.GetValue(key.Name + "\\AssistantComputerControl", theKey, null) == null) {
                key.CreateSubKey("AssistantComputerControl");
            }
            key = key.OpenSubKey("AssistantComputerControl", true);
            key.SetValue(theKey, setTo);
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

        private class DropboxJson {
            public DropboxJson business { get; set; }
            public DropboxJson personal { get; set; }
            public string Path { get; set; }
        }

        public static string GetDropboxFolder() {
            if (ApplicationInstalled("Dropbox")) {
                string infoPath = @"Dropbox\info.json";
                string jsonPath = Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), infoPath);

                if (!Directory.Exists(Directory.GetDirectoryRoot(jsonPath))) return "";
                if (!File.Exists(jsonPath)) jsonPath = Path.Combine(Environment.GetEnvironmentVariable("AppData"), infoPath);
                if (!File.Exists(jsonPath)) return "";

                string jsonContent = File.ReadAllText(jsonPath);
                try {
                    DropboxJson dropboxJson = JsonConvert.DeserializeObject<DropboxJson>(jsonContent);
                    if (dropboxJson != null) {
                        if (dropboxJson.personal != null) {
                            return dropboxJson.personal.Path;
                        } else if (dropboxJson.business != null) {
                            return dropboxJson.business.Path;
                        }
                    }
                } catch {
                    DoDebug("Failed to deserialize Dropbox Json");
                    DoDebug(jsonContent);
                }
            } else {
                DoDebug("Dropbox not installed");
                return "";
            }
            return "";
        }

        public static void SetStartup(bool status) {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (status) {
                rk.SetValue(appName, Application.ExecutablePath);
                DoDebug("ACC now starts with Windows");
            } else {
                rk.DeleteValue(appName, false);
                DoDebug("ACC no longer starts with Windows");
            }
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

        public static string CheckPath() {
            string path = currentLocation;

            if (Properties.Settings.Default.ActionFilePath != "") {
                //Custom path set
                if (Directory.Exists(Properties.Settings.Default.ActionFilePath)) {
                    path = Properties.Settings.Default.ActionFilePath;
                }
            } else {
                string dropboxFolder = GetDropboxFolder();
                if (dropboxFolder == "" || dropboxFolder == null || !Directory.Exists(dropboxFolder)) {
                    if (Properties.Settings.Default.HasCompletedTutorial && gettingStarted is null && !hasAskedForSetupAgain) {
                        //Dropbox not found & no custom filepath, go through setup again?
                        hasAskedForSetupAgain = true;
                        var msgBox = MessageBox.Show("Dropbox (required) doesn't seem to be installed... Do you want to go through the setup guide again?", "[ERROR] No folder specified | " + messageBoxTitle, MessageBoxButtons.YesNo);
                        if (msgBox == DialogResult.Yes) {
                            ShowGettingStarted();
                        }
                    }
                } else {
                    string dropboxACCpath = dropboxFolder + @"\AssistantComputerControl";
                    if (!Directory.Exists(dropboxACCpath)) {
                        DirectoryInfo di = Directory.CreateDirectory(dropboxACCpath);
                        di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                    }
                    path = dropboxACCpath;
                }
            }
            
            return Path.HasExtension(path) ? Path.GetDirectoryName(path) : path;
        }

        public static void DoDebug(string str) {
            try {
                if (!File.Exists(logFilePath)) {
                    CreateLogFile();
                }
                File.AppendAllText(logFilePath, DateTime.Now.ToString() + ": " + str + Environment.NewLine);
                if (debug) {
                    Console.WriteLine(str);
                }
            } catch (Exception e) {
                Console.WriteLine("Failed to write to log, exception");
            }
        }

        static public void ClearFile(string filePath) {
            DoDebug("Clearing file");
            if (File.Exists(filePath)) {
                while (ActionChecker.FileInUse(filePath)) ;
                File.Delete(filePath);
                while (File.Exists(filePath)) ;
                DoDebug("Action-file deleted");
            } else {
                DoDebug("No file to delete");
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

                gettingStarted.FormClosing += delegate { settingsForm = null; };
            } else {
                //Focus
                gettingStarted.Focus();
            }
        }
    }
}