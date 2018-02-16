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
        static public string softwareVersion = "0.3";
        static public bool debug = false,
            unmute_volume_change = true,
            is_performing_action = false;
        static private bool checkForUpdates = true;
        private static SysTrayIcon sysIcon = new SysTrayIcon();

        static public string currentLocationFull = Assembly.GetEntryAssembly().Location,
            currentLocation = Path.GetDirectoryName(currentLocationFull),
            dataFolderLocation = Path.Combine(currentLocation, "ACC_Data"),
            shortcutLocation = Path.Combine(currentLocation, "shortcuts"),
            actionFilePath = Path.Combine(currentLocation, "computerAction.txt"),
            configFilePath = String.Concat(Assembly.GetEntryAssembly().Location, ".config"),
            errorMessage = "",
            startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup),
            messageBoxTitle = "AssistantComputerControl",
            startShortcutName = "AssistantComputerControl.url";
        static public int fileEditedSecondMargin = 30;

        public static void doDebug(string str) {
            if (debug) {
                Console.WriteLine(str);
            }
        }

        private static void configSetup() {
            doDebug("Checking if config exists...");
            if (!File.Exists(configFilePath)) {
                doDebug("Config doesn't exist - creating config...");
                //Creating config file & adding content
                createSetConfig();

                if (!File.Exists(configFilePath)) {
                    //If the file was somehow not created...
                    doDebug("ERROR: The file config file was not created");
                }
            } else {
                doDebug("Config exists (" + configFilePath + ")");

                if (new FileInfo(configFilePath).Length == 0) {
                    doDebug("Config file is empty. Setting content...");
                    createSetConfig();
                } else {
                    if (configValidator.config("config_version") != softwareVersion) {
                        //Config is different version than the software version
                        doDebug("Config is different version than software");

                        //Get and save config values
                        Dictionary<string, string> config_values = configValidator.getValues();
                        //Set config to the newest version
                        File.WriteAllText(configFilePath, string.Empty);
                        createSetConfig();
                        //Set config values back to old ones
                        configValidator.writeKey(config_values);
                    }
                }
            }

            //Setting variables to config values...
            configValidator validator = new configValidator();

            var originalActionFilePath = actionFilePath;
            validator.validate("ActionFilePath", ref actionFilePath);

            FileInfo fi = null;
            try {
                fi = new FileInfo(actionFilePath);
            } catch (ArgumentException) { } catch (PathTooLongException) { } catch (NotSupportedException) { }
            if (ReferenceEquals(fi, null)) {
                //File name is not valid
                doDebug("Config actionFile directory not found, setting back to default");
                actionFilePath = originalActionFilePath;
            }

            validator.validate("FileEditedMargin", ref fileEditedSecondMargin);
            validator.validate("CheckForUpdates", ref checkForUpdates);
            validator.validate("UnmuteOnVolumeChange", ref unmute_volume_change);
        }

        private static void createSetConfig() {
            string webConfig = (new WebClient()).DownloadString("https://gh.albe.pw/acc/configs/v0.3/acc.exe.config");
            using (var tw = new StreamWriter(configFilePath, true)) {
                tw.WriteLine(webConfig);
                tw.Close();
            }
        }

        static public void clearFile() {
            if (File.Exists(actionFilePath)) {
                File.Delete(actionFilePath);
                doDebug("Action-file deleted");
            } else {
                doDebug("Action-file doesn't exist, nothing to delete");
            }
        }

        static public void createStartupLink() {
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

        static private void setupDataFolder() {
            if(!Directory.Exists(dataFolderLocation)) {
                Directory.CreateDirectory(dataFolderLocation);
            }
        }
        static private void createFirstTimeFile() {
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
                MessageBox.Show("ACC is already running.", "Already running | " + messageBoxTitle + "");
                Process.GetCurrentProcess().Kill();
            }

            setupDataFolder();
            configSetup();

            if(checkForUpdates) {
                ACC_Updater updater = new ACC_Updater();
                updater.check();
            }
            if(File.Exists(Path.Combine(dataFolderLocation, "updated.txt"))) {
                string installer_path = File.ReadAllText(Path.Combine(dataFolderLocation, "updated.txt"));
                if(installer_path != String.Empty) {
                    if(File.Exists(installer_path)) {
                        File.Delete(installer_path);
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
                        createStartupLink();
                        MessageBox.Show("Good choice! ACC is now fully operational, happy computer-controlling!", "Wuu! | " + messageBoxTitle + "");
                    } else if (dialogResult == DialogResult.No) {
                        MessageBox.Show("Alrighty. If you regret and want ACC to open automatically you always right-click on " + messageBoxTitle + " in the tray and click \"Open on startup\"!", "Aww | " + messageBoxTitle + "");
                    }
                }
                createFirstTimeFile();
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
                sysIcon.addOpenOnStartupMenu();
            }
            clearFile();
          
            /* WATCHER */
            FileSystemWatcher watcher = new FileSystemWatcher();
            string checkPath = Path.GetDirectoryName(actionFilePath);
            watcher.Path = (String.IsNullOrEmpty(checkPath) ? currentLocation : (Directory.Exists(checkPath) ? checkPath : currentLocation));

            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            //File to look for
            watcher.Filter = actionFilePath;
            
            //TODO: Check for dublicates
            //watcher.Filter += Path.GetFileNameWithoutExtension(actionFilePath) + " (1)" + Path.GetExtension(actionFilePath);

            //watcher.Created += new FileSystemEventHandler(actionChecker.fileFound);
            watcher.Changed += new FileSystemEventHandler(actionChecker.fileFound);

            //Begin watching
            watcher.EnableRaisingEvents = true;
            /* END WATCHER */

            doDebug("\n[" + messageBoxTitle + "] Initiated.\nCurrent location: " + currentLocation + "\naction file location: " + actionFilePath + "\n");

            Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(sysIcon);
        }

        //On application exit event
        private static void OnApplicationExit(object sender, EventArgs e) {
            sysIcon.hideIcon();
        }
        private static bool ConsoleEventCallback(int eventType) {
            if (eventType == 2) {
                //Closing console
                sysIcon.hideIcon();
            }
            return false;
        }
        private static ConsoleEventDelegate handler;
        private delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
        public static void exit() {
            sysIcon.hideIcon();
            Application.Exit();
            //Application.Exit doesn't close the application on update...?
            Environment.Exit(1);
        }
        private static void trayExit(object sender, EventArgs e) {
            exit();
        }
        private static void trayOpenHelp(object sender, EventArgs e) {
            Process.Start("https://github.com/AlbertMN/AssistantComputerControl/wiki");
        }
        private static void trayOpenFolder(object sender, EventArgs e) {
            Process.Start(Directory.GetCurrentDirectory());
        }

        public static void trayCreateStartupLink(object sender, EventArgs e) {
            DialogResult dialogResult = MessageBox.Show("Do you want this software to automatically open when Windows starts (recommended)? Click \"Yes\"", "Open on startup? | " + messageBoxTitle, MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes) {
                createStartupLink();
                MessageBox.Show("Good choice! ACC will now start with Windows!", "Wuu! | " + messageBoxTitle + "");
            } else if (dialogResult == DialogResult.No) {
                MessageBox.Show("Alrighty. If you regret and want ACC to open automatically you always right-click on " + messageBoxTitle + " in the tray and click \"Open on startup\"!", "Aww | " + messageBoxTitle);
            }
        }
    }
}
