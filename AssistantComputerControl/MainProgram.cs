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

namespace AssistantComputerControl {
    class MainProgram {
        public const string softwareVersion = "1.0.2",
            releaseDate = "2018-07-21 21:19",
            appName = "AssistantComputerControl";
        static public bool debug = true,
            unmuteVolumeChange = true,
            isPerformingAction = false,
            isCheckingForUpdate = false,

            testingAction = false;

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

        [STAThread]
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        static void Main(string[] args) {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            SetupDataFolder();
            if (File.Exists(logFilePath))
                File.WriteAllText(logFilePath, string.Empty);
            else
                CreateLogFile();

            //Check if software already runs, if so kill this instance
            if (Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location)).Length > 1) {
                DoDebug("ACC is already running, killing this proccess");
                MessageBox.Show("ACC is already running.", "Already running | " + messageBoxTitle + "");
                Process.GetCurrentProcess().Kill();
            }

            DoDebug("[ACC begun (v" + softwareVersion + ")]");
            AnalyticsSettings.SetupAnalyticsAsync();

            if (Properties.Settings.Default.CheckForUpdates) {
                if (HasInternet()) {
                    new Thread(() => {
                        new ACC_Updater().Check();
                    }).Start();
                } else {
                    DoDebug("Couldn't check for new update as PC does not have access to the internet");
                }
            }
            
            if (File.Exists(Path.Combine(dataFolderLocation, "updated.txt"))) {
                File.Delete(Path.Combine(dataFolderLocation, "updated.txt"));
                new AboutVersion().Show();
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

            SetupListener();

            DoDebug("\n[" + messageBoxTitle + "] Initiated. \nListening in: \"" + CheckPath() + "\" for \"." + Properties.Settings.Default.ActionFileExtension + "\" extensions");

            Application.EnableVisualStyles();
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

            Application.Run();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args) {
            Exception e = (Exception)args.ExceptionObject;
            string errorLogLoc = Path.Combine(dataFolderLocation, "error_log.txt");

            if (!File.Exists(errorLogLoc)) {
                using (var tw = new StreamWriter(errorLogLoc, true)) {
                    tw.WriteLine(e);
                    tw.Close();
                }
            }

            File.AppendAllText(errorLogLoc, DateTime.Now.ToString() + ": " + e + Environment.NewLine);
            if (debug) {
                Console.WriteLine(e);
            }
            MessageBox.Show("An unhandled critical error occurred. Please contact the developer (https://github.com/AlbertMN/AssistantComputerControl/issues)");
        }

        private static void CreateLogFile() {
            using (var tw = new StreamWriter(logFilePath, true)) {
                tw.WriteLine(string.Empty);
                tw.Close();
            }
        }

        public static void SetupListener() {
            if (Directory.Exists(CheckPath())) {
                watcher = new FileSystemWatcher() {
                    Path = CheckPath(),
                    NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                       | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                    Filter = "*." + Properties.Settings.Default.ActionFileExtension,
                    EnableRaisingEvents = true
                };
                watcher.Changed += new FileSystemEventHandler(ActionChecker.FileFound);
            }
        }

        public static void SetCheckFolder(string setTo) {
            SetRegKey("ActionFolder", setTo);

            Properties.Settings.Default.ActionFilePath = setTo;
            Properties.Settings.Default.Save();

            SetupListener();
        }
        public static void SetCheckExtension(string setTo) {
            SetRegKey("ActionExtension", setTo);

            Properties.Settings.Default.ActionFileExtension = setTo;
            Properties.Settings.Default.Save();
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
            public string Path { get; set; }
        }

        public static string GetDropboxFolder() {
            if (ApplicationInstalled("Dropbox")) {
                string infoPath = @"Dropbox\info.json";
                string jsonPath = Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), infoPath);

                if (!Directory.Exists(Directory.GetDirectoryRoot(jsonPath))) return "";
                if (!File.Exists(jsonPath)) jsonPath = Path.Combine(Environment.GetEnvironmentVariable("AppData"), infoPath);
                if (!File.Exists(jsonPath)) return "";

                string jsonContent = File.ReadAllText(jsonPath).Replace("{\"personal\":", "");
                jsonContent = jsonContent.Remove(jsonContent.Length - 1, 1);
                DropboxJson latestBeta = JsonConvert.DeserializeObject<DropboxJson>(jsonContent);

                string dropboxPath = latestBeta.Path;

                return dropboxPath;
            } else {
                DoDebug("Dropbox not installed");
                return "";
            }
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

        public static string CheckPath() {
            string path = "";

            if (Properties.Settings.Default.ActionFilePath != "") {
                //Custom path set
                path = Properties.Settings.Default.ActionFilePath;
            } else {
                string dropboxFolder = GetDropboxFolder();
                if (dropboxFolder == "" || dropboxFolder == null || !Directory.Exists(dropboxFolder)) {
                    /*if (Properties.Settings.Default.HasCompletedTutorial) {
                        //Dropbox not found & no custom filepath, go through setup again?
                        var msgBox = MessageBox.Show("Dropbox (required) doesn't seem to be installed... Do you want to go through the setup guide again?", "[ERROR] No folder specified | " + messageBoxTitle, MessageBoxButtons.YesNo);
                        if (msgBox == DialogResult.Yes) {
                            ShowGettingStarted();
                        }
                    }*/
                } else {
                    string dropboxACCpath = dropboxFolder + @"\AssistantComputerControl";
                    if (!Directory.Exists(dropboxACCpath)) {
                        DirectoryInfo di = Directory.CreateDirectory(dropboxACCpath);
                        di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                    }
                    path = dropboxACCpath;
                }
            }

            return path;
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
                Console.WriteLine("Failed to write to log, exception; " + e);
            }
        }

        static public void ClearFile(string filePath) {
            DoDebug("Clearing file");
            if (File.Exists(filePath)) {
                while (ActionChecker.FileInUse(filePath)) ;
                File.Delete(filePath);
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

        public static void ShowGettingStarted() {
            if (gettingStarted is null) {
                //New instance
                gettingStarted = new GettingStarted();
                gettingStarted.Show();

                gettingStarted.FormClosing += delegate { settingsForm = null; };
            } else {
                //Focus
                gettingStarted.Focus();
            }
        }
    }
}
