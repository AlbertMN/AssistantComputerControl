/*
 * AssistantComputerControl
 * Made by Albert MN.
 * Updated: v1.4.0, 15-01-2020
 * 
 * Use:
 * - The 'Getting Started' setup guide
 */

using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssistantComputerControl {
    [ComVisible(true)]
    public partial class GettingStarted : Form {

        public static Form thisForm;
        public static WebBrowser theWebBrowser = null, theDoneActionViewBrowser;
        private static TabControl theTabControl;
        public static GettingStarted theInstance;
        public static bool isConfiguringActions = false;

        [ComVisible(true)]
        public class WebBrowserHandler {
            private static string backgroundCheckerServiceName;
            public static bool stopCheck = false;
            private static string customSetPath = String.Empty;

            private bool CheckSetPath(string chosenService) {
                if (customSetPath != String.Empty) {
                    if (Directory.Exists(customSetPath)) {
                        if (!customSetPath.Contains("AssistantComputerControl") && !customSetPath.Contains("assistantcomputercontrol")) {
                            customSetPath = Path.Combine(customSetPath, "AssistantComputerControl");
                            MainProgram.DoDebug("Changed path to include 'AssistantComputerControl': " + customSetPath);

                            if (!Directory.Exists(customSetPath))
                                Directory.CreateDirectory(customSetPath);
                        }

                        Properties.Settings.Default.ActionFilePath = customSetPath;
                        Properties.Settings.Default.Save();

                        MainProgram.SetupListener();
                        return true;
                    }
                } else {
                    string checkPath = CloudServiceFunctions.GetCloudServicePath(chosenService);
                    MainProgram.DoDebug("Checking: " + checkPath);
                    if (!String.IsNullOrEmpty(checkPath)) {
                        if (Directory.Exists(checkPath)) {
                            if (!checkPath.Contains("AssistantComputerControl") && !checkPath.Contains("assistantcomputercontrol")) {
                                checkPath = Path.Combine(checkPath, "AssistantComputerControl");
                                MainProgram.DoDebug("Changed path to include 'AssistantComputerControl': " + checkPath);

                                if (!Directory.Exists(checkPath))
                                    Directory.CreateDirectory(checkPath);
                            }

                            Properties.Settings.Default.ActionFilePath = checkPath;
                            Properties.Settings.Default.Save();

                            MainProgram.SetupListener();
                            return true;
                        }
                    }
                }
                return false;
            }

            public void SetPath(string chosenService) {
                if (!CheckSetPath(chosenService)) {
                    theWebBrowser.Document.InvokeScript("DoneError");
                }
            }

            public void AllDone(string chosenService) {
                MainProgram.DoDebug("'AllDone' pressed");
                if (CheckSetPath(chosenService)) {
                    theTabControl.SelectTab(3);

                    MainProgram.SetRegKey("ActionFolder", MainProgram.CheckPath());
                } else {
                    theWebBrowser.Document.InvokeScript("DoneError");
                }
            }

            public void ClearCustomSetPath() {
                customSetPath = String.Empty;
            }

            public void ExpertChosen() {
                theTabControl.SelectTab(2);
            }

            public void StopCheck() {
                stopCheck = true;
            }

            public void CheckManualPath(string path) {
                MainProgram.DoDebug("Checking manually-entered path...");
                try {
                    Path.GetFullPath(path);
                } catch {
                    MainProgram.DoDebug("Path not good");
                    theWebBrowser.Document.InvokeScript("ManualPathValidated", new Object[1] { false });
                    return;
                }
                if (Directory.Exists(path)) {
                    //Good
                    theWebBrowser.Document.InvokeScript("ManualPathValidated", new Object[1] { true });
                    MainProgram.DoDebug("Path good");
                    customSetPath = path;
                } else {
                    theWebBrowser.Document.InvokeScript("ManualPathValidated", new Object[1] { false });
                    MainProgram.DoDebug("Path not good");
                }
            }

            public void SkipGuide() {
                SetupDone();
                MainProgram.DoDebug("Skipped setup guide");
                thisForm.Close();
            }

            /* Translations */
            public void SetLanguage(string lang) {
                if (Array.Exists(Translator.languagesArray, element => element == lang)) {
                    MainProgram.DoDebug("Language \"" + lang + "\" chosen in 'Getting Started'");

                    Translator.SetLanguage(lang);

                    Properties.Settings.Default.ActiveLanguage = lang;
                    Properties.Settings.Default.Save();

                    SendTranslationToWeb();
                } else {
                    MessageBox.Show("Language is invalid - pick another one");
                    MainProgram.DoDebug("Invalid language chosen (Getting Started) - should not happen unless user tampers with the JSON files");
                }
            }

            public void SendTranslationToWeb() {
                //theWebBrowser.Invoke(new Action(() => {
                //theWebBrowser.Document.InvokeScript("actionWentThrough", objArray);
                //}));

                //var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                //var json = serializer.Serialize(Translator.languagesArray);

                //TODO; check why this crashed once. Have not been able to re-create.
                /*
                 * Microsoft.CSharp.RuntimeBinder.RuntimeBinderException: 'Cannot perform runtime binding on a null reference'
                 */

                object obj2 = null;
                if (Translator.activeLanguage != "English") {
                    obj2 = Convert.ToString(Translator.fallbackDynamicJsonTranslation["translations"]["getting_started"]);
                }

                theWebBrowser.Invoke(new Action(() => {
                    theWebBrowser.Document.InvokeScript(
                        "SetTranslation",
                        new Object[2] {
                            Convert.ToString(Translator.dynamicJsonTranslation["translations"]["getting_started"]),
                            obj2
                        }
                    );
                }));
            }

            /* End translations */

            public void SetupSuccess(string cloud_serivce) {
                theDoneActionViewBrowser.Url = new Uri("https://assistantcomputercontrol.com/integrated_action_grid.php?lang=" + Properties.Settings.Default.ActiveLanguage + "&max_version_number=" + MainProgram.softwareVersion + "&cloud_service=" + cloud_serivce);

                thisForm.Size = new Size(thisForm.Size.Width, thisForm.Size.Height + 150);
                theTabControl.Size = new Size(theTabControl.Size.Width, theTabControl.Size.Height + 150);
                theWebBrowser.Size = new Size(theWebBrowser.Size.Width, theWebBrowser.Size.Height + 150);

                theTabControl.SelectTab(3);
                SetupDone();
            }

            public void ValidIE() {
                theInstance.backToSetupGuide.Visible = true;
                theTabControl.SelectTab(1);
            }

            public void SetODtype(string type) {
                MainProgram.DoDebug("Setting OneDrive type to " + type);
                MainProgram.SetCheckFolder(Environment.GetEnvironmentVariable(type));
            }

            public void CloudServiceChosen(string service = "") {
                switch (service) {
                    case "dropbox":
                    case "onedrive":
                    case "googledrive":
                        backgroundCheckerServiceName = service;
                        break;
                    default:
                        return;
                }

                if (CloudServiceFunctions.GetCloudServicePath(backgroundCheckerServiceName) != "") {
                    //Cloud service found
                    MainProgram.DoDebug("Cloud service " + backgroundCheckerServiceName + " is installed");
                    bool partial = false;

                    if (backgroundCheckerServiceName == "googledrive") {
                        partial = CloudServiceFunctions.GetGoogleDriveFolder() != String.Empty;
                    }

                    if (theWebBrowser != null) {
                        IntPtr theHandle = IntPtr.Zero;
                        try {
                            theHandle = theWebBrowser.Handle;
                        } catch {
                            MainProgram.DoDebug("Failed to get web browser handle.");
                            MessageBox.Show(Translator.__("cloud_setup_failed", "general"), MainProgram.messageBoxTitle);
                        }

                        if (theHandle != IntPtr.Zero) {
                            if (theWebBrowser.Handle != null) {
                                theWebBrowser.Document.InvokeScript("CloudServiceInstalled", new Object[2] { true, partial });
                            }
                        }
                    }

                    if (partial) {
                        CheckLocalGoogleDrive();
                    }
                } else {
                    //Not found
                    new Thread(() => {
                        Thread.CurrentThread.IsBackground = true;
                        string checkValue = "";
                        stopCheck = false;

                        MainProgram.DoDebug("Could not find cloud service. Running loop to check");
                        while (checkValue == "" && !stopCheck) {
                            checkValue = CloudServiceFunctions.GetCloudServicePath(backgroundCheckerServiceName);
                            Thread.Sleep(1000);
                        }
                        if (stopCheck) {
                            stopCheck = false;
                            return;
                        }
                        
                        //Cloud service has been installed since we last checked!
                        MainProgram.DoDebug("Cloud service has been installed since last check. Proceed.");

                        if (theWebBrowser != null) {
                            if (theWebBrowser.Handle != null) {
                                theWebBrowser.Invoke(new Action(() => {
                                    if (backgroundCheckerServiceName == "googledrive") {
                                        bool partial = CloudServiceFunctions.GetGoogleDriveFolder() != String.Empty;
                                        theWebBrowser.Document.InvokeScript("CloudServiceInstalled", new Object[2] { true, partial });
                                        if (partial)
                                            CheckLocalGoogleDrive();
                                    } else {
                                        theWebBrowser.Document.InvokeScript("CloudServiceInstalled", new Object[1] { true });
                                    }
                                }));
                            }
                        }
                    }).Start();
                }
            }

            private void CheckLocalGoogleDrive() {
                new Thread(() => {
                    Thread.CurrentThread.IsBackground = true;
                    stopCheck = false;

                    MainProgram.DoDebug("Starting loop to check for Google Drive folder locally");
                    while (backgroundCheckerServiceName == "googledrive" && CloudServiceFunctions.GetGoogleDriveFolder() == String.Empty && !stopCheck) {
                        Thread.Sleep(1000);
                    }
                    if (stopCheck) {
                        stopCheck = false;
                        return;
                    }

                    if (backgroundCheckerServiceName == "googledrive") {
                        //Cloud service has been installed since we last checked!
                        MainProgram.DoDebug("Google Drive has been added to local PC. Proceed.");
                        theWebBrowser.Invoke(new Action(() => {
                            theWebBrowser.Document.InvokeScript("CloudServiceInstalled", new Object[2] { true, false });
                        }));
                    } else {
                        MainProgram.DoDebug("Service has since been changed. Stopping the search for Google Drive.");
                    }
                }).Start();
            }
        }

        public GettingStarted(int startTab = 0) {
            //Start function

            thisForm = this;

            InitializeComponent();
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            theTabControl = tabControl;

            FormClosed += delegate {
                if (MainProgram.aboutVersionAwaiting) {
                    Properties.Settings.Default.LastKnownVersion = MainProgram.softwareVersion;
                    new NewVersion().Show();
                    Properties.Settings.Default.Save();
                }
            };

            tabControl.Appearance = TabAppearance.FlatButtons;
            tabControl.ItemSize = new Size(0, 1);
            tabControl.SizeMode = TabSizeMode.Fixed;
            tabControl.BackColor = Color.White;
            tabControl.SelectTab(startTab);

            tabControl.Selected += delegate {
                if (tabControl.SelectedIndex == 1) {
                    //Clicked on recommended setup guide (HTML), can show "move on" popover now
                    //theWebBrowser.Document.InvokeScript("showHelpPopover"); // Why would I do this...?
                } else if (tabControl.SelectedIndex == 2) {
                    expert.Focus();
                }
            };

            backToSetupGuide.Visible = false;
            theInstance = this;

            //Set GettingStarted web-browser things (unless user has IE >9)
            //Check for IE version using JS, as the C# way requires admin rights, which we don't want to ask for just because of this...

            string fileName = Path.Combine(MainProgram.currentLocation, "WebFiles/IECheck.html");
            /* Internet Explorer test */
            if (File.Exists(fileName)) {
                string fileLoc = "file:///" + fileName;
                Uri theUri = new Uri(fileLoc);
                ieWebBrowser.Url = theUri;
            } else {
                ieWebBrowser.Visible = false;
            }

            ieWebBrowser.ObjectForScripting = new WebBrowserHandler();
            theWebBrowser = ieWebBrowser;

            //theWebBrowser.DocumentCompleted += BrowserDocumentCompleted;
            theWebBrowser.Navigating += BrowserNavigating;
            theWebBrowser.NewWindow += NewBrowserWindow;

            /* Getting Started */
            fileName = Path.Combine(MainProgram.currentLocation, "WebFiles/GettingStarted.html");
            if (File.Exists(fileName)) {
                string fileLoc = "file:///" + fileName;
                Uri theUri = new Uri(fileLoc);
                GettingStartedWebBrowser.Url = theUri;
            } else {
                GettingStartedWebBrowser.Visible = false;
            }

            GettingStartedWebBrowser.ObjectForScripting = new WebBrowserHandler();
            theWebBrowser = GettingStartedWebBrowser;
            theDoneActionViewBrowser = doneActionViewBrowser;

            theDoneActionViewBrowser.DocumentCompleted += DoneActionGridLoadCompleted;
            theDoneActionViewBrowser.Navigating += BrowserNavigating;
            theDoneActionViewBrowser.NewWindow += NewBrowserWindow;

            theWebBrowser.DocumentCompleted += BrowserDocumentCompleted;

            theWebBrowser.Navigating += BrowserNavigating;
            theWebBrowser.NewWindow += NewBrowserWindow;


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

            VisibleChanged += delegate {
                MainProgram.testingAction = Visible;
                MainProgram.gettingStarted = Visible ? this : null;
                isConfiguringActions = Visible;
            };
            FormClosed += delegate {
                //Is needed
                isConfiguringActions = false;
                MainProgram.testingAction = false;
                MainProgram.gettingStarted = null;
            };


            this.HandleCreated += delegate {
                Invoke(new Action(() => {
                    FlashWindow.Flash(this);
                    if (Application.OpenForms[this.Name] != null) {
                        Application.OpenForms[this.Name].Activate();
                        Application.OpenForms[this.Name].Focus();
                    }
                }));
            };

            //"Expert setup" translations
            customSetupTitle.Text = Translator.__("title", "expert_setup");
            customSetupInfo.Text = Translator.__("description", "expert_setup");
            actionFolderPathLabel.Text = Translator.__("action_folder_path", "expert_setup");
            actionFileExtensionLabel.Text = Translator.__("action_file_extension", "expert_setup");
            disclaimerLabel.Text = Translator.__("disclaimer", "expert_setup");
            backToSetupGuide.Text = Translator.__("back_to_setup", "expert_setup");
            expertDoneButton.Text = Translator.__("done_button", "expert_setup");
        } // End main function

        public void SendActionThrough(Object[] objArray) {
            /*if ((string)objArray[0] == "success") { //We don't wanna do this...
                SetupDone();
            }*/
            this.Invoke(new Action(() => {
                FlashWindow.Flash(this);
                if (Application.OpenForms[this.Name] != null) {
                    Application.OpenForms[this.Name].Activate();
                    Application.OpenForms[this.Name].Focus();
                }
            }));

            theWebBrowser.Invoke(new Action(() => {
                theWebBrowser.Document.InvokeScript("actionWentThrough", objArray);
            }));
        }

        private void BrowserNavigating(object sender, WebBrowserNavigatingEventArgs e) {
            if (e.Url.ToString() == "#" || e.Url.ToString() == "about:blank") {
                return;
            }

            if (!(e.Url.ToString().Equals("about:blank", StringComparison.InvariantCultureIgnoreCase))) {
                //Process.Start(e.Url.ToString());
                //e.Cancel = true;
                //return;
            }
        }

        private void DoneActionGridLoadCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {
            this.SetupDocumentLinks((sender as WebBrowser).Document.All);
        }

        private void BrowserDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {
            //OneDrive business?
            bool hasWorkDrive = !String.IsNullOrEmpty(Environment.GetEnvironmentVariable("OneDriveCommercial")) && !String.IsNullOrEmpty(Environment.GetEnvironmentVariable("OneDriveConsumer"));
            MainProgram.DoDebug("Has work OneDrive? " + hasWorkDrive);
            theWebBrowser.Document.InvokeScript("HasWorkOneDrive", new Object[1] { hasWorkDrive });
            theWebBrowser.Document.InvokeScript("SetAccVersionNum", new Object[1] { MainProgram.softwareVersion });

            /*if ((WebBrowser)sender != null) {
            //Maybe add this to avoid errors - rarely happens though
            }*/

            this.SetupDocumentLinks((sender as WebBrowser).Document.All);

            /* Translation stuff */
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(Translator.languagesArray);
            
            theWebBrowser.Document.InvokeScript("SetLanguages", new Object[1] { json });
        }

        void SetupDocumentLinks(HtmlElementCollection elements) {
            string tagUpper = "";

            foreach (HtmlElement tag in elements) {
                tagUpper = tag.TagName.ToUpper();

                if ((tagUpper == "AREA") || (tagUpper == "A")) {
                    tag.MouseUp += new HtmlElementEventHandler(link_MouseUp);
                }
            }

            void link_MouseUp(object sender, HtmlElementEventArgs e) {
                Regex pattern = new Regex("href=\\\"(.+?)\\\"");
                Match match = pattern.Match((sender as HtmlElement).OuterHtml);
                if (match.Groups.Count >= 1) {
                    string link = match.Groups[1].Value;

                    if (link.Length > 0) {
                        if (link[0] != '#') {
                            Process.Start(link);
                        }
                    }
                }
            }
        }
        
        private void NewBrowserWindow(object sender, CancelEventArgs e) {
            e.Cancel = true;
        }

        private static void SetupDone() {
            MainProgram.testingAction = false;
            isConfiguringActions = false;

            MainProgram.DoDebug("Completed setup guide");
            Properties.Settings.Default.HasCompletedTutorial = true;
            Properties.Settings.Default.Save();
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
            theDoneActionViewBrowser.Url = new Uri("https://assistantcomputercontrol.com/integrated_action_grid.php?lang=" + Properties.Settings.Default.ActiveLanguage + "&max_version_number=" + MainProgram.softwareVersion + "&cloud_service=none");

            thisForm.Size = new Size(thisForm.Size.Width, thisForm.Size.Height + 150);
            theTabControl.Size = new Size(theTabControl.Size.Width, theTabControl.Size.Height + 150);
            theWebBrowser.Size = new Size(theWebBrowser.Size.Width, theWebBrowser.Size.Height + 150);

            theTabControl.SelectTab(3);
            SetupDone();
        }

        private void iftttActions_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("https://assistantcomputercontrol.com/#what-can-it-do");
        }

        private void skipGuide_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            SetupDone();
            MainProgram.DoDebug("Skipped setup guide");
            Close();
        }

        private void backToSetupGuide_Click(object sender, EventArgs e) {
            tabControl.SelectTab(1);
        }
    }
}