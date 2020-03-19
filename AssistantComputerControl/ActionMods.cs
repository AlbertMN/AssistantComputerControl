using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace AssistantComputerControl {
    class ActionMods {
        private static Dictionary<string, string> modActions = null;
        private static string validateErrMsg = "";

        //Check mod folder and init mods
        public static void CheckMods() {
            try {
                modActions = new Dictionary<string, string>();
                string[] dirs = Directory.GetDirectories(MainProgram.actionModsPath, "*", SearchOption.TopDirectoryOnly);

                foreach (string dir in dirs) {
                    string theFile = Path.Combine(dir, "info.json");
                    if (File.Exists(theFile)) {
                        //Info file exists - read it
                        string fileContent = ReadInfoFile(theFile);
                        if (fileContent != null) {
                            ValidateAddMod(fileContent, dir);
                        } else {
                            MainProgram.DoDebug("Failed to read info.json file at; " + dir);
                        }
                    } else {
                        MainProgram.DoDebug("Invalid folder in action mods; '" + dir + "'. Doesn't contain an info.json file.");
                    }
                }
            } catch (Exception e) {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }

        //Validate & init
        private static bool ValidateAddMod(string fileContent, string dir) {
            try {
                dynamic jsonTest = JsonConvert.DeserializeObject<dynamic>(fileContent);
                if (ValidateInfoJson(jsonTest)) {
                    string scriptFile = jsonTest["options"]["file_name"], scriptFileLocation = Path.Combine(dir, scriptFile);
                    if (File.Exists(scriptFileLocation)) {
                        string actionName = jsonTest["action_name"];
                        if (!ModActionExists(actionName)) {
                            MainProgram.DoDebug("[Mod loaded] " + jsonTest["title"] + " v" + jsonTest["version"] + " (" + actionName + ")");

                            modActions.Add(actionName, new DirectoryInfo(dir).Name);

                            ExecuteModAction(actionName); //Test
                        } else {
                            MainProgram.DoDebug("[Mod init error] A mod with this name (" + actionName + ") is already loaded (no dublicates allowed)");
                        }
                    } else {
                        MainProgram.DoDebug("[Mod init error] Action mod script doesn't exist at; " + scriptFileLocation);
                    }
                } else {
                    MainProgram.DoDebug("[Mod init error] " + validateErrMsg);
                }
            } catch (Exception e) {
                MainProgram.DoDebug("[Mod init error] Failed parse JSON from info.json file at; " + dir + ". Exception; " + e.Message);
            }

            return false;
        }

        //Validate the JSON
        private static bool ValidateInfoJson(dynamic theJson) {
            if (theJson["action_name"] != null && theJson["options"] != null && theJson["title"] != null && theJson["version"] != null) {
                if (theJson["options"]["file_name"] != null && theJson["options"]["requires_param"] != null && theJson["options"]["require_second_param"] != null) {
                    string scriptFile = theJson["options"]["file_name"];
                    if (!scriptFile.Contains("/") && !scriptFile.Contains(@"\") && !scriptFile.Contains("..")) {
                        return true;
                    } else {
                        validateErrMsg = "Invalid info.json action mod script; file has to be located within the action mod folder";
                    }
                } else {
                    validateErrMsg = "Invalid info.json action mod file; " + (theJson) + ". Missing some required options.";
                }
            } else {
                validateErrMsg = "Invalid info.json action mod file; " + (theJson) + ". Missing some required settings.";
            }
            return false;
        } 

        public static bool ModActionExists(string name) {
            return modActions.ContainsKey(name);
        }

        //Get python path & compare versions
        private static string GetPythonPath(string requiredVersion = "", string maxVersion = "") {
            string[] possiblePythonLocations = new string[3] {
                @"HKLM\SOFTWARE\Python\PythonCore\",
                @"HKCU\SOFTWARE\Python\PythonCore\",
                @"HKLM\SOFTWARE\Wow6432Node\Python\PythonCore\"
            };
            //Version number, install path
            Dictionary<string, string> pythonLocations = new Dictionary<string, string>();

            foreach (string possibleLocation in possiblePythonLocations) {
                string regKey = possibleLocation.Substring(0, 4), actualPath = possibleLocation.Substring(5);
                RegistryKey theKey = (regKey == "HKLM" ? Registry.LocalMachine : Registry.CurrentUser);
                RegistryKey theValue = theKey.OpenSubKey(actualPath);

                foreach (var v in theValue.GetSubKeyNames()) {
                    RegistryKey productKey = theValue.OpenSubKey(v);
                    if (productKey != null) {
                        try {
                            string pythonExePath = productKey.OpenSubKey("InstallPath").GetValue("ExecutablePath").ToString();
                            if (pythonExePath != null && pythonExePath != "") {
                                //Console.WriteLine("Got python version; " + v + " at path; " + pythonExePath);
                                pythonLocations.Add(v.ToString(), pythonExePath);
                            }
                        } catch {
                            //Install path doesn't exist
                        }
                    }
                }
            }

            if (pythonLocations.Count > 0) {
                System.Version desiredVersion = new System.Version(requiredVersion == "" ? "0.0.1" : requiredVersion),
                    maxPVersion = new System.Version(maxVersion == "" ? "999.999.999" : maxVersion);

                string highestVersion = "", highestVersionPath = "";

                foreach (KeyValuePair<string, string> pVersion in pythonLocations) {
                    //TODO; if on 64-bit machine, prefer the 64 bit version over 32 and vice versa
                    int index = pVersion.Key.IndexOf("-"); //For x-32 and x-64 in version numbers
                    string formattedVersion = index > 0 ? pVersion.Key.Substring(0, index) : pVersion.Key;

                    System.Version thisVersion = new System.Version(formattedVersion);
                    int comparison = desiredVersion.CompareTo(thisVersion),
                        maxComparison = maxPVersion.CompareTo(thisVersion);

                    if (comparison <= 0) {
                        //Version is greater or equal
                        if (maxComparison >= 0) {
                            desiredVersion = thisVersion;

                            highestVersion = pVersion.Key;
                            highestVersionPath = pVersion.Value;
                        } else {
                            //Console.WriteLine("Version is too high; " + maxComparison.ToString());
                        }
                    } else {
                        //Console.WriteLine("Version (" + pVersion.Key + ") is not within the spectrum.");
                    }
                }

                //Console.WriteLine(highestVersion);
                //Console.WriteLine(highestVersionPath);
                return highestVersionPath;
            }

            return "";
        }

        //Execute mod
        public static void ExecuteModAction(string name, string parameter = "", string secondaryParameter = "") {
            MainProgram.DoDebug("\nRunning MOD ACTION!\n");

            string modLocation = Path.Combine(MainProgram.actionModsPath, modActions[name]), infoJsonFile = Path.Combine(modLocation, "info.json");
            if (File.Exists(infoJsonFile)) {
                string modFileContent = ReadInfoFile(infoJsonFile);
                if (modFileContent != null) {
                    try {
                        dynamic jsonTest = JsonConvert.DeserializeObject<dynamic>(modFileContent);
                        if (jsonTest != null) {
                            if (ValidateInfoJson(jsonTest)) {
                                //JSON is valid - get script file
                                string scriptFile = jsonTest["options"]["file_name"], scriptFileLocation = Path.Combine(modLocation, scriptFile);
                                if (File.Exists(scriptFileLocation)) {
                                    try {
                                        ProcessStartInfo p = new ProcessStartInfo {
                                            UseShellExecute = false,
                                            CreateNoWindow = true,
                                            RedirectStandardOutput = true,
                                            RedirectStandardError = true
                                        };

                                        string theExtension = Path.GetExtension(scriptFile);

                                        if (theExtension == ".ps1") {
                                            //Is powershell - open it correctly
                                            p.FileName = "powershell.exe";
                                            p.Arguments = $"-WindowStyle Hidden -file \"{scriptFileLocation}\" \"{Path.Combine(MainProgram.CheckPath(), "*")}\" \"*.{Properties.Settings.Default.ActionFileExtension}\"";
                                        } else if (theExtension == ".py") {
                                            //Python - open it correctly
                                            MainProgram.DoDebug("Is python!");

                                            string minPythonVersion = (jsonTest["options"]["min_python_version"] != null ? jsonTest["options"]["min_python_version"] : ""),
                                                maxPythonVersion = (jsonTest["options"]["max_python_version"] != null ? jsonTest["options"]["max_python_version"] : ""),
                                                pythonPath = GetPythonPath(minPythonVersion, maxPythonVersion);

                                            if (pythonPath != "") {
                                                MainProgram.DoDebug("Python path; " + pythonPath);
                                                p.FileName = GetPythonPath();
                                                p.Arguments = scriptFileLocation;
                                            } else {
                                                //No python version (or one with the min-max requirements) not found.
                                                if (minPythonVersion == "" && maxPythonVersion == "") {
                                                    //Python just not found
                                                    MessageBox.Show("We could not locate Python on your computer. Please either download Python or specify its path in the ACC settings if it's already installed.", MainProgram.messageBoxTitle);
                                                } else {
                                                    if (minPythonVersion != "" && maxPythonVersion != "") {
                                                        //Both min & max set
                                                        MessageBox.Show("We could not locate a version of Python between v" + minPythonVersion + " and v" + maxPythonVersion + ". Please either download a version of Python in between the specified versions, or specify its path in the ACC settings if it's already installed.", MainProgram.messageBoxTitle);
                                                    } else {
                                                        if (minPythonVersion != "") {
                                                            //Min only
                                                            MessageBox.Show("We could not locate a version of Python greater than v" + minPythonVersion + ". Please either download Python (min version " + minPythonVersion + ") or specify its path in the ACC settings if it's already installed.", MainProgram.messageBoxTitle);
                                                        } else {
                                                            //Max only
                                                            MessageBox.Show("We could not locate a version of Python lower than v" + maxPythonVersion + ". Please either download Python (max version " + maxPythonVersion + ") or specify its path in the ACC settings if it's already installed.", MainProgram.messageBoxTitle);
                                                        }
                                                    }
                                                }

                                                return;
                                            }
                                        } else {
                                            //"Other" filetype. Simply open file.
                                            p.FileName = scriptFileLocation;
                                            p.Arguments = "how to do dis?";
                                        }

                                        Process theP = Process.Start(p);

                                        string output = theP.StandardOutput.ReadToEnd();
                                        theP.WaitForExit();

                                        Console.WriteLine(output);
                                    } catch (Exception e) {
                                        //Process init failed - it shouldn't, but better safe than sorry
                                        MainProgram.DoDebug("6");
                                        Console.WriteLine(e);
                                    }
                                } else {
                                    //Script file doesn't exist
                                    MainProgram.DoDebug("5");
                                }
                            } else {
                                //JSON is not valid; validateErrMsg
                                MainProgram.DoDebug("4");
                            }
                        } else {
                            //JSON is invalid or failed
                            MainProgram.DoDebug("3");
                        }
                    } catch (Exception e) {
                        //Failed to parse
                        MainProgram.DoDebug("2");
                        Console.WriteLine(e.Message);
                    }
                } else {
                    //Couldn't read file
                    MainProgram.DoDebug("1");
                }
            } else {
                MainProgram.DoDebug("0; " + modLocation);
            }

            MainProgram.DoDebug("\n\n");
        }

        private static string ReadInfoFile(string file) {
            try {
                return File.ReadAllText(file);
            } catch {
                return null;
            }
        }
    }
}
