using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistantComputerControl {
    class CloudServiceFunctions {
        private class DropboxJson {
            public DropboxJson business { get; set; }
            public DropboxJson personal { get; set; }
            public string Path { get; set; }
        }

        public static string GetCloudServicePath(string type = "") {
            switch (type) {
                case "dropbox":
                    return GetDropboxFolder();
                case "onedrive":
                    return GetOneDriveFolder();
                case "googledrive":
                    return GetGoogleDriveFolder();
            }

            return "";
        }

        public static bool GoogleDriveInstalled() {
            return GetGoogleDriveFolder() != String.Empty;
        }
        public static string GetGoogleDriveFolder() {
            string registryKey = @"Software\Google\Drive";
            RegistryKey key = Registry.CurrentUser.OpenSubKey(registryKey);
            if (key != null) {
                string installed = key.GetValue("Installed").ToString();
                key.Close();
                if (installed != null) {
                    if (installed == "True") {
                        string checkPath = Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "Google Drive");
                        if (Directory.Exists(checkPath)) {
                            return checkPath;
                        }
                        return "partial";
                    }
                }
            }
            return "";
        }
        public static bool OneDriveInstalled() {
            return !String.IsNullOrEmpty(GetOneDriveFolder());
        }
        public static string GetOneDriveFolder() {
            return Environment.GetEnvironmentVariable("OneDrive");
        }

        public static string GetDropboxFolder() {
            if (MainProgram.ApplicationInstalled("Dropbox")) {
                string infoPath = @"Dropbox\info.json";
                string jsonPath = Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), infoPath);

                if (!Directory.Exists(Directory.GetDirectoryRoot(jsonPath))) return "";
                if (!File.Exists(jsonPath)) jsonPath = Path.Combine(Environment.GetEnvironmentVariable("AppData"), infoPath);
                if (!File.Exists(jsonPath)) return "";

                string jsonContent = File.ReadAllText(jsonPath);
                try {
                    DropboxJson dropboxJson = JsonConvert.DeserializeObject<DropboxJson>(jsonContent);
                    if (dropboxJson != null) {
                        if (dropboxJson.personal != null) {
                            return dropboxJson.personal.Path;
                        } else if (dropboxJson.business != null) {
                            return dropboxJson.business.Path;
                        }
                    }
                } catch {
                    MainProgram.DoDebug("Failed to deserialize Dropbox Json");
                    MainProgram.DoDebug(jsonContent);
                }
            } else {
                MainProgram.DoDebug("Dropbox not installed");
                return "";
            }
            return "";
        }
    }
}