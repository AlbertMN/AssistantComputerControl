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
            this.GettingStartedWebBrowser = new System.Windows.Forms.WebBrowser();
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
            this.expert.SuspendLayout();
            this.analytics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.Done.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.setupSelect);
            this.tabControl.Controls.Add(this.expert);
            this.tabControl.Controls.Add(this.analytics);
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
            this.setupSelect.Controls.Add(this.GettingStartedWebBrowser);
            this.setupSelect.Location = new System.Drawing.Point(4, 22);
            this.setupSelect.Name = "setupSelect";
            this.setupSelect.Padding = new System.Windows.Forms.Padding(3);
            this.setupSelect.Size = new System.Drawing.Size(783, 445);
            this.setupSelect.TabIndex = 0;
            this.setupSelect.Text = "Select setup";
            // 
            // GettingStartedWebBrowser
            // 
            this.GettingStartedWebBrowser.AllowNavigation = false;
            this.GettingStartedWebBrowser.AllowWebBrowserDrop = false;
            this.GettingStartedWebBrowser.IsWebBrowserContextMenuEnabled = false;
            this.GettingStartedWebBrowser.Location = new System.Drawing.Point(-1, 1);
            this.GettingStartedWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.GettingStartedWebBrowser.Name = "GettingStartedWebBrowser";
            this.GettingStartedWebBrowser.Size = new System.Drawing.Size(784, 442);
            this.GettingStartedWebBrowser.TabIndex = 5;
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
            this.analytics.Location = new System.Drawing.Point(4, 22);
            this.analytics.Name = "analytics";
            this.analytics.Size = new System.Drawing.Size(783, 445);
            this.analytics.TabIndex = 4;
            this.analytics.Text = "analytics";
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label15.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label15.Location = new System.Drawing.Point(6, 427);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(774, 23);
            this.label15.TabIndex = 22;
            this.label15.Text = "Your choice to decline or accept is logged for statistical purposes.";
            this.label15.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AssistantComputerControl.Properties.Resources.logo_PNG;
            this.pictureBox1.Location = new System.Drawing.Point(355, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(75, 75);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 20;
            this.pictureBox1.TabStop = false;
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label14.Location = new System.Drawing.Point(6, 266);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(774, 23);
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
            this.analyticsMoveOn.Location = new System.Drawing.Point(206, 366);
            this.analyticsMoveOn.Name = "analyticsMoveOn";
            this.analyticsMoveOn.Size = new System.Drawing.Size(365, 52);
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
            this.analyticsEnabledBox.Location = new System.Drawing.Point(288, 337);
            this.analyticsEnabledBox.Name = "analyticsEnabledBox";
            this.analyticsEnabledBox.Size = new System.Drawing.Size(207, 24);
            this.analyticsEnabledBox.TabIndex = 18;
            this.analyticsEnabledBox.Text = "Enable anonymous analytics";
            this.analyticsEnabledBox.UseVisualStyleBackColor = true;
            this.analyticsEnabledBox.CheckedChanged += new System.EventHandler(this.analyticsEnabledBox_CheckedChanged);
            // 
            // analyticsLearnMore
            // 
            this.analyticsLearnMore.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.analyticsLearnMore.Location = new System.Drawing.Point(6, 287);
            this.analyticsLearnMore.Name = "analyticsLearnMore";
            this.analyticsLearnMore.Size = new System.Drawing.Size(774, 26);
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
            this.label13.Location = new System.Drawing.Point(3, 167);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(777, 22);
            this.label13.TabIndex = 15;
            this.label13.Text = "The core collected data is;";
            this.label13.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label12.Location = new System.Drawing.Point(300, 190);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(244, 61);
            this.label12.TabIndex = 14;
            this.label12.Text = "• which actions you execute\r\n• how many times you execute them\r\n• which assistant" +
    " you use the most";
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label11.Location = new System.Drawing.Point(3, 116);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(777, 45);
            this.label11.TabIndex = 13;
            this.label11.Text = resources.GetString("label11.Text");
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(3, 76);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(777, 42);
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
            this.iftttActions.Size = new System.Drawing.Size(771, 23);
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
        private System.Windows.Forms.TabPage Done;
        private System.Windows.Forms.TabPage expert;
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
        private System.Windows.Forms.CheckBox startWithWindowsCheckbox;
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
        private System.Windows.Forms.WebBrowser GettingStartedWebBrowser;
    }
}