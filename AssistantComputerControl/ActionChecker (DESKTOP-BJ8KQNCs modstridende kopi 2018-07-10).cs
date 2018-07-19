using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace AssistantComputerControl {
    class ActionChecker {
        private static string successMessage = "";

        //Logout
        [DllImport("user32.dll", SetLastError = true)]
        static extern int ExitWindowsEx(uint uFlags, uint dwReason);

        //Lock
        [DllImport("user32.dll")]
        public static extern bool LockWorkStation();

        //Music-control
        public const int KEYEVENTF_EXTENTEDKEY = 1;
        public const int KEYEVENTF_KEYUP = 0;
        public const int VK_MEDIA_NEXT_TRACK = 0xB0; //Next track
        public const int VK_MEDIA_PLAY_PAUSE = 0xB3; //Play/pause
        public const int VK_MEDIA_PREV_TRACK = 0xB1; //Previous track
        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        private static bool requireParameter(string param) {
            if(param != null) {
                return true;
            } else {
                MainProgram.DoDebug("ERROR: Parameter not set");
                MainProgram.errorMessage = "Parameter not set";
            }
            return false;
        }
        public static bool FileInUse(string file) {
            try {
                using (Stream stream = new FileStream(file, FileMode.Open)) {
                    // File/Stream manipulating code here
                    MainProgram.DoDebug("Can read file");
                    return false;
                }
            } catch {
                MainProgram.DoDebug("File is in use, retrying");
                Thread.Sleep(50);
                return true;
            }
        }

        static public void FileFound(object source, FileSystemEventArgs e) {
            string file = e.FullPath;

            if (!MainProgram.isPerformingAction) {
                MainProgram.isPerformingAction = true;
                MainProgram.DoDebug("File exists, checking the content...");

                while (FileInUse(file));
                if (new FileInfo(file).Length != 0) {
                    MainProgram.DoDebug("Action set. File is not empty...");

                    string line = File.ReadAllText(file);
                    MainProgram.DoDebug("Read complete, content: " + line);
                    DateTime lastModified = File.GetLastWriteTime(file);
                    string action = line;
                    string parameter = null;
                    string timeParam = null;

                    if (lastModified.AddSeconds(Properties.Settings.Default.FileEditedMargin) > DateTime.Now) {
                        //If file has been modified recently - check for action
                        MainProgram.DoDebug("File modified within the last " + Properties.Settings.Default.FileEditedMargin + " seconds...");

                        //Check for timestamp param - for future use (possibly) to calculate how long the request took -
                        //however Google Assistant doesn't return second-stamp, only hour and minute, so unless they implement it; can't be used
                        if (line.Contains("[") && line.Contains("]")) {
                            action = line.Split('[')[0];
                            timeParam = line.Split('[')[1];
                            timeParam.Replace("]", "");

                            if (timeParam.Contains("time;")) {
                                timeParam = timeParam.Split(';')[1];
                                MainProgram.DoDebug("Action has time parameter; " + timeParam);
                            }
                        }

                        if (action.Contains(":")) {
                            //Contains a parameter
                            action = line.Split(':')[0];
                            parameter = line.Split(':')[1];
                            if (parameter == "") parameter = null;
                        }

                        if (MainProgram.testingAction)
                            MainProgram.DoDebug("Test went through: " + action);

                        switch (action) {
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
                                    MainProgram.testActionMessage = shutdownParameters;
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
                                            restartParameters = !parameter.Contains("/r") ? "/r " : "" + parameter;
                                        } else {
                                            restartParameters = !parameter.Contains("/r") ? "/r " : "" + parameter + " /t 0";
                                        }
                                    }
                                }

                                if (MainProgram.testingAction) {
                                    MainProgram.testActionMessage = restartParameters;
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
                                if (MainProgram.testingAction) {

                                }
                                MainProgram.DoDebug("Putting computer to sleep...");

                                if (parameter == null) {
                                    Application.SetSuspendState(PowerState.Suspend, true, true);
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
                                    Application.SetSuspendState(PowerState.Suspend, doForce, true);
                                }

                                successMessage = "Put computer to sleep";
                                break;
                            case "logout":
                                //Logs out of the current user
                                MainProgram.DoDebug("Logging out of user...");

                                ExitWindowsEx(0, 0);
                                successMessage = "Logged out of user";
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

                                AudioManager.SetMasterVolumeMute(doMute);
                                successMessage = (doMute ? "Muted " : "Unmuted") + "pc";
                                break;
                            case "set_volume":
                                //Sets volume to a specific percent
                                //Requires parameter (percent, int)
                                if (requireParameter(parameter)) {
                                    if (double.TryParse(parameter, out double volumeLevel)) {
                                        if (volumeLevel >= 0 && volumeLevel <= 100) {
                                            if (Properties.Settings.Default.UnmuteOnVolumeChange) {
                                                AudioManager.SetMasterVolumeMute(false);
                                            }
                                            AudioManager.SetMasterVolume((float)volumeLevel);
                                            if (AudioManager.GetMasterVolume() != volumeLevel) {
                                                //Something went wrong... Audio not set to parameter-level
                                                MainProgram.DoDebug("ERROR: Volume was not sat");
                                                MainProgram.errorMessage = "Something went wrong when setting the volume";
                                            } else {
                                                successMessage = "Set volume to " + volumeLevel + "%";
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
                                if (requireParameter(parameter)) {
                                    switch (parameter) {
                                        case "previous":
                                            keybd_event(VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_EXTENTEDKEY, 0);
                                            successMessage = "MUSIC: Skipped song";
                                            break;
                                        /*case "previousx2": //WIP
                                            keybd_event(VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_EXTENTEDKEY, 0);
                                            keybd_event(VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_EXTENTEDKEY, 0);
                                            success_message = "MUSIC: Skipped song (x2)";
                                            break;*/
                                        case "next":
                                            keybd_event(VK_MEDIA_NEXT_TRACK, 0, KEYEVENTF_EXTENTEDKEY, 0);
                                            successMessage = "MUSIC: Next song";
                                            break;
                                        case "play_pause":
                                            keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENTEDKEY, 0);
                                            successMessage = "MUSIC: Played/Paused";
                                            break;
                                        default:
                                            MainProgram.DoDebug("ERROR: Unknown parameter");
                                            MainProgram.errorMessage = "Unknown parameter \"" + parameter + "\"";
                                            break;
                                    }
                                }
                                break;
                            case "open":
                                if (requireParameter(parameter)) {
                                    string fileLocation = Path.Combine(MainProgram.shortcutLocation, parameter);
                                    if (File.Exists(fileLocation)) {
                                        Process.Start(fileLocation);
                                        successMessage = "OPEN: opened file; " + fileLocation;
                                    } else {
                                        MainProgram.DoDebug("ERROR: file doesn't exist (" + fileLocation + ")");
                                        MainProgram.errorMessage = "File doesn't exist (" + fileLocation + ")";
                                    }
                                }
                                break;
                            case "die":
                                //Exit ACC
                                Application.Exit();
                                break;
                            default:
                                //Unknown action
                                MainProgram.DoDebug("ERROR: Unknown action");
                                MainProgram.errorMessage = "Unknown action \"" + action + "\"";
                                break;
                        }
                        if (successMessage != "") {
                            MainProgram.DoDebug("\nSUCCESS: " + successMessage + "\n");
                        }

                        if (MainProgram.testingAction) {
                            MainProgram.testActionWindow.ActionExecuted(successMessage, MainProgram.errorMessage, action, parameter);
                        }
                        successMessage = "";
                    } else {
                        MainProgram.DoDebug("No action set within the last " + Properties.Settings.Default.FileEditedMargin + " seconds. File last edited; " + lastModified + ". PC time; " + DateTime.Now);
                        MainProgram.errorMessage = "No action set lately";
                    }
                } else {
                    MainProgram.DoDebug("File is empty");
                    MainProgram.errorMessage = "No action set (file is empty)";
                }
                MainProgram.ClearFile(file);
                if (MainProgram.errorMessage.Length != 0 && !MainProgram.debug) {
                    MessageBox.Show(MainProgram.errorMessage, "Error | " + MainProgram.messageBoxTitle);
                    MainProgram.errorMessage = "";
                }
                MainProgram.isPerformingAction = false;
            } else {
                MainProgram.DoDebug("Already performing an action");
            }
        }
    }
}
