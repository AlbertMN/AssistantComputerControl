/*
 * AssistantComputerControl
 * Made by Albert MN.
 * Updated: v1.4.0, 15-01-2020
 * 
 * Use:
 * - Cleans action files up after they've been processed
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                Thread.Sleep(150);

                if (isCleaning) {
                    MainProgram.DoDebug("[CLEANUP] Another cleanup service in progress. Waiting...");
                    while (isCleaning) {
                        Thread.Sleep(200);
                    }
                    MainProgram.DoDebug("[CLEANUP] Other cleanup service done - starting check...");
                }
                isCleaning = true;

                if (AllHiddenCheck() != 0 && EmptyCheck()) {
                    int tries = 0;
                    while (!Check() && tries <= 10) {
                        tries++;
                        Thread.Sleep(1000);
                    }

                    if (tries >= 10) {
                        MainProgram.DoDebug("[CLEANUP] Timeout. Failed to remove files in action folder.");
                    } else {
                        int filesAmount = AllHiddenCheck();
                        if (filesAmount != 0) {
                            MainProgram.DoDebug("[CLEANUP] Did not timeout, but action folder is still not empty (" + filesAmount.ToString() + " non-hidden files in folder) - not supposed to happen Emtpy check returns " + (EmptyCheck() ? "true" : "false"));
                        } else {
                            MainProgram.DoDebug("[CLEANUP] Successful");
                        }
                    }
                } else {
                    MainProgram.DoDebug("[CLEANUP] Action folder is completely empty");
                }

                /* Launch a follow-up investigation */
                new Thread(() => {
                    Thread.CurrentThread.IsBackground = true;

                    void ExtraCleanup() {
                        MainProgram.DoDebug("[CLEANUP] Running extra cleanup (followup)");

                        try {
                            var ps1File = Path.Combine(MainProgram.currentLocation, "ExtraCleanupper.ps1");

                            Process p = new Process();
                            p.StartInfo.FileName = "powershell.exe";
                            p.StartInfo.Arguments = $"-WindowStyle Hidden -file \"{ps1File}\" \"{Path.Combine(MainProgram.CheckPath(), "*")}\" \"*.{Properties.Settings.Default.ActionFileExtension}\"";
                            p.StartInfo.UseShellExecute = false;
                            p.StartInfo.CreateNoWindow = true;
                            p.Start();
                        } catch {
                            MainProgram.DoDebug("[CLEANUP] Extra checkup failed");
                        }
                    }

                    Thread.Sleep(5000);
                    ExtraCleanup();
                    Thread.Sleep(25000);
                    ExtraCleanup();
                }).Start();

                isCleaning = false;
            }).Start();
        }

        private bool EmptyCheck() {
            return Directory.GetFiles(MainProgram.CheckPath()).Length > 0;
        }

        private int AllHiddenCheck() {
            int count = 0;
            foreach (string file in Directory.GetFiles(MainProgram.CheckPath(), "*." + Properties.Settings.Default.ActionFileExtension)) {
                bool hidden = (File.GetAttributes(file) & FileAttributes.Hidden) == FileAttributes.Hidden;
                if (!hidden || file.Contains("computerAction")) {
                    //MainProgram.DoDebug("[CLEANUP] Found file; " + file);
                    count++;
                }
            }
            return count;
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
                        //string newFilename = Path.Combine(tmpFolder, "action_" + DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() + "_" + Guid.NewGuid() + "." + Properties.Settings.Default.ActionFileExtension);
                        //string newFilename = Path.Combine(Path.Combine(MainProgram.CheckPath(), "used_actions"), "action_" + DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() + "_" + Guid.NewGuid() + "." + Properties.Settings.Default.ActionFileExtension);
                        //File.Move(file, newFilename);
                        //File.Delete(newFilename);
                        File.Delete(file);
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