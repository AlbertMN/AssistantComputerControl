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
            this.saveLanguageButton = new System.Windows.Forms.Button();
            this.languageLabel = new System.Windows.Forms.Label();
            this.programLanguage = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.multiPcSupportReadMore = new System.Windows.Forms.LinkLabel();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.defaultComputer = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.fileReadDelay = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.maxDeleteFiles = new System.Windows.Forms.NumericUpDown();
            this.warnDeletion = new System.Windows.Forms.CheckBox();
            this.doSetupAgain = new System.Windows.Forms.Button();
            this.versionInfo = new System.Windows.Forms.Label();
            this.checkForUpdate = new System.Windows.Forms.Button();
            this.betaProgram = new System.Windows.Forms.CheckBox();
            this.fileEditedMargin = new System.Windows.Forms.NumericUpDown();
            this.infoTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileReadDelay)).BeginInit();
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
            this.headingLabel.Size = new System.Drawing.Size(175, 20);
            this.headingLabel.TabIndex = 0;
            this.headingLabel.Text = "overall_settings_title";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 165);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "file_edited_margin";
            // 
            // checkUpdates
            // 
            this.checkUpdates.AutoSize = true;
            this.checkUpdates.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkUpdates.Location = new System.Drawing.Point(8, 114);
            this.checkUpdates.Name = "checkUpdates";
            this.checkUpdates.Size = new System.Drawing.Size(124, 18);
            this.checkUpdates.TabIndex = 4;
            this.checkUpdates.Text = "check_for_updates";
            this.checkUpdates.UseVisualStyleBackColor = true;
            this.checkUpdates.CheckedChanged += new System.EventHandler(this.checkUpdates_CheckedChanged);
            // 
            // advancedSettingsButton
            // 
            this.advancedSettingsButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.advancedSettingsButton.Location = new System.Drawing.Point(8, 253);
            this.advancedSettingsButton.Name = "advancedSettingsButton";
            this.advancedSettingsButton.Size = new System.Drawing.Size(171, 23);
            this.advancedSettingsButton.TabIndex = 11;
            this.advancedSettingsButton.Text = "advanced_settings_btn";
            this.advancedSettingsButton.UseVisualStyleBackColor = true;
            this.advancedSettingsButton.Click += new System.EventHandler(this.advancedSettingsButton_Click);
            // 
            // testButton
            // 
            this.testButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.testButton.Location = new System.Drawing.Point(8, 311);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(171, 23);
            this.testButton.TabIndex = 13;
            this.testButton.Text = "action_simulator_btn";
            this.testButton.UseVisualStyleBackColor = true;
            this.testButton.Click += new System.EventHandler(this.testButton_Click);
            // 
            // startWithWindows
            // 
            this.startWithWindows.AutoSize = true;
            this.startWithWindows.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startWithWindows.Location = new System.Drawing.Point(8, 91);
            this.startWithWindows.Name = "startWithWindows";
            this.startWithWindows.Size = new System.Drawing.Size(124, 18);
            this.startWithWindows.TabIndex = 3;
            this.startWithWindows.Text = "start_with_windows";
            this.startWithWindows.UseVisualStyleBackColor = true;
            this.startWithWindows.CheckedChanged += new System.EventHandler(this.startWithWindows_CheckedChanged);
            // 
            // computerName
            // 
            this.computerName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.computerName.Location = new System.Drawing.Point(362, 124);
            this.computerName.Name = "computerName";
            this.computerName.Size = new System.Drawing.Size(120, 20);
            this.computerName.TabIndex = 22;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(359, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "computer_name_field";
            // 
            // logButton
            // 
            this.logButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.logButton.Location = new System.Drawing.Point(8, 282);
            this.logButton.Name = "logButton";
            this.logButton.Size = new System.Drawing.Size(171, 23);
            this.logButton.TabIndex = 12;
            this.logButton.Text = "open_log_btn";
            this.logButton.UseVisualStyleBackColor = true;
            this.logButton.Click += new System.EventHandler(this.logButton_Click);
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.mainPanel.Controls.Add(this.saveLanguageButton);
            this.mainPanel.Controls.Add(this.languageLabel);
            this.mainPanel.Controls.Add(this.programLanguage);
            this.mainPanel.Controls.Add(this.label10);
            this.mainPanel.Controls.Add(this.multiPcSupportReadMore);
            this.mainPanel.Controls.Add(this.label9);
            this.mainPanel.Controls.Add(this.label8);
            this.mainPanel.Controls.Add(this.defaultComputer);
            this.mainPanel.Controls.Add(this.label7);
            this.mainPanel.Controls.Add(this.fileReadDelay);
            this.mainPanel.Controls.Add(this.label6);
            this.mainPanel.Controls.Add(this.maxDeleteFiles);
            this.mainPanel.Controls.Add(this.warnDeletion);
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
            // saveLanguageButton
            // 
            this.saveLanguageButton.Location = new System.Drawing.Point(165, 61);
            this.saveLanguageButton.Name = "saveLanguageButton";
            this.saveLanguageButton.Size = new System.Drawing.Size(43, 21);
            this.saveLanguageButton.TabIndex = 2;
            this.saveLanguageButton.Text = "save_language_btn";
            this.saveLanguageButton.UseVisualStyleBackColor = true;
            this.saveLanguageButton.Click += new System.EventHandler(this.saveLanguageButton_Click);
            // 
            // languageLabel
            // 
            this.languageLabel.AutoSize = true;
            this.languageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.languageLabel.Location = new System.Drawing.Point(4, 42);
            this.languageLabel.Name = "languageLabel";
            this.languageLabel.Size = new System.Drawing.Size(145, 13);
            this.languageLabel.TabIndex = 33;
            this.languageLabel.Text = "program_language_label";
            // 
            // programLanguage
            // 
            this.programLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.programLanguage.FormattingEnabled = true;
            this.programLanguage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.programLanguage.Location = new System.Drawing.Point(7, 61);
            this.programLanguage.MaxDropDownItems = 15;
            this.programLanguage.Name = "programLanguage";
            this.programLanguage.Size = new System.Drawing.Size(154, 21);
            this.programLanguage.TabIndex = 1;
            this.programLanguage.Tag = "";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(488, 145);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(146, 13);
            this.label10.TabIndex = 24;
            this.label10.Text = "default_computer_description";
            // 
            // multiPcSupportReadMore
            // 
            this.multiPcSupportReadMore.AutoSize = true;
            this.multiPcSupportReadMore.Location = new System.Drawing.Point(363, 79);
            this.multiPcSupportReadMore.Name = "multiPcSupportReadMore";
            this.multiPcSupportReadMore.Size = new System.Drawing.Size(102, 13);
            this.multiPcSupportReadMore.TabIndex = 20;
            this.multiPcSupportReadMore.TabStop = true;
            this.multiPcSupportReadMore.Text = "multi_pc_read_more";
            this.multiPcSupportReadMore.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.multiPcSupportReadMore_LinkClicked);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(362, 40);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(121, 15);
            this.label9.TabIndex = 19;
            this.label9.Text = "multi_pc_description";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(359, 14);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(128, 22);
            this.label8.TabIndex = 18;
            this.label8.Text = "multi_pc_title";
            // 
            // defaultComputer
            // 
            this.defaultComputer.AutoSize = true;
            this.defaultComputer.Checked = true;
            this.defaultComputer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.defaultComputer.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.defaultComputer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.defaultComputer.Location = new System.Drawing.Point(489, 124);
            this.defaultComputer.Name = "defaultComputer";
            this.defaultComputer.Size = new System.Drawing.Size(167, 18);
            this.defaultComputer.TabIndex = 23;
            this.defaultComputer.Text = "default_computer_checkbox";
            this.defaultComputer.UseVisualStyleBackColor = true;
            this.defaultComputer.CheckedChanged += new System.EventHandler(this.defaultComputer_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(134, 225);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(130, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "file_read_delay_desc_text";
            this.infoTooltip.SetToolTip(this.label7, "Sometimes the software might read a file before it has been properly synced. This" +
        " setting will delay the file read x amount of seconds");
            // 
            // fileReadDelay
            // 
            this.fileReadDelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fileReadDelay.Location = new System.Drawing.Point(8, 223);
            this.fileReadDelay.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.fileReadDelay.Name = "fileReadDelay";
            this.fileReadDelay.Size = new System.Drawing.Size(120, 20);
            this.fileReadDelay.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 207);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "file_read_delay";
            // 
            // maxDeleteFiles
            // 
            this.maxDeleteFiles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.maxDeleteFiles.Location = new System.Drawing.Point(236, 397);
            this.maxDeleteFiles.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.maxDeleteFiles.Name = "maxDeleteFiles";
            this.maxDeleteFiles.Size = new System.Drawing.Size(46, 20);
            this.maxDeleteFiles.TabIndex = 17;
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
            this.warnDeletion.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.warnDeletion.Location = new System.Drawing.Point(218, 375);
            this.warnDeletion.Name = "warnDeletion";
            this.warnDeletion.Size = new System.Drawing.Size(127, 18);
            this.warnDeletion.TabIndex = 16;
            this.warnDeletion.Text = "delete_warning_text";
            this.warnDeletion.UseVisualStyleBackColor = true;
            this.warnDeletion.CheckedChanged += new System.EventHandler(this.warnDeletion_CheckedChanged);
            // 
            // doSetupAgain
            // 
            this.doSetupAgain.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.doSetupAgain.Location = new System.Drawing.Point(8, 340);
            this.doSetupAgain.Name = "doSetupAgain";
            this.doSetupAgain.Size = new System.Drawing.Size(171, 23);
            this.doSetupAgain.TabIndex = 14;
            this.doSetupAgain.Text = "do_setup_again";
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
            this.versionInfo.TabIndex = 25;
            this.versionInfo.Text = "Version x";
            this.versionInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkForUpdate
            // 
            this.checkForUpdate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkForUpdate.Location = new System.Drawing.Point(7, 400);
            this.checkForUpdate.Name = "checkForUpdate";
            this.checkForUpdate.Size = new System.Drawing.Size(171, 23);
            this.checkForUpdate.TabIndex = 15;
            this.checkForUpdate.Text = "check_for_updates_btn";
            this.checkForUpdate.UseVisualStyleBackColor = true;
            this.checkForUpdate.Click += new System.EventHandler(this.checkForUpdate_Click);
            // 
            // betaProgram
            // 
            this.betaProgram.AutoSize = true;
            this.betaProgram.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.betaProgram.Location = new System.Drawing.Point(8, 138);
            this.betaProgram.Name = "betaProgram";
            this.betaProgram.Size = new System.Drawing.Size(119, 18);
            this.betaProgram.TabIndex = 5;
            this.betaProgram.Text = "join_beta_program";
            this.betaProgram.UseVisualStyleBackColor = true;
            this.betaProgram.CheckedChanged += new System.EventHandler(this.betaProgram_CheckedChanged);
            // 
            // fileEditedMargin
            // 
            this.fileEditedMargin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fileEditedMargin.Location = new System.Drawing.Point(8, 181);
            this.fileEditedMargin.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.fileEditedMargin.Name = "fileEditedMargin";
            this.fileEditedMargin.Size = new System.Drawing.Size(120, 20);
            this.fileEditedMargin.TabIndex = 7;
            // 
            // infoTooltip
            // 
            this.infoTooltip.Popup += new System.Windows.Forms.PopupEventHandler(this.infoTooltip_Popup);
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
            ((System.ComponentModel.ISupportInitialize)(this.fileReadDelay)).EndInit();
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
        private System.Windows.Forms.NumericUpDown maxDeleteFiles;
        private System.Windows.Forms.CheckBox warnDeletion;
        private System.Windows.Forms.NumericUpDown fileReadDelay;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox defaultComputer;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.LinkLabel multiPcSupportReadMore;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox programLanguage;
        private System.Windows.Forms.Label languageLabel;
        private System.Windows.Forms.Button saveLanguageButton;
    }
}