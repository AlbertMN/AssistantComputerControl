using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using AudioSwitcher.AudioApi.CoreAudio;

namespace HomeComputerControl {
    class actionChecker {
        private string success_message = "";

        //Logout
        [DllImport("user32.dll", SetLastError = true)]
        static extern int ExitWindowsEx(uint uFlags, uint dwReason);

        //Lock
        [DllImport("user32.dll")]
        public static extern bool LockWorkStation();

        //Music-control
        public const int KEYEVENTF_EXTENTEDKEY = 1;
        public const int KEYEVENTF_KEYUP = 0;
        public const int VK_MEDIA_NEXT_TRACK = 0xB0;// code to jump to next track
        public const int VK_MEDIA_PLAY_PAUSE = 0xB3;// code to play or pause a song
        public const int VK_MEDIA_PREV_TRACK = 0xB1;// code to jump to prev track
        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        private bool requireParameter(string param) {
            if(param != null) {
                return true;
            } else {
                MainProgram.doDebug("ERROR: Parameter not set");
                MainProgram.error_message = "Parameter not set";
            }
            return false;
        }

        public void check() {
            if (MainProgram.debug)
            MainProgram.doDebug("Initiating check-loop...");

            while (true) {
                if (File.Exists(MainProgram.actionFilePath)) {
                    MainProgram.doDebug("File exists, checking the content...");

                    if (new FileInfo(MainProgram.actionFilePath).Length != 0) {
                        MainProgram.doDebug("Action set. File is not empty...");

                        string line = File.ReadAllLines(MainProgram.actionFilePath)[0];
                        DateTime lastModified = File.GetLastWriteTime(MainProgram.actionFilePath);
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

                            //Playback device
                            CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;

                            switch (action) {
                                case "shutdown":
                                    //Shuts down the computer
                                    MainProgram.doDebug("Shutting down computer...");

                                    Process.Start("shutdown", "/s /t 0");
                                    success_message = "Shutdown computer";
                                    break;
                                case "restart":
                                    //Restart the computer
                                    MainProgram.doDebug("Restarting computer...");

                                    Process.Start("shutdown", "/r /t 0");
                                    break;
                                case "sleep":
                                    //Puts computer to sleep
                                    MainProgram.doDebug("Sleeping computer...");
                                    Application.SetSuspendState(PowerState.Suspend, true, true);

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
                                    bool doMute = false;

                                    if(parameter == null) {
                                        //No parameter - toggle
                                        doMute = !defaultPlaybackDevice.IsMuted;
                                    } else {
                                        //Parameter set;
                                        switch(parameter) {
                                            case "true":
                                                doMute = true;
                                                break;
                                            case "false":
                                                doMute = false;
                                                break;
                                            default:
                                                MainProgram.doDebug("ERROR: Parameter (" + parameter + ") is invalid for \"mute\". Accepted parameters: \"true\" and \"false\"");
                                                MainProgram.error_message = "Parameter \"" + parameter + "\" is invalid for the \"mute\" action. Accepted parameters: \"true\" and \"false\")";
                                                break;
                                        }
                                    }

                                    defaultPlaybackDevice.Mute(!defaultPlaybackDevice.IsMuted);
                                    success_message = (defaultPlaybackDevice.IsMuted ? "Muted " : "Unmuted") + "pc";
                                    break;
                                case "set_volume":
                                    //Sets volume to a specific percent
                                    //Requires parameter (percent, int)
                                    if (requireParameter(parameter)) {
                                        double volume_level;
                                        if(double.TryParse(parameter, out volume_level)) {
                                            if(volume_level >= 0 && volume_level <= 100) {
                                                if(MainProgram.unmute_volume_change) {
                                                    defaultPlaybackDevice.Mute(false);
                                                }
                                                defaultPlaybackDevice.Volume = volume_level;
                                                if (defaultPlaybackDevice.Volume != volume_level) {
                                                    //Something went wrong... Audio not set to parameter-level
                                                    MainProgram.doDebug("ERROR: Volume was not sat");
                                                    MainProgram.error_message = "Something went wrong when setting the volume";
                                                } else {
                                                    success_message = "Set volume to " + volume_level + "%";
                                                }
                                            } else {
                                                MainProgram.doDebug("ERROR: Parameter is an invalid number, range; 0-100 (" + volume_level + ")");
                                                MainProgram.error_message = "Can't set volume to " + volume_level + "%, has to be a number from 0-100";
                                            }
                                        } else {
                                            MainProgram.doDebug("ERROR: Parameter (" + parameter + ") not convertable to double");
                                            MainProgram.error_message = "Not a valid parameter (has to be a number)";
                                        }
                                    }
                                    break;

                                case "music":
                                    if(requireParameter(parameter)) {
                                        switch(parameter) {
                                            case "previous":
                                                keybd_event(VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_EXTENTEDKEY, 0);
                                                success_message = "MUSIC: Skipped song";
                                                break;
                                            /*case "previousx2": //in the works...
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
                                                MainProgram.error_message = "Unknown parameter \"" + parameter + "\"";
                                                break;
                                        }
                                    }
                                    break;
                                default:
                                    //Unknown action
                                    MainProgram.doDebug("ERROR: Unknown action");
                                    MainProgram.error_message = "Unknown action \"" + action + "\"";
                                    break;
                            }

                            if(success_message != "") {
                                MainProgram.doDebug("\nSUCCESS: " + success_message + "\n");
                            }
                        } else {
                            MainProgram.doDebug("No action set within the last " + MainProgram.fileEditedSecondMargin + " seconds");
                            MainProgram.error_message = "No action set lately";
                        }
                    } else {
                        MainProgram.doDebug("File is empty");
                        MainProgram.error_message = "No action set (file is empty)";
                    }
                    MainProgram.clearFile();
                    if (MainProgram.error_message.Length != 0 && !MainProgram.debug) {
                        MessageBox.Show(MainProgram.error_message, "Error | HomeComputerControl");
                        MainProgram.error_message = "";
                    }
                } else {
                    //File doesn't exist... Check again after wait
                }
                Thread.Sleep(MainProgram.sleepTime);
            }
        }
    }
}
