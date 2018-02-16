using System.IO;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;

namespace AssistantComputerControl {
    class ACC_Updater {
        public void check() {
            string latest_version;
            using (WebClient client = new WebClient()) {
                latest_version = client.DownloadString("https://gh.albe.pw/acc/releases/latest/version.txt");
            }
            if (latest_version != MainProgram.softwareVersion) {
                //New version available
                MainProgram.doDebug("New software version found");
                DialogResult dialogResult = MessageBox.Show("A new version of " + MainProgram.messageBoxTitle + " is available (v" + latest_version + "), do you wish to install it?", "New update found | " + MainProgram.messageBoxTitle, MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes) {
                    MainProgram.doDebug("User chose \"yes\" to install update");
                    install();
                } else if(dialogResult == DialogResult.No) {
                    MainProgram.doDebug("User did not want to install update");
                }
            } else {
                MainProgram.doDebug("Software up to date");
            }
            if (File.Exists(Path.Combine(MainProgram.dataFolderLocation, "updated.txt"))) {
                File.Delete("updated.txt");
            }
        }

        public void install() {
            MainProgram.doDebug("Installing...");
            //Create file for the updater, containing the name (and full path) of the ACC-exe which is being updated
            if (File.Exists(Path.Combine(MainProgram.dataFolderLocation, "updated.txt"))) {
                File.Delete("updated.txt");
            }
            if (File.Exists(Path.Combine(MainProgram.dataFolderLocation, "acc_location.txt"))) {
                File.WriteAllText(Path.Combine(MainProgram.dataFolderLocation, "acc_location.txt"), string.Empty);
            }
            using (var tw = new StreamWriter(Path.Combine(MainProgram.dataFolderLocation, "acc_location.txt"), true)) {
                tw.WriteLine(MainProgram.currentLocationFull);
                tw.Close();
            }
            string downloadLocation = Path.Combine(MainProgram.currentLocation, "ACC_installer.exe");
            using (var client = new WebClient()) {
                client.DownloadFile("https://gh.albe.pw/acc/installer/ACC_installer.exe", downloadLocation);
            }
            Process.Start(downloadLocation);
            MainProgram.exit();
        }
    }
}
