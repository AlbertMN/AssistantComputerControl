using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32;
using System.Net;
using Newtonsoft.Json;
using System.Linq;

namespace AssistantComputerControl {
    class MainProgram {
        public const string softwareVersion = "0.4.0",
            releaseDate = "2018-07-09 02:46",
            appName = "AssistantComputerControl";
        static public bool debug = true,
            unmuteVolumeChange = true,
            isPerformingAction = false,

            testingAction = false;

        public TestStatus currentTestStatus = TestStatus.ongoing;
        public enum TestStatus {
            success,
            error,
            ongoing
        }

        static public string currentLocationFull = Assembly.GetEntryAssembly().Location,
            defaultActionFolder = CheckPath(),

            currentLocation = Path.GetDirectoryName(currentLocationFull),
            dataFolderLocation = Path.Combine(currentLocation, "ACC_Data"),
            shortcutLocation = Path.Combine(currentLocation, "shortcuts"),
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
            //Check if software already runs, if so kill this instance
            if (Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location)).Length > 1) {
                DoDebug("ACC is already running, killing this proccess");
                MessageBox.Show("ACC is already running.", "Already running | " + messageBoxTitle + "");
                Process.GetCurrentProcess().Kill();
            }
            SetupDataFolder();
            if(File.Exists(logFilePath))
                File.WriteAllText(logFilePath, string.Empty);

            DoDebug("[ACC begun (v" + softwareVersion + ")]");
            AnalyticsSettings.SetupArray();

            if (Properties.Settings.Default.CheckForUpdates) {
                if (HasInternet()) {
                    new ACC_Updater().Check();
                } else {
                    DoDebug("Couldn't check for new update as PC does not have access to the internet");
                }
            }
            if(File.Exists(Path.Combine(dataFolderLocation, "updated.txt"))) {
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
            foreach (string file in Directory.GetFiles(CheckPath(), "*." + Properties.Settings.Default.ActionFileExtension)) {
                ClearFile(file);
            }
            
            //WATCHER
            FileSystemWatcher watcher = new FileSystemWatcher() {
                Path = CheckPath(),
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*." + Properties.Settings.Default.ActionFileExtension,
                EnableRaisingEvents = true
            };
            watcher.Changed += new FileSystemEventHandler(ActionChecker.FileFound);
            //END WATCHER

            DoDebug("\n[" + messageBoxTitle + "] Initiated. \nListening in: \"" + CheckPath() + "\" for \"." + Properties.Settings.Default.ActionFileExtension + "\" extensions");

            Application.EnableVisualStyles();
            sysIcon.TrayIcon.Icon = Properties.Resources.ACC_icon;

            //REMOVE THIS
            //ShowGettingStarted();

            //Create "first time" reg-key
            //ShowGettingStarted();
            
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

        public static void SetCheckFolder(string setTo) {
            SetRegKey("ActionFolder", setTo);

            Properties.Settings.Default.ActionFilePath = setTo;
            Properties.Settings.Default.Save();
        }
        public static void SetCheckExtension(string setTo) {
            SetRegKey("ActionExtension", setTo);

            Properties.Settings.Default.ActionFileExtension = setTo;
            Properties.Settings.Default.Save();
        }

        private static void SetRegKey(string theKey, string setTo) {
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
            string path;

            if (Properties.Settings.Default.ActionFilePath != "") {
                //Custom path set
                path = Properties.Settings.Default.ActionFilePath;
            } else {
                string dropboxFolder = GetDropboxFolder();
                if (dropboxFolder == "" || !Directory.Exists(dropboxFolder)) {
                    //Dropbox not found & no custom filepath, warn and close
                    path = "";

                    var msgBox = MessageBox.Show("Dropbox is not installed, and no custom folder path is set (advanced). Do you want to install Dropbox?", "[ERROR] No folder specified | " + messageBoxTitle, MessageBoxButtons.YesNo);
                    if (msgBox == DialogResult.Yes) {
                        Process.Start("https://www.dropbox.com/");
                        MessageBox.Show("Great! I'll wait until you've installed Dropbox!", "Waiting for Dropbox to be installed | " + messageBoxTitle);

                        while (GetDropboxFolder() == "") {
                            Thread.Sleep(10000);
                        }
                        var firstTime = MessageBox.Show("I see you've installed Dropbox! Let's get ACC set up properly. Is this your first time using ACC? ", "Almost ready... | " + messageBoxTitle, MessageBoxButtons.YesNo);
                        if (firstTime == DialogResult.Yes) {
                            //Go through setup tutorial

                            //TO-DO
                        } else {
                            MessageBox.Show("Well, seems like you got everything under control! Enjoy AssistantComputerControl!", "All done! | " + messageBoxTitle);
                        }
                    } else {
                        var customSettings = MessageBox.Show("Alrighty. If you don't want to use Dropbox, you can choose a custom path in the settings, " +
                            "press 'yes' to open the settings, and pick a custom folder - this is advanced, should only be done if you know exactly what you're doing. " +
                            "Otherwise Dropbox is the best option, and what this software is currently supporting by default.\nGo advanced?",
                            "One option left... | " + messageBoxTitle, MessageBoxButtons.YesNo);
                        if (customSettings == DialogResult.Yes) {
                            //Show settings
                        } else {
                            MessageBox.Show("OK. You can always re-open ACC if you wish to set up ACC another time :)", "Maybe another time | " + messageBoxTitle);
                        }
                    }
                } else {
                    string dropboxACCpath = dropboxFolder + @"\AssistantComputerControl";
                    if (!Directory.Exists(dropboxACCpath)) {
                        DirectoryInfo di = Directory.CreateDirectory(dropboxACCpath);
                        di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

                        Console.WriteLine("EYY");
                    }
                    path = dropboxACCpath;
                }
            }

            return path;
        }

        public static void DoDebug(string str) {
            File.AppendAllText(logFilePath, DateTime.Now.ToString() + ": " + str + Environment.NewLine);
            if (debug) {
                Console.WriteLine(str);
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
        static private void CreateFirstTimeFile() {
            if (!File.Exists(Path.Combine(dataFolderLocation, "first_time.txt"))) {
                File.Create(Path.Combine(dataFolderLocation, "first_time.txt")).Close();
                File.SetAttributes(Path.Combine(dataFolderLocation, "first_time.txt"), FileAttributes.Hidden);
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
