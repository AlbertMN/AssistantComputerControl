/*
 * AssistantComputerControl
 * Made by Albert MN.
 * Updated: v1.2.2, 10-02-2019
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

        [STAThread]
        static public void FileFound(object source, FileSystemEventArgs e) {
            ProcessFile(e.FullPath);
        }

        private static DateTime lastActionModified;
        private static int unsuccessfulReads = 0;

        [STAThread]
        static public void ProcessFile(string file, bool tryingAgain = false) {
            MainProgram.DoDebug("Processing file...");
            string originalFileName = file;

            float fileReadDelay = Properties.Settings.Default.FileReadDelay;
            if (fileReadDelay > 0) {
                MainProgram.DoDebug("User has set file delay to " + fileReadDelay.ToString() + "s, waiting before processing...");
                Thread.Sleep((int)fileReadDelay * 1000);
            }

            if (!File.Exists(file)) {
                MainProgram.DoDebug("File doesn't exist (anymore).");
                return;
            }

            bool hidden;

            try {
                hidden = (File.GetAttributes(file) & FileAttributes.Hidden) == FileAttributes.Hidden;
            } catch {
                MainProgram.DoDebug("Failed to get file attribute (file probably doesn't exist) - won't move on.");
                return;
            }
            
            if (hidden && !tryingAgain) {
                MainProgram.DoDebug("File is hidden and has therefore (most likely) already been processed and executed. Ignoring it...");
                return;
            }

            //DateTime lastModified = File.GetCreationTime(file);
            DateTime lastModified = File.GetLastWriteTime(file);
            if (lastModified == lastActionModified && !tryingAgain) {
                MainProgram.DoDebug("File has the exact same 'last modified' timestamp as the previous action - most likely a dublicate; ignoring");
                return;
                
            }
            lastActionModified = lastModified;

            if (lastModified.AddSeconds(Properties.Settings.Default.FileEditedMargin) < DateTime.Now) {
                //if (File.GetLastWriteTime(file).AddSeconds(Properties.Settings.Default.FileEditedMargin) < DateTime.Now) {
                    //Extra security - sometimes the "creation" time is a bit behind, but the "modify" timestamp is usually right.

                MainProgram.DoDebug("The file is more than " + Properties.Settings.Default.FileEditedMargin.ToString() + "s old, meaning it won't be executed.");
                MainProgram.DoDebug("File creation time: " + lastModified.ToString());
                MainProgram.DoDebug("Local time: " + DateTime.Now.ToString());

                new CleanupService().Start();
                return;
                //}
            }

            MainProgram.DoDebug("\n[ -- DOING ACTION(S) -- ]");
            MainProgram.DoDebug(" - " + file);
            MainProgram.DoDebug(" - File exists, checking the content...");

            if (new FileInfo(file).Length != 0) {
                string fullContent = "";
                //Sentry issue @804439508

                int tries = 0;
                while (FileInUse(file) || tries >= 20) {
                    tries++;
                }

                if (tries >= 20 && FileInUse(file)) {
                    MainProgram.DoDebug("File still in use and can't be read. Try again.");
                    return;
                }

                try {
                    string fileContent;
                    fileContent = File.ReadAllText(file);
                    fullContent = Regex.Replace(fileContent, @"\t|\r", "");
                } catch (Exception e) {
                    if (unsuccessfulReads < 20) {
                        MainProgram.DoDebug("Failed to read file - trying again in 200ms... (trying max 20 times)");
                        unsuccessfulReads++;
                        Thread.Sleep(200);
                        ProcessFile(file, true);

                        return;
                    } else {
                        MainProgram.DoDebug("Could not read file on final try; " + e);
                        unsuccessfulReads = 0;
                        return;
                    }
                }
                MainProgram.DoDebug(" - Read complete, content: " + fullContent);

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

                if ((File.GetAttributes(file) & FileAttributes.Hidden) != FileAttributes.Hidden) {
                    try {
                        File.SetAttributes(file, FileAttributes.Hidden);
                    } catch {
                        //
                    }
                }
            }

            MainProgram.DoDebug("[ -- DONE -- ]");

            if (MainProgram.errorMessage.Length != 0) {
                MessageBox.Show(MainProgram.errorMessage, "Error | " + MainProgram.messageBoxTitle);
                //Thread.Sleep(5000);
                MainProgram.errorMessage = "";
            }
        }

        private static void CheckAction(string theLine, string theFile) {
            string action = theLine
                , parameter = null
                , fullContent = null;

            action = theLine;
            fullContent = theLine;

            bool isDefaultComputer = Properties.Settings.Default.DefaultComputer;
            string theComputerName = null, thisComputerName = Properties.Settings.Default.ComputerName;

            //COMPUTER NAME!
            //PREVIOUSLY; Whether it's Google Assistant or Amazon Alexa (included in the default IFTTT applets)
            if (theLine.Contains("[") && theLine.Contains("]")) {
                action = theLine.Split('[')[0];
                theComputerName = theLine.Split('[')[1];
                theComputerName = theComputerName.Split(']')[0];

                if (theComputerName == "google" || theComputerName == "alexa") {
                    MainProgram.DoDebug(" - The targetted 'PC name' is set to 'google' or 'alexa' - this is the old format; these names are reserved and cannot be used as 'Computer name'");
                } else {
                    MainProgram.DoDebug(" - Targetted computer is; " + theComputerName);
                    if (thisComputerName == theComputerName) {
                        MainProgram.DoDebug(" - This computer is the target!");
                    } else {
                        //Not this computer!
                        MainProgram.DoDebug(" - Computer name \"" + theComputerName + "\" does not match \"" + thisComputerName + "\"");
                        return;
                    }
                }
            } else {
                if (thisComputerName == String.Empty) {
                    if (!isDefaultComputer) {
                        MainProgram.DoDebug(" - Applet has not specified a computer name, this computer has no computer name and is NOT a 'default computer' - ignoring");
                        return;
                    } else {
                        MainProgram.DoDebug(" - Applet has not specified a computer name, this computer has no computer name BUT it's a 'default computer' - executing");
           
                    }
                } else {
                    if (!isDefaultComputer) {
                        MainProgram.DoDebug(" - Applet has not specified a computer name, this computer HAS a computer name and is NOT a 'default computer' - ignoring");
                        return;
                    } else {
                        MainProgram.DoDebug(" - Applet has not specified a computer name, this computer HAS a computer name and IS a 'default computer' - executing");
        
                    }
                }
            }

            try {
                File.SetAttributes(theFile, FileAttributes.Hidden);
            } catch {
                MainProgram.DoDebug("Failed to set attribute; hidden on action file");
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

            lastActionWasFatal = false;
            ExecuteAction(action, theLine, parameter);

            if (!lastActionWasFatal) {
                MainProgram.DoDebug("Non-fatal action. Starting cleanup service.");
                new CleanupService().Start();
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

        public static void ExecuteAction(string action, string line, string parameter) {
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
                                break;
                            case "next":
                                break;
                            case "play_pause":
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
                case "key_shortcut":
                    if (RequireParameter(parameter)) {
                        actionExecution.KeyShortcut(parameter);
                    }
                    break;
                case "write_out":
                    if (RequireParameter(parameter)) {
                        actionExecution.WriteOut(parameter, line);
                    }
                    break;
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
                case "move":
                    if (RequireParameter(parameter)) {
                        actionExecution.MoveSubject(parameter);
                    }
                    break;
                case "kill_process":
                    if (RequireParameter(parameter)) {
                        actionExecution.KillProcess(parameter);
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
                MainProgram.DoDebug("\nSUCCESS: " + successMessage + "\n");
            }

            if (MainProgram.testingAction) {
                MainProgram.testActionWindow.ActionExecuted(successMessage, MainProgram.errorMessage, action, parameter, line);
            }
            successMessage = "";
        }
    }
}