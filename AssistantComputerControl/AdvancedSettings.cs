using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace AssistantComputerControl {
    public partial class AdvancedSettings : Form {

        bool hasUnfocused = false;

        public AdvancedSettings() {
            InitializeComponent();

            actionFolderPath.KeyDown += new KeyEventHandler(FreakingStopDingSound);
            actionFileExtension.KeyDown += new KeyEventHandler(FreakingStopDingSound);

            void FreakingStopDingSound(Object o, KeyEventArgs e) {
                if (e.KeyCode == Keys.Enter) {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }

            actionFolderPath.KeyDown += delegate { pathChanged(); };
            actionFolderPath.KeyUp += delegate { pathChanged(); };

            void pathChanged()  {
                if (Directory.Exists(actionFolderPath.Text)) {
                    MainProgram.SetCheckFolder(actionFolderPath.Text);
                } else {
                    MessageBox.Show("Folder does not exist");
                    actionFolderPath.Text = MainProgram.CheckPath();
                }
            }

            actionFileExtension.KeyDown += delegate { MainProgram.SetCheckExtension(actionFileExtension.Text); };
            actionFileExtension.KeyUp += delegate { MainProgram.SetCheckExtension(actionFileExtension.Text); };

            actionFolderPath.Text = MainProgram.CheckPath();
            actionFileExtension.Text = Properties.Settings.Default.ActionFileExtension;

            mainPanel.Click += delegate { mainPanel.Focus(); };
            actionFolderPath.GotFocus += delegate { if (!hasUnfocused) { mainPanel.Focus(); hasUnfocused = true; } }; //Fixes it being auto-foxused
        }

        private void pickFolderBtn_Click(object sender, EventArgs e) {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog() {
                InitialDirectory = Directory.Exists(MainProgram.CheckPath()) ? MainProgram.CheckPath() : Assembly.GetEntryAssembly().Location,
                IsFolderPicker = true
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok) {
                if (Directory.Exists(dialog.FileName)) {
                    actionFolderPath.Text = dialog.FileName;
                } else {
                    MessageBox.Show("Folder does not exist");
                }
                MainProgram.SetCheckFolder(actionFolderPath.Text);
            }
        }

        private void shouldIEditLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("https://github.com/AlbertMN/AssistantComputerControl/wiki/Application-advanced-settings-expert-setup");
        }
    }
}
