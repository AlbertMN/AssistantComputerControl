using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AssistantComputerControl {
    class CleanupService {
        public static bool isCleaning = false;
        //public static List<string> cleanedFiles = new List<string>();

        public void Start() {
            new Thread(() => {
                Thread.CurrentThread.IsBackground = true;
                MainProgram.DoDebug("[CLEANUP] Service started");

                if (isCleaning) {
                    MainProgram.DoDebug("[CLEANUP] Another cleanup service in progress. Waiting...");
                    while (isCleaning) {
                        Thread.Sleep(200);
                    }
                    MainProgram.DoDebug("[CLEANUP] Other cleanup service done - starting check...");
                }
                isCleaning = true;

                int tries = 0;
                while (!Check() && tries <= 10) {
                    tries++;
                    Thread.Sleep(1000);
                }

                if (tries >= 10) {
                    MainProgram.DoDebug("[CLEANUP] Timeout. Failed to remove files in action folder.");
                } else {
                    if (!AllHiddenCheck()) {
                        MainProgram.DoDebug("[CLEANUP] Did not timeout, but action folder is still not empty - not supposed to happen");
                    } else {
                        MainProgram.DoDebug("[CLEANUP] Successful");
                    }
                }
                isCleaning = false;
            }).Start();
        }

        private bool EmptyCheck() {
            return Directory.GetFiles(MainProgram.CheckPath()).Length > 0;
        }

        private bool AllHiddenCheck() {
            int count = 0;
            foreach (string file in Directory.GetFiles(MainProgram.CheckPath(), "*." + Properties.Settings.Default.ActionFileExtension)) {
                bool hidden = (File.GetAttributes(file) & FileAttributes.Hidden) == FileAttributes.Hidden;
                if (!hidden || file.Contains("computerAction")) {
                    count++;
                }
            }
            return count == 0;
        }
        
        private bool Check() {
            if (EmptyCheck()) {
                DirectoryInfo di = new DirectoryInfo(MainProgram.CheckPath());

                int numFiles = 0;

                try {
                    foreach (string file in Directory.GetFiles(MainProgram.CheckPath(), "*." + Properties.Settings.Default.ActionFileExtension)) {
                        //if (cleanedFiles.Contains(file)) continue;

                        bool hidden = (File.GetAttributes(file) & FileAttributes.Hidden) == FileAttributes.Hidden;
                        if (!hidden) {
                            File.SetAttributes(file, FileAttributes.Hidden);
                        }

                        string tmpFolder = Path.Combine(Path.GetTempPath(), "AssistantComputerControl");
                        if (!Directory.Exists(tmpFolder)) {
                            Directory.CreateDirectory(tmpFolder);
                        }
                        
                        //string newFilename = Path.Combine(Path.GetDirectoryName(file), "action_" + DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() + "_" + Guid.NewGuid() + "." + Properties.Settings.Default.ActionFileExtension);
                        string newFilename = Path.Combine(tmpFolder, "action_" + DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() + "_" + Guid.NewGuid() + "." + Properties.Settings.Default.ActionFileExtension);
                        File.Move(file, newFilename);
                        File.Delete(newFilename);
                        //cleanedFiles.Add(newFilename);
                        numFiles++;
                    }
                } catch (Exception e) {
                    MainProgram.DoDebug("[CLEANUP] Failed to rename a file in action folder; " + e.Message + ". Successfully renamed and hid " + numFiles.ToString() + " files");
                    return false;
                }

                if (numFiles > 0) {
                    Thread.Sleep(1000);
                }

                return true;
                /*if (!EmptyCheck()) {
                    MainProgram.DoDebug("[CLEANUP] All action folder files successfully renamed and hid (" + numFiles.ToString() + " files)");
                    return true;
                } else {
                    MainProgram.DoDebug("[CLEANUP] Folder wasn't cleared");
                }*/
            } else {
                MainProgram.DoDebug("[CLEANUP] Check done - action folder is empty");
            }

            return false;
        }
    }
}