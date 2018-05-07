using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;
using System.Net;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Runtime.InteropServices;

namespace AssistantComputerControl {
    class MainProgram {
        static public string softwareVersion = "0.3.1";
        static public bool debug = true,
            unmuteVolumeChange = true,
            isPerformingAction = false;
        static private bool checkForUpdates = true;
        private static SysTrayIcon sysIcon = new SysTrayIcon();

        static public string currentLocationFull = Assembly.GetEntryAssembly().Location,
            currentLocation = Path.GetDirectoryName(currentLocationFull),
            dataFolderLocation = Path.Combine(currentLocation, "ACC_Data"),
            shortcutLocation = Path.Combine(currentLocation, "shortcuts"),
            actionFilePath = "",
            actionFileExtension = "txt",
            configFilePath = String.Concat(Assembly.GetEntryAssembly().Location, ".config"),
            logFilePath = Path.Combine(dataFolderLocation, "log.txt"),
            errorMessage = "",
            startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup),
            messageBoxTitle = "AssistantComputerControl",
            startShortcutName = "AssistantComputerControl.url";
        static public int fileEditedSecondMargin = 30;

        public static void DoDebug(string str) {
            File.AppendAllText(logFilePath, DateTime.Now.ToString() + ": " + str + Environment.NewLine);
            if (debug) {
                Console.WriteLine(str);
            }
        }

        private static void ConfigSetup() {
            DoDebug("Checking if config exists...");
            if (!File.Exists(configFilePath)) {
                DoDebug("Config doesn't exist - creating config...");
                //Creating config file & adding content
                CreateSetConfig();

                if (!File.Exists(configFilePath)) {
                    //If the file was somehow not created...
                    DoDebug("ERROR: The file config file was not created");
                }
            } else {
                DoDebug("Config exists (" + configFilePath + ")");

                if (new FileInfo(configFilePath).Length == 0) {
                    DoDebug("Config file is empty. Setting content...");
                    CreateSetConfig();
                } else {
                    if (ConfigValidator.config("config_version") != softwareVersion) {
                        //Config is different version than the software version
                        DoDebug("Config is different version than software");

                        //Get and save config values
                        Dictionary<string, string> configValues = ConfigValidator.getValues();
                        //Set config to the newest version
                        File.WriteAllText(configFilePath, string.Empty);
                        CreateSetConfig();
                        //Set config values back to old ones
                        ConfigValidator.WriteKey(configValues);
                    }
                }
            }

            //Setting variables to config values...
            ConfigValidator validator = new ConfigValidator();

            var originalActionFilePath = actionFilePath;
            validator.Validate("ActionFilePath", ref actionFilePath);

            FileInfo fi = null;
            try {
                fi = new FileInfo(actionFilePath);
            } catch (ArgumentException) { } catch (PathTooLongException) { } catch (NotSupportedException) { }
            if (ReferenceEquals(fi, null)) {
                //File name is not valid
                DoDebug("Config actionFile directory not found, setting back to default");
                actionFilePath = originalActionFilePath;
            }

            validator.Validate("ActionFileExtension", ref actionFileExtension);
            if (actionFileExtension == "")
                actionFileExtension = "txt";

            validator.Validate("FileEditedMargin", ref fileEditedSecondMargin);
            validator.Validate("CheckForUpdates", ref checkForUpdates);
            validator.Validate("UnmuteOnVolumeChange", ref unmuteVolumeChange);
        }

        private static void CreateSetConfig() {
            string webConfig = (new WebClient()).DownloadString("http://gh.albe.pw/acc/configs/v0.3/acc.exe.config");
            using (var tw = new StreamWriter(configFilePath, true)) {
                tw.WriteLine(webConfig);
                tw.Close();
            }
        }

        static public void ClearFile(string filePath) {
            DoDebug("Clearing file");
            if (File.Exists(filePath)) {
                while (ActionChecker.fileInUse(filePath)) ;
                File.Delete(filePath);
                DoDebug("Action-file deleted");
            } else {
                DoDebug("No file to delete");
            }
        }

        static public void CreateStartupLink() {
            if (Directory.Exists(startupFolder)) {
                using (StreamWriter writer = new StreamWriter(Path.Combine(startupFolder, startShortcutName))) {
                    string app = Assembly.GetExecutingAssembly().Location;
                    writer.WriteLine("[InternetShortcut]");
                    writer.WriteLine("URL=file:///" + app);
                    writer.WriteLine("IconIndex=0");
                    string icon = app.Replace('\\', '/');
                    writer.WriteLine("IconFile=" + icon);
                    writer.Flush();
                }
            }
        }

        static private void SetupDataFolder() {
            if(!Directory.Exists(dataFolderLocation)) {
                Directory.CreateDirectory(dataFolderLocation);
            }
        }
        static private void CreateFirstTimeFile() {
            if (!File.Exists(Path.Combine(dataFolderLocation, "first_time.txt"))) {
                File.Create(Path.Combine(dataFolderLocation, "first_time.txt")).Close();
                File.SetAttributes(Path.Combine(dataFolderLocation, "first_time.txt"), FileAttributes.Hidden);
            }
        }

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
            ConfigSetup();

            if(checkForUpdates) {
                ACC_Updater updater = new ACC_Updater();
                updater.Check();
            }
            if(File.Exists(Path.Combine(dataFolderLocation, "updated.txt"))) {
                string installerPath = File.ReadAllText(Path.Combine(dataFolderLocation, "updated.txt"));
                if(installerPath != String.Empty) {
                    if(File.Exists(installerPath)) {
                        File.Delete(installerPath);
                    }
                }
                File.Delete(Path.Combine(dataFolderLocation, "updated.txt"));
                MessageBox.Show("ACC has been updated to version v" + softwareVersion + "!", "Updated | " + messageBoxTitle, MessageBoxButtons.OK);
            }

            //On console close: hide NotifyIcon
            Application.ApplicationExit += new EventHandler(OnApplicationExit);
            handler = new ConsoleEventDelegate(ConsoleEventCallback);
            SetConsoleCtrlHandler(handler, true);

            //If it's the first time:
            if (!File.Exists(Path.Combine(dataFolderLocation, "first_time.txt"))) {
                if (!File.Exists(@startupFolder + @"\" + @startShortcutName)) {
                    DialogResult dialogResult = MessageBox.Show("Thanks for using " + messageBoxTitle + "! Do you want this software to automatically open when Windows starts (recommended)? Click \"Yes\"", "Open on startup? | " + messageBoxTitle, MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes) {
                        CreateStartupLink();
                        MessageBox.Show("Good choice! ACC is now fully operational, happy computer-controlling!", "Wuu! | " + messageBoxTitle + "");
                    } else if (dialogResult == DialogResult.No) {
                        MessageBox.Show("Alrighty. If you regret and want ACC to open automatically you always right-click on " + messageBoxTitle + " in the tray and click \"Open on startup\"!", "Aww | " + messageBoxTitle + "");
                    }
                }
                CreateFirstTimeFile();
            }

            //Create shortcut folder if doesn't exist
            if(!Directory.Exists(shortcutLocation)) {
                Directory.CreateDirectory(shortcutLocation);

                //Create example-file
                using (StreamWriter sw = File.CreateText(Path.Combine(shortcutLocation, @"example.txt"))) {
                    sw.WriteLine("This is an example file.");
                    sw.WriteLine("If you haven't already, make your assistant open this file!");
                }
            }

            //If a startup link is (still) not created, add trayMenu item for it
            if (!File.Exists(Path.Combine(startupFolder, startShortcutName))) {
                sysIcon.AddOpenOnStartupMenu();
            }

            foreach (string file in Directory.GetFiles(currentLocation, "*." + actionFileExtension)) {
                ClearFile(file);
            }

            /* WATCHER */
            FileSystemWatcher watcher = new FileSystemWatcher();
            string checkPath = actionFilePath;
            watcher.Path = (String.IsNullOrEmpty(checkPath) ? currentLocation : (Directory.Exists(checkPath) ? checkPath : currentLocation));
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = "*.txt";
            watcher.Changed += new FileSystemEventHandler(ActionChecker.FileFound);
            watcher.EnableRaisingEvents = true;
            /* END WATCHER */

            DoDebug("\n[" + messageBoxTitle + "] Initiated. \nCurrent location: " + currentLocation);
            DoDebug("Listening for actions to execute...");

            Application.EnableVisualStyles();
            Application.Run(sysIcon);
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
    }
}
