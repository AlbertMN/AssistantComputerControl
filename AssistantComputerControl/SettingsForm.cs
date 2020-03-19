/*
 * AssistantComputerControl
 * Made by Albert MN.
 * Updated: v1.4.0, 15-01-2020
 * 
 * Use:
 * - Settings Form
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace AssistantComputerControl {
    public partial class SettingsForm : Form {
        public static AdvancedSettings advancedSettings = null;
        public static TestActionWindow actionTester = null;

        public SettingsForm() {
            InitializeComponent();

            versionInfo.Text = "|Version " + MainProgram.softwareVersion;

            computerName.KeyDown += new KeyEventHandler(FreakingStopDingSound);
            fileEditedMargin.KeyDown += new KeyEventHandler(FreakingStopDingSound);
            fileReadDelay.KeyDown += new KeyEventHandler(FreakingStopDingSound);

            void FreakingStopDingSound(Object o, KeyEventArgs e) {
                if (e.KeyCode == Keys.Enter) {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }

            //Set values
            startWithWindows.Checked = MainProgram.ACCStartsWithWindows();
            checkUpdates.Checked = Properties.Settings.Default.CheckForUpdates;
            betaProgram.Checked = Properties.Settings.Default.BetaProgram;
            warnDeletion.Checked = Properties.Settings.Default.WarnWhenDeletingManyFiles;
            defaultComputer.Checked = Properties.Settings.Default.DefaultComputer;

            computerName.Text = Properties.Settings.Default.ComputerName;
            fileEditedMargin.Value = (decimal)Properties.Settings.Default.FileEditedMargin;
            fileReadDelay.Value = (decimal)Properties.Settings.Default.FileReadDelay;
            maxDeleteFiles.Value = Properties.Settings.Default.MaxDeleteFiles;
            maxDeleteFiles.Enabled = warnDeletion.Checked;

            infoTooltip.SetToolTip(betaProgram, "Receive updates on new beta versions (often unstable, experimental builds)");

            mainPanel.Click += delegate { mainPanel.Focus(); };

            //On change
            //Has to be down & up, otherwise the last character isn't appended for some reason
            computerName.KeyDown += delegate { Properties.Settings.Default.ComputerName = computerName.Text; Properties.Settings.Default.Save(); };
            computerName.KeyUp += delegate { Properties.Settings.Default.ComputerName = computerName.Text; Properties.Settings.Default.Save(); };
            fileEditedMargin.ValueChanged += delegate { Properties.Settings.Default.FileEditedMargin = (float)fileEditedMargin.Value; Properties.Settings.Default.Save(); };
            fileReadDelay.ValueChanged += delegate { Properties.Settings.Default.FileReadDelay = (float)fileReadDelay.Value; Properties.Settings.Default.Save(); };
            maxDeleteFiles.ValueChanged += delegate { Properties.Settings.Default.MaxDeleteFiles = (int)maxDeleteFiles.Value; Properties.Settings.Default.Save(); };

            /* Translations */
            int i = 0;
            string activeLanguage = Properties.Settings.Default.ActiveLanguage;
            foreach (string item in Translator.languagesArray) {
                programLanguage.Items.Add(item);

                if (activeLanguage == item) {
                    programLanguage.SelectedIndex = i;
                }
                ++i;
            }
            Text = Translator.__("window_name", "settings");

            foreach (Control x in this.Controls) {
                Translator.TranslateWinForms("settings", x.Controls);
            }
        }

        private void advancedSettingsButton_Click(object sender, EventArgs e) {
            ShowAdvancedSettings();
        }

        private void testButton_Click(object sender, EventArgs e) {
            ShowTester();
        }

        private void logButton_Click(object sender, EventArgs e) {
            Process.Start(MainProgram.logFilePath);
        }

        private void startWithWindows_CheckedChanged(object sender, EventArgs e) {
            MainProgram.SetStartup(startWithWindows.Checked);
        }

        private void checkUpdates_CheckedChanged(object sender, EventArgs e) {
            betaProgram.Enabled = checkUpdates.Checked;
            if (Properties.Settings.Default.CheckForUpdates != checkUpdates.Checked) {
                Properties.Settings.Default.CheckForUpdates = checkUpdates.Checked;
                Properties.Settings.Default.Save();

                if (checkUpdates.Checked) {
                    new SoftwareUpdater().Check();
                }
            }
        }

        private void betaProgram_CheckedChanged(object sender, EventArgs e) {
            if (Properties.Settings.Default.BetaProgram != betaProgram.Checked) {
                Properties.Settings.Default.BetaProgram = betaProgram.Checked;
                Properties.Settings.Default.Save();

                new Thread(() => {
                    if (betaProgram.Checked) {
                        new SoftwareUpdater().Check();
                    }
                }).Start();
            }
        }

        private void SettingsForm_Load(object sender, EventArgs e) {

        }

        private void mainPanel_Paint(object sender, PaintEventArgs e) {
            //Hmh...
            //mainPanel.Focus();
        }

        private void checkForUpdate_Click(object sender, EventArgs e) {
            //Doesn't return true for some reason... To fix
            if (MainProgram.HasInternet()) {
                if (!new SoftwareUpdater().Check(true)) {
                    MessageBox.Show(Translator.__("no_new_update", "check_for_update"), MainProgram.messageBoxTitle);
                }
            } else {
                MessageBox.Show(Translator.__("update_check_failed", "check_for_update"), Translator.__("error", "general") + " | " + MainProgram.messageBoxTitle);
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

        private void doSetupAgain_Click(object sender, EventArgs e) {
            MainProgram.ShowGettingStarted();
        }

        private void warnDeletion_CheckedChanged(object sender, EventArgs e) {
            maxDeleteFiles.Enabled = warnDeletion.Checked;

            Properties.Settings.Default.Save();
        }

        private void multiPcSupportReadMore_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("https://acc.readme.io/docs/controlling-multiple-computers");
        }

        private void defaultComputer_CheckedChanged(object sender, EventArgs e) {
            if (Properties.Settings.Default.DefaultComputer != defaultComputer.Checked) {
                Properties.Settings.Default.DefaultComputer = defaultComputer.Checked;

                Properties.Settings.Default.Save();
            }
        }

        private void infoTooltip_Popup(object sender, PopupEventArgs e) {

        }

        private void saveLanguageButton_Click(object sender, EventArgs e) {
            string lang = programLanguage.Text;
            if (Array.Exists(Translator.languagesArray, element => element == lang)) {
                MainProgram.DoDebug("Language \"" + lang + "\" chosen");

                Translator.SetLanguage(lang);
                MainProgram.reopenSettingsOnClose = true;

                Properties.Settings.Default.ActiveLanguage = programLanguage.Text;
                Properties.Settings.Default.Save();

                this.Close();
            } else {
                MessageBox.Show("Language is invalid");
                MainProgram.DoDebug("Invalid language chosen");
            }
        }

        private void changelogOpen_Click(object sender, EventArgs e) {
            new NewVersion().Show();
        }
    }
}