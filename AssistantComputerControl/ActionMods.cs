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

                return highestVersionPath;
            }

            return "";
        }

        //Execute mod (action status, action return message, was fatal)
        public static Tuple<bool, string, bool> ExecuteModAction(string name, string parameter = "") {
            MainProgram.DoDebug("[MOD ACTION] Running mod action \"" + name + "\"");
            string actionErrMsg = "No error message set",
                modLocation = Path.Combine(MainProgram.actionModsPath, modActions[name]),
                infoJsonFile = Path.Combine(modLocation, "info.json");

            if (File.Exists(infoJsonFile)) {
                string modFileContent = ReadInfoFile(infoJsonFile);
                if (modFileContent != null) {
                    try {
                        dynamic jsonTest = JsonConvert.DeserializeObject<dynamic>(modFileContent);
                        if (jsonTest != null) {
                            if (ValidateInfoJson(jsonTest)) {
                                //JSON is valid - get script file
                                string scriptFile = jsonTest["options"]["file_name"], scriptFileLocation = Path.Combine(modLocation, scriptFile);
                                bool actionIsFatal = jsonTest["options"]["is_fatal"] ?? false,
                                    requiresParameter = jsonTest["options"]["requires_param"] ?? false,
                                    requiresSecondaryParameter = jsonTest["options"]["require_second_param"] ?? false;

                                if (requiresParameter ? !String.IsNullOrEmpty(parameter) : true) {
                                    Console.WriteLine("Requires parameter? " + requiresParameter);
                                    Console.WriteLine("Has parameter? " + !String.IsNullOrEmpty(parameter));

                                    string[] secondaryParameters = ActionChecker.GetSecondaryParam(parameter);
                                    if (requiresSecondaryParameter ? secondaryParameters.Length > 1 : true) { //Also returns the first parameter
                                        if (File.Exists(scriptFileLocation)) {
                                            string accArg = requiresParameter && requiresSecondaryParameter ? JsonConvert.SerializeObject(ActionChecker.GetSecondaryParam(parameter)) : (requiresParameter ? parameter : "");

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
                                                    p.Arguments = String.Format("-WindowStyle Hidden -file \"{0}\" \"{1}\"", scriptFileLocation, accArg);
                                                } else if (theExtension == ".py") {
                                                    //Python - open it correctly
                                                    string minPythonVersion = (jsonTest["options"]["min_python_version"] ?? ""),
                                                        maxPythonVersion = (jsonTest["options"]["max_python_version"] ?? ""),
                                                        pythonPath = GetPythonPath(minPythonVersion, maxPythonVersion);

                                                    if (pythonPath != "") {
                                                        p.FileName = GetPythonPath();
                                                        p.Arguments = String.Format("{0} \"{1}\"", scriptFileLocation, accArg);
                                                    } else {
                                                        //No python version (or one with the min-max requirements) not found.
                                                        string pythonErr;

                                                        if (minPythonVersion == "" && maxPythonVersion == "") {
                                                            //Python just not found
                                                            pythonErr = "We could not locate Python on your computer. Please either download Python or specify its path in the ACC settings if it's already installed.";
                                                        } else {
                                                            if (minPythonVersion != "" && maxPythonVersion != "") {
                                                                //Both min & max set
                                                                pythonErr = "We could not locate a version of Python between v" + minPythonVersion + " and v" + maxPythonVersion + ". Please either download a version of Python in between the specified versions, or specify its path in the ACC settings if it's already installed.";
                                                            } else {
                                                                if (minPythonVersion != "") {
                                                                    //Min only
                                                                    pythonErr = "We could not locate a version of Python greater than v" + minPythonVersion + ". Please either download Python (min version " + minPythonVersion + ") or specify its path in the ACC settings if it's already installed.";
                                                                } else {
                                                                    //Max only
                                                                    pythonErr = "We could not locate a version of Python lower than v" + maxPythonVersion + ". Please either download Python (max version " + maxPythonVersion + ") or specify its path in the ACC settings if it's already installed.";
                                                                }
                                                            }
                                                        }

                                                        return Tuple.Create(false, pythonErr, false);
                                                    }
                                                } else if (theExtension == ".bat" || theExtension == ".cmd" || theExtension == ".btm") {
                                                    //Is batch - open it correctly (https://en.wikipedia.org/wiki/Batch_file#Filename_extensions)
                                                    p.FileName = "cmd.exe";
                                                    p.Arguments = String.Format("/c {0} \"{1}\"", scriptFileLocation, accArg);
                                                } else {
                                                    //"Other" filetype. Simply open file.
                                                    p.FileName = scriptFileLocation;
                                                    p.Arguments = accArg;
                                                }

                                                Process theP = Process.Start(p);

                                                if (!theP.WaitForExit(5000)) {
                                                    MainProgram.DoDebug("Action mod timed out");
                                                    theP.Kill();
                                                }
                                                string output = theP.StandardOutput.ReadToEnd();

                                                using (StringReader reader = new StringReader(output)) {
                                                    string line;
                                                    while ((line = reader.ReadLine()) != null) {
                                                        if (line.Length >= 17) {
                                                            if (line.Substring(0, 12) == "[ACC RETURN]") {
                                                                //Return for ACC
                                                                string returnContent = line.Substring(13),
                                                                    comment = returnContent.Contains(',') ? returnContent.Split(',')[1] : "";
                                                                bool actionSuccess = returnContent.Substring(0, 4) == "true";

                                                                if (comment.Length > 0 && char.IsWhiteSpace(comment, 0)) {
                                                                    comment = comment.Substring(1);
                                                                }

                                                                if (actionSuccess) {
                                                                    //Action was successfull
                                                                    Console.WriteLine("Action successful!");
                                                                } else {
                                                                    //Action was not successfull
                                                                    Console.WriteLine("Action failed :(");
                                                                }

                                                                Console.WriteLine("Comment; " + comment);
                                                                return Tuple.Create(actionSuccess, (comment.Length > 0 ? comment : "Action mod returned no reason for failing"), actionIsFatal);
                                                            }
                                                        }
                                                    }
                                                }

                                                return Tuple.Create(false, "Action mod didn't return anything to ACC", false);
                                            } catch (Exception e) {
                                                //Process init failed - it shouldn't, but better safe than sorry
                                                actionErrMsg = "Process initiation failed";
                                                Console.WriteLine(e);
                                            }
                                        } else {
                                            //Script file doesn't exist
                                            actionErrMsg = "Action mod script doesn't exist";
                                        }
                                    } else {
                                        actionErrMsg = "Action \"" + name + "\" requires a secondary parameter to be set";
                                    }
                                } else {
                                    actionErrMsg = "Action \"" + name + "\" requires a parameter to be set";
                                }
                            } else {
                                //JSON is not valid; validateErrMsg
                                actionErrMsg = validateErrMsg;
                            }
                        } else {
                            //JSON is invalid or failed
                            actionErrMsg = "Action mod JSON is invalid";
                        }
                    } catch (Exception e) {
                        //Failed to parse
                        actionErrMsg = "Failed to parse action mod JSON";
                        Console.WriteLine(e.Message);
                    }
                } else {
                    //Couldn't read file
                    MainProgram.DoDebug("1");
                }
            } else {
                MainProgram.DoDebug("0; " + modLocation);
            }

            return Tuple.Create(false, actionErrMsg, false);
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
