/*
 * AssistantComputerControl
 * Made by Albert MN.
 * Updated: v1.2.0, 05-01-2019
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
        public string successMessage;

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
        String[,] charactersType = new String[,]
        {
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
            MainProgram.errorMessage = errorMsg;
        }

        /*
         * Actions
         */
        public void Shutdown(string parameter) {
            string shutdownParameters = "/s /t 0";
            if (parameter != null) {
                if (parameter == "abort") {
                    shutdownParameters = "abort";
                } else {
                    if (parameter.Contains("/t") || parameter.Contains("-t")) {
                        shutdownParameters = !parameter.Contains("/s") && !parameter.Contains("-s") ? "/s " : "" + parameter;
                    } else {
                        shutdownParameters = !parameter.Contains("/s") && !parameter.Contains("-s") ? "/s " : "" + parameter + " /t 0";
                    }
                }
            }

            if (MainProgram.testingAction) {
                successMessage = "Simulated shutdown";
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
                } else {
                    if (parameter.Contains("/t") || parameter.Contains("-t")) {
                        restartParameters = !parameter.Contains("/r") && !parameter.Contains("-r") ? "/r " : "" + parameter;
                    } else {
                        restartParameters = !parameter.Contains("/r") && !parameter.Contains("-r") ? "/r " : "" + parameter + " /t 0";
                    }
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
            if (double.TryParse(parameter, out double volumeLevel)) {
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
            string fileLocation = (!location.Contains(@":\") || !location.Contains(@":/")) ? Path.Combine(MainProgram.shortcutLocation, location) : location;

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

        public void KeyShortcut(string parameter) {
            /*
             * Added by : Joshua Miller
             * How to use it :
             *  - To seperate keys please us '+' (to use '+' do {ADD})
             *  - Things like ctrl will be converted to control key
             */
            
            // Split up commands
            char splitChar = '+';
            String[] keyCombinationInput = parameter.Split(splitChar);
            // Will be added onto to make what to type
            String keyCombinationPress = "";
            // Put commands into correct form
            for (int index = 0; index < keyCombinationInput.Length; index++)
            {
                // Get current command
                String command = keyCombinationInput[index];
                // If not empty
                if (command != "")
                {
                    // If one character (not command)
                    if (command.Length == 1)
                    {
                        // Add to the out
                        keyCombinationPress = keyCombinationPress + command.ToLower();
                    }
                    else
                    // If it is a command (probably)
                    {
                        // Check if it is a possible command and needs to be changed
                        bool foundYet = false;
                        for (int countInCharacterArray = 0; countInCharacterArray < charactersType.GetLength(0) && foundYet == false; countInCharacterArray++)
                        {
                            String characterTestNow = charactersType[countInCharacterArray, 0];
                            if (Equals(command.ToUpper(), characterTestNow))
                            {
                                keyCombinationPress += charactersType[countInCharacterArray, 1];
                                foundYet = true;
                            }
                            else if (Equals(command.ToUpper(), charactersType[countInCharacterArray, 1]))
                            {
                                keyCombinationPress += charactersType[countInCharacterArray, 1];
                                foundYet = true;
                            }
                        }
                        if (foundYet == false)
                        {
                            MainProgram.DoDebug("KeyShortcut Action - Warning: A command " + command.ToUpper() + " was not identified, please be weary as this may not work");
                            MainProgram.DoDebug("KeyShortcut Action - Warning: Adding Anyway");
                            keyCombinationPress += command;
                        }
                    }
                }
                else
                {
                    MainProgram.DoDebug("KeyShortcut Action - Warning: A character inside the paramater was blank");
                }
            }

            // Is it testing?
            if (MainProgram.testingAction)
            {
                successMessage = ("Simulated sending the combination: " + keyCombinationPress);
            } else
            {
                // Try pressing keys
                bool keysPressedSuccess = true;
                try
                {
                    SendKeys.SendWait(keyCombinationPress);
                }
                catch (ArgumentException)
                {
                    Error("Key combination is not valid");
                    keysPressedSuccess = false;
                }
                if (keysPressedSuccess)
                {
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
            string fileLocation = ActionChecker.GetSecondaryParam(parameter)[0]
                            , toAppend = ActionChecker.GetSecondaryParam(parameter).Length > 1 ? ActionChecker.GetSecondaryParam(parameter)[1] : null;

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

            if (MainProgram.testingAction) {
                successMessage = "Simulated making a message box with the content \"" + theMessage + "\" and " + (theTitle == null ? "no title" : "title \"" + theTitle + "\"");
            } else {
                new Thread(() => {
                    Thread.CurrentThread.Priority = ThreadPriority.Highest;
                    MessageBox.Show(theMessage, theTitle ?? "ACC Generated Message Box");
                }).Start();
            }
        }

        public void MoveSubject(string parameter) {
            string theSubject = ActionChecker.GetSecondaryParam(parameter)[0]
                            , moveTo = (ActionChecker.GetSecondaryParam(parameter).Length > 1 ? ActionChecker.GetSecondaryParam(parameter)[1] : null);

            FileAttributes attr = File.GetAttributes(theSubject);
            if (attr.HasFlag(FileAttributes.Directory)) {
                //Is directory
                if (Directory.Exists(theSubject)) {
                    if (Directory.Exists(moveTo)) {
                        //Move subject to folder
                    } else {
                        if (Directory.Exists(Path.GetDirectoryName(moveTo))) {
                            //Move subject folder and rename it to x
                        } else {
                            //error
                        }
                    }
                } else {
                    Error("Directory doesn't exist");
                }
            } else {
                //Is file
                if (File.Exists(theSubject)) {

                } else {
                    Error("File doesn't exist");
                }
            }

            if (MainProgram.testingAction) {
                
            }
        }

        public void WindowsToast() {
            //Todo - quite a bit of work
        }

        /* End of actions */
    }
}
