using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace AssistantComputerControl {
    public partial class SettingsForm : Form {
        public static AdvancedSettings advancedSettings = null;
        public static TestActionWindow actionTester = null;

        public SettingsForm() {
            InitializeComponent();
            versionInfo.Text = "Version " + MainProgram.softwareVersion;

            computerName.KeyDown += new KeyEventHandler(FreakingStopDingSound);
            fileEditedMargin.KeyDown += new KeyEventHandler(FreakingStopDingSound);

            void FreakingStopDingSound(Object o, KeyEventArgs e) {
                if (e.KeyCode == Keys.Enter) {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }

            //Set values
            startWithWindows.Checked = Properties.Settings.Default.StartWithWindows;
            checkUpdates.Checked = Properties.Settings.Default.CheckForUpdates;
            betaProgram.Checked = Properties.Settings.Default.BetaProgram;

            computerName.Text = Properties.Settings.Default.ComputerName;
            fileEditedMargin.Value = (decimal)Properties.Settings.Default.FileEditedMargin;

            infoTooltip.SetToolTip(betaProgram, "Receive updates on new beta versions (often unstable, experimental builds)");

            mainPanel.Click += delegate { mainPanel.Focus(); };

            //On change
            //Has to be down & up, otherwise the last character isn't appended for some reason
            computerName.KeyDown += delegate { Properties.Settings.Default.ComputerName = computerName.Text; Properties.Settings.Default.Save(); Console.WriteLine("Logged"); };
            computerName.KeyUp += delegate { Properties.Settings.Default.ComputerName = computerName.Text; Properties.Settings.Default.Save(); Console.WriteLine("Logged"); };
            fileEditedMargin.ValueChanged += delegate { Properties.Settings.Default.FileEditedMargin = (float)fileEditedMargin.Value; Properties.Settings.Default.Save(); };
        }

        private void advancedSettingsButton_Click(object sender, EventArgs e) {
            ShowAdvancedSettings();
        }

        private void testButton_Click(object sender, EventArgs e) {
            ShowTester();
        }

        private void logButton_Click(object sender, EventArgs e) {
            Process.Start(MainProgram.currentLocation + @"\ACC_Data\log.txt");
        }

        private void startWithWindows_CheckedChanged(object sender, EventArgs e) {
            if (Properties.Settings.Default.StartWithWindows != startWithWindows.Checked) {
                Properties.Settings.Default.StartWithWindows = startWithWindows.Checked;
                MainProgram.SetStartup(startWithWindows.Checked);

                Properties.Settings.Default.Save();
            }
        }

        private void checkUpdates_CheckedChanged(object sender, EventArgs e) {
            betaProgram.Enabled = checkUpdates.Checked;
            if (Properties.Settings.Default.CheckForUpdates != checkUpdates.Checked) {
                Properties.Settings.Default.CheckForUpdates = checkUpdates.Checked;
                Properties.Settings.Default.Save();

                if (checkUpdates.Checked) {
                    new ACC_Updater().Check();
                }
            }
        }

        private void betaProgram_CheckedChanged(object sender, EventArgs e) {
            if (Properties.Settings.Default.BetaProgram != betaProgram.Checked) {
                Properties.Settings.Default.BetaProgram = betaProgram.Checked;
                Properties.Settings.Default.Save();

                if (betaProgram.Checked) {
                    new ACC_Updater().Check();
                }
            }
        }

        private void SettingsForm_Load(object sender, EventArgs e) {

        }

        private void mainPanel_Paint(object sender, PaintEventArgs e) {
            //Hmh...
            //mainPanel.Focus();
        }

        private void checkForUpdate_Click(object sender, EventArgs e) {
            if (MainProgram.HasInternet()) {
                if (!new ACC_Updater().Check()) {
                    //No new update
                    MessageBox.Show("No new update found. You're up to date!", "Check for update | " + MainProgram.messageBoxTitle);
                }
            } else {
                MessageBox.Show("Could not check for update as your computer is not connected to the internet", "Error | " + MainProgram.messageBoxTitle);
            }
        }

        public void ShowAdvancedSettings() {
            if (advancedSettings is null) {
                //New instance
                advancedSettings = new AdvancedSettings();
                advancedSettings.Show();

                advancedSettings.Owner = this;
                advancedSettings.FormClosing += delegate { advancedSettings = null; Enabled = true; };
                advancedSettings.Load += delegate { Enabled = false; };
            } else {
                //Focus
                advancedSettings.Focus();
            }
        }

        public void ShowTester() {
            if (actionTester is null) {
                //New instance
                actionTester = new TestActionWindow();
                actionTester.Show();

                actionTester.FormClosing += delegate { actionTester = null; };
            } else {
                //Focus
                actionTester.Focus();
            }
        }
    }
}
