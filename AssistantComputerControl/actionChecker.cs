using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using AudioSwitcher.AudioApi.CoreAudio;

namespace AssistantComputerControl {
    class actionChecker {
        private static CoreAudioController coreAudio = null;
        private static CoreAudioDevice defaultPlaybackDevice;
        private static string success_message = "";

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
                MainProgram.doDebug("ERROR: Parameter not set");
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
                MainProgram.doDebug("Setting new default device");
                defaultPlaybackDevice = coreAudio.DefaultPlaybackDevice;
            } else {
                MainProgram.doDebug("Same default device");
            }
        }
        private static void WaitForFile(FileInfo file) {
            FileStream stream = null;
            bool FileReady = false;
            while (!FileReady) {
                try {
                    using (stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None)) {
                        MainProgram.doDebug("Can be used");
                        FileReady = true;
                    }
                } catch (IOException) {
                    //File isn't ready yet, so we need to keep on waiting until it is.
                }
                //We'll want to wait a bit between polls, if the file isn't ready.
                if (!FileReady) Thread.Sleep(200);
            }
        }

        static public void fileFound(object source, FileSystemEventArgs e) {
            if (!MainProgram.is_performing_action) {
                MainProgram.is_performing_action = true;
                WaitForFile(new FileInfo(MainProgram.actionFilePath));

                MainProgram.doDebug("File exists, checking the content...");

                if (File.Exists(MainProgram.actionFilePath)) {
                    if (new FileInfo(MainProgram.actionFilePath).Length != 0) {
                        MainProgram.doDebug("Action set. File is not empty...");

                        string line = File.ReadAllLines(MainProgram.actionFilePath)[0];
                        DateTime lastModified = File.GetLastWriteTime(MainProgram.actionFilePath);
                        MainProgram.clearFile();
                        string action;
                        string parameter = null;

                        if (lastModified.AddSeconds(MainProgram.fileEditedSecondMargin) > DateTime.UtcNow) {
                            //If file has been modified recently - check for action
                            MainProgram.doDebug("File modified within the last " + MainProgram.fileEditedSecondMargin + " seconds...");

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

                                    MainProgram.doDebug("Shutting down computer...");
                                    if (parameter == null) {
                                        Process.Start("shutdown", "/s /t 0");
                                    } else {
                                        if (parameter.Contains("/t")) {
                                            Process.Start("shutdown", "/s " + parameter);
                                        } else {
                                            Process.Start("shutdown", "/s " + parameter + " /t 0");
                                        }
                                    }
                                    success_message = "Shutting down";
                                    break;
                                case "restart":
                                    //Restart the computer
                                    MainProgram.doDebug("Restarting computer...");

                                    if (parameter == null) {
                                        Process.Start("shutdown", "/r /t 0");
                                    } else {
                                        if (parameter.Contains("/t")) {
                                            Process.Start("shutdown", "/r " + parameter);
                                        } else {
                                            Process.Start("shutdown", "/r " + parameter + " /t 0");
                                        }
                                    }
                                    success_message = "Restarting";
                                    break;
                                case "sleep":
                                    //Puts computer to sleep
                                    MainProgram.doDebug("Sleeping computer...");

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
                                                MainProgram.doDebug("ERROR: Parameter (" + parameter + ") is invalid for \"" + action + "\". Accepted parameters: \"true\" and \"false\"");
                                                MainProgram.errorMessage = "Parameter \"" + parameter + "\" is invalid for the \"" + action + "\" action. Accepted parameters: \"true\" and \"false\")";
                                                break;
                                        }
                                        Application.SetSuspendState(PowerState.Suspend, doForce, true);
                                    }

                                    success_message = "Put computer to sleep";
                                    break;
                                case "logout":
                                    //Logs out of the current user
                                    MainProgram.doDebug("Logging out of user...");

                                    ExitWindowsEx(0, 0);
                                    success_message = "Logged out of user";
                                    break;
                                case "lock":
                                    //Lock computer
                                    MainProgram.doDebug("Locking computer...");

                                    LockWorkStation();
                                    success_message = "Locked pc";
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
                                                MainProgram.doDebug("ERROR: Parameter (" + parameter + ") is invalid for \"" + action + "\". Accepted parameters: \"true\" and \"false\"");
                                                MainProgram.errorMessage = "Parameter \"" + parameter + "\" is invalid for the \"" + action + "\" action. Accepted parameters: \"true\" and \"false\")";
                                                break;
                                        }
                                    }
                                    defaultPlaybackDevice.Mute(!defaultPlaybackDevice.IsMuted);
                                    success_message = (defaultPlaybackDevice.IsMuted ? "Muted " : "Unmuted") + "pc";
                                    break;
                                case "set_volume":
                                    //Sets volume to a specific percent
                                    //Requires parameter (percent, int)
                                    updateDefaultPlaybackDevice();

                                    if (requireParameter(parameter)) {
                                        double volume_level;
                                        if (double.TryParse(parameter, out volume_level)) {
                                            if (volume_level >= 0 && volume_level <= 100) {
                                                if (MainProgram.unmute_volume_change) {
                                                    defaultPlaybackDevice.Mute(false);
                                                }
                                                defaultPlaybackDevice.Volume = volume_level;
                                                if (defaultPlaybackDevice.Volume != volume_level) {
                                                    //Something went wrong... Audio not set to parameter-level
                                                    MainProgram.doDebug("ERROR: Volume was not sat");
                                                    MainProgram.errorMessage = "Something went wrong when setting the volume";
                                                } else {
                                                    success_message = "Set volume to " + volume_level + "%";
                                                }
                                            } else {
                                                MainProgram.doDebug("ERROR: Parameter is an invalid number, range; 0-100 (" + volume_level + ")");
                                                MainProgram.errorMessage = "Can't set volume to " + volume_level + "%, has to be a number from 0-100";
                                            }
                                        } else {
                                            MainProgram.doDebug("ERROR: Parameter (" + parameter + ") not convertable to double");
                                            MainProgram.errorMessage = "Not a valid parameter (has to be a number)";
                                        }
                                    }
                                    break;

                                case "music":
                                    if (requireParameter(parameter)) {
                                        switch (parameter) {
                                            case "previous":
                                                keybd_event(VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_EXTENTEDKEY, 0);
                                                success_message = "MUSIC: Skipped song";
                                                break;
                                            /*case "previousx2": //WIP
                                                keybd_event(VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_EXTENTEDKEY, 0);
                                                keybd_event(VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_EXTENTEDKEY, 0);
                                                success_message = "MUSIC: Skipped song (x2)";
                                                break;*/
                                            case "next":
                                                keybd_event(VK_MEDIA_NEXT_TRACK, 0, KEYEVENTF_EXTENTEDKEY, 0);
                                                success_message = "MUSIC: Next song";
                                                break;
                                            case "play_pause":
                                                keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENTEDKEY, 0);
                                                success_message = "MUSIC: Played/Paused";
                                                break;
                                            default:
                                                MainProgram.doDebug("ERROR: Unknown parameter");
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
                                            success_message = "OPEN: opened file; " + fileLocation;
                                        } else {
                                            MainProgram.doDebug("ERROR: file doesn't exist (" + fileLocation + ")");
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
                                    MainProgram.doDebug("ERROR: Unknown action");
                                    MainProgram.errorMessage = "Unknown action \"" + action + "\"";
                                    break;
                            }
                            if (success_message != "") {
                                MainProgram.doDebug("\nSUCCESS: " + success_message + "\n");
                            }
                        } else {
                            MainProgram.doDebug("No action set within the last " + MainProgram.fileEditedSecondMargin + " seconds");
                            MainProgram.errorMessage = "No action set lately";
                        }
                    } else {
                        MainProgram.doDebug("File is empty");
                        MainProgram.errorMessage = "No action set (file is empty)";
                    }
                    MainProgram.clearFile();
                    if (MainProgram.errorMessage.Length != 0 && !MainProgram.debug) {
                        MessageBox.Show(MainProgram.errorMessage, "Error | " + MainProgram.messageBoxTitle);
                        MainProgram.errorMessage = "";
                    }
                } else {
                    MainProgram.doDebug("Action file doesn't exist");
                    MainProgram.errorMessage = "Action file doesn't exist";
                }
                MainProgram.is_performing_action = false;
            } else {
                MainProgram.doDebug("Already performing an action");
            }
        }
    }
}
