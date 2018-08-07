namespace AssistantComputerControl {
    partial class GettingStarted {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GettingStarted));
            this.tooltip = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl = new System.Windows.Forms.TabControl();
            this.setupSelect = new System.Windows.Forms.TabPage();
            this.skipGuide = new System.Windows.Forms.LinkLabel();
            this.finalOptionButton = new System.Windows.Forms.Button();
            this.recommended = new System.Windows.Forms.TabPage();
            this.GuideWebBrowser = new System.Windows.Forms.WebBrowser();
            this.expert = new System.Windows.Forms.TabPage();
            this.gotoGoogleDriveGuide = new System.Windows.Forms.LinkLabel();
            this.label6 = new System.Windows.Forms.Label();
            this.expertDoneButton = new System.Windows.Forms.Button();
            this.pickFolderBtn = new System.Windows.Forms.Button();
            this.customSetupInfo = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.actionFileExtension = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.actionFolderPath = new System.Windows.Forms.TextBox();
            this.Done = new System.Windows.Forms.TabPage();
            this.startWithWindowsCheckbox = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.iftttActions = new System.Windows.Forms.LinkLabel();
            this.closeWindowButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pickFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.expertPanel = new AssistantComputerControl.MyPanel();
            this.expertLabel3 = new System.Windows.Forms.LinkLabel();
            this.expertLabel4 = new System.Windows.Forms.Label();
            this.expertLabel2 = new System.Windows.Forms.Label();
            this.expertLabel1 = new System.Windows.Forms.Label();
            this.expertImage = new System.Windows.Forms.PictureBox();
            this.recommendedPanel = new AssistantComputerControl.MyPanel();
            this.label10 = new System.Windows.Forms.Label();
            this.recommendedLabel3 = new System.Windows.Forms.Label();
            this.recommendedLabel2 = new System.Windows.Forms.Label();
            this.recommendedLabel = new System.Windows.Forms.Label();
            this.recommendedImage = new System.Windows.Forms.PictureBox();
            this.tabControl.SuspendLayout();
            this.setupSelect.SuspendLayout();
            this.recommended.SuspendLayout();
            this.expert.SuspendLayout();
            this.Done.SuspendLayout();
            this.expertPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.expertImage)).BeginInit();
            this.recommendedPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recommendedImage)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.setupSelect);
            this.tabControl.Controls.Add(this.recommended);
            this.tabControl.Controls.Add(this.expert);
            this.tabControl.Controls.Add(this.Done);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(791, 471);
            this.tabControl.TabIndex = 6;
            // 
            // setupSelect
            // 
            this.setupSelect.BackColor = System.Drawing.Color.White;
            this.setupSelect.Controls.Add(this.skipGuide);
            this.setupSelect.Controls.Add(this.expertPanel);
            this.setupSelect.Controls.Add(this.recommendedPanel);
            this.setupSelect.Controls.Add(this.finalOptionButton);
            this.setupSelect.Location = new System.Drawing.Point(4, 22);
            this.setupSelect.Name = "setupSelect";
            this.setupSelect.Padding = new System.Windows.Forms.Padding(3);
            this.setupSelect.Size = new System.Drawing.Size(783, 445);
            this.setupSelect.TabIndex = 0;
            this.setupSelect.Text = "Select setup";
            // 
            // skipGuide
            // 
            this.skipGuide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.skipGuide.AutoSize = true;
            this.skipGuide.Location = new System.Drawing.Point(587, 429);
            this.skipGuide.Name = "skipGuide";
            this.skipGuide.Size = new System.Drawing.Size(193, 13);
            this.skipGuide.TabIndex = 19;
            this.skipGuide.TabStop = true;
            this.skipGuide.Text = "No thanks, I don\'t need no guide! (skip)";
            this.skipGuide.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.skipGuide_LinkClicked);
            // 
            // finalOptionButton
            // 
            this.finalOptionButton.BackColor = System.Drawing.SystemColors.Highlight;
            this.finalOptionButton.FlatAppearance.BorderSize = 0;
            this.finalOptionButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.finalOptionButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.finalOptionButton.Location = new System.Drawing.Point(71, 347);
            this.finalOptionButton.Name = "finalOptionButton";
            this.finalOptionButton.Size = new System.Drawing.Size(626, 41);
            this.finalOptionButton.TabIndex = 8;
            this.finalOptionButton.Text = "Recommended setup";
            this.finalOptionButton.UseVisualStyleBackColor = false;
            this.finalOptionButton.Click += new System.EventHandler(this.finalOptionButton_Click);
            // 
            // recommended
            // 
            this.recommended.Controls.Add(this.GuideWebBrowser);
            this.recommended.Location = new System.Drawing.Point(4, 22);
            this.recommended.Name = "recommended";
            this.recommended.Padding = new System.Windows.Forms.Padding(3);
            this.recommended.Size = new System.Drawing.Size(783, 445);
            this.recommended.TabIndex = 1;
            this.recommended.Text = "Recommended chosen";
            this.recommended.UseVisualStyleBackColor = true;
            // 
            // GuideWebBrowser
            // 
            this.GuideWebBrowser.AllowNavigation = false;
            this.GuideWebBrowser.AllowWebBrowserDrop = false;
            this.GuideWebBrowser.IsWebBrowserContextMenuEnabled = false;
            this.GuideWebBrowser.Location = new System.Drawing.Point(-4, 0);
            this.GuideWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.GuideWebBrowser.Name = "GuideWebBrowser";
            this.GuideWebBrowser.ScriptErrorsSuppressed = true;
            this.GuideWebBrowser.ScrollBarsEnabled = false;
            this.GuideWebBrowser.Size = new System.Drawing.Size(784, 442);
            this.GuideWebBrowser.TabIndex = 4;
            this.GuideWebBrowser.Url = new System.Uri("http://acc.albe.pw/recommended_setup.html?4215", System.UriKind.Absolute);
            this.GuideWebBrowser.WebBrowserShortcutsEnabled = false;
            // 
            // expert
            // 
            this.expert.BackColor = System.Drawing.Color.White;
            this.expert.Controls.Add(this.gotoGoogleDriveGuide);
            this.expert.Controls.Add(this.label6);
            this.expert.Controls.Add(this.expertDoneButton);
            this.expert.Controls.Add(this.pickFolderBtn);
            this.expert.Controls.Add(this.customSetupInfo);
            this.expert.Controls.Add(this.label4);
            this.expert.Controls.Add(this.label1);
            this.expert.Controls.Add(this.actionFileExtension);
            this.expert.Controls.Add(this.label3);
            this.expert.Controls.Add(this.label2);
            this.expert.Controls.Add(this.actionFolderPath);
            this.expert.Location = new System.Drawing.Point(4, 22);
            this.expert.Name = "expert";
            this.expert.Size = new System.Drawing.Size(783, 445);
            this.expert.TabIndex = 2;
            this.expert.Text = "Expert chosen";
            // 
            // gotoGoogleDriveGuide
            // 
            this.gotoGoogleDriveGuide.AutoSize = true;
            this.gotoGoogleDriveGuide.Location = new System.Drawing.Point(438, 210);
            this.gotoGoogleDriveGuide.Name = "gotoGoogleDriveGuide";
            this.gotoGoogleDriveGuide.Size = new System.Drawing.Size(57, 13);
            this.gotoGoogleDriveGuide.TabIndex = 14;
            this.gotoGoogleDriveGuide.TabStop = true;
            this.gotoGoogleDriveGuide.Text = "Click here.";
            this.gotoGoogleDriveGuide.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.gotoGoogleDriveGuide_LinkClicked);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(20, 362);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(743, 21);
            this.label6.TabIndex = 13;
            this.label6.Text = "You can always edit these settings by right-clicking the ACC icon in the taskbar," +
    " click on \"Settings\" and then \"Advanced settings\"";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // expertDoneButton
            // 
            this.expertDoneButton.BackColor = System.Drawing.Color.LimeGreen;
            this.expertDoneButton.FlatAppearance.BorderSize = 0;
            this.expertDoneButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expertDoneButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.expertDoneButton.Location = new System.Drawing.Point(18, 386);
            this.expertDoneButton.Name = "expertDoneButton";
            this.expertDoneButton.Size = new System.Drawing.Size(745, 41);
            this.expertDoneButton.TabIndex = 12;
            this.expertDoneButton.Text = "Done";
            this.expertDoneButton.UseVisualStyleBackColor = false;
            this.expertDoneButton.Click += new System.EventHandler(this.expertDoneButton_Click);
            // 
            // pickFolderBtn
            // 
            this.pickFolderBtn.Location = new System.Drawing.Point(320, 273);
            this.pickFolderBtn.Name = "pickFolderBtn";
            this.pickFolderBtn.Size = new System.Drawing.Size(25, 25);
            this.pickFolderBtn.TabIndex = 8;
            this.pickFolderBtn.Text = "...";
            this.pickFolderBtn.UseVisualStyleBackColor = true;
            this.pickFolderBtn.Click += new System.EventHandler(this.pickFolderBtn_Click);
            // 
            // customSetupInfo
            // 
            this.customSetupInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customSetupInfo.Location = new System.Drawing.Point(15, 46);
            this.customSetupInfo.Name = "customSetupInfo";
            this.customSetupInfo.Size = new System.Drawing.Size(554, 187);
            this.customSetupInfo.TabIndex = 11;
            this.customSetupInfo.Text = resources.GetString("customSetupInfo.Text");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 24);
            this.label4.TabIndex = 10;
            this.label4.Text = "Custom setup";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 329);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 24);
            this.label1.TabIndex = 9;
            this.label1.Text = ".";
            // 
            // actionFileExtension
            // 
            this.actionFileExtension.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actionFileExtension.Location = new System.Drawing.Point(31, 332);
            this.actionFileExtension.Name = "actionFileExtension";
            this.actionFileExtension.Size = new System.Drawing.Size(73, 23);
            this.actionFileExtension.TabIndex = 7;
            this.actionFileExtension.Text = "txt";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(15, 312);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Action file extension";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 254);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Action folder path";
            // 
            // actionFolderPath
            // 
            this.actionFolderPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actionFolderPath.Location = new System.Drawing.Point(18, 274);
            this.actionFolderPath.Name = "actionFolderPath";
            this.actionFolderPath.Size = new System.Drawing.Size(302, 23);
            this.actionFolderPath.TabIndex = 5;
            // 
            // Done
            // 
            this.Done.BackColor = System.Drawing.Color.White;
            this.Done.Controls.Add(this.startWithWindowsCheckbox);
            this.Done.Controls.Add(this.label9);
            this.Done.Controls.Add(this.iftttActions);
            this.Done.Controls.Add(this.closeWindowButton);
            this.Done.Controls.Add(this.label7);
            this.Done.Controls.Add(this.label5);
            this.Done.Location = new System.Drawing.Point(4, 22);
            this.Done.Name = "Done";
            this.Done.Size = new System.Drawing.Size(783, 445);
            this.Done.TabIndex = 3;
            this.Done.Text = "Done!";
            // 
            // startWithWindowsCheckbox
            // 
            this.startWithWindowsCheckbox.AutoSize = true;
            this.startWithWindowsCheckbox.Checked = true;
            this.startWithWindowsCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.startWithWindowsCheckbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startWithWindowsCheckbox.Location = new System.Drawing.Point(248, 165);
            this.startWithWindowsCheckbox.Name = "startWithWindowsCheckbox";
            this.startWithWindowsCheckbox.Size = new System.Drawing.Size(282, 17);
            this.startWithWindowsCheckbox.TabIndex = 17;
            this.startWithWindowsCheckbox.Text = "Start this software with Windows (highly recommended)";
            this.startWithWindowsCheckbox.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(337, 208);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(0, 17);
            this.label9.TabIndex = 16;
            // 
            // iftttActions
            // 
            this.iftttActions.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iftttActions.Location = new System.Drawing.Point(9, 209);
            this.iftttActions.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.iftttActions.Name = "iftttActions";
            this.iftttActions.Size = new System.Drawing.Size(771, 15);
            this.iftttActions.TabIndex = 14;
            this.iftttActions.TabStop = true;
            this.iftttActions.Text = "If you haven\'t enabled all the  IFTTT actions, click here to get a list of all th" +
    "e different applets!";
            this.iftttActions.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.iftttActions.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.iftttActions_LinkClicked);
            // 
            // closeWindowButton
            // 
            this.closeWindowButton.BackColor = System.Drawing.SystemColors.HotTrack;
            this.closeWindowButton.FlatAppearance.BorderSize = 0;
            this.closeWindowButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeWindowButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.closeWindowButton.Location = new System.Drawing.Point(114, 235);
            this.closeWindowButton.Name = "closeWindowButton";
            this.closeWindowButton.Size = new System.Drawing.Size(551, 68);
            this.closeWindowButton.TabIndex = 13;
            this.closeWindowButton.Text = "Close this Window";
            this.closeWindowButton.UseVisualStyleBackColor = false;
            this.closeWindowButton.Click += new System.EventHandler(this.closeWindowButton_Click);
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(3, 183);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(777, 24);
            this.label7.TabIndex = 12;
            this.label7.Text = "AssistantComputerControl is now fully set up. Happy computer-controlling!";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 128);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(777, 42);
            this.label5.TabIndex = 11;
            this.label5.Text = "All done!";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // expertPanel
            // 
            this.expertPanel.Controls.Add(this.expertLabel3);
            this.expertPanel.Controls.Add(this.expertLabel4);
            this.expertPanel.Controls.Add(this.expertLabel2);
            this.expertPanel.Controls.Add(this.expertLabel1);
            this.expertPanel.Controls.Add(this.expertImage);
            this.expertPanel.Location = new System.Drawing.Point(447, 66);
            this.expertPanel.Name = "expertPanel";
            this.expertPanel.Size = new System.Drawing.Size(250, 250);
            this.expertPanel.TabIndex = 4;
            // 
            // expertLabel3
            // 
            this.expertLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expertLabel3.Location = new System.Drawing.Point(133, 36);
            this.expertLabel3.Name = "expertLabel3";
            this.expertLabel3.Size = new System.Drawing.Size(68, 17);
            this.expertLabel3.TabIndex = 4;
            this.expertLabel3.TabStop = true;
            this.expertLabel3.Text = "read more.";
            // 
            // expertLabel4
            // 
            this.expertLabel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.expertLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expertLabel4.Location = new System.Drawing.Point(3, 52);
            this.expertLabel4.Name = "expertLabel4";
            this.expertLabel4.Size = new System.Drawing.Size(244, 40);
            this.expertLabel4.TabIndex = 3;
            this.expertLabel4.Text = "Not recommended\r\nWill take you through the advanced settings\r\nNo comitment";
            this.expertLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // expertLabel2
            // 
            this.expertLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.expertLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expertLabel2.Location = new System.Drawing.Point(51, 32);
            this.expertLabel2.Name = "expertLabel2";
            this.expertLabel2.Size = new System.Drawing.Size(90, 19);
            this.expertLabel2.TabIndex = 2;
            this.expertLabel2.Text = "Total freedom";
            this.expertLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // expertLabel1
            // 
            this.expertLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.expertLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expertLabel1.Location = new System.Drawing.Point(3, 8);
            this.expertLabel1.Name = "expertLabel1";
            this.expertLabel1.Size = new System.Drawing.Size(244, 26);
            this.expertLabel1.TabIndex = 1;
            this.expertLabel1.Text = "Expert/custom setup";
            this.expertLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // expertImage
            // 
            this.expertImage.Image = global::AssistantComputerControl.Properties.Resources.ExpertIcon;
            this.expertImage.Location = new System.Drawing.Point(54, 100);
            this.expertImage.Name = "expertImage";
            this.expertImage.Size = new System.Drawing.Size(140, 108);
            this.expertImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.expertImage.TabIndex = 0;
            this.expertImage.TabStop = false;
            // 
            // recommendedPanel
            // 
            this.recommendedPanel.Controls.Add(this.label10);
            this.recommendedPanel.Controls.Add(this.recommendedLabel3);
            this.recommendedPanel.Controls.Add(this.recommendedLabel2);
            this.recommendedPanel.Controls.Add(this.recommendedLabel);
            this.recommendedPanel.Controls.Add(this.recommendedImage);
            this.recommendedPanel.Location = new System.Drawing.Point(71, 66);
            this.recommendedPanel.Name = "recommendedPanel";
            this.recommendedPanel.Size = new System.Drawing.Size(250, 250);
            this.recommendedPanel.TabIndex = 0;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(3, 48);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(245, 23);
            this.label10.TabIndex = 4;
            this.label10.Text = "\r\n(for those who use Google Assistant, or Amazon Alexa)";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // recommendedLabel3
            // 
            this.recommendedLabel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.recommendedLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recommendedLabel3.Location = new System.Drawing.Point(3, 70);
            this.recommendedLabel3.Name = "recommendedLabel3";
            this.recommendedLabel3.Size = new System.Drawing.Size(244, 19);
            this.recommendedLabel3.TabIndex = 3;
            this.recommendedLabel3.Text = "Recommended";
            this.recommendedLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // recommendedLabel2
            // 
            this.recommendedLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.recommendedLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recommendedLabel2.Location = new System.Drawing.Point(3, 32);
            this.recommendedLabel2.Name = "recommendedLabel2";
            this.recommendedLabel2.Size = new System.Drawing.Size(244, 19);
            this.recommendedLabel2.TabIndex = 2;
            this.recommendedLabel2.Text = "Setup using Dropbox and IFTTT";
            this.recommendedLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // recommendedLabel
            // 
            this.recommendedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.recommendedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recommendedLabel.Location = new System.Drawing.Point(3, 8);
            this.recommendedLabel.Name = "recommendedLabel";
            this.recommendedLabel.Size = new System.Drawing.Size(244, 26);
            this.recommendedLabel.TabIndex = 1;
            this.recommendedLabel.Text = "Recommended setup";
            this.recommendedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // recommendedImage
            // 
            this.recommendedImage.Image = global::AssistantComputerControl.Properties.Resources.DropboxRecommendedLogo;
            this.recommendedImage.Location = new System.Drawing.Point(53, 94);
            this.recommendedImage.Name = "recommendedImage";
            this.recommendedImage.Size = new System.Drawing.Size(140, 130);
            this.recommendedImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.recommendedImage.TabIndex = 0;
            this.recommendedImage.TabStop = false;
            // 
            // GettingStarted
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(815, 495);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "GettingStarted";
            this.Text = "Getting Started | AssistantComputerControl setup";
            this.tabControl.ResumeLayout(false);
            this.setupSelect.ResumeLayout(false);
            this.setupSelect.PerformLayout();
            this.recommended.ResumeLayout(false);
            this.expert.ResumeLayout(false);
            this.expert.PerformLayout();
            this.Done.ResumeLayout(false);
            this.Done.PerformLayout();
            this.expertPanel.ResumeLayout(false);
            this.expertPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.expertImage)).EndInit();
            this.recommendedPanel.ResumeLayout(false);
            this.recommendedPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recommendedImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip tooltip;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage setupSelect;
        private System.Windows.Forms.TabPage recommended;
        private System.Windows.Forms.Button finalOptionButton;
        private System.Windows.Forms.TabPage Done;
        private System.Windows.Forms.TabPage expert;
        private MyPanel recommendedPanel;
        private System.Windows.Forms.PictureBox recommendedImage;
        private System.Windows.Forms.Label recommendedLabel;
        private System.Windows.Forms.Label recommendedLabel2;
        private System.Windows.Forms.Label recommendedLabel3;
        private MyPanel expertPanel;
        private System.Windows.Forms.Label expertLabel4;
        private System.Windows.Forms.Label expertLabel2;
        private System.Windows.Forms.Label expertLabel1;
        private System.Windows.Forms.PictureBox expertImage;
        private System.Windows.Forms.LinkLabel expertLabel3;
        private System.Windows.Forms.WebBrowser GuideWebBrowser;
        private System.Windows.Forms.TextBox actionFileExtension;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox actionFolderPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label customSetupInfo;
        private System.Windows.Forms.FolderBrowserDialog pickFolder;
        private System.Windows.Forms.Button pickFolderBtn;
        private System.Windows.Forms.Button expertDoneButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button closeWindowButton;
        private System.Windows.Forms.LinkLabel iftttActions;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox startWithWindowsCheckbox;
        private System.Windows.Forms.LinkLabel skipGuide;
        private System.Windows.Forms.LinkLabel gotoGoogleDriveGuide;
    }
}