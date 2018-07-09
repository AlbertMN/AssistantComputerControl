using System.IO;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using Newtonsoft.Json;
using System;

namespace AssistantComputerControl {
    class ACC_Updater {
        private const string releaseJsonUrl = "http://acc.albe.pw/versions/release/latest_version.json";
        private const string betaJsonUrl = "http://acc.albe.pw/versions/beta/latest_version.json";

        public bool Check() {
            MainProgram.DoDebug("Checking for updates...");

            string latestReleaseJson = null;
            string latestBetaJson = null;

            //Check and get latest
            if (RemoteFileExists(releaseJsonUrl)) {
                using (WebClient client = new WebClient()) {
                    latestReleaseJson = client.DownloadString(releaseJsonUrl);
                }
            }

            //Check and get beta
            if (Properties.Settings.Default.BetaProgram && RemoteFileExists(betaJsonUrl)) {
                using (WebClient client = new WebClient()) {
                    latestBetaJson = client.DownloadString(betaJsonUrl);
                }
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

                    if (DateTime.Parse(latestRelease.datetime) > DateTime.Parse(MainProgram.releaseDate)) {
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

                    if (DateTime.Parse(latestBeta.datetime) > DateTime.Parse(MainProgram.releaseDate)) {
                        //Newer build
                        newVersion = latestBeta;
                    } else {
                        //Not new, move on
                        MainProgram.DoDebug("Software up to date (beta program enabled)");
                        return false;
                    }
                }

                if (newVersion != null) {
                    //New version available
                    MainProgram.DoDebug("New software version found (" + newVersion.version + ") [" + newVersion.type + "], current; " + MainProgram.softwareVersion);
                    DialogResult dialogResult = MessageBox.Show("A new version of " + MainProgram.messageBoxTitle + " is available (v" + newVersion.version + " [" + newVersion.type + "]), do you wish to install it?", "New update found | " + MainProgram.messageBoxTitle, MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes) {
                        MainProgram.DoDebug("User chose \"yes\" to install update");
                        Install(newVersion.installpath);
                    } else if (dialogResult == DialogResult.No) {
                        MainProgram.DoDebug("User did not want to install update");
                    }

                    if (File.Exists(Path.Combine(MainProgram.dataFolderLocation, "updated.txt"))) {
                        File.Delete("updated.txt");
                    }
                    return true;
                }
            } else {
                MainProgram.DoDebug("Could not reach the webserver (both 'release' and 'beta' json files couldn't be reached)");
            }
            return false;
        }

        private bool RemoteFileExists(string url) {
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

        public void Install(string installPath) {
            MainProgram.DoDebug("Installing new version...");
            if (File.Exists(Path.Combine(MainProgram.dataFolderLocation, "updated.txt"))) {
                File.Delete("updated.txt");
            }

            HttpWebResponse response = null;
            var request = (HttpWebRequest)WebRequest.Create(installPath);
            request.Method = "HEAD";
            bool wentThrough = true;

            try {
                response = (HttpWebResponse)request.GetResponse();
            } catch (WebException ex) {
                //A WebException will be thrown if the status of the response is not `200 OK`
                MainProgram.DoDebug("Failed to update, installation URL does not exist (" + installPath + "). Error;");
                MainProgram.DoDebug(ex.Message);
                MessageBox.Show("Couldn't find the new version online. Please try again later.", "Error | " + MainProgram.messageBoxTitle);
                wentThrough = false;
            } finally {
                if (response != null) {
                    response.Close();
                }
            }

            if (!wentThrough)
                return;

            string downloadLocation = Path.Combine(Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "Downloads"), "AssistantComputerControl installer.exe");
            using (var client = new WebClient()) {
                client.DownloadFile(installPath, downloadLocation);
            }
            Process.Start(downloadLocation);
            MainProgram.Exit();
        }
    }
}
