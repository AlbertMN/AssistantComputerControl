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
using System.Reflection;
using System.Net;
using System.Collections.Specialized;

namespace HomeComputerControl {
    class MainProgram {
        static public string softwareVersion = "0.1";
        static public bool debug = true,
            unmute_volume_change = true;

        static actionChecker checker = new actionChecker();
        static public string currentLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
            actionFilePath = "computerAction.txt",
            configFilePath = String.Concat(Assembly.GetEntryAssembly().Location, ".config"),
            error_message = "";
        static public int fileEditedSecondMargin = 30,
            sleepTime = 500;

        public static void doDebug(string str) {
            if (debug) {
                Console.WriteLine(str);
            }
        }

        private static void configSetup() {
            doDebug("Checking if config exists...");

            if (!File.Exists(configFilePath)) {
                doDebug("Config doesn't exist - creating config...");
                //Creating config file & adding content
                createSetConfig();

                if (!File.Exists(configFilePath)) {
                    //If the file was somehow not created...
                    doDebug("ERROR: The file config file was not created");
                }
            } else {
                doDebug("Config exists (" + configFilePath + ")");

                if (new FileInfo(configFilePath).Length == 0) {
                    doDebug("Config file is empty. Setting content...");
                    createSetConfig();
                }
            }

            //Setting variables to config values...
            configValidator validator = new configValidator();
            validator.validate("ActionFilePath", ref actionFilePath);
            validator.validate("FileEditedMargin", ref fileEditedSecondMargin);
            validator.validate("SleepTime", ref sleepTime, 10, fileEditedSecondMargin * 1000);
            validator.validate("UnmuteOnVolumeChange", ref unmute_volume_change);
        }

        private static void createSetConfig() {
            string webConfig = (new WebClient()).DownloadString("https://gh.albe.pw/hcc/configs/v0.2/hcc.exe.config");
            using (var tw = new StreamWriter(configFilePath, true)) {
                tw.WriteLine(webConfig);
                tw.Close();
            }
        }

        static public void clearFile() {
            if (File.Exists(actionFilePath)) {
                doDebug("Action-file exists, deleting...");
                File.Delete(actionFilePath);
                doDebug("Action-file deleted");
            } else {
                doDebug("Action-file doesn't exist, nothing to delete");
            }
        }

        static void Main(string[] args) {
            configSetup();
            doDebug("\n[HomeComputerControl] starting up...\nCurrent location: " + currentLocation + "\naction file location: " + actionFilePath + "\n");
            clearFile();

            checker.check();
            if (debug) {
                doDebug("Done checking, program finished (somehow...)");
                Console.ReadKey();
            }
        }


    }
}
