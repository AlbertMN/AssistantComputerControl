using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Threading;
using System.Reflection;

namespace HomeComputerControl {
    class Program {
        static string currentLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        static private string filePath = currentLocation + "/computerAction.txt";
        static string error_message = "";

        //Logout
        [DllImport("user32.dll", SetLastError = true)]
        static extern int ExitWindowsEx(uint uFlags, uint dwReason);

        //Lock
        [DllImport("user32.dll")]
        public static extern bool LockWorkStation();

        static void clearFile() {
            if (File.Exists(filePath)) {
                File.Delete(filePath);
            }
        }

        static void Main(string[] args) {
            clearFile();
            while (true) {
                if (File.Exists(filePath)) {
                    if (new FileInfo(filePath).Length != 0) {
                        string line = File.ReadAllLines(filePath)[0];
                        DateTime lastModified = File.GetLastWriteTime(filePath);
                        string action;
                        string parameter = null;

                        if(lastModified.AddSeconds(30) > DateTime.UtcNow) {
                            //If file has been modified recently - check for action
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
                                    Process.Start("shutdown", "/s /t 0");
                                    break;
                                case "restart":

                                    //Restart the computer
                                    Process.Start("shutdown", "/r /t 0");
                                    break;
                                case "sleep":

                                    //Puts computer to sleep
                                    Application.SetSuspendState(PowerState.Suspend, true, true);
                                    break;
                                case "logout":

                                    //Logs out of the current user
                                    ExitWindowsEx(0, 0);
                                    break;
                                case "lock":

                                    //Lock computer
                                    LockWorkStation();
                                    break;
                                case "mute":

                                    //Mutes windows
                                    error_message = "This feature is not supported yet";
                                    break;
                                case "set_volume":
                                    //Requires parameter
                                    if (parameter != null) {

                                    } else {
                                        error_message = "Parameter not set";
                                    }
                                    break;
                                default:
                                    //Unknown action
                                    error_message = "Unknown action \"" + action + "\"";
                                    break;
                            }
                        } else {
                            error_message = "No action set lately";
                        }
                    } else {
                        error_message = "No action set (file is empty)";
                    }
                    clearFile();
                    if (error_message.Length != 0) {
                        MessageBox.Show(error_message, "Error | HomeComputerControl");
                        error_message = "";
                    }
                } else {
                    //File doesn't exist... On hold
                }
                Thread.Sleep(500);
            }
        }
    }
}
