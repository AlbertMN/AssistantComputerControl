using System.IO;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace AssistantComputerControl {
    class ACC_Updater {
        private const string releaseJsonUrl = "http://acc.albe.pw/versions/latest_version.php?type=release";
        private const string betaJsonUrl = "http://acc.albe.pw/versions/latest_version.php?type=beta";
        
        public bool Check() {
            if (MainProgram.isCheckingForUpdate)
                return false;

            MainProgram.isCheckingForUpdate = true;
            MainProgram.DoDebug("Checking for updates...");

            string latestReleaseJson = null;
            string latestBetaJson = null;

            //Check and get latest
            if (RemoteFileExists(releaseJsonUrl)) {
                using (WebClient client = new WebClient()) {
                    latestReleaseJson = client.DownloadString(releaseJsonUrl);
                }
                if (latestReleaseJson == string.Empty)
                    latestReleaseJson = null;
            }

            //Check and get beta
            if (Properties.Settings.Default.BetaProgram && RemoteFileExists(betaJsonUrl)) {
                using (WebClient client = new WebClient()) {
                    latestBetaJson = client.DownloadString(betaJsonUrl);
                }
                if (latestBetaJson == string.Empty)
                    latestReleaseJson = null;
            }

            if (latestReleaseJson != null || latestBetaJson != null) {
                Version newVersion = null
                    , latestRelease
                    , latestBeta;

                if (latestReleaseJson != null && latestBetaJson != null) {
                    //Beta program enabled; check both release and beta for newest update
                    latestRelease = JsonConvert.DeserializeObject<Version>(latestReleaseJson);
                    latestBeta = JsonConvert.DeserializeObject<Version>(latestBetaJson);

                    if (DateTime.Parse(latestRelease.datetime) > DateTime.Parse(MainProgram.releaseDate) || DateTime.Parse(latestBeta.datetime) > DateTime.Parse(MainProgram.releaseDate)) {
                        //Both latest release and beta is ahead of this current build
                        if (DateTime.Parse(latestRelease.datetime) > DateTime.Parse(latestBeta.datetime)) {
                            //Release is newest
                            newVersion = latestRelease;
                        } else {
                            //Beta is newest
                            newVersion = latestBeta;
                        }
                    } else {
                        //None of them are newer. Nothing new
                        MainProgram.DoDebug("Software up to date (beta program enabled)");
                        return false;
                    }
                } else if (latestReleaseJson != null && latestBetaJson == null) {
                    //Only check latest
                    latestRelease = JsonConvert.DeserializeObject<Version>(latestReleaseJson);

                    if (DateTime.Parse(latestRelease.datetime) > DateTime.Parse(MainProgram.releaseDate) && latestRelease.version != MainProgram.softwareVersion) {
                        //Newer build
                        newVersion = latestRelease;
                    } else {
                        //Not new, move on
                        MainProgram.DoDebug("Software up to date");
                        return false;
                    }
                } else if (latestReleaseJson == null && latestBetaJson != null) {
                    //Couldn't reach "latest" update, but beta-updates are enabled
                    latestBeta = JsonConvert.DeserializeObject<Version>(latestBetaJson);

                    if(latestBeta != null) {
                        if (DateTime.Parse(latestBeta.datetime) > DateTime.Parse(MainProgram.releaseDate)) {
                            //Newer build
                            newVersion = latestBeta;
                        } else {
                            //Not new, move on
                            MainProgram.DoDebug("Software up to date (beta program enabled)");
                            return false;
                        }
                    }
                } else {
                    MainProgram.DoDebug("Both release and beta is NULL, no new updates, or no contact to the server.");
                }

                if (newVersion != null && newVersion.version != MainProgram.softwareVersion) {
                    //New version available
                    MainProgram.DoDebug("New software version found (" + newVersion.version + ") [" + newVersion.type + "], current; " + MainProgram.softwareVersion);
                    DialogResult dialogResult = MessageBox.Show("A new version of " + MainProgram.messageBoxTitle + " is available (v" + newVersion.version + " [" + newVersion.type + "]), do you wish to install it?", "New update found | " + MainProgram.messageBoxTitle, MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes) {
                        MainProgram.DoDebug("User chose \"yes\" to install update");
                        DownloadFile(newVersion.installpath + "&upgrade=true");
                    } else if (dialogResult == DialogResult.No) {
                        MainProgram.DoDebug("User did not want to install update");
                    }
                    return true;
                } else {
                    MainProgram.DoDebug("Software up to date");
                }
            } else {
                MainProgram.DoDebug("Could not reach the webserver (both 'release' and 'beta' json files couldn't be reached)");
            }
            return false;
        }

        public static bool RemoteFileExists(string url) {
            try {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TRUE if the Status code == 200
                response.Close();
                return (response.StatusCode == HttpStatusCode.OK);
            } catch {
                //Any exception will returns false.
                return false;
            }
        }

        private static string targetLocation = "";
        public static void DownloadFile(string url) {
            if (RemoteFileExists(url)) {

                WebClient client = new WebClient();
                Uri uri = new Uri(url);
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(FileDownloadedCallback);

                targetLocation = Path.Combine(Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "Downloads"), "ACCsetup.exe");
                if (File.Exists(targetLocation)) {
                    try {
                        File.Delete(targetLocation);
                    } catch (Exception ex) {
                        MainProgram.DoDebug("Failed to delete file at " + targetLocation);
                        MainProgram.DoDebug("Error; " + ex);
                    }
                }
                client.DownloadFileAsync(uri, targetLocation);
            } else {
                MainProgram.DoDebug("Failed to update, installation URL does not exist (" + url + ").");
                MessageBox.Show("Couldn't find the new version online. Please try again later.", "Error | " + MainProgram.messageBoxTitle);
            }
        }

        private static void FileDownloadedCallback(object sender, AsyncCompletedEventArgs e) {
            if (!e.Cancelled) {
                //Download success
                Process.Start(targetLocation);
                MainProgram.DoDebug("New installer successfully downloaded and opened.");
            } else {
                MainProgram.DoDebug("Failed to download new version of ACC. Error; " + e.Error);
                MessageBox.Show("Failed to download new version. Try again later!", "Error | " + MainProgram.messageBoxTitle);
            }
        }
    }
}
