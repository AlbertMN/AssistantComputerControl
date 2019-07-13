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
            this.Done.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.setupSelect);
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
        private System.Windows.Forms.WebBrowser GettingStartedWebBrowser;
    }
}