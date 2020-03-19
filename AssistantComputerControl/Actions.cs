/*
 * AssistantComputerControl
 * Made by Albert MN.
 * Updated: v1.4.0, 15-01-2020
 * 
 * Use:
 * - Functions for all the actions
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
    class Actions {
        public bool wasFatal = false;
        public string successMessage, errorMessage;

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

        //Key Shortcut
        /* 
         * Keys and how to convert
         * Each one on the left is accepted as a key inside the paramater
         */
        String[,] charactersType = new String[,] {
            { "BACKSPACE",          "{BACKSPACE}"},
            { "BREAK",              "{BREAK}"},
            { "CAPS_LOCK",          "{CAPSLOCK}"},
            { "DEL",                "{DELETE}"},
            { "DELETE",             "{DELETE}"},
            { "DOWN_ARROW",         "{DOWN}"},
            { "END",                "{END}"},
            { "ENTER",              "{ENTER}"},
            { "ESC",                "{ESC}"},
            { "HELP",               "{HELP}"},
            { "HOME",               "{HOME}"},
            { "INS",                "{INS}"},
            { "INSERT",             "{INSERT}"},
            { "LEFT_ARROW",         "{LEFT}"},
            { "NUM_LOCK",           "{NUMLOCK}"},
            { "PAGE_DOWN",          "{PGDN}"},
            { "PAGE_UP",            "{PGUP}"},
            { "PRINT_SCREEN",       "{PRTSC}"},
            { "RIGHT_ARROW",        "{RIGHT}"},
            { "SCROLL_LOCK",        "{SCROLLLOCK}"},
            { "TAB",                "{TAB}"},
            { "UP_ARROW",           "{UP}"},
            { "F1",                 "{F1}"},
            { "F2",                 "{F2}"},
            { "F3",                 "{F3}"},
            { "F4",                 "{F4}"},
            { "F5",                 "{F5}"},
            { "F6",                 "{F6}"},
            { "F7",                 "{F7}"},
            { "F8",                 "{F8}"},
            { "F9",                 "{F9}"},
            { "F10",                "{F10}"},
            { "F11",                "{F11}"},
            { "F12",                "{F12}"},
            { "F13",                "{F13}"},
            { "F14",                "{F14}"},
            { "F15",                "{F15}"},
            { "F16",                "{F16}"},
            { "Keypad_add",         "{ADD}"},
            { "Keypad_subtract",    "{SUBTRACT}"},
            { "Keypad_multiply",    "{MULTIPLY}"},
            { "Keypad_divide",      "{DIVIDE}"},
            { "SHIFT",              "+"},
            { "CTRL",               "^"},
            { "ALT",                "%"}
        };

        //Baisc things
        private void Error(string errorMsg, string debugMsg = null) {
            MainProgram.DoDebug("ERROR: " + (String.IsNullOrEmpty(debugMsg) ? errorMsg : debugMsg));
            //MainProgram.errorMessage = errorMsg;
            errorMessage = errorMsg;
        }

        /*
         * Actions
         */
        public void Shutdown(string parameter) {
            string shutdownParameters = "/s /t 0";
            if (parameter != null) {
                if (parameter == "abort") {
                    shutdownParameters = "abort";
                } else if (parameter.Contains("/t") || parameter.Contains("-t")) {
                    shutdownParameters = (!parameter.Contains("/s") && !parameter.Contains("-s") ? "/s " : "") + parameter;
                } else {
                    shutdownParameters = (!parameter.Contains("/s") && !parameter.Contains("-s") ? "/s " : "") + parameter + " /t 0";
                }
            }

            if (MainProgram.testingAction) {
                successMessage = "Simulated shutdown with parameters; " + shutdownParameters;
                wasFatal = false;
            } else {
                if (shutdownParameters != "abort") {
                    MainProgram.DoDebug("Shutting down computer...");
                    successMessage = "Shutting down";
                    wasFatal = true;
                    Process.Start("shutdown", shutdownParameters);
                } else {
                    MainProgram.DoDebug("Cancelling shutdown...");
                    wasFatal = false;
                    Process.Start("shutdown", "/a");
                    successMessage = "Aborted shutdown";
                }
            }
        }

        public void Restart(string parameter) {
            string restartParameters = "/r /t 0";
            if (parameter != null) {
                if (parameter == "abort") {
                    restartParameters = "abort";
                } else if (parameter.Contains("/t") || parameter.Contains("-t")) {
                    restartParameters = (!parameter.Contains("/r") && !parameter.Contains("-r") ? "/r " : "") + parameter;
                } else {
                    restartParameters = (!parameter.Contains("/r") && !parameter.Contains("-r") ? "/r " : "") + parameter + " /t 0";
                }
            }
            if (MainProgram.testingAction) {
                successMessage = "Simulated restart";
            } else {
                if (restartParameters != "abort") {
                    MainProgram.DoDebug("Restarting computer...");
                    successMessage = "Restarting";
                    wasFatal = true;
                    Process.Start("shutdown", restartParameters);
                } else {
                    MainProgram.DoDebug("Cancelling restart...");
                    wasFatal = true;
                    Process.Start("shutdown", "/a");
                    successMessage = "Aborted restart";
                }
            }
        }

        public void Sleep(string parameter) {
            if (parameter == null) {
                if (!MainProgram.testingAction) {
                    MainProgram.DoDebug("Sleeping computer...");
                    wasFatal = true;
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
                        Error("Parameter \"" + parameter + "\" is invalid for the \"sleep\" action. Accepted parameters: \"true\" and \"false\")");
                        break;
                }
                if (!MainProgram.testingAction) {
                    MainProgram.DoDebug("Sleeping computer...");
                    wasFatal = true;
                    Application.SetSuspendState(PowerState.Suspend, doForce, true);
                }
            }

            if (MainProgram.testingAction) {
                successMessage = "Simulated PC sleep";
            } else {
                successMessage = "Put computer to sleep";
            }
        }

        public void Hibernate(string parameter) {
            if (parameter == null) {
                if (!MainProgram.testingAction) {
                    MainProgram.DoDebug("Hibernating computer...");
                    wasFatal = true;
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
                        Error("Parameter \"" + parameter + "\" is invalid for the \"hibernate\" action. Accepted parameters: \"true\" and \"false\")");
                        break;
                }
                if (!MainProgram.testingAction) {
                    MainProgram.DoDebug("Hibernating computer...");
                    wasFatal = true;
                    Application.SetSuspendState(PowerState.Hibernate, doForce, true);
                }
            }

            if (MainProgram.testingAction) {
                successMessage = "Simulated PC hibernate";
            } else {
                successMessage = "Put computer in hibernation";
            }
        }
        public void Logout(string parameter) {
            if (MainProgram.testingAction) {
                successMessage = "Simulated logout";
            } else {
                MainProgram.DoDebug("Logging out of user...");
                successMessage = "Logged out of user";
                wasFatal = true;
                ExitWindowsEx(0, 0);
            }
        }
        public void Lock(string parameter) {
            if (MainProgram.testingAction) {
                successMessage = "Simulated PC lock";
            } else {
                MainProgram.DoDebug("Locking computer...");
                wasFatal = true;
                LockWorkStation();
                successMessage = "Locked pc";
            }
        }
        public void Mute(string parameter) {
            bool doMute = true;

            if (parameter == null) {
                //No parameter - toggle
                try {
                    doMute = !AudioManager.GetMasterVolumeMute();
                } catch {
                    Error("Failed to mute; no volume object.");
                }
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
                        Error("Parameter \"" + parameter + "\" is invalid for the \"mute\" action. Accepted parameters: \"true\" and \"false\")");
                        break;
                }
            }

            if (MainProgram.testingAction) {
                successMessage = "Simulated PC" + (doMute ? "muted " : "unmute");
            } else {
                try {
                    //Sometimes fails - sentry @833243007
                    AudioManager.SetMasterVolumeMute(doMute);
                } catch {
                    Error("Failed to set PC mute status", "Failed to set PC mute static. Exception caught.");
                }
                successMessage = (doMute ? "Muted " : "Unmuted") + " pc";
            }
        }
        public void SetVolume(string parameter) {
            bool numberIsGood = double.TryParse(parameter, out double volumeLevel);

            if (!numberIsGood) {
                double newNum = new WordsToNumbers().EnglishGet(parameter);
                numberIsGood = (newNum != -1);
                volumeLevel = newNum;
            }

            if (numberIsGood) {
                if (volumeLevel >= 0 && volumeLevel <= 100) {
                    if (!MainProgram.testingAction) {
                        if (Properties.Settings.Default.UnmuteOnVolumeChange) {
                            try {
                                //Sometimes fails - sentry @833243007
                                AudioManager.SetMasterVolumeMute(false);
                            } catch {
                                Error("Failed to unmute PC", "Failed to unmute PC. Exception caught.");
                            }
                        }
                        try {
                            AudioManager.SetMasterVolume((float)volumeLevel);
                        } catch {
                            //Might not have an audio device...
                            Error("Failed to set PC volume", "Failed to set PC volume. Exception caught.");
                        }
                    }
                    if (!MainProgram.testingAction) {
                        try {
                            /*if ((int)AudioManager.GetMasterVolume() != (int)volumeLevel) {
                                //Something went wrong... Audio not set to parameter-level
                                MainProgram.DoDebug("ERROR: Volume was not set properly. Master volume is " + AudioManager.GetMasterVolume() + ", not " + volumeLevel);
                                MainProgram.errorMessage = "Something went wrong when setting the volume";
                            } else {
                                successMessage = "Set volume to " + volumeLevel + "%";
                            }*/
                            //Don't check - have faith. The check causes trouble (removed in v1.2.4)
                            successMessage = "Set volume to " + volumeLevel + "%";

                        } catch {
                            Error("Failed to check volume");
                        }
                    } else {
                        successMessage = "Simulated setting system volume to " + volumeLevel + "%";
                    }
                } else {
                    Error("Can't set volume to " + volumeLevel + "%, has to be a number from 0-100");
                }
            } else {
                Error("Not a valid parameter (has to be a number)", "Parameter (" + parameter + ") not convertable to double");
            }
        }
        public void Music(string parameter) {
            switch (parameter) {
                case "previous":
                    if (MainProgram.testingAction) {
                        successMessage = "MUSIC: Simulated going to previous track";
                    } else {
                        keybd_event(VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_EXTENTEDKEY, 0);
                        successMessage = "MUSIC: Skipped song";
                    }
                    break;
                case "previousx2":
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
                    if (MainProgram.testingAction) {
                        successMessage = "MUSIC: Simulated going to next song";
                    } else {
                        keybd_event(VK_MEDIA_NEXT_TRACK, 0, KEYEVENTF_EXTENTEDKEY, 0);
                        successMessage = "MUSIC: Next song";
                    }
                    break;
                case "play_pause":
                    if (MainProgram.testingAction) {
                        successMessage = "MUSIC: Simulated play/pause";
                    } else {
                        keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENTEDKEY, 0);
                        successMessage = "MUSIC: Played/Paused";
                    }
                    break;
                default:
                    Error("Unknown parameter \"" + parameter + "\"");
                    break;
            }
        }
        public void Open(string parameter) {
            string location = ActionChecker.GetSecondaryParam(parameter)[0], arguments = (ActionChecker.GetSecondaryParam(parameter).Length > 1 ? ActionChecker.GetSecondaryParam(parameter)[1] : null);
            string fileLocation = (!location.Contains(@":\") || !location.Contains(@":/")) ? "" : location;
            if (fileLocation == "") {
                string combinedPath = "";
                try {
                    combinedPath = Path.Combine(MainProgram.shortcutLocation, location);
                    //MainProgram.DoDebug();
                } catch {
                    Error("Given path (" +  location+ ") is invalid (could not combine)");
                }

                if (combinedPath != "") {
                    fileLocation = combinedPath;
                }
            }

            if (fileLocation != "") {
                MainProgram.DoDebug(fileLocation);
                try {
                    if (File.Exists(fileLocation) || Directory.Exists(fileLocation) || Uri.IsWellFormedUriString(fileLocation, UriKind.Absolute)) {
                        if (!MainProgram.testingAction) {
                            try {
                                Process p = new Process();
                                p.StartInfo.FileName = fileLocation;
                                if (arguments != null)
                                    p.StartInfo.Arguments = arguments;
                                p.Start();
                                successMessage = "OPEN: opened file/url; " + fileLocation;
                            } catch (Exception e) {
                                MainProgram.DoDebug("Failed to open file; " + e.Message);
                                Error("Failed to open file (" + fileLocation + ")");
                            }
                        } else {
                            successMessage = "OPEN: simulated opening file; " + fileLocation;
                        }
                    } else {
                        Error("File or directory doesn't exist (" + fileLocation + ")");
                    }
                } catch {
                    MainProgram.DoDebug("Error when opening file");
                }
            }
        }
        public void OpenAll(string parameter) {
            string fileLocation = (!parameter.Contains(@":\") || !parameter.Contains(@":/")) ? Path.Combine(MainProgram.shortcutLocation, parameter) : parameter;

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
                Error("Directory doesn't exist (" + fileLocation + ")");
            }
        }
        public void Die(string parameter) {
            if (!MainProgram.testingAction) {
                successMessage = "Shutting down ACC";
                Application.Exit();
            } else {
                successMessage = "Simulated shutting down ACC";
            }
        }
        public void MonitorsOff(string parameter) {
            if (!MainProgram.testingAction) {
                Form f = new Form();
                SendMessage(f.Handle, WM_SYSCOMMAND, (IntPtr)SC_MONITORPOWER, (IntPtr)2);
                f.Close();
                successMessage = "Turned monitors off";
            } else {
                successMessage = "Simulated turning monitors off";
            }
        }

        private static void PressKey(char c) {
            //TODO
            try {
                SendKeys.SendWait(c.ToString());
            } catch (Exception e) {
                MainProgram.DoDebug("Failed to press key \"" + c.ToString() + "\", exception; " + e);
            }
        }

        public void WriteOut(string parameter, string line) {
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

        public void KeyShortcut(string parameter) {
            /*
             * Added by: Joshua Miller
             * How to use it:
             *  - To seperate keys please use '+' (to use '+' do {ADD})
             *  - Things like ctrl will be converted to control key
             */

            // Split up commands
            char splitChar = '+';
            String[] keyCombinationInput = parameter.Split(splitChar);
            // Will be added onto to make what to type
            String keyCombinationPress = "";
            // Put commands into correct form
            for (int index = 0; index < keyCombinationInput.Length; index++) {
                // Get current command
                String command = keyCombinationInput[index];
                // If not empty
                if (command != "") {
                    // If one character (not command)
                    if (command.Length == 1) {
                        // Add to the out
                        keyCombinationPress = keyCombinationPress + command.ToLower();
                    } else {
                        // If it is a command (probably)
                        // Check if it is a possible command and needs to be changed
                        bool foundYet = false;
                        for (int countInCharacterArray = 0; countInCharacterArray < charactersType.GetLength(0) && foundYet == false; countInCharacterArray++) {
                            String characterTestNow = charactersType[countInCharacterArray, 0];
                            if (Equals(command.ToUpper(), characterTestNow)) {
                                keyCombinationPress += charactersType[countInCharacterArray, 1];
                                foundYet = true;
                            } else if (Equals(command.ToUpper(), charactersType[countInCharacterArray, 1])) {
                                keyCombinationPress += charactersType[countInCharacterArray, 1];
                                foundYet = true;
                            }
                        }
                        if (foundYet == false) {
                            MainProgram.DoDebug("KeyShortcut Action - Warning: A command " + command.ToUpper() + " was not identified, please be weary as this may not work");
                            MainProgram.DoDebug("KeyShortcut Action - Warning: Adding Anyway");
                            keyCombinationPress += command;
                        }
                    }
                } else {
                    MainProgram.DoDebug("KeyShortcut Action - Warning: A character inside the paramater was blank");
                }
            }

            // Is it testing?
            if (MainProgram.testingAction) {
                successMessage = ("Simulated sending the combination: " + keyCombinationPress);
            } else {
                // Try pressing keys
                bool keysPressedSuccess = true;
                try {
                    SendKeys.SendWait(keyCombinationPress);
                } catch (ArgumentException) {
                    Error("Key combination is not valid");
                    keysPressedSuccess = false;
                }
                if (keysPressedSuccess) {
                    successMessage = ("Sending the combination: " + keyCombinationPress);
                }
            }
        }

        public void CreateFile(string parameter) {
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
                            while (!ActionChecker.FileInUse(toDelete)) ;
                            File.Delete(toDelete);
                        } else {
                            //Actually create file
                            var myFile = File.Create(fileLocation);
                            myFile.Close();
                        }
                    } catch (Exception exc) {
                        succeeded = false;
                        Error("Couldn't create file - folder might be locked. Try running ACC as administrator.", exc.Message);
                    }

                    if (succeeded) {
                        if (!MainProgram.testingAction) {
                            successMessage = "Created file at " + fileLocation;
                        } else {
                            successMessage = "Simulated creating file at " + fileLocation;
                        }
                    }
                } else {
                    Error("File parent folder doesn't exist (" + parentPath + ")");
                }
            } else {
                Error("File already exists");
            }
        }
        public void DeleteFile(string parameter) {
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
                    Error("Couldn't access file/folder - file might be in use or locked. Try running ACC as administrator.", exc.Message);
                }

                if (succeeded) {
                    if (!MainProgram.testingAction) {
                        successMessage = "Deleted file/folder at " + fileLocation;
                    } else {
                        successMessage = "Simulated deleting file/folder at " + fileLocation;
                    }
                }
            } else {
                Error("File or folder doesn't exist");
            }
        }
        public void AppendText(string parameter) {
            string fileLocation = ActionChecker.GetSecondaryParam(parameter)[0],
                toAppend = ActionChecker.GetSecondaryParam(parameter).Length > 1 ? ActionChecker.GetSecondaryParam(parameter)[1] : null;

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
                            Error("Couldn't create file - folder might be locked. Try running ACC as administrator.", exc.Message);
                        }

                        if (succeeded) {
                            if (!MainProgram.testingAction) {
                                successMessage = "Appended \"" + toAppend + "\" to file at " + fileLocation;
                            } else {
                                successMessage = "Simulated appending \"" + toAppend + "\" to file at " + fileLocation;
                            }
                        }
                    } else {
                        Error("File doesn't exists");
                    }
                } else {
                    Error("Can't append nothing");
                }
            } else {
                Error("Parameter doesn't contain a string to append");
            }
        }
        public void DoMessageBox(string parameter) {
            string theMessage = ActionChecker.GetSecondaryParam(parameter)[0]
                            , theTitle = (ActionChecker.GetSecondaryParam(parameter).Length > 1 ? ActionChecker.GetSecondaryParam(parameter)[1] : null);

            MainProgram.DoDebug("TESTTTTT ::::: " + (theTitle == null).ToString());

            if (MainProgram.testingAction) {
                successMessage = "Simulated making a message box with the content \"" + theMessage + "\" and " + (String.IsNullOrEmpty(theTitle) ? "no title" : "custom title \"" + theTitle + "\"");
            } else {
                new Thread(() => {
                    Thread.CurrentThread.Priority = ThreadPriority.Highest;
                    MessageBox.Show(theMessage, String.IsNullOrEmpty(theTitle) ? "ACC Generated Message Box" : theTitle);
                }).Start();
            }
        }

        public void KillProcess(string parameter) {
            bool paramIsNum = int.TryParse(parameter, out int pid);

            if (paramIsNum) {
                try {
                    //Results in an exception if process doesn't exist
                    Process theP = Process.GetProcessById(pid);

                    if (!MainProgram.testingAction) {
                        try {
                            //theP.Close();
                            theP.Kill();
                            successMessage = "Killed process with ID " + pid.ToString();
                        } catch (Exception e) {
                            Error("Failed to kill process with ID " + pid.ToString() + "; " + e.Message);
                        }
                    } else {
                        successMessage = "Successfully simulated killing process";
                    }
                } catch {
                    Error("A process with the ID " + pid.ToString() + " doesn't exist");
                }
            } else {
                try {
                    //Results in an exception if process doesn't exist
                    Process[] thePs = Process.GetProcessesByName(parameter);

                    if (!MainProgram.testingAction) {
                        try {
                            foreach (Process p in thePs) {
                                p.Kill();
                            }
                            successMessage = "Killed all processes with name " + parameter;
                        } catch (Exception e) {
                            Error("Failed to kill processes with name " + parameter + "; " + e.Message);
                        }
                    } else {
                        successMessage = "Successfully simulated killing processes";
                    }
                } catch {
                    Error("A process with the name " + parameter + " doesn't exist");
                }
            }
        }

        public void MoveSubject(string parameter) {
            string theSubject = ActionChecker.GetSecondaryParam(parameter)[0],
                moveTo = (ActionChecker.GetSecondaryParam(parameter).Length > 1 ? ActionChecker.GetSecondaryParam(parameter)[1] : null);

            FileAttributes attr = FileAttributes.Normal; //Has to have a default value
            bool subjectExists = true;
            try {
                attr = File.GetAttributes(theSubject);
            } catch {
                subjectExists = false;
            }
            
            if (subjectExists) {
                if (attr.HasFlag(FileAttributes.Directory)) {
                    //Is directory
                    bool isPath = true;
                    try {
                        Path.GetFullPath(moveTo);
                    } catch {
                        isPath = false;
                    }
                    if (isPath) {
                        if (Directory.Exists(theSubject)) {
                            if (Directory.Exists(moveTo)) {
                                //Dest is a folder
                                string newMoveTo = Path.Combine(moveTo, Path.GetFileName(new DirectoryInfo(theSubject).Name));

                                if (MainProgram.testingAction) {
                                    successMessage = "Simulated moving folder from \"" + theSubject + "\" to \"" + newMoveTo + "\"";
                                } else {
                                    try {
                                        Directory.Move(theSubject, newMoveTo);
                                        successMessage = "Moved folder from \"" + theSubject + "\" to \"" + newMoveTo + "\"";
                                    } catch (Exception e) {
                                        Error("[Move action] Couldn't move folder (" + theSubject + ") to path; '" + newMoveTo + "'. Got error; " + e.Message);
                                    }
                                }
                            } else {
                                //Dest is a new folder name!
                                if (MainProgram.testingAction) {
                                    successMessage = "Simulated moving (renaming) folder from \"" + theSubject + "\" to \"" + moveTo + "\"";
                                } else {
                                    try {
                                        Directory.Move(theSubject, moveTo);
                                        successMessage = "Moved (renamed) folder from \"" + theSubject + "\" to \"" + moveTo + "\"";
                                    } catch (Exception e) {
                                        Error("[Move action] Couldn't move folder (" + theSubject + "); " + e.Message);
                                    }
                                }
                            }
                        } else {
                            Error("[Move action] Subject folder doesn't exist");
                        }
                    } else {
                        Error("[Move action] To move a folder, the destination (secondary parameter) must be the path to a folder");
                    }
                } else {
                    //Is file
                    if (File.Exists(theSubject)) {
                        if (File.Exists(moveTo) || Directory.Exists(moveTo)) {
                            attr = File.GetAttributes(moveTo);
                            if (attr.HasFlag(FileAttributes.Directory)) {
                                //Desination is a folder
                                if (Directory.Exists(moveTo)) {
                                    var theDestination = Path.Combine(moveTo, Path.GetFileName(theSubject));

                                    if (MainProgram.testingAction) {
                                        successMessage = "Simulated moving file from \"" + moveTo + "\" to \"" + theDestination + "\"";
                                    } else {
                                        try {
                                            File.Move(theSubject, theDestination);
                                            successMessage = "Moved file from \"" + moveTo + "\" to \"" + theDestination + "\"";
                                        } catch (Exception e) {
                                            Error("[Move action] Couldn't move file to folder; " + e.Message);
                                        }
                                    }
                                } else {
                                    Error("[Move action] Desination folder doesn't exist");
                                }
                            } else {
                                //Desination is a file - either overwrite existing file, or "rename" file to dest
                                try {
                                    File.Move(theSubject, moveTo);
                                } catch (Exception e) {
                                    Error("[Move action] Couldn't move file; " + e.Message);
                                }
                            }
                        } else {
                            try {
                                File.Move(theSubject, moveTo);
                            } catch (Exception e) {
                                Error("[Move action] Couldn't move file; " + e.Message);
                            }
                        }
                    } else {
                        Error("[Move action] Subject file doesn't exist");
                    }
                }
            } else {
                Error("[Move action] Subject (folder or file) doesn't exist");
            }

            if (MainProgram.testingAction) {
                
            }
        }

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
        static extern IntPtr SendMessageNew(IntPtr hWnd, Int32 Msg, IntPtr wParam, IntPtr lParam);

        const int WM_COMMAND = 0x111;
        const int MIN_ALL = 419;
        const int MIN_ALL_UNDO = 416;

        public void MinimizeAll() {
            if (MainProgram.testingAction) {
                successMessage = "Simulated minimize of all windows";
            } else {
                IntPtr lHwnd = FindWindow("Shell_TrayWnd", null);
                SendMessageNew(lHwnd, WM_COMMAND, (IntPtr)MIN_ALL, IntPtr.Zero);
                successMessage = "Minimized all windows";
            }
        }

        public void MaximizeAll() {
            if (MainProgram.testingAction) {
                successMessage = "Simulated minimize of all windows";
            } else {
                IntPtr lHwnd = FindWindow("Shell_TrayWnd", null);
                SendMessageNew(lHwnd, WM_COMMAND, (IntPtr)MIN_ALL_UNDO, IntPtr.Zero);
                successMessage = "Minimized all windows";
            }
        }

        //Maximize & minimize specific windows
        private const int SW_MAXIMIZE = 3;
        private const int SW_MINIMIZE = 6;
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public void MinimizeMaximizeWindow(string parameter, int type) {
            bool paramIsNum = int.TryParse(parameter, out int pid);

            if (paramIsNum) {
                try {
                    //Results in an exception if process doesn't exist
                    Process theP = Process.GetProcessById(pid);

                    if (!MainProgram.testingAction) {
                        try {
                            ShowWindow(theP.MainWindowHandle, type);
                            successMessage = (type == SW_MAXIMIZE ? "Maximized" : "Minimized") + " process with ID " + pid.ToString();
                        } catch (Exception e) {
                            Error("Failed to " + (type == SW_MAXIMIZE ? "maximize" : "minimize") + " process with ID " + pid.ToString() + "; " + e.Message);
                        }
                    } else {
                        successMessage = "Successfully simulated " + (type == SW_MAXIMIZE ? "maximize" : "minimize") + " window(s)";
                    }
                } catch {
                    Error("A process with the ID " + pid.ToString() + " doesn't exist");
                }
            } else {
                try {
                    //Results in an exception if process doesn't exist
                    Process[] thePs = Process.GetProcessesByName(parameter);

                    if (!MainProgram.testingAction) {
                        try {
                            IntPtr hwnd = FindWindowByCaption(IntPtr.Zero, parameter);
                            ShowWindow(hwnd, type);

                            foreach (Process p in thePs) {
                                ShowWindow(p.MainWindowHandle, type);
                            }
                            successMessage = (type == SW_MAXIMIZE ? "Maximized" : "Minimized") + " all processes with name " + parameter;
                        } catch (Exception e) {
                            Error("Failed to " + (type == SW_MAXIMIZE ? "maximize" : "minimize") + " processes with name " + parameter + "; " + e.Message);
                        }
                    } else {
                        successMessage = "Successfully simulated " + (type == SW_MAXIMIZE ? "maximize" : "minimize") + " window(s)";
                    }
                } catch {
                    Error("A process with the name " + parameter + " doesn't exist");
                }
            }
        }

        public void MinimizeWindow(string parameter) {
            MinimizeMaximizeWindow(parameter, SW_MINIMIZE);
        }

        public void MaximizeWindow(string parameter) {
            MinimizeMaximizeWindow(parameter, SW_MAXIMIZE);
        }

        public void WindowsToast() {
            //Todo - quite a bit of work - later (pushed for way too many updates now, whoopppss)
        }

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        private static extern bool SetCursorPos(int x, int y);
        public void MoveMouse(String parameter) {
            parameter = parameter.Trim(new char[] { '(',')',' '});
            String[] parameterSplit = parameter.Split(',');
        
            // Error Checking
            // If there is the correct amount of arguments
            if (parameterSplit.Length != 2) {
                Error("The parameter is either formatted incorrectly or does not have 2 values");
            // If param 1 is a number
            } else if ((Int32.TryParse(parameterSplit[0], out int param1))) {
                if ((Int32.TryParse(parameterSplit[1], out int param2))) {
                    if (MainProgram.testingAction) {
                        successMessage = "Simulated moving mouse to (" + (Int32.Parse(parameterSplit[0]).ToString() + ", " + Int32.Parse(parameterSplit[1])).ToString() + ")";
                    } else {
                        SetCursorPos(Int32.Parse(parameterSplit[0]), Int32.Parse(parameterSplit[1]));
                        successMessage = "Moved mouse to (" + (Int32.Parse(parameterSplit[0]).ToString() + ", " + Int32.Parse(parameterSplit[1])).ToString() + ")";
                    }
                } else {
                    Error("Parameter 2 is not a number");
                }

            // It isn't a number
            } else {
                Error("Parameter 1 is not a number");
            }
        }

        [DllImport("user32.dll", EntryPoint = "mouse_event")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        public const int MOUSEEVENTF_RIGHTUP = 0x10;
        public const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
        public const int MOUSEEVENTF_MIDDLEUP = 0x40;

        public void MouseClick(string parameter = "", string secondaryParameter = "") {
            /*
             * Action made by community member Joshua Miller (modified by Albert)
             */

            int timesToClick = 1;
            string type = "left";

            if (parameter == String.Empty) {
                type = "left";
            }

            if (secondaryParameter != String.Empty) {
                if (Int32.TryParse(secondaryParameter, out int repeatAmount)) {
                    timesToClick = repeatAmount;
                } else {
                    Error("Secondary parameter (how many times to click) is not a valid number");
                    return;
                }
            }

            switch (type) {
                case "left":
                    //Default
                    if (MainProgram.testingAction) {
                        successMessage = "Simulated pressing the left mouse button " + timesToClick + " time(s)";
                    } else {
                        for (int count = 0; count < timesToClick; count++) {
                            mouse_event(MOUSEEVENTF_LEFTDOWN, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                            mouse_event(MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                        }
                        successMessage = "Pressed the the left mouse button " + timesToClick + " time(s)";
                    }
                    break;
                case "right":
                    if (MainProgram.testingAction) {
                        successMessage = "Simulated pressing the right mouse button " + timesToClick + " time(s)";
                    } else {
                        for (int count = 0; count < timesToClick; count++) {
                            mouse_event(MOUSEEVENTF_RIGHTDOWN, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                            mouse_event(MOUSEEVENTF_RIGHTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                        }
                        successMessage = "Pressed the the right mouse button " + timesToClick + " time(s)";
                    }
                    break;
                case "middle":
                    if (MainProgram.testingAction) {
                        successMessage = "Simulated pressing the middle mouse button " + timesToClick + " time(s)";
                    } else {
                        for (int count = 0; count < timesToClick; count++) {
                            mouse_event(MOUSEEVENTF_MIDDLEDOWN, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                            mouse_event(MOUSEEVENTF_MIDDLEUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                        }
                        successMessage = "Pressed the the middle mouse button " + timesToClick + " time(s)";
                    }
                    break;
                default:
                    Error("Invalid mouse-click type (" + type + ")");
                    break;
            }
        }

        public void Wait(string parameter) {
            /*
             * Action made by community member Joshua Miller
             */

            if (Int32.TryParse(parameter, out int time)) {
                if (!MainProgram.testingAction) {
                    Thread.Sleep(time);
                    successMessage = "Waited " + time + " miliseconds";
                } else {
                    successMessage = "Simulated the 'wait' action - sleep for " + time.ToString() + " miliseconds";
                }
            } else {
                Error("Time Parameter is not a number");
            }
        }

        /* End of actions */
    }
}
