/*
 * AssistantComputerControl
 * Made by Albert MN.
 * Updated: v1.3.3, 16-12-2019
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
        //Static variables
        public static ulong lastFileUid;
        public static List<ulong> executedFiles = new List<ulong>();

        //Local
        private int unsuccessfulReads = 0;
        public bool isConfiguringOffset = false;
        public List<double> lastModifiedOffsets = null;
        

        private static bool RequireParameter(string param) {
            if(param != null) {
                if(param != "") {
                    return true;
                }
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


        /* Start credit */
        //Thanks to Stackoverflow user "Ash" for this brilliant solution https://stackoverflow.com/a/1866788/4880538
        class WinAPI {
            [DllImport("ntdll.dll", SetLastError = true)]
            public static extern IntPtr NtQueryInformationFile(IntPtr fileHandle, ref IO_STATUS_BLOCK IoStatusBlock, IntPtr pInfoBlock, uint length, FILE_INFORMATION_CLASS fileInformation);

            public struct IO_STATUS_BLOCK {
                uint status;
                ulong information;
            }
            public struct _FILE_INTERNAL_INFORMATION {
                public ulong IndexNumber;
            }

            // Abbreviated, there are more values than shown
            public enum FILE_INFORMATION_CLASS {
                FileDirectoryInformation = 1,     // 1
                FileFullDirectoryInformation,     // 2
                FileBothDirectoryInformation,     // 3
                FileBasicInformation,         // 4
                FileStandardInformation,      // 5
                FileInternalInformation      // 6
            }

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool GetFileInformationByHandle(IntPtr hFile, out BY_HANDLE_FILE_INFORMATION lpFileInformation);

            public struct BY_HANDLE_FILE_INFORMATION {
                public uint FileAttributes;
                public FILETIME CreationTime;
                public FILETIME LastAccessTime;
                public FILETIME LastWriteTime;
                public uint VolumeSerialNumber;
                public uint FileSizeHigh;
                public uint FileSizeLow;
                public uint NumberOfLinks;
                public uint FileIndexHigh;
                public uint FileIndexLow;
            }
        }
        public static ulong getFileUID(string theFile) {
            WinAPI.BY_HANDLE_FILE_INFORMATION objectFileInfo = new WinAPI.BY_HANDLE_FILE_INFORMATION();

            if (File.Exists(theFile)) {
                FileInfo fi = new FileInfo(theFile);
                FileStream fs = fi.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                WinAPI.GetFileInformationByHandle(fs.Handle, out objectFileInfo);

                fs.Close();

                ulong fileIndex = ((ulong)objectFileInfo.FileIndexHigh << 32) + (ulong)objectFileInfo.FileIndexLow;

                return fileIndex;
            }
            return 0;
        }
        /* End credit */

        public static void ErrorMessageBox(string msg, string title = "") {
            new Thread(() => {
                MessageBox.Show(msg, (!String.IsNullOrEmpty(title) ? title : "Error | " + MainProgram.messageBoxTitle));
            }).Start();
        }

        [STAThread]
        public void FileFound(object source, FileSystemEventArgs e) {
            ProcessFile(e.FullPath);
        }

        [STAThread]
        public void ProcessFile(string file, bool tryingAgain = false) {
            //Custom 'file read delay'
            float fileReadDelay = Properties.Settings.Default.FileReadDelay;
            if (fileReadDelay > 0) {
                MainProgram.DoDebug("User has set file delay to " + fileReadDelay.ToString() + "s, waiting before processing...");
                Thread.Sleep((int)fileReadDelay * 1000);
            }

            if (!File.Exists(file)) {
                MainProgram.DoDebug("File doesn't exist (anymore).");
                return;
            }

            //Make sure the file isn't in use before trying to access it
            int tries = 0;
            while (FileInUse(file) || tries >= 20) {
                tries++;
            }
            if (tries >= 20 && FileInUse(file)) {
                MainProgram.DoDebug("File in use in use and can't be read. Try again.");
                return;
            }

            //Check unique file ID (dublicate check)
            ulong theFileUid = 0;
            bool gotFileUid = false;
            tries = 0;
            while (!gotFileUid || tries >= 30) {
                try {
                    theFileUid = getFileUID(file);
                    gotFileUid = true;
                } catch {
                    Thread.Sleep(50);
                }
            }
            if (tries >= 30 && !gotFileUid) {
                MainProgram.DoDebug("File in use in use and can't be read. Try again.");
                return;
            }


            //Validate UID
            if (lastFileUid == 0) {
                lastFileUid = Properties.Settings.Default.LastActionFileUid;
            }
            if (lastFileUid == theFileUid && !tryingAgain) {
                //Often times this function is called three times per file - check if it has already been (or is being) processed
                return;
            }
            if (executedFiles.Contains(theFileUid) && !tryingAgain) {
                MainProgram.DoDebug("Tried to execute an already-executed file (UID " + theFileUid.ToString() + ")");
                return;
            }
            lastFileUid = theFileUid;
            executedFiles.Add(theFileUid);
            Properties.Settings.Default.LastActionFileUid = lastFileUid;
            Properties.Settings.Default.Save();

            MainProgram.DoDebug("Processing file...");
            string originalFileName = file, fullContent = "";

            if (!File.Exists(file)) {
                MainProgram.DoDebug("File no longer exists when trying to read file.");
                return;
            }

            //READ FILE
            if (new FileInfo(file).Length != 0) {
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


            } else {
                MainProgram.DoDebug(" - File is empty");
                ErrorMessageBox("No action was set in the action file.");
            }
            //END READ

            //DateTime lastModified = File.GetCreationTime(file);
            DateTime lastModified = File.GetLastWriteTime(file);
 
            if (lastModified.AddSeconds(Properties.Settings.Default.FileEditedMargin) < DateTime.Now) {
                //if (File.GetLastWriteTime(file).AddSeconds(Properties.Settings.Default.FileEditedMargin) < DateTime.Now) {
                    //Extra security - sometimes the "creation" time is a bit behind, but the "modify" timestamp is usually right.

                MainProgram.DoDebug("The file is more than " + Properties.Settings.Default.FileEditedMargin.ToString() + "s old, meaning it won't be executed.");
                MainProgram.DoDebug("File creation time: " + lastModified.ToString());
                MainProgram.DoDebug("Local time: " + DateTime.Now.ToString());

                if (GettingStarted.isConfiguringActions) {
                    //Possibly configure an offset - if this always happens

                    Console.WriteLine(" -------------- IS CONFIGURING");

                    isConfiguringOffset = true;
                    if (lastModifiedOffsets == null) {
                        lastModifiedOffsets = new List<double>();
                    }

                    lastModifiedOffsets.Add((DateTime.Now - lastModified).TotalSeconds);
                    if (lastModifiedOffsets.Count >= 3) {
                        Console.WriteLine("Average is; " + (int)(lastModifiedOffsets.Average() + 30));
                    }
                } else {
                    new CleanupService().Start();
                    return;
                }
                //}
            }
            
            MainProgram.DoDebug("\n[ -- DOING ACTION(S) -- ]");
            MainProgram.DoDebug(" - " + file + " UID; " + theFileUid);
            MainProgram.DoDebug(" - File exists, checking the content...");

            //Process the file
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

            MainProgram.DoDebug("[ -- DONE -- ]");
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

            Actions theActionExecution = ExecuteAction(action, theLine, parameter);
            if (!theActionExecution.wasFatal) {
                MainProgram.DoDebug("Non-fatal action. Starting cleanup service.");
                new CleanupService().Start();
            }

            if (!String.IsNullOrEmpty(theActionExecution.successMessage)) {
                MainProgram.DoDebug("\nSUCCESS: " + theActionExecution.successMessage + "\n");
            }
            
            if (!String.IsNullOrEmpty(theActionExecution.errorMessage)) {
                MainProgram.DoDebug("[ERROR]: " + theActionExecution.errorMessage);
                ErrorMessageBox(theActionExecution.errorMessage, "Action Error  " + MainProgram.messageBoxTitle);
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

        public static Actions ExecuteAction(string action, string line, string parameter) {
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
                    actionExecution.errorMessage = "Unknown action \"" + action + "\"";
                    break;
            }

            if (MainProgram.testingAction) {
                MainProgram.testActionWindow.ActionExecuted(actionExecution.successMessage, actionExecution.errorMessage, action, parameter, line);
            }

            return actionExecution;
        }
    }
}