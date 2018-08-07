using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssistantComputerControl {
    public partial class GettingStarted : Form {

        private MyPanel selectedPanel = null;
        public WebBrowser theWebBrowser = null;

        public GettingStarted() {
            InitializeComponent();

            tabControl.Appearance = TabAppearance.FlatButtons;
            tabControl.ItemSize = new Size(0, 1);
            tabControl.SizeMode = TabSizeMode.Fixed;
            tabControl.BackColor = Color.Transparent;

            tabControl.Selected += delegate {
                if (tabControl.SelectedIndex == 1) {
                    //Clicked on recommended setup guide (HTML), can show "move on" popover now
                    theWebBrowser.Document.InvokeScript("showHelpPopover");
                } else if (tabControl.SelectedIndex == 2) {
                    expert.Focus();
                }
            };

            //Auto-select "recommended" panel
            RecommendedClicked(null, null);
            finalOptionButton.FlatStyle = FlatStyle.Flat;
            finalOptionButton.FlatAppearance.BorderSize = 0;

            setupSelect.MouseHover += delegate {
                if (selectedPanel != recommendedPanel) {
                    recommendedPanel.borderColor = Pens.Black;
                    recommendedPanel.Refresh();
                }
                if (selectedPanel != expertPanel) {
                    expertPanel.borderColor = Pens.Black;
                    expertPanel.Refresh();
                }
            };

            //Recommended panel
            recommendedPanel.MouseHover += RecommendedHovered;
            recommendedPanel.Click += RecommendedClicked;

            recommendedLabel.MouseHover += RecommendedHovered;
            recommendedLabel.Click += RecommendedClicked;

            recommendedLabel2.MouseHover += RecommendedHovered;
            recommendedLabel2.Click += RecommendedClicked;

            recommendedLabel3.MouseHover += RecommendedHovered;
            recommendedLabel3.Click += RecommendedClicked;

            recommendedImage.MouseHover += RecommendedHovered;
            recommendedImage.Click += RecommendedClicked;

            //Expert panel
            expertPanel.MouseHover += ExpertHovered;
            expertPanel.Click += ExpertClicked;

            expertLabel1.MouseHover += ExpertHovered;
            expertLabel1.Click += ExpertClicked;

            expertLabel2.MouseHover += ExpertHovered;
            expertLabel2.Click += ExpertClicked;

            expertLabel2.MouseHover += ExpertHovered;
            expertLabel2.Click += ExpertClicked;

            expertLabel3.MouseHover += ExpertHovered;
            expertLabel3.Click += delegate {
                Process.Start("https://acc.readme.io/v1.0/docs/application-advanced-settings-expert-setup");
            };
            tooltip.SetToolTip(expertLabel3, "This will open a link in your default browser");

            expertLabel4.MouseHover += ExpertHovered;
            expertLabel4.Click += ExpertClicked;

            expertImage.MouseHover += ExpertHovered;
            expertImage.Click += ExpertClicked;

            //Further expert settings
            actionFolderPath.KeyDown += new KeyEventHandler(FreakingStopDingSound);
            actionFileExtension.KeyDown += new KeyEventHandler(FreakingStopDingSound);

            void FreakingStopDingSound(Object o, KeyEventArgs e) {
                if (e.KeyCode == Keys.Enter) {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }

            actionFolderPath.Text = MainProgram.CheckPath();
            actionFileExtension.Text = Properties.Settings.Default.ActionFileExtension;

            actionFolderPath.KeyDown += delegate {
                MainProgram.SetCheckFolder(actionFolderPath.Text);
                actionFolderPath.Text = MainProgram.CheckPath();
            };
            actionFolderPath.KeyUp += delegate {
                MainProgram.SetCheckFolder(actionFolderPath.Text);
                actionFolderPath.Text = MainProgram.CheckPath();
            };

            actionFileExtension.KeyDown += delegate { MainProgram.SetCheckExtension(actionFileExtension.Text); };
            actionFileExtension.KeyUp += delegate { MainProgram.SetCheckExtension(actionFileExtension.Text); };

            expert.Click += delegate {
                expert.Focus();
            };
            customSetupInfo.Click += delegate {
                expert.Focus();
            };

            expertDoneButton.FlatStyle = FlatStyle.Flat;
            expertDoneButton.FlatAppearance.BorderSize = 0;

            closeWindowButton.FlatStyle = FlatStyle.Flat;
            closeWindowButton.FlatAppearance.BorderSize = 0;

            //Browser
            theWebBrowser = GuideWebBrowser;

            theWebBrowser.DocumentCompleted += BrowserDocumentCompleted;
            theWebBrowser.Navigating += BrowserNavigating;
            theWebBrowser.NewWindow += NewBrowserWindow;

            VisibleChanged += delegate {
                MainProgram.testingAction = Visible;
                MainProgram.gettingStarted = Visible ? this : null;
            };
            FormClosed += delegate {
                MainProgram.testingAction = false;
                MainProgram.gettingStarted = null;
            };
        }

        public void SendActionThrough(Object[] objArray) {
            if ((string)objArray[0] == "success") {
                SetupDone();
            }
            theWebBrowser.Invoke(new Action(() => {
                theWebBrowser.Document.InvokeScript("actionWentThrough", objArray);
            }));
        }

        private void BrowserNavigating(object sender, WebBrowserNavigatingEventArgs e) {
            if (!(e.Url.ToString().Equals("about:blank", StringComparison.InvariantCultureIgnoreCase))) {
                Process.Start(e.Url.ToString());
                e.Cancel = true;
            }

            e.Cancel = true;
            Process.Start(e.Url.ToString());
        }
        private void BrowserDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {
            if (MainProgram.GetDropboxFolder() != "") {
                //Dropbox found
                theWebBrowser.Document.InvokeScript("dropboxInstalled", new Object[1] { true });
            } else {
                //Not found
                new Thread(() => {
                    Thread.CurrentThread.IsBackground = true;
                    
                    while (MainProgram.GetDropboxFolder() == "") {
                        Thread.Sleep(1000);
                    }
                    //Dropbox has been installed!
                    theWebBrowser.Document.InvokeScript("dropboxInstalled", new Object[1] { true });
                }).Start();
            }


            string tagUpper = "";

            foreach (HtmlElement tag in (sender as WebBrowser).Document.All) {
                tagUpper = tag.TagName.ToUpper();

                if ((tagUpper == "AREA") || (tagUpper == "A")) {
                    tag.MouseUp += new HtmlElementEventHandler(this.link_MouseUp);
                }
            }
        }
        void link_MouseUp(object sender, HtmlElementEventArgs e) {
            Regex pattern = new Regex("href=\\\"(.+?)\\\"");
            Match match = pattern.Match((sender as HtmlElement).OuterHtml);
            string link = match.Groups[1].Value;

            if (link[0] != '#')
                Process.Start(link);
            else if (link == "#recommendedFinished") {
                tabControl.SelectTab(3);
            }
        }
        private void NewBrowserWindow(object sender, CancelEventArgs e) {
            e.Cancel = true;
        }


        //Recommended panel
        void RecommendedHovered(object sender, EventArgs e) {
            if (selectedPanel != recommendedPanel) {
                recommendedPanel.borderColor = Pens.CornflowerBlue;
                recommendedPanel.Refresh();
            }
            if (selectedPanel != expertPanel) {
                expertPanel.borderColor = Pens.Black;
                expertPanel.Refresh();
            }
        }
        void RecommendedClicked(object sender, EventArgs e) {
            if (selectedPanel != recommendedPanel) {
                MainProgram.DoDebug("Selected recommended");
                finalOptionButton.Text = "Recommended setup";
                selectedPanel = recommendedPanel;
                recommendedPanel.borderColor = Pens.Blue;

                expertPanel.borderColor = Pens.Black;
                expertPanel.Refresh();
            }

            recommendedPanel.Refresh();
        }
        
        //Expert panel
        void ExpertHovered(object sender, EventArgs e) {
            if (selectedPanel != expertPanel) {
                expertPanel.borderColor = Pens.CornflowerBlue;
                expertPanel.Refresh();
            }
            if (selectedPanel != recommendedPanel) {
                recommendedPanel.borderColor = Pens.Black;
                recommendedPanel.Refresh();
            }
        }
        void ExpertClicked(object sender, EventArgs e) {
            if (selectedPanel != expertPanel) {
                MainProgram.DoDebug("Selected expert");
                finalOptionButton.Text = "Expert setup (are you sure?)";
                selectedPanel = expertPanel;
                expertPanel.borderColor = Pens.Blue;

                recommendedPanel.borderColor = Pens.Black;
                recommendedPanel.Refresh();

            }
            expertPanel.Refresh();
        } 

        private void SetupDone() {
            //Start with Windows if user said so
            if (Properties.Settings.Default.StartWithWindows != startWithWindowsCheckbox.Checked) {
                Properties.Settings.Default.StartWithWindows = startWithWindowsCheckbox.Checked;
                MainProgram.SetStartup(startWithWindowsCheckbox.Checked);

                Properties.Settings.Default.Save();

                MainProgram.DoDebug("Starting with Windows now");
            }
            MainProgram.DoDebug("Completed setup guide");
            Properties.Settings.Default.HasCompletedTutorial = true;
            Properties.Settings.Default.Save();
        }

        private void finalOptionButton_Click(object sender, EventArgs e) {
            if (selectedPanel == recommendedPanel) {
                tabControl.SelectTab(1);
            } else {
                tabControl.SelectTab(2);
                expert.Focus();
            }
        }

        private void pickFolderBtn_Click(object sender, EventArgs e) {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog() {
                InitialDirectory = MainProgram.CheckPath(),
                IsFolderPicker = true
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok) {
                MainProgram.SetCheckFolder(dialog.FileName);
                actionFolderPath.Text = MainProgram.CheckPath();
            }
        }

        private void expertDoneButton_Click(object sender, EventArgs e) {
            tabControl.SelectTab(3);
        }

        private void closeWindowButton_Click(object sender, EventArgs e) {
            SetupDone();
            Close();
        }

        private void iftttActions_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("https://github.com/AlbertMN/AssistantComputerControl#supported-computer-actions");
        }

        private void skipGuide_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            SetupDone();
            MainProgram.DoDebug("Skipped setup guide");
            Close();
        }

        private void gotoGoogleDriveGuide_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("https://acc.readme.io/docs/use-google-drive-ifttt-instead-of-dropbox");
        }
    }
}