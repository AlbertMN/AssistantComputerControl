using System.IO;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Threading;

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
                if (latestReleaseJson == string.Empty) {
                    latestReleaseJson = null;
                }
            }

            //Check and get beta
            if (Properties.Settings.Default.BetaProgram && RemoteFileExists(betaJsonUrl)) {
                using (WebClient client = new WebClient()) {
                    latestBetaJson = client.DownloadString(betaJsonUrl);
                }
                if (latestBetaJson == string.Empty)
                    latestBetaJson = null;
            }

            if (latestReleaseJson != null || latestBetaJson != null) {
                Version newVersion = null
                    , latestRelease
                    , latestBeta;

                if (latestReleaseJson != null && latestBetaJson != null) {
                    //Beta program enabled; check both release and beta for newest update
                    latestRelease = JsonConvert.DeserializeObject<Version>(latestReleaseJson);
                    latestBeta = JsonConvert.DeserializeObject<Version>(latestBetaJson);

                    if (DateTime.Parse(latestRelease.datetime) > DateTime.Parse(MainProgram.releaseDate) ||
                        DateTime.Parse(latestBeta.datetime) > DateTime.Parse(MainProgram.releaseDate)) {
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
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "HEAD";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                response.Close();
                return (response.StatusCode == HttpStatusCode.OK);
            } catch {
                return false;
            }
        }

        private static string targetLocation = "";
        public static void DownloadFile(string url) {
            if (RemoteFileExists(url)) {
                MainProgram.DoDebug("Downloading file...");
                WebClient client = new WebClient();
                Uri uri = new Uri(url);

                MainProgram.updateProgressWindow = new UpdateProgress();
                MainProgram.updateProgressWindow.Show();

                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressChanged);
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

                Application.Run();
            } else {
                MainProgram.DoDebug("Failed to update, installation URL does not exist (" + url + ").");
                MessageBox.Show("Couldn't find the new version online. Please try again later.", "Error | " + MainProgram.messageBoxTitle);
            }
        }
        static private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            if (MainProgram.updateProgressWindow != null)
                MainProgram.updateProgressWindow.SetProgress(e);
        }

        private static void FileDownloadedCallback(object sender, AsyncCompletedEventArgs e) {
            if (MainProgram.updateProgressWindow != null) {
                MainProgram.updateProgressWindow.Close();
                MainProgram.updateProgressWindow = null;
            }

            MainProgram.DoDebug("Finished downloading");

            if (!e.Cancelled) {
                //Download success
                Process.Start(targetLocation);
                MainProgram.DoDebug("New installer successfully downloaded and opened.");
            } else {
                MainProgram.DoDebug("Failed to download new version of ACC. Error; " + e.Error);
                MessageBox.Show("Failed to download new version. Try again later!", "Error | " + MainProgram.messageBoxTitle);
            }

            Thread.CurrentThread.Abort();
        }
    }
}