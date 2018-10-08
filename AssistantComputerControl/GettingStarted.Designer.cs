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
            this.analytics = new System.Windows.Forms.TabPage();
            this.label15 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label14 = new System.Windows.Forms.Label();
            this.analyticsMoveOn = new System.Windows.Forms.Button();
            this.analyticsEnabledBox = new System.Windows.Forms.CheckBox();
            this.analyticsLearnMore = new System.Windows.Forms.LinkLabel();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.Done = new System.Windows.Forms.TabPage();
            this.startWithWindowsCheckbox = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.iftttActions = new System.Windows.Forms.LinkLabel();
            this.closeWindowButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pickFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.tabControl.SuspendLayout();
            this.setupSelect.SuspendLayout();
            this.expertPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.expertImage)).BeginInit();
            this.recommendedPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recommendedImage)).BeginInit();
            this.recommended.SuspendLayout();
            this.expert.SuspendLayout();
            this.analytics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.Done.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.setupSelect);
            this.tabControl.Controls.Add(this.recommended);
            this.tabControl.Controls.Add(this.expert);
            this.tabControl.Controls.Add(this.analytics);
            this.tabControl.Controls.Add(this.Done);
            this.tabControl.Location = new System.Drawing.Point(18, 18);
            this.tabControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1186, 725);
            this.tabControl.TabIndex = 6;
            // 
            // setupSelect
            // 
            this.setupSelect.BackColor = System.Drawing.Color.White;
            this.setupSelect.Controls.Add(this.skipGuide);
            this.setupSelect.Controls.Add(this.finalOptionButton);
            this.setupSelect.Controls.Add(this.expertPanel);
            this.setupSelect.Controls.Add(this.recommendedPanel);
            this.setupSelect.Location = new System.Drawing.Point(4, 29);
            this.setupSelect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.setupSelect.Name = "setupSelect";
            this.setupSelect.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.setupSelect.Size = new System.Drawing.Size(1178, 692);
            this.setupSelect.TabIndex = 0;
            this.setupSelect.Text = "Select setup";
            // 
            // skipGuide
            // 
            this.skipGuide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.skipGuide.AutoSize = true;
            this.skipGuide.Location = new System.Drawing.Point(880, 660);
            this.skipGuide.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.skipGuide.Name = "skipGuide";
            this.skipGuide.Size = new System.Drawing.Size(284, 20);
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
            this.finalOptionButton.Location = new System.Drawing.Point(106, 534);
            this.finalOptionButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.finalOptionButton.Name = "finalOptionButton";
            this.finalOptionButton.Size = new System.Drawing.Size(939, 63);
            this.finalOptionButton.TabIndex = 8;
            this.finalOptionButton.Text = "Recommended setup";
            this.finalOptionButton.UseVisualStyleBackColor = false;
            this.finalOptionButton.Click += new System.EventHandler(this.finalOptionButton_Click);
            // 
            // expertPanel
            // 
            this.expertPanel.Controls.Add(this.expertLabel3);
            this.expertPanel.Controls.Add(this.expertLabel4);
            this.expertPanel.Controls.Add(this.expertLabel2);
            this.expertPanel.Controls.Add(this.expertLabel1);
            this.expertPanel.Controls.Add(this.expertImage);
            this.expertPanel.Location = new System.Drawing.Point(670, 102);
            this.expertPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.expertPanel.Name = "expertPanel";
            this.expertPanel.Size = new System.Drawing.Size(375, 385);
            this.expertPanel.TabIndex = 4;
            // 
            // expertLabel3
            // 
            this.expertLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expertLabel3.Location = new System.Drawing.Point(200, 55);
            this.expertLabel3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.expertLabel3.Name = "expertLabel3";
            this.expertLabel3.Size = new System.Drawing.Size(102, 26);
            this.expertLabel3.TabIndex = 4;
            this.expertLabel3.TabStop = true;
            this.expertLabel3.Text = "read more.";
            // 
            // expertLabel4
            // 
            this.expertLabel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.expertLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expertLabel4.Location = new System.Drawing.Point(4, 80);
            this.expertLabel4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.expertLabel4.Name = "expertLabel4";
            this.expertLabel4.Size = new System.Drawing.Size(366, 62);
            this.expertLabel4.TabIndex = 3;
            this.expertLabel4.Text = "Not recommended\r\nWill take you through the advanced settings\r\nNo comitment";
            this.expertLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // expertLabel2
            // 
            this.expertLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.expertLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expertLabel2.Location = new System.Drawing.Point(76, 49);
            this.expertLabel2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.expertLabel2.Name = "expertLabel2";
            this.expertLabel2.Size = new System.Drawing.Size(135, 29);
            this.expertLabel2.TabIndex = 2;
            this.expertLabel2.Text = "Total freedom";
            this.expertLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // expertLabel1
            // 
            this.expertLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.expertLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expertLabel1.Location = new System.Drawing.Point(4, 12);
            this.expertLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.expertLabel1.Name = "expertLabel1";
            this.expertLabel1.Size = new System.Drawing.Size(366, 40);
            this.expertLabel1.TabIndex = 1;
            this.expertLabel1.Text = "Expert/custom setup";
            this.expertLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // expertImage
            // 
            this.expertImage.Image = global::AssistantComputerControl.Properties.Resources.ExpertIcon;
            this.expertImage.Location = new System.Drawing.Point(81, 154);
            this.expertImage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
            this.recommendedPanel.Location = new System.Drawing.Point(106, 102);
            this.recommendedPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.recommendedPanel.Name = "recommendedPanel";
            this.recommendedPanel.Size = new System.Drawing.Size(375, 385);
            this.recommendedPanel.TabIndex = 0;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(4, 74);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(368, 35);
            this.label10.TabIndex = 4;
            this.label10.Text = "\r\n(for those who use Google Assistant, or Amazon Alexa)";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // recommendedLabel3
            // 
            this.recommendedLabel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.recommendedLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recommendedLabel3.Location = new System.Drawing.Point(4, 108);
            this.recommendedLabel3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.recommendedLabel3.Name = "recommendedLabel3";
            this.recommendedLabel3.Size = new System.Drawing.Size(366, 29);
            this.recommendedLabel3.TabIndex = 3;
            this.recommendedLabel3.Text = "Recommended";
            this.recommendedLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // recommendedLabel2
            // 
            this.recommendedLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.recommendedLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recommendedLabel2.Location = new System.Drawing.Point(4, 49);
            this.recommendedLabel2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.recommendedLabel2.Name = "recommendedLabel2";
            this.recommendedLabel2.Size = new System.Drawing.Size(366, 29);
            this.recommendedLabel2.TabIndex = 2;
            this.recommendedLabel2.Text = "Setup using Dropbox and IFTTT";
            this.recommendedLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // recommendedLabel
            // 
            this.recommendedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.recommendedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recommendedLabel.Location = new System.Drawing.Point(4, 12);
            this.recommendedLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.recommendedLabel.Name = "recommendedLabel";
            this.recommendedLabel.Size = new System.Drawing.Size(366, 40);
            this.recommendedLabel.TabIndex = 1;
            this.recommendedLabel.Text = "Recommended setup";
            this.recommendedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // recommendedImage
            // 
            this.recommendedImage.Image = global::AssistantComputerControl.Properties.Resources.DropboxRecommendedLogo;
            this.recommendedImage.Location = new System.Drawing.Point(80, 145);
            this.recommendedImage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.recommendedImage.Name = "recommendedImage";
            this.recommendedImage.Size = new System.Drawing.Size(140, 130);
            this.recommendedImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.recommendedImage.TabIndex = 0;
            this.recommendedImage.TabStop = false;
            // 
            // recommended
            // 
            this.recommended.Controls.Add(this.GuideWebBrowser);
            this.recommended.Location = new System.Drawing.Point(4, 29);
            this.recommended.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.recommended.Name = "recommended";
            this.recommended.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.recommended.Size = new System.Drawing.Size(1178, 692);
            this.recommended.TabIndex = 1;
            this.recommended.Text = "Recommended chosen";
            this.recommended.UseVisualStyleBackColor = true;
            // 
            // GuideWebBrowser
            // 
            this.GuideWebBrowser.AllowNavigation = false;
            this.GuideWebBrowser.AllowWebBrowserDrop = false;
            this.GuideWebBrowser.IsWebBrowserContextMenuEnabled = false;
            this.GuideWebBrowser.Location = new System.Drawing.Point(-6, 0);
            this.GuideWebBrowser.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.GuideWebBrowser.MinimumSize = new System.Drawing.Size(30, 31);
            this.GuideWebBrowser.Name = "GuideWebBrowser";
            this.GuideWebBrowser.ScriptErrorsSuppressed = true;
            this.GuideWebBrowser.ScrollBarsEnabled = false;
            this.GuideWebBrowser.Size = new System.Drawing.Size(1176, 680);
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
            this.expert.Location = new System.Drawing.Point(4, 29);
            this.expert.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.expert.Name = "expert";
            this.expert.Size = new System.Drawing.Size(1178, 692);
            this.expert.TabIndex = 2;
            this.expert.Text = "Expert chosen";
            // 
            // gotoGoogleDriveGuide
            // 
            this.gotoGoogleDriveGuide.AutoSize = true;
            this.gotoGoogleDriveGuide.Location = new System.Drawing.Point(657, 323);
            this.gotoGoogleDriveGuide.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.gotoGoogleDriveGuide.Name = "gotoGoogleDriveGuide";
            this.gotoGoogleDriveGuide.Size = new System.Drawing.Size(82, 20);
            this.gotoGoogleDriveGuide.TabIndex = 14;
            this.gotoGoogleDriveGuide.TabStop = true;
            this.gotoGoogleDriveGuide.Text = "Click here.";
            this.gotoGoogleDriveGuide.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.gotoGoogleDriveGuide_LinkClicked);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(30, 557);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(1114, 32);
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
            this.expertDoneButton.Location = new System.Drawing.Point(27, 594);
            this.expertDoneButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.expertDoneButton.Name = "expertDoneButton";
            this.expertDoneButton.Size = new System.Drawing.Size(1118, 63);
            this.expertDoneButton.TabIndex = 12;
            this.expertDoneButton.Text = "Done";
            this.expertDoneButton.UseVisualStyleBackColor = false;
            this.expertDoneButton.Click += new System.EventHandler(this.expertDoneButton_Click);
            // 
            // pickFolderBtn
            // 
            this.pickFolderBtn.Location = new System.Drawing.Point(480, 420);
            this.pickFolderBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pickFolderBtn.Name = "pickFolderBtn";
            this.pickFolderBtn.Size = new System.Drawing.Size(38, 38);
            this.pickFolderBtn.TabIndex = 8;
            this.pickFolderBtn.Text = "...";
            this.pickFolderBtn.UseVisualStyleBackColor = true;
            this.pickFolderBtn.Click += new System.EventHandler(this.pickFolderBtn_Click);
            // 
            // customSetupInfo
            // 
            this.customSetupInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customSetupInfo.Location = new System.Drawing.Point(22, 71);
            this.customSetupInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.customSetupInfo.Name = "customSetupInfo";
            this.customSetupInfo.Size = new System.Drawing.Size(831, 288);
            this.customSetupInfo.TabIndex = 11;
            this.customSetupInfo.Text = resources.GetString("customSetupInfo.Text");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(18, 34);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(189, 32);
            this.label4.TabIndex = 10;
            this.label4.Text = "Custom setup";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(24, 506);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 32);
            this.label1.TabIndex = 9;
            this.label1.Text = ".";
            // 
            // actionFileExtension
            // 
            this.actionFileExtension.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actionFileExtension.Location = new System.Drawing.Point(46, 511);
            this.actionFileExtension.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.actionFileExtension.Name = "actionFileExtension";
            this.actionFileExtension.Size = new System.Drawing.Size(108, 30);
            this.actionFileExtension.TabIndex = 7;
            this.actionFileExtension.Text = "txt";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(22, 480);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(185, 25);
            this.label3.TabIndex = 8;
            this.label3.Text = "Action file extension";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(20, 391);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 25);
            this.label2.TabIndex = 6;
            this.label2.Text = "Action folder path";
            // 
            // actionFolderPath
            // 
            this.actionFolderPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actionFolderPath.Location = new System.Drawing.Point(27, 422);
            this.actionFolderPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.actionFolderPath.Name = "actionFolderPath";
            this.actionFolderPath.Size = new System.Drawing.Size(451, 30);
            this.actionFolderPath.TabIndex = 5;
            // 
            // analytics
            // 
            this.analytics.BackColor = System.Drawing.Color.White;
            this.analytics.Controls.Add(this.label15);
            this.analytics.Controls.Add(this.pictureBox1);
            this.analytics.Controls.Add(this.label14);
            this.analytics.Controls.Add(this.analyticsMoveOn);
            this.analytics.Controls.Add(this.analyticsEnabledBox);
            this.analytics.Controls.Add(this.analyticsLearnMore);
            this.analytics.Controls.Add(this.label13);
            this.analytics.Controls.Add(this.label12);
            this.analytics.Controls.Add(this.label11);
            this.analytics.Controls.Add(this.label8);
            this.analytics.Location = new System.Drawing.Point(4, 29);
            this.analytics.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.analytics.Name = "analytics";
            this.analytics.Size = new System.Drawing.Size(1178, 692);
            this.analytics.TabIndex = 4;
            this.analytics.Text = "analytics";
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label15.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label15.Location = new System.Drawing.Point(9, 657);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(1161, 35);
            this.label15.TabIndex = 22;
            this.label15.Text = "Your choice to decline or accept is logged for statistical purposes.";
            this.label15.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AssistantComputerControl.Properties.Resources.logo_PNG;
            this.pictureBox1.Location = new System.Drawing.Point(532, 11);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(112, 115);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 20;
            this.pictureBox1.TabStop = false;
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label14.Location = new System.Drawing.Point(9, 409);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(1161, 35);
            this.label14.TabIndex = 21;
            this.label14.Text = "We are not interested in data about you or your computer, just the ACC software a" +
    "nd how it\'s used.";
            this.label14.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // analyticsMoveOn
            // 
            this.analyticsMoveOn.BackColor = System.Drawing.Color.LightSeaGreen;
            this.analyticsMoveOn.FlatAppearance.BorderSize = 0;
            this.analyticsMoveOn.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.analyticsMoveOn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.analyticsMoveOn.Location = new System.Drawing.Point(309, 563);
            this.analyticsMoveOn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.analyticsMoveOn.Name = "analyticsMoveOn";
            this.analyticsMoveOn.Size = new System.Drawing.Size(548, 80);
            this.analyticsMoveOn.TabIndex = 19;
            this.analyticsMoveOn.Text = "OK";
            this.analyticsMoveOn.UseVisualStyleBackColor = false;
            this.analyticsMoveOn.Click += new System.EventHandler(this.analyticsMoveOn_Click);
            // 
            // analyticsEnabledBox
            // 
            this.analyticsEnabledBox.Checked = true;
            this.analyticsEnabledBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.analyticsEnabledBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.analyticsEnabledBox.Location = new System.Drawing.Point(432, 518);
            this.analyticsEnabledBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.analyticsEnabledBox.Name = "analyticsEnabledBox";
            this.analyticsEnabledBox.Size = new System.Drawing.Size(310, 37);
            this.analyticsEnabledBox.TabIndex = 18;
            this.analyticsEnabledBox.Text = "Enable anonymous analytics";
            this.analyticsEnabledBox.UseVisualStyleBackColor = true;
            this.analyticsEnabledBox.CheckedChanged += new System.EventHandler(this.analyticsEnabledBox_CheckedChanged);
            // 
            // analyticsLearnMore
            // 
            this.analyticsLearnMore.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.analyticsLearnMore.Location = new System.Drawing.Point(9, 442);
            this.analyticsLearnMore.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.analyticsLearnMore.Name = "analyticsLearnMore";
            this.analyticsLearnMore.Size = new System.Drawing.Size(1161, 40);
            this.analyticsLearnMore.TabIndex = 17;
            this.analyticsLearnMore.TabStop = true;
            this.analyticsLearnMore.Text = "If you wish to know more, click here for a more detailed article on exactly what " +
    "is gathered, and why.";
            this.analyticsLearnMore.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.analyticsLearnMore.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.analyticsLearnMore_LinkClicked);
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label13.Location = new System.Drawing.Point(4, 257);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(1166, 34);
            this.label13.TabIndex = 15;
            this.label13.Text = "The core collected data is;";
            this.label13.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label12.Location = new System.Drawing.Point(450, 292);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(366, 94);
            this.label12.TabIndex = 14;
            this.label12.Text = "• which actions you execute\r\n• how many times you execute them\r\n• which assistant" +
    " you use the most";
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label11.Location = new System.Drawing.Point(4, 178);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(1166, 69);
            this.label11.TabIndex = 13;
            this.label11.Text = resources.GetString("label11.Text");
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(4, 117);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(1166, 65);
            this.label8.TabIndex = 12;
            this.label8.Text = "Analytics - Want to help make ACC better?";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.Done.Location = new System.Drawing.Point(4, 29);
            this.Done.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Done.Name = "Done";
            this.Done.Size = new System.Drawing.Size(1178, 692);
            this.Done.TabIndex = 3;
            this.Done.Text = "Done!";
            // 
            // startWithWindowsCheckbox
            // 
            this.startWithWindowsCheckbox.AutoSize = true;
            this.startWithWindowsCheckbox.Checked = true;
            this.startWithWindowsCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.startWithWindowsCheckbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startWithWindowsCheckbox.Location = new System.Drawing.Point(372, 254);
            this.startWithWindowsCheckbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.startWithWindowsCheckbox.Name = "startWithWindowsCheckbox";
            this.startWithWindowsCheckbox.Size = new System.Drawing.Size(419, 24);
            this.startWithWindowsCheckbox.TabIndex = 17;
            this.startWithWindowsCheckbox.Text = "Start this software with Windows (highly recommended)";
            this.startWithWindowsCheckbox.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(506, 320);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(0, 25);
            this.label9.TabIndex = 16;
            // 
            // iftttActions
            // 
            this.iftttActions.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iftttActions.Location = new System.Drawing.Point(14, 322);
            this.iftttActions.Name = "iftttActions";
            this.iftttActions.Size = new System.Drawing.Size(1156, 35);
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
            this.closeWindowButton.Location = new System.Drawing.Point(171, 362);
            this.closeWindowButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.closeWindowButton.Name = "closeWindowButton";
            this.closeWindowButton.Size = new System.Drawing.Size(826, 105);
            this.closeWindowButton.TabIndex = 13;
            this.closeWindowButton.Text = "Close this Window";
            this.closeWindowButton.UseVisualStyleBackColor = false;
            this.closeWindowButton.Click += new System.EventHandler(this.closeWindowButton_Click);
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(4, 282);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(1166, 37);
            this.label7.TabIndex = 12;
            this.label7.Text = "AssistantComputerControl is now fully set up. Happy computer-controlling!";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(4, 197);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(1166, 65);
            this.label5.TabIndex = 11;
            this.label5.Text = "All done!";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GettingStarted
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1222, 762);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "GettingStarted";
            this.Text = "Getting Started | AssistantComputerControl setup";
            this.tabControl.ResumeLayout(false);
            this.setupSelect.ResumeLayout(false);
            this.setupSelect.PerformLayout();
            this.expertPanel.ResumeLayout(false);
            this.expertPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.expertImage)).EndInit();
            this.recommendedPanel.ResumeLayout(false);
            this.recommendedPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recommendedImage)).EndInit();
            this.recommended.ResumeLayout(false);
            this.expert.ResumeLayout(false);
            this.expert.PerformLayout();
            this.analytics.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.Done.ResumeLayout(false);
            this.Done.PerformLayout();
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
        private System.Windows.Forms.TabPage analytics;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.LinkLabel analyticsLearnMore;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox analyticsEnabledBox;
        private System.Windows.Forms.Button analyticsMoveOn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
    }
}