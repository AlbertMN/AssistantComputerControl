/*
 * AssistantComputerControl
 * Made by Albert MN.
 * Updated: v1.2.1, 06-01-2019
 * 
 * Use:
 * - Checks and execute action files
 */

using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace AssistantComputerControl {
    class ActionChecker {
        private static string successMessage = "";
        private static bool lastActionWasFatal;

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

        static public void FileFound(object source, FileSystemEventArgs e) {
            ProcessFile(e.FullPath);
        }

        private static DateTime lastActionModified;

        static public void ProcessFile(string file) {
            string originalFileName = file;

            if (!File.Exists(file)) {
                //MainProgram.DoDebug("File doesn't exist.");
                return;
            }
            //DateTime lastModified = File.GetCreationTime(file);
            DateTime lastModified = File.GetLastWriteTime(file);
            //MainProgram.DoDebug(lastModified.ToString());

            if (lastModified == lastActionModified || (File.GetAttributes(file) & FileAttributes.Hidden) == FileAttributes.Hidden) {
                //If file is hidden or has exact same "last modified" date as last file (possible trying to do the same twice)
                /*MainProgram.DoDebug("Dublicate");

                try {
                    //File.Delete(file);
                } catch {

                }*/
                return;
            }
            lastActionModified = lastModified;

            if (lastModified.AddSeconds(Properties.Settings.Default.FileEditedMargin) < DateTime.Now) {
                //if (File.GetLastWriteTime(file).AddSeconds(Properties.Settings.Default.FileEditedMargin) < DateTime.Now) {
                    //Extra security - sometimes the "creation" time is a bit behind, but the "modify" timestamp is usually right.

                    MainProgram.DoDebug("The file is more than " + Properties.Settings.Default.FileEditedMargin.ToString() + "s old, meaning it won't be executed.");
                    MainProgram.DoDebug("File creation time: " + lastModified.ToString());
                    MainProgram.DoDebug("Local time: " + DateTime.Now.ToString());
                    return;
                //}
            } 

            MainProgram.DoDebug("\n[ -- DOING ACTION(S) -- ]");
            MainProgram.DoDebug(" - " + file);
            MainProgram.DoDebug(" - File exists, checking the content...");

            try {
                //Thread.Sleep(200);
                File.SetAttributes(file, FileAttributes.Hidden);
            } catch {
                //
            }

            if (new FileInfo(file).Length != 0) {
                string fullContent = "";
                try {
                    //Sentry issue @804439508
                    fullContent = Regex.Replace(File.ReadAllText(file), @"\t|\r", "");
                } catch {
                    MainProgram.DoDebug("Could not read file.");
                    return;
                }
                MainProgram.DoDebug(" - Read complete, content: " + fullContent);
                File.SetAttributes(file, FileAttributes.Hidden);

                using (StringReader reader = new StringReader(fullContent)) {
                    string theLine = string.Empty;
                    do {
                        theLine = reader.ReadLine();
                        if (theLine != null) {
                            MainProgram.DoDebug("\n[EXECUTING ACTION]");
                            CheckAction(theLine, file);
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

            if (MainProgram.errorMessage.Length != 0) {
                MessageBox.Show(MainProgram.errorMessage, "Error | " + MainProgram.messageBoxTitle);
                MainProgram.errorMessage = "";
            }
        }

        private static void CheckAction(string theLine, string theFile) {
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

            lastActionWasFatal = true;
            ExecuteAction(action, theLine, parameter, assistantParam);

            if (!lastActionWasFatal) {
                MainProgram.DoDebug("Non-fatal action. Deleting.");
                try {
                    File.Delete(theFile);
                } catch {
                    MainProgram.DoDebug("Error trying to delete action-file.");
                }
            } else {
                MainProgram.DoDebug("Action was fatal action - won't delete just yet - renaming.");
                try {
                    string newName = string.Format(@"{0}." + Properties.Settings.Default.ActionFileExtension, DateTime.Now.Ticks);
                    //File.Move(theFile, Path.Combine(Path.GetDirectoryName(theFile), newName));
                } catch {
                    MainProgram.DoDebug("Failed to rename file.");
                }
            }
        }

        public static string[] GetSecondaryParam(string param) {
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

            Actions actionExecution = new Actions();

            switch (action.ToLower()) {
                case "shutdown":
                    //Shuts down the computer
                    actionExecution.Shutdown(parameter);
                    break;
                case "restart":
                    //Restart the computer
                    actionExecution.Restart(parameter);
                    break;
                case "sleep":
                    //Puts computer to sleep
                    actionExecution.Sleep(parameter);
                    break;
                case "hibernate":
                    //Hibernates computer
                    actionNumber = 12;
                    actionExecution.Hibernate(parameter);
                    break;
                case "logout":
                    //Logs out of the current user
                    actionExecution.Logout(parameter);
                    break;
                case "lock":
                    //Lock computer
                    actionExecution.Lock(parameter);
                    break;
                case "mute":
                    //Mutes windows
                    //Parameter optional (true/false)
                    actionExecution.Mute(parameter);
                    break;
                case "set_volume":
                    //Sets volume to a specific percent
                    //Requires parameter (percent, int)
                    if (RequireParameter(parameter)) {
                        actionExecution.SetVolume(parameter);
                    }
                    break;
                case "music":
                    if (RequireParameter(parameter)) {
                        switch (parameter) {
                            case "previous":
                            case "previousx2":
                                actionNumber = 8;
                                break;
                            case "next":
                                actionNumber = 10;
                                break;
                            case "play_pause":
                                actionNumber = 9;
                                break;
                        }
                        actionExecution.Music(parameter);
                    }
                    break;
                case "open":
                    if (RequireParameter(parameter)) {
                        actionExecution.Open(parameter);
                    }
                    break;
                case "open_all":
                    if (RequireParameter(parameter)) {
                        actionExecution.OpenAll(parameter);
                    }
                    break;
                case "die":
                    //Exit ACC
                    actionExecution.Die(parameter);
                    break;
                case "monitors_off":
                    actionExecution.MonitorsOff(parameter);
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
                        actionExecution.WriteOut(parameter, line);
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
                        actionExecution.CreateFile(parameter);
                    }
                    break;
                case "delete_file":
                    if (RequireParameter(parameter)) {
                        actionExecution.DeleteFile(parameter);
                    }
                    break;
                case "append_text":
                    if (RequireParameter(parameter)) {
                        actionExecution.AppendText(parameter);
                    }
                    break;
                case "message_box":
                    if (RequireParameter(parameter)) {
                        actionExecution.DoMessageBox(parameter);
                    }
                    break;
                default:
                    //Unknown action
                    MainProgram.DoDebug("ERROR: Unknown action \"" + action + "\"");
                    MainProgram.errorMessage = "Unknown action \"" + action + "\"";
                    break;
            }

            successMessage = actionExecution.successMessage;

            lastActionWasFatal = actionExecution.wasFatal;
            actionExecution.wasFatal = false;

            if (successMessage != "") {
                if (actionNumber != null) {
                    //Has specified number
                    MainProgram.AnalyticsAddCount(null, (int)actionNumber, assistantParam);
                } else {
                    //YOLO
                    MainProgram.AnalyticsAddCount(action, null, assistantParam);
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