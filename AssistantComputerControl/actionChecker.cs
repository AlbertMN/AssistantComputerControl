using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace AssistantComputerControl {
    class ActionChecker {
        private static string successMessage = "";
        private static List<string> inProgressFileNames = new List<string>();

        //Logout
        [DllImport("user32.dll", SetLastError = true)]
        static extern int ExitWindowsEx(uint uFlags, uint dwReason);

        //Lock
        [DllImport("user32.dll")]
        public static extern bool LockWorkStation();

        //Turn off monitor
        const int WM_SYSCOMMAND = 0x112;
        const int SC_MONITORPOWER = 0xF170;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        //Music-control
        public const int KEYEVENTF_EXTENTEDKEY = 1;
        public const int KEYEVENTF_KEYUP = 0;
        public const int VK_MEDIA_NEXT_TRACK = 0xB0; //Next track
        public const int VK_MEDIA_PLAY_PAUSE = 0xB3; //Play/pause
        public const int VK_MEDIA_PREV_TRACK = 0xB1; //Previous track
        public const int VK_RCONTROL = 0xA3; //Right Control key code

        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        private static bool RequireParameter(string param) {
            if(param != null) {
                if(param != "") {
                    return true;
                } else {
                    MainProgram.DoDebug("ERROR: Parameter is empty");
                    MainProgram.errorMessage = "Parameter is empty";
                }
            } else {
                MainProgram.DoDebug("ERROR: Parameter not set");
                MainProgram.errorMessage = "Parameter not set";
            }
            return false;
        }
        public static bool FileInUse(string file) {
            if(File.Exists(file)) {
                try {
                    using (Stream stream = new FileStream(file, FileMode.Open)) {
                        // File/Stream manipulating code here
                        return false;
                    }
                } catch {
                    MainProgram.DoDebug("File is in use, retrying");
                    Thread.Sleep(50);
                    return true;
                }
            }
            return false;
        }

        private static void PressKey(char c) {
            try {
                SendKeys.SendWait(c.ToString());
            } catch (Exception e) {
                MainProgram.DoDebug("Failed to press key \"" + c.ToString() + "\", exception; " + e);
            }
        }

        static public void FileFound(object source, FileSystemEventArgs e) {
            ProcessFile(e.FullPath);
        }

        private static DateTime lastActionModified;

        static public void ProcessFile(string file) {
            string originalFileName = file;

            if (!File.Exists(file)/* || inProgressFileNames.Contains(file) || file.Contains("in_progress")*/) {
                return;
            }
            DateTime lastModified = File.GetCreationTime(file);
            //MainProgram.DoDebug(lastModified.ToString());

            if (lastModified == lastActionModified || (File.GetAttributes(file) & FileAttributes.Hidden) == FileAttributes.Hidden) {
                //If file is hidden or has exact same "last modified" date as last file (possible trying to do the same twice)
                return;
            }
            lastActionModified = lastModified;

            if (lastModified.AddSeconds(Properties.Settings.Default.FileEditedMargin) < DateTime.Now) {
                MainProgram.DoDebug("The file is more than " + Properties.Settings.Default.FileEditedMargin.ToString() + "s old, meaning it won't be executed.");
                MainProgram.DoDebug("File creation time: " + lastModified.ToString());
                MainProgram.DoDebug("Local time: " + DateTime.Now.ToString());
                return;
            }
            inProgressFileNames.Add(originalFileName);

            MainProgram.DoDebug("\n[ -- DOING ACTION(S) -- ]");
            MainProgram.DoDebug(" - " + file);
            MainProgram.DoDebug(" - File exists, checking the content...");

            try {
                Thread.Sleep(200);
                File.SetAttributes(file, FileAttributes.Hidden);
            } catch {
                //
            }

            /*while (FileInUse(file));
            string newFileName = Path.Combine(MainProgram.CheckPath(), "in_progress_" + Guid.NewGuid().ToString("n").Substring(0, 8) + "." + Properties.Settings.Default.ActionFileExtension);
            inProgressFileNames.Add(newFileName);
            try {
                File.Move(file, newFileName);
                MainProgram.DoDebug(" - File moved");
            } catch {
                //File in use
                inProgressFileNames.Remove(file);
                inProgressFileNames.Remove(originalFileName);
                MainProgram.DoDebug(" - Can't move file - used by another process, ignoring");

                try {
                    Thread.Sleep(200);
                    File.SetAttributes(file, FileAttributes.Hidden);
                } catch {
                    //
                }

                return;
            }
            file = newFileName;
            while (File.Exists(originalFileName));
            while (!File.Exists(file)) ;*/

            if (new FileInfo(file).Length != 0) {
                //string line = Regex.Replace(File.ReadAllText(file), @"\t|\n|\r", "");
                string fullContent = Regex.Replace(File.ReadAllText(file), @"\t|\r", "");
                MainProgram.DoDebug(" - Read complete, content: " + fullContent);

                //MainProgram.ClearFile(file);
                //while (File.Exists(file)) ;
                File.SetAttributes(file, FileAttributes.Hidden);

                using (StringReader reader = new StringReader(fullContent)) {
                    string theLine = string.Empty;
                    do {
                        theLine = reader.ReadLine();
                        if (theLine != null) {
                            MainProgram.DoDebug("\n[EXECUTING ACTION]");
                            CheckAction(theLine);
                        }

                    } while (theLine != null);
                }
            } else {
                MainProgram.DoDebug(" - File is empty");
                MainProgram.errorMessage = "No action set (file is empty)";

                //MainProgram.ClearFile(file);
                //while (File.Exists(file)) ;
                File.SetAttributes(file, FileAttributes.Hidden);
            }


            MainProgram.DoDebug("[ -- DONE -- ]");
            inProgressFileNames.Remove(file);
            inProgressFileNames.Remove(originalFileName);

            if (MainProgram.errorMessage.Length != 0) {
                MessageBox.Show(MainProgram.errorMessage, "Error | " + MainProgram.messageBoxTitle);
                MainProgram.errorMessage = "";
            }
        }

        private static void CheckAction(string theLine) {
            string action = theLine
                , parameter = null
                , fullContent = null;

            action = theLine;
            fullContent = theLine;
            string assistantParam = null;

            //Whether it's Google Assistant or Amazon Alexa (included in the default IFTTT applets)
            if (theLine.Contains("[") && theLine.Contains("]")) {
                action = theLine.Split('[')[0];
                assistantParam = theLine.Split('[')[1];
                assistantParam = assistantParam.Split(']')[0];

                MainProgram.DoDebug(" - Executing using; " + assistantParam);
            }

            if (action.Contains(":")) {
                //Contains a parameter
                string[] splitAction = action.Split(':');
                parameter = splitAction[1];
                action = splitAction[0];
                if (splitAction.Length > 1) {
                    int i = 0;
                    foreach (string moreParam in splitAction) {
                        if (i != 0 && i != 1) {
                            parameter += ":" + moreParam;
                        }
                        i++;
                    }
                }
                if (parameter == "") parameter = null;
            }

            if (MainProgram.testingAction)
                MainProgram.DoDebug(" - Test went through: " + action);

            MainProgram.DoDebug(" - Action: " + action);
            MainProgram.DoDebug(" - Parameter: " + parameter);
            MainProgram.DoDebug(" - Full line: " + theLine);
            ExecuteAction(action, theLine, parameter, assistantParam);
        }

        private static string[] GetSecondaryParam(string param) {
            if (param.Contains("{") && param.Contains("}")) {
                string[] toReturn = param.Split('{');
                int i = 0;
                foreach (string s in toReturn) {
                    toReturn[i] = s.Replace("}", "");
                    i++;
                }
                return toReturn;
            }
            return new string[1] { param };
        }




        public const int KEYEVENTF_EXTENDEDKEY = 0x0001; //Key down flag
        public const int VK_LCONTROL = 0xA2; //Left Control key code
        public const int A = 0x41; //A key code
        public const int C = 0x43; //C key code

        public static void PressKeys() {
            // Hold Control down and press A
            keybd_event(VK_LCONTROL, 0, KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(A, 0, KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(A, 0, KEYEVENTF_KEYUP, 0);
            keybd_event(VK_LCONTROL, 0, KEYEVENTF_KEYUP, 0);

            // Hold Control down and press C
            keybd_event(VK_LCONTROL, 0, KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(C, 0, KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(C, 0, KEYEVENTF_KEYUP, 0);
            keybd_event(VK_LCONTROL, 0, KEYEVENTF_KEYUP, 0);
        }



        private static void ExecuteAction(string action, string line, string parameter, string assistantParam) {
            int? actionNumber = null;
            switch (action.ToLower()) {
                case "shutdown":
                    //Shuts down the computer
                    string shutdownParameters = "/s /t 0";
                    if (parameter != null) {
                        if (parameter == "abort") {
                            shutdownParameters = "abort";
                        } else {
                            if (parameter.Contains("/t")) {
                                shutdownParameters = !parameter.Contains("/s") ? "/s " : "" + parameter;
                            } else {
                                shutdownParameters = !parameter.Contains("/s") ? "/s " : "" + parameter + " /t 0";
                            }
                        }
                    }

                    if (MainProgram.testingAction) {
                        successMessage = "Simulated shutdown";
                    } else {
                        if (shutdownParameters != "abort") {
                            MainProgram.DoDebug("Shutting down computer...");
                            successMessage = "Shutting down";
                            Process.Start("shutdown", shutdownParameters);
                        } else {
                            MainProgram.DoDebug("Cancelling shutdown...");
                            Process.Start("shutdown", "/a");
                            successMessage = "Aborted shutdown";
                        }
                    }
                    break;
                case "restart":
                    //Restart the computer
                    string restartParameters = "/r /t 0";
                    if (parameter != null) {
                        if (parameter == "abort") {
                            restartParameters = "abort";
                        } else {
                            if (parameter.Contains("/t")) {
                                restartParameters = !parameter.Contains("/r") ? "/s " : "" + parameter;
                            } else {
                                restartParameters = !parameter.Contains("/r") ? "/s " : "" + parameter + " /t 0";
                            }
                        }
                    }
                    if (MainProgram.testingAction) {
                        successMessage = "Simulated restart";
                    } else {
                        if (restartParameters != "abort") {
                            MainProgram.DoDebug("Restarting computer...");
                            successMessage = "Restarting";
                            Process.Start("shutdown", restartParameters);
                        } else {
                            MainProgram.DoDebug("Cancelling restart...");
                            Process.Start("shutdown", "/a");
                            successMessage = "Aborted restart";
                        }
                    }
                    break;
                case "sleep":
                    //Puts computer to sleep
                    if (parameter == null) {
                        if (!MainProgram.testingAction) {
                            MainProgram.DoDebug("Sleeping computer...");
                            Application.SetSuspendState(PowerState.Suspend, true, true);
                        }
                    } else {
                        bool doForce = true;
                        switch (parameter) {
                            case "true":
                                doForce = true;
                                break;
                            case "false":
                                doForce = false;
                                break;
                            default:
                                MainProgram.DoDebug("ERROR: Parameter (" + parameter + ") is invalid for \"" + action + "\". Accepted parameters: \"true\" and \"false\"");
                                MainProgram.errorMessage = "Parameter \"" + parameter + "\" is invalid for the \"" + action + "\" action. Accepted parameters: \"true\" and \"false\")";
                                break;
                        }
                        if (!MainProgram.testingAction) {
                            MainProgram.DoDebug("Sleeping computer...");
                            Application.SetSuspendState(PowerState.Suspend, doForce, true);
                        }
                    }

                    if (MainProgram.testingAction) {
                        successMessage = "Simulated PC sleep";
                    } else {
                        successMessage = "Put computer to sleep";
                    }
                    break;
                case "hibernate":
                    //Hibernates computer
                    actionNumber = 12;
                    if (parameter == null) {
                        if (!MainProgram.testingAction) {
                            MainProgram.DoDebug("Hibernating computer...");
                            Application.SetSuspendState(PowerState.Hibernate, true, true);
                        }
                    } else {
                        bool doForce = true;
                        switch (parameter) {
                            case "true":
                                doForce = true;
                                break;
                            case "false":
                                doForce = false;
                                break;
                            default:
                                MainProgram.DoDebug("ERROR: Parameter (" + parameter + ") is invalid for \"" + action + "\". Accepted parameters: \"true\" and \"false\"");
                                MainProgram.errorMessage = "Parameter \"" + parameter + "\" is invalid for the \"" + action + "\" action. Accepted parameters: \"true\" and \"false\")";
                                break;
                        }
                        if (!MainProgram.testingAction) {
                            MainProgram.DoDebug("Hibernating computer...");
                            Application.SetSuspendState(PowerState.Hibernate, doForce, true);
                        }
                    }

                    if (MainProgram.testingAction) {
                        successMessage = "Simulated PC hibernate";
                    } else {
                        successMessage = "Put computer in hibernation";
                    }
                    break;
                case "logout":
                    //Logs out of the current user
                    if (MainProgram.testingAction) {
                        successMessage = "Simulated logout";
                    } else {
                        MainProgram.DoDebug("Logging out of user...");
                        successMessage = "Logged out of user";
                        ExitWindowsEx(0, 0);
                    }
                    break;
                case "lock":
                    //Lock computer
                    if (MainProgram.testingAction) {
                        successMessage = "Simulated PC lock";
                    } else {
                        MainProgram.DoDebug("Locking computer...");
                        LockWorkStation();
                        successMessage = "Locked pc";
                    }
                    break;
                case "mute":
                    //Mutes windows
                    //Parameter optional (true/false)
                    bool doMute = true;

                    if (parameter == null) {
                        //No parameter - toggle
                        doMute = !AudioManager.GetMasterVolumeMute();
                    } else {
                        //Parameter set;
                        switch (parameter) {
                            case "true":
                                doMute = true;
                                break;
                            case "false":
                                doMute = false;
                                break;
                            default:
                                MainProgram.DoDebug("ERROR: Parameter (" + parameter + ") is invalid for \"" + action + "\". Accepted parameters: \"true\" and \"false\"");
                                MainProgram.errorMessage = "Parameter \"" + parameter + "\" is invalid for the \"" + action + "\" action. Accepted parameters: \"true\" and \"false\")";
                                break;
                        }
                    }

                    if (MainProgram.testingAction) {
                        successMessage = "Simulated PC" + (doMute ? "muted " : "unmute");
                    } else {
                        AudioManager.SetMasterVolumeMute(doMute);
                        successMessage = (doMute ? "Muted " : "Unmuted") + " pc";
                    }
                    break;
                case "set_volume":
                    //Sets volume to a specific percent
                    //Requires parameter (percent, int)
                    if (RequireParameter(parameter)) {
                        if (double.TryParse(parameter, out double volumeLevel)) {
                            if (volumeLevel >= 0 && volumeLevel <= 100) {
                                if (!MainProgram.testingAction) {
                                    if (Properties.Settings.Default.UnmuteOnVolumeChange) {
                                        AudioManager.SetMasterVolumeMute(false);
                                    }
                                    AudioManager.SetMasterVolume((float)volumeLevel);
                                }
                                if (!MainProgram.testingAction) {
                                    if ((int)AudioManager.GetMasterVolume() != (int)volumeLevel) {
                                        //Something went wrong... Audio not set to parameter-level
                                        MainProgram.DoDebug("ERROR: Volume was not set properly. Master volume is " + AudioManager.GetMasterVolume() + ", not " + volumeLevel);
                                        MainProgram.errorMessage = "Something went wrong when setting the volume";
                                    } else {
                                        successMessage = "Set volume to " + volumeLevel + "%";
                                    }
                                } else {
                                    successMessage = "Simulated setting system volume to " + volumeLevel + "%";
                                }
                            } else {
                                MainProgram.DoDebug("ERROR: Parameter is an invalid number, range; 0-100 (" + volumeLevel + ")");
                                MainProgram.errorMessage = "Can't set volume to " + volumeLevel + "%, has to be a number from 0-100";
                            }
                        } else {
                            MainProgram.DoDebug("ERROR: Parameter (" + parameter + ") not convertable to double");
                            MainProgram.errorMessage = "Not a valid parameter (has to be a number)";
                        }
                    }
                    break;
                case "music":
                    if (RequireParameter(parameter)) {
                        switch (parameter) {
                            case "previous":
                                actionNumber = 8;

                                if (MainProgram.testingAction) {
                                    successMessage = "MUSIC: Simulated going to previous track";
                                } else {
                                    keybd_event(VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_EXTENTEDKEY, 0);
                                    successMessage = "MUSIC: Skipped song";
                                }
                                break;
                            case "previousx2":
                                actionNumber = 8;
                                if (MainProgram.testingAction) {
                                    successMessage = "MUSIC: Simulated double-previous track";
                                } else {
                                    keybd_event(VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_EXTENTEDKEY, 0);
                                    Thread.Sleep(100);
                                    keybd_event(VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_EXTENTEDKEY, 0);
                                    successMessage = "MUSIC: Skipped song (x2)";
                                }
                                break;
                            case "next":
                                actionNumber = 10;

                                if (MainProgram.testingAction) {
                                    successMessage = "MUSIC: Simulated going to next song";
                                } else {
                                    keybd_event(VK_MEDIA_NEXT_TRACK, 0, KEYEVENTF_EXTENTEDKEY, 0);
                                    successMessage = "MUSIC: Next song";
                                }
                                break;
                            case "play_pause":
                                actionNumber = 9;

                                if (MainProgram.testingAction) {
                                    successMessage = "MUSIC: Simulated play/pause";
                                } else {
                                    keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENTEDKEY, 0);
                                    successMessage = "MUSIC: Played/Paused";
                                }
                                break;
                            default:
                                MainProgram.DoDebug("ERROR: Unknown parameter");
                                MainProgram.errorMessage = "Unknown parameter \"" + parameter + "\"";
                                break;
                        }
                    }
                    break;
                case "open":
                    if (RequireParameter(parameter)) {
                        string fileLocation = (!parameter.Contains(@":\")) ? Path.Combine(MainProgram.shortcutLocation, parameter) : parameter;

                        if (File.Exists(fileLocation) || Directory.Exists(fileLocation) || Uri.IsWellFormedUriString(fileLocation, UriKind.Absolute)) {
                            if (!MainProgram.testingAction) {
                                Process.Start(fileLocation);
                                successMessage = "OPEN: opened file/url; " + fileLocation;
                            } else {
                                successMessage = "OPEN: simulated opening file; " + fileLocation;
                            }
                        } else {
                            MainProgram.DoDebug("ERROR: file or directory doesn't exist (" + fileLocation + ")");
                            MainProgram.errorMessage = "File or directory doesn't exist (" + fileLocation + ")";
                        }
                    }
                    break;
                case "open_all":
                    if (RequireParameter(parameter)) {
                        string fileLocation = (!parameter.Contains(@":\")) ? Path.Combine(MainProgram.shortcutLocation, parameter) : parameter;

                        if (Directory.Exists(fileLocation) || Uri.IsWellFormedUriString(fileLocation, UriKind.Absolute)) {
                            DirectoryInfo d = new DirectoryInfo(fileLocation);
                            int x = 0;
                            foreach (var dirFile in d.GetFiles()) {
                                if (!MainProgram.testingAction)
                                    Process.Start(dirFile.FullName);
                                x++;
                            }

                            if (!MainProgram.testingAction) {
                                successMessage = "OPEN: opened " + x + " files in; " + fileLocation;
                            } else {
                                successMessage = "OPEN: simulated opening " + x + " files in; " + fileLocation;
                            }
                        } else {
                            MainProgram.DoDebug("ERROR: directory doesn't exist (" + fileLocation + ")");
                            MainProgram.errorMessage = "Directory doesn't exist (" + fileLocation + ")";
                        }
                    }
                    break;
                case "die":
                    //Exit ACC
                    if (!MainProgram.testingAction) {
                        successMessage = "Shutting down ACC";
                        Application.Exit();
                    } else {
                        successMessage = "Simulated shutting down ACC";
                    }
                    break;
                case "monitors_off":
                    if (!MainProgram.testingAction) {
                        Form f = new Form();
                        SendMessage(f.Handle, WM_SYSCOMMAND, (IntPtr)SC_MONITORPOWER, (IntPtr)2);
                        f.Close();
                        successMessage = "Turned monitors off";
                    } else {
                        successMessage = "Simulated turning monitors off";
                    }
                    break;
                /*case "keypress":
                    if (RequireParameter(parameter)) {
                        if (parameter.Length > 1) {
                            if (!MainProgram.testingAction) {
                                successMessage = "Pressed \"" + parameter + "\"";
                                PressKey((char)parameter[0]);
                            } else {
                                successMessage = "Simulated press of \"" + parameter + "\"";
                            }
                        } else {
                            MainProgram.DoDebug("ERROR: Parameter can only be one character long");
                            MainProgram.errorMessage = "(Keypress) Parameter can only be one character long";
                        }
                    }
                    break;*/
                case "write_out":
                    if (RequireParameter(parameter)) {
                        int i = 0;
                        string writtenString = "";
                        foreach (char c in parameter) {
                            char toWrite = (i == 0 && Properties.Settings.Default.WriteOutUCFirst ? Char.ToUpper(c) : c);
                            if (!MainProgram.testingAction) PressKey(toWrite);
                            writtenString += toWrite;

                            if (i > line.Length && Properties.Settings.Default.WriteOutDotLast) {
                                if (!MainProgram.testingAction) PressKey('.');
                                writtenString += ".";
                            }
                            i++;
                        }
                        if (!MainProgram.testingAction) {
                            successMessage = "Wrote \"" + writtenString + "\"";
                        } else {
                            successMessage = "Simulated writing \"" + writtenString + "\"";
                        }
                    }
                    break;
                /*case "key_shortcut": //TODO - version 1.3(?)
                    //Currently just keeps holding CTRL down, fucking everything up
                    //Do "testing" check
                    if (RequireParameter(parameter)) {
                        parameter = parameter.Replace("ctrl", "%");

                        /*foreach (char c in parameter) {
                            if (c == '%') {
                                keybd_event(VK_RCONTROL, 0, KEYEVENTF_EXTENTEDKEY, 0);
                                keybd_event(VK_RCONTROL, 0, KEYEVENTF_KEYUP, 0);
                            } else {
                                PressKey(c);
                            }
                        }*/
                    /*}
                    PressKeys();
                    break;*/
                case "create_file":
                    if (RequireParameter(parameter)) {
                        string fileLocation = parameter;
                        if (!File.Exists(fileLocation)) {
                            string parentPath = Path.GetDirectoryName(fileLocation);
                            if (Directory.Exists(parentPath)) {
                                bool succeeded = true;
                                try {
                                    string toDelete;
                                    //Is file
                                    if (MainProgram.testingAction) {
                                        //Create a test-file and delete it to test if has permission
                                        toDelete = Path.Combine(parentPath, "acc_testfile.txt");
                                        var myFile = File.Create(toDelete);
                                        myFile.Close();
                                        while (!File.Exists(toDelete)) ;
                                        while (!FileInUse(toDelete)) ;
                                        File.Delete(toDelete);
                                    } else {
                                        //Actually create file
                                        var myFile = File.Create(fileLocation);
                                        myFile.Close();
                                    }
                                } catch (Exception exc) {
                                    succeeded = false;
                                    MainProgram.DoDebug(exc.Message);
                                    MainProgram.errorMessage = "Couldn't create file - folder might be locked. Try running ACC as administrator.";
                                }

                                if (succeeded) {
                                    if (!MainProgram.testingAction) {
                                        successMessage = "Created file at " + fileLocation;
                                    } else {
                                        successMessage = "Simulated creating file at " + fileLocation;
                                    }
                                }
                            } else {
                                MainProgram.errorMessage = "File parent folder doesn't exist (" + parentPath + ")";
                                MainProgram.DoDebug("File parent folder doesn't exist (" + parentPath + ")");
                            }
                        } else {
                            MainProgram.errorMessage = "File already exists";
                            MainProgram.DoDebug("File already exists");
                        }
                    }
                    break;
                case "delete_file":
                    if (RequireParameter(parameter)) {
                        string fileLocation = parameter;
                        if (File.Exists(fileLocation) || Directory.Exists(fileLocation)) {
                            FileAttributes attr = File.GetAttributes(fileLocation);
                            bool succeeded = true;
                            MainProgram.DoDebug("Deleting file/folder at " + fileLocation);

                            try {
                                string toDelete;
                                if (attr.HasFlag(FileAttributes.Directory)) {
                                    //Is folder
                                    MainProgram.DoDebug("Deleting folder...");
                                    DirectoryInfo d = new DirectoryInfo(fileLocation);
                                    bool doDelete = true;
                                    if (d.GetFiles().Length > Properties.Settings.Default.MaxDeleteFiles && Properties.Settings.Default.WarnWhenDeletingManyFiles) {
                                        //Has more than x files - do warning
                                        DialogResult dialogResult = MessageBox.Show("You're about to delete more than " + Properties.Settings.Default.MaxDeleteFiles.ToString() + " files at " + fileLocation + " - are you sure you wish to proceed?",
                                            "Are you sure?", MessageBoxButtons.YesNo);
                                        if (dialogResult == DialogResult.Yes) {

                                        } else if (dialogResult == DialogResult.No) {
                                            doDelete = false;
                                        }
                                    }

                                    if (doDelete) {
                                        if (MainProgram.testingAction) {
                                            //Make test-folder and delete it to test if has permission
                                            toDelete = Path.Combine(Directory.GetParent(fileLocation).FullName, "acc_testfolder");
                                            Directory.CreateDirectory(toDelete);
                                            Directory.Delete(toDelete);
                                        } else {
                                            //Actually delete folder
                                            Directory.Delete(fileLocation);
                                            MainProgram.DoDebug("Deleted directory at " + fileLocation);
                                        }
                                    } else {
                                        MainProgram.errorMessage = "";
                                    }
                                } else {
                                    //Is file
                                    if (MainProgram.testingAction) {
                                        //Make test-file and delete it to test if has permission
                                        MainProgram.DoDebug("(Fake) Deleting file...");

                                        toDelete = Path.Combine(fileLocation, "acc_testfile.txt");
                                        File.Create(toDelete);
                                        File.Delete(toDelete);
                                    } else {
                                        //Actually delete file
                                        MainProgram.DoDebug("Deleting file...");
                                        File.Delete(fileLocation);
                                    }
                                }
                            } catch (Exception exc) {
                                succeeded = false;
                                MainProgram.DoDebug(exc.Message);
                                MainProgram.errorMessage = "Couldn't access file/folder - file might be in use or locked. Try running ACC as administrator.";
                            }

                            if (succeeded) {
                                if (!MainProgram.testingAction) {
                                    successMessage = "Deleted file/folder at " + fileLocation;
                                } else {
                                    successMessage = "Simulated deleting file/folder at " + fileLocation;
                                }
                            }
                        } else {
                            MainProgram.errorMessage = "File or folder doesn't exist";
                            MainProgram.DoDebug("File or folder doesn't exist");
                        }
                    }
                    break;
                case "append_text":
                    if (RequireParameter(parameter)) {
                        string fileLocation = GetSecondaryParam(parameter)[0]
                            , toAppend = GetSecondaryParam(parameter).Length > 1 ? GetSecondaryParam(parameter)[1] : null;

                        MainProgram.DoDebug("Appending \"" + toAppend + "\" to " + fileLocation);

                        if (fileLocation != null && toAppend != null) {
                            if (toAppend != "") {
                                if (File.Exists(fileLocation)) {
                                    string parentPath = Path.GetDirectoryName(fileLocation);
                                    bool succeeded = true;
                                    try {
                                        //Is file
                                        if (MainProgram.testingAction) {
                                            //Write empty string to file to test permission
                                            using (StreamWriter w = File.AppendText(fileLocation)) {
                                                w.Write(String.Empty);
                                            }
                                        } else {
                                            //Actually write to file
                                            using (StreamWriter w = File.AppendText(fileLocation)) {
                                                //string[] lines = toAppend.Split(new string[] { "\n" }, StringSplitOptions.None);
                                                string[] lines = toAppend.Split(new string[] { "\\n" }, StringSplitOptions.None);
                                                MainProgram.DoDebug(lines.Length.ToString());
                                                int i = 0;
                                                foreach (string appendChild in lines) {
                                                    if (i == 0)
                                                        w.Write(appendChild);
                                                    else {
                                                        w.WriteLine(String.Empty);
                                                        w.Write(appendChild);
                                                    }

                                                    i++;
                                                }
                                            }
                                        }
                                    } catch (Exception exc) {
                                        succeeded = false;
                                        MainProgram.DoDebug(exc.Message);
                                        MainProgram.errorMessage = "Couldn't create file - folder might be locked. Try running ACC as administrator.";
                                    }

                                    if (succeeded) {
                                        if (!MainProgram.testingAction) {
                                            successMessage = "Appended \"" + toAppend + "\" to file at " + fileLocation;
                                        } else {
                                            successMessage = "Simulated appending \"" + toAppend + "\" to file at " + fileLocation;
                                        }
                                    }
                                } else {
                                    MainProgram.errorMessage = "File doesn't exists";
                                    MainProgram.DoDebug("File doesn't exists");
                                }
                            } else {
                                MainProgram.errorMessage = "Can't append nothing";
                                MainProgram.DoDebug("Can't append nothing");
                            }
                        } else {
                            MainProgram.errorMessage = "Parameter doesn't contain a string to append";
                            MainProgram.DoDebug("Parameter doesn't contain a string to append");
                        }
                    }
                    break;
                case "message_box":
                    if (RequireParameter(parameter)) {
                        string theMessage = GetSecondaryParam(parameter)[0]
                            , theTitle = (GetSecondaryParam(parameter).Length > 1 ? GetSecondaryParam(parameter)[1] : null);

                        if (MainProgram.testingAction) {
                            successMessage = "Simulated making a message box with the content \"" + theMessage + "\" and " + (theTitle == null ? "no title" : "title \"" + theTitle + "\"");
                        } else {
                            new Thread(() => {
                                Thread.CurrentThread.Priority = ThreadPriority.Highest;
                                MessageBox.Show(theMessage, theTitle ?? "ACC Generated Message Box");
                            }).Start();
                        }
                    }
                    break;
                default:
                    //Unknown action
                    MainProgram.DoDebug("ERROR: Unknown action \"" + action + "\"");
                    MainProgram.errorMessage = "Unknown action \"" + action + "\"";
                    break;
            }
            if (successMessage != "") {
                if (actionNumber != null) {
                    //Has specified number
                    AnalyticsSettings.AddCount((int)actionNumber, assistantParam);
                } else {
                    //YOLO
                    AnalyticsSettings.AddCount(action, assistantParam);
                }

                MainProgram.DoDebug("\nSUCCESS: " + successMessage + "\n");
            }

            if (MainProgram.testingAction) {
                MainProgram.testActionWindow.ActionExecuted(successMessage, MainProgram.errorMessage, action, parameter, line);
            }
            successMessage = "";
        }
    }
}