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
            if(File.Exists(file)) {
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
            return false;
        }
        
        static public void FileFound(object source, FileSystemEventArgs e) {
            string file = e.FullPath
                , action = null
                , parameter = null
                , fullContent = null;

            if (!MainProgram.isPerformingAction) {
                MainProgram.isPerformingAction = true;
                MainProgram.DoDebug("File exists, checking the content...");

                while (FileInUse(file));
                if (!File.Exists(file)) {
                    MainProgram.isPerformingAction = false;
                    return;
                }
                if (new FileInfo(file).Length != 0) {
                    MainProgram.DoDebug("Action set. File is not empty...");
                    action = "empty file";

                    string line = File.ReadAllText(file);
                    MainProgram.DoDebug("Read complete, content: " + line);
                    DateTime lastModified = File.GetLastWriteTime(file);
                    action = line;
                    fullContent = line;
                    string assistantParam = null;

                    if (lastModified.AddSeconds(Properties.Settings.Default.FileEditedMargin) > DateTime.Now) {
                        //If file has been modified recently - check for action
                        MainProgram.DoDebug("File modified within the last " + Properties.Settings.Default.FileEditedMargin + " seconds...");

                        //Whether it's Google Assistant or Amazon Alexa (included in the default IFTTT applets)
                        if (line.Contains("[") && line.Contains("]")) {
                            action = line.Split('[')[0];
                            assistantParam = line.Split('[')[1];
                            assistantParam = assistantParam.Split(']')[0];

                            MainProgram.DoDebug("Executed using; " + assistantParam);
                        }

                        if (action.Contains(":")) {
                            //Contains a parameter
                            action = line.Split(':')[0];
                            parameter = line.Split(':')[1];
                            if (parameter == "") parameter = null;
                        }

                        if (MainProgram.testingAction)
                            MainProgram.DoDebug("Test went through: " + action);

                        //TO-DO; Optimize action-execution (less code) & make it its own function
                        int? actionNumber = null;
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
                                if (requireParameter(parameter)) {
                                    if (double.TryParse(parameter, out double volumeLevel)) {
                                        if (volumeLevel >= 0 && volumeLevel <= 100) {
                                            if (!MainProgram.testingAction) {
                                                if (Properties.Settings.Default.UnmuteOnVolumeChange) {
                                                    AudioManager.SetMasterVolumeMute(false);
                                                }
                                                AudioManager.SetMasterVolume((float)volumeLevel);
                                            }
                                            if (AudioManager.GetMasterVolume() != volumeLevel) {
                                                //Something went wrong... Audio not set to parameter-level
                                                MainProgram.DoDebug("ERROR: Volume was not sat");
                                                MainProgram.errorMessage = "Something went wrong when setting the volume";
                                            } else {
                                                if (!MainProgram.testingAction) {
                                                    successMessage = "Set volume to " + volumeLevel + "%";
                                                } else {
                                                    successMessage = "Simulated setting system volume to " + volumeLevel + "%";
                                                }
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
                                            actionNumber = 8;

                                            if (MainProgram.testingAction) {
                                                successMessage = "MUSIC: Simulated skipping song";
                                            } else {
                                                keybd_event(VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_EXTENTEDKEY, 0);
                                                successMessage = "MUSIC: Skipped song";
                                            }
                                            break;
                                        /*case "previousx2": //WIP
                                            keybd_event(VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_EXTENTEDKEY, 0);
                                            keybd_event(VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_EXTENTEDKEY, 0);
                                            success_message = "MUSIC: Skipped song (x2)";
                                            break;*/
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
                                if (requireParameter(parameter)) {
                                    string fileLocation = (!parameter.Contains(@":\")) ? Path.Combine(MainProgram.shortcutLocation, parameter) : parameter;
                                    if (File.Exists(fileLocation)) {
                                        if (!MainProgram.testingAction) {
                                            Process.Start(fileLocation);
                                            successMessage = "OPEN: opened file; " + fileLocation;
                                        } else {
                                            successMessage = "OPEN: simulated opening file; " + fileLocation;
                                        }
                                    } else {
                                        MainProgram.DoDebug("ERROR: file doesn't exist (" + fileLocation + ")");
                                        MainProgram.errorMessage = "File doesn't exist (" + fileLocation + ")";
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
                            default:
                                //Unknown action
                                MainProgram.DoDebug("ERROR: Unknown action");
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
                    } else {
                        MainProgram.DoDebug("No action set within the last " + Properties.Settings.Default.FileEditedMargin + " seconds. File last edited; " + lastModified + ". PC time; " + DateTime.Now);
                        MainProgram.errorMessage = "No action set within the last " + Properties.Settings.Default.FileEditedMargin + " seconds. File last edited; " + lastModified + ". PC time; " + DateTime.Now;
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
                MainProgram.errorMessage = "Already performing an action";
            }

            if (MainProgram.testingAction) {
                MainProgram.testActionWindow.ActionExecuted(successMessage, MainProgram.errorMessage, action, parameter, fullContent);
            }
            successMessage = "";
        }
    }
}