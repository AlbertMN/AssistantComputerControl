namespace AssistantComputerControl {
    partial class SettingsForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.headingLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.checkUpdates = new System.Windows.Forms.CheckBox();
            this.advancedSettingsButton = new System.Windows.Forms.Button();
            this.testButton = new System.Windows.Forms.Button();
            this.startWithWindows = new System.Windows.Forms.CheckBox();
            this.computerName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.logButton = new System.Windows.Forms.Button();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.deprecated = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.maxDeleteFiles = new System.Windows.Forms.NumericUpDown();
            this.warnDeletion = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.anonymousAnalyticsCheckbox = new System.Windows.Forms.CheckBox();
            this.doSetupAgain = new System.Windows.Forms.Button();
            this.versionInfo = new System.Windows.Forms.Label();
            this.checkForUpdate = new System.Windows.Forms.Button();
            this.betaProgram = new System.Windows.Forms.CheckBox();
            this.fileEditedMargin = new System.Windows.Forms.NumericUpDown();
            this.infoTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxDeleteFiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileEditedMargin)).BeginInit();
            this.SuspendLayout();
            // 
            // headingLabel
            // 
            this.headingLabel.AutoSize = true;
            this.headingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headingLabel.Location = new System.Drawing.Point(3, 2);
            this.headingLabel.Name = "headingLabel";
            this.headingLabel.Size = new System.Drawing.Size(133, 20);
            this.headingLabel.TabIndex = 0;
            this.headingLabel.Text = "Overall settings";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "File edited margin (seconds)";
            // 
            // checkUpdates
            // 
            this.checkUpdates.AutoSize = true;
            this.checkUpdates.Location = new System.Drawing.Point(7, 53);
            this.checkUpdates.Name = "checkUpdates";
            this.checkUpdates.Size = new System.Drawing.Size(119, 17);
            this.checkUpdates.TabIndex = 4;
            this.checkUpdates.Text = "Check for updates?";
            this.checkUpdates.UseVisualStyleBackColor = true;
            this.checkUpdates.CheckedChanged += new System.EventHandler(this.checkUpdates_CheckedChanged);
            // 
            // advancedSettingsButton
            // 
            this.advancedSettingsButton.Location = new System.Drawing.Point(7, 184);
            this.advancedSettingsButton.Name = "advancedSettingsButton";
            this.advancedSettingsButton.Size = new System.Drawing.Size(150, 23);
            this.advancedSettingsButton.TabIndex = 5;
            this.advancedSettingsButton.Text = "Advanced settings";
            this.advancedSettingsButton.UseVisualStyleBackColor = true;
            this.advancedSettingsButton.Click += new System.EventHandler(this.advancedSettingsButton_Click);
            // 
            // testButton
            // 
            this.testButton.Location = new System.Drawing.Point(7, 242);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(150, 23);
            this.testButton.TabIndex = 6;
            this.testButton.Text = "Action Simulator";
            this.testButton.UseVisualStyleBackColor = true;
            this.testButton.Click += new System.EventHandler(this.testButton_Click);
            // 
            // startWithWindows
            // 
            this.startWithWindows.AutoSize = true;
            this.startWithWindows.Location = new System.Drawing.Point(7, 30);
            this.startWithWindows.Name = "startWithWindows";
            this.startWithWindows.Size = new System.Drawing.Size(123, 17);
            this.startWithWindows.TabIndex = 7;
            this.startWithWindows.Text = "Start with Windows?";
            this.startWithWindows.UseVisualStyleBackColor = true;
            this.startWithWindows.CheckedChanged += new System.EventHandler(this.startWithWindows_CheckedChanged);
            // 
            // computerName
            // 
            this.computerName.Enabled = false;
            this.computerName.Location = new System.Drawing.Point(7, 98);
            this.computerName.Name = "computerName";
            this.computerName.Size = new System.Drawing.Size(120, 20);
            this.computerName.TabIndex = 9;
            this.computerName.Text = "Not yet implemented";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(186, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Computer name (Not yet implemented)";
            // 
            // logButton
            // 
            this.logButton.Location = new System.Drawing.Point(7, 213);
            this.logButton.Name = "logButton";
            this.logButton.Size = new System.Drawing.Size(150, 23);
            this.logButton.TabIndex = 10;
            this.logButton.Text = "Open log";
            this.logButton.UseVisualStyleBackColor = true;
            this.logButton.Click += new System.EventHandler(this.logButton_Click);
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.mainPanel.Controls.Add(this.deprecated);
            this.mainPanel.Controls.Add(this.label5);
            this.mainPanel.Controls.Add(this.label4);
            this.mainPanel.Controls.Add(this.maxDeleteFiles);
            this.mainPanel.Controls.Add(this.warnDeletion);
            this.mainPanel.Controls.Add(this.label3);
            this.mainPanel.Controls.Add(this.anonymousAnalyticsCheckbox);
            this.mainPanel.Controls.Add(this.doSetupAgain);
            this.mainPanel.Controls.Add(this.versionInfo);
            this.mainPanel.Controls.Add(this.checkForUpdate);
            this.mainPanel.Controls.Add(this.betaProgram);
            this.mainPanel.Controls.Add(this.fileEditedMargin);
            this.mainPanel.Controls.Add(this.headingLabel);
            this.mainPanel.Controls.Add(this.testButton);
            this.mainPanel.Controls.Add(this.logButton);
            this.mainPanel.Controls.Add(this.startWithWindows);
            this.mainPanel.Controls.Add(this.computerName);
            this.mainPanel.Controls.Add(this.advancedSettingsButton);
            this.mainPanel.Controls.Add(this.checkUpdates);
            this.mainPanel.Controls.Add(this.label1);
            this.mainPanel.Controls.Add(this.label2);
            this.mainPanel.Location = new System.Drawing.Point(12, 12);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(776, 426);
            this.mainPanel.TabIndex = 11;
            this.mainPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.mainPanel_Paint);
            // 
            // deprecated
            // 
            this.deprecated.AutoSize = true;
            this.deprecated.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deprecated.Location = new System.Drawing.Point(133, 144);
            this.deprecated.Name = "deprecated";
            this.deprecated.Size = new System.Drawing.Size(120, 13);
            this.deprecated.TabIndex = 22;
            this.deprecated.Text = "deprecated as of v1.1.0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(308, 324);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "(\"delete\" action)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(165, 324);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "files are about to be deleted";
            // 
            // maxDeleteFiles
            // 
            this.maxDeleteFiles.Location = new System.Drawing.Point(113, 322);
            this.maxDeleteFiles.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.maxDeleteFiles.Name = "maxDeleteFiles";
            this.maxDeleteFiles.Size = new System.Drawing.Size(46, 20);
            this.maxDeleteFiles.TabIndex = 19;
            this.maxDeleteFiles.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // warnDeletion
            // 
            this.warnDeletion.AutoSize = true;
            this.warnDeletion.Checked = true;
            this.warnDeletion.CheckState = System.Windows.Forms.CheckState.Checked;
            this.warnDeletion.Location = new System.Drawing.Point(8, 323);
            this.warnDeletion.Name = "warnDeletion";
            this.warnDeletion.Size = new System.Drawing.Size(108, 17);
            this.warnDeletion.TabIndex = 18;
            this.warnDeletion.Text = "Warn when over ";
            this.warnDeletion.UseVisualStyleBackColor = true;
            this.warnDeletion.CheckedChanged += new System.EventHandler(this.warnDeletion_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(157, 301);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(165, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "(amount of executed actions etc.)";
            // 
            // anonymousAnalyticsCheckbox
            // 
            this.anonymousAnalyticsCheckbox.AutoSize = true;
            this.anonymousAnalyticsCheckbox.Checked = true;
            this.anonymousAnalyticsCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.anonymousAnalyticsCheckbox.Location = new System.Drawing.Point(8, 300);
            this.anonymousAnalyticsCheckbox.Name = "anonymousAnalyticsCheckbox";
            this.anonymousAnalyticsCheckbox.Size = new System.Drawing.Size(152, 17);
            this.anonymousAnalyticsCheckbox.TabIndex = 16;
            this.anonymousAnalyticsCheckbox.Text = "Send anonymous analytics";
            this.anonymousAnalyticsCheckbox.UseVisualStyleBackColor = true;
            this.anonymousAnalyticsCheckbox.CheckedChanged += new System.EventHandler(this.anonymousAnalyticsCheckbox_CheckedChanged);
            // 
            // doSetupAgain
            // 
            this.doSetupAgain.Location = new System.Drawing.Point(7, 271);
            this.doSetupAgain.Name = "doSetupAgain";
            this.doSetupAgain.Size = new System.Drawing.Size(150, 23);
            this.doSetupAgain.TabIndex = 15;
            this.doSetupAgain.Text = "Do the setup guide again";
            this.doSetupAgain.UseVisualStyleBackColor = true;
            this.doSetupAgain.Click += new System.EventHandler(this.doSetupAgain_Click);
            // 
            // versionInfo
            // 
            this.versionInfo.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.versionInfo.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.versionInfo.Location = new System.Drawing.Point(542, 400);
            this.versionInfo.Name = "versionInfo";
            this.versionInfo.Size = new System.Drawing.Size(231, 23);
            this.versionInfo.TabIndex = 14;
            this.versionInfo.Text = "Version x";
            this.versionInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkForUpdate
            // 
            this.checkForUpdate.Location = new System.Drawing.Point(7, 400);
            this.checkForUpdate.Name = "checkForUpdate";
            this.checkForUpdate.Size = new System.Drawing.Size(109, 23);
            this.checkForUpdate.TabIndex = 13;
            this.checkForUpdate.Text = "Check for update";
            this.checkForUpdate.UseVisualStyleBackColor = true;
            this.checkForUpdate.Click += new System.EventHandler(this.checkForUpdate_Click);
            // 
            // betaProgram
            // 
            this.betaProgram.AutoSize = true;
            this.betaProgram.Location = new System.Drawing.Point(132, 53);
            this.betaProgram.Name = "betaProgram";
            this.betaProgram.Size = new System.Drawing.Size(134, 17);
            this.betaProgram.TabIndex = 12;
            this.betaProgram.Text = "Join the beta program?";
            this.betaProgram.UseVisualStyleBackColor = true;
            this.betaProgram.CheckedChanged += new System.EventHandler(this.betaProgram_CheckedChanged);
            // 
            // fileEditedMargin
            // 
            this.fileEditedMargin.Enabled = false;
            this.fileEditedMargin.Location = new System.Drawing.Point(7, 142);
            this.fileEditedMargin.Name = "fileEditedMargin";
            this.fileEditedMargin.Size = new System.Drawing.Size(120, 20);
            this.fileEditedMargin.TabIndex = 11;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.mainPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxDeleteFiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileEditedMargin)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label headingLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkUpdates;
        private System.Windows.Forms.Button advancedSettingsButton;
        private System.Windows.Forms.Button testButton;
        private System.Windows.Forms.CheckBox startWithWindows;
        private System.Windows.Forms.TextBox computerName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button logButton;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.NumericUpDown fileEditedMargin;
        private System.Windows.Forms.CheckBox betaProgram;
        private System.Windows.Forms.ToolTip infoTooltip;
        private System.Windows.Forms.Button checkForUpdate;
        private System.Windows.Forms.Label versionInfo;
        private System.Windows.Forms.Button doSetupAgain;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox anonymousAnalyticsCheckbox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown maxDeleteFiles;
        private System.Windows.Forms.CheckBox warnDeletion;
        private System.Windows.Forms.Label deprecated;
    }
}