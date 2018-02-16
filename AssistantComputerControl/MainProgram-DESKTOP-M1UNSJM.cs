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
using System.Net;

using YamlDotNet;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.RepresentationModel;

namespace HomeComputerControl {
    class MainProgram {
        static public bool debug = true;

        static actionChecker checker = new actionChecker();
        static public string currentLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
            actionFilePath = @currentLocation + "/computerAction.txt",
            configFilePath = @currentLocation + "/hcc_config.yml",
            error_message = "";
        static public int fileEditedSecondMargin = 30;

        public static void doDebug(string str = null) {
            if(str != null) {
                if (debug) {
                    Console.WriteLine(str);
                }
            } else {
                if (debug)
                    Console.WriteLine("Debug string is null");
            }
        }

        private static void configSetup() {
            doDebug("Checking if config exists...");

            if (!File.Exists(configFilePath)) {
                doDebug("Config doesn't exist - creating config...");
                //Creating config file & adding content
                setConfigContent();

                if (!File.Exists(configFilePath)) {
                    //If the file was somehow not created...
                    doDebug("ERROR: The file config file was not created");
                }
            } else {
                doDebug("Config exists (" + configFilePath + ")");

                if (new FileInfo(configFilePath).Length == 0) {
                    doDebug("Config file is empty. Setting content...");
                    setConfigContent();
                }
            }

            //Setting variables to config values...
            //ehh....
        }

        private static void setConfigContent() {
            string textFromFile = (new WebClient()).DownloadString("http://albe.pw/test.yml");

            using (StreamWriter sw = File.CreateText(configFilePath)) {
                sw.Write(textFromFile);
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
            doDebug("[HomeComputerControl] starting up...\nCurrent location: " + currentLocation + "\naction file location: " + actionFilePath + "\n");

            configSetup();
            clearFile();

            checker.check();
            if (debug) {
                doDebug("Done checking, program finished (somehow...)");
                Console.ReadKey();
            }
        }

    }
}
