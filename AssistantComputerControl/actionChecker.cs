using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using AudioSwitcher.AudioApi.CoreAudio;

namespace AssistantComputerControl {
    class ActionChecker {
        private static CoreAudioController coreAudio = null;
        private static CoreAudioDevice defaultPlaybackDevice;
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
        private static void updateDefaultPlaybackDevice() {
            if(coreAudio == null) {
                coreAudio = new CoreAudioController();
                defaultPlaybackDevice = coreAudio.DefaultPlaybackDevice;
            }

            if (!defaultPlaybackDevice.IsDefaultDevice) {
                MainProgram.DoDebug("Setting new default device");
                defaultPlaybackDevice = coreAudio.DefaultPlaybackDevice;
            } else {
                MainProgram.DoDebug("Same default device");
            }
        }
        public static bool fileInUse(string file) {
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

                while (fileInUse(file));
                if (new FileInfo(file).Length != 0) {
                    MainProgram.DoDebug("Action set. File is not empty...");

                    string line = File.ReadAllText(file);
                    MainProgram.DoDebug("Read complete, content: " + line);
                    DateTime lastModified = File.GetLastWriteTime(file);
                    string action;
                    string parameter = null;

                    if (lastModified.AddSeconds(MainProgram.fileEditedSecondMargin) > DateTime.Now) {
                        //If file has been modified recently - check for action
                        MainProgram.DoDebug("File modified within the last " + MainProgram.fileEditedSecondMargin + " seconds...");

                        if (line.Contains(":")) {
                            //Contains a parameter
                            action = line.Split(':')[0];
                            parameter = line.Split(':')[1];
                        } else {
                            action = line;
                        }

                        switch (action) {
                            case "shutdown":
                                //Shuts down the computer
                                MainProgram.DoDebug("Shutting down computer...");
                                if (parameter == null) {
                                    Process.Start("shutdown", "/s /t 0");
                                } else {
                                    if (parameter.Contains("/t")) {
                                        Process.Start("shutdown", "/s " + parameter);
                                    } else {
                                        Process.Start("shutdown", "/s " + parameter + " /t 0");
                                    }
                                }
                                successMessage = "Shutting down";
                                break;
                            case "restart":
                                //Restart the computer
                                MainProgram.DoDebug("Restarting computer...");

                                if (parameter == null) {
                                    Process.Start("shutdown", "/r /t 0");
                                } else {
                                    if(parameter == "abort") {
                                        Process.Start("shutdown", "/a");
                                    } else {
                                        if (parameter.Contains("/t")) {
                                            Process.Start("shutdown", "/r " + parameter);
                                        } else {
                                            Process.Start("shutdown", "/r " + parameter + " /t 0");
                                        }
                                    }
                                }
                                successMessage = "Restarting";
                                break;
                            case "sleep":
                                //Puts computer to sleep
                                MainProgram.DoDebug("Sleeping computer...");

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
                                MainProgram.DoDebug("Locking computer...");

                                LockWorkStation();
                                successMessage = "Locked pc";
                                break;
                            case "mute":
                                //Mutes windows
                                //Parameter optional (true/false)
                                updateDefaultPlaybackDevice();
                                bool doMute = false;

                                if (parameter == null) {
                                    //No parameter - toggle
                                    doMute = !defaultPlaybackDevice.IsMuted;
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
                                defaultPlaybackDevice.Mute(!defaultPlaybackDevice.IsMuted);
                                successMessage = (defaultPlaybackDevice.IsMuted ? "Muted " : "Unmuted") + "pc";
                                break;
                            case "set_volume":
                                //Sets volume to a specific percent
                                //Requires parameter (percent, int)
                                updateDefaultPlaybackDevice();

                                if (requireParameter(parameter)) {
                                    double volumeLevel;
                                    if (double.TryParse(parameter, out volumeLevel)) {
                                        if (volumeLevel >= 0 && volumeLevel <= 100) {
                                            if (MainProgram.unmuteVolumeChange) {
                                                defaultPlaybackDevice.Mute(false);
                                            }
                                            defaultPlaybackDevice.Volume = volumeLevel;
                                            if (defaultPlaybackDevice.Volume != volumeLevel) {
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
                    } else {
                        MainProgram.DoDebug("No action set within the last " + MainProgram.fileEditedSecondMargin + " seconds. File last edited; " + lastModified + ". PC time; " + DateTime.Now);
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
