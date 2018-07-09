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
            this.versionInfo = new System.Windows.Forms.Label();
            this.checkForUpdate = new System.Windows.Forms.Button();
            this.betaProgram = new System.Windows.Forms.CheckBox();
            this.fileEditedMargin = new System.Windows.Forms.NumericUpDown();
            this.infoTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileEditedMargin)).BeginInit();
            this.SuspendLayout();
            // 
            // headingLabel
            // 
            this.headingLabel.AutoSize = true;
            this.headingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headingLabel.Location = new System.Drawing.Point(4, 3);
            this.headingLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.headingLabel.Name = "headingLabel";
            this.headingLabel.Size = new System.Drawing.Size(195, 29);
            this.headingLabel.TabIndex = 0;
            this.headingLabel.Text = "Overall settings";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 194);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(208, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "File edited margin (seconds)";
            // 
            // checkUpdates
            // 
            this.checkUpdates.AutoSize = true;
            this.checkUpdates.Location = new System.Drawing.Point(10, 82);
            this.checkUpdates.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkUpdates.Name = "checkUpdates";
            this.checkUpdates.Size = new System.Drawing.Size(174, 24);
            this.checkUpdates.TabIndex = 4;
            this.checkUpdates.Text = "Check for updates?";
            this.checkUpdates.UseVisualStyleBackColor = true;
            this.checkUpdates.CheckedChanged += new System.EventHandler(this.checkUpdates_CheckedChanged);
            // 
            // advancedSettingsButton
            // 
            this.advancedSettingsButton.Location = new System.Drawing.Point(10, 283);
            this.advancedSettingsButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.advancedSettingsButton.Name = "advancedSettingsButton";
            this.advancedSettingsButton.Size = new System.Drawing.Size(204, 35);
            this.advancedSettingsButton.TabIndex = 5;
            this.advancedSettingsButton.Text = "Advanced settings";
            this.advancedSettingsButton.UseVisualStyleBackColor = true;
            this.advancedSettingsButton.Click += new System.EventHandler(this.advancedSettingsButton_Click);
            // 
            // testButton
            // 
            this.testButton.Location = new System.Drawing.Point(10, 372);
            this.testButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(204, 35);
            this.testButton.TabIndex = 6;
            this.testButton.Text = "Test assistant";
            this.testButton.UseVisualStyleBackColor = true;
            this.testButton.Click += new System.EventHandler(this.testButton_Click);
            // 
            // startWithWindows
            // 
            this.startWithWindows.AutoSize = true;
            this.startWithWindows.Location = new System.Drawing.Point(10, 46);
            this.startWithWindows.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.startWithWindows.Name = "startWithWindows";
            this.startWithWindows.Size = new System.Drawing.Size(179, 24);
            this.startWithWindows.TabIndex = 7;
            this.startWithWindows.Text = "Start with Windows?";
            this.startWithWindows.UseVisualStyleBackColor = true;
            this.startWithWindows.CheckedChanged += new System.EventHandler(this.startWithWindows_CheckedChanged);
            // 
            // computerName
            // 
            this.computerName.Location = new System.Drawing.Point(10, 151);
            this.computerName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.computerName.Name = "computerName";
            this.computerName.Size = new System.Drawing.Size(178, 26);
            this.computerName.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 126);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "Computer name";
            // 
            // logButton
            // 
            this.logButton.Location = new System.Drawing.Point(10, 328);
            this.logButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.logButton.Name = "logButton";
            this.logButton.Size = new System.Drawing.Size(204, 35);
            this.logButton.TabIndex = 10;
            this.logButton.Text = "Open log";
            this.logButton.UseVisualStyleBackColor = true;
            this.logButton.Click += new System.EventHandler(this.logButton_Click);
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.mainPanel.Controls.Add(this.button1);
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
            this.mainPanel.Location = new System.Drawing.Point(18, 18);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(1164, 655);
            this.mainPanel.TabIndex = 11;
            this.mainPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.mainPanel_Paint);
            // 
            // versionInfo
            // 
            this.versionInfo.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.versionInfo.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.versionInfo.Location = new System.Drawing.Point(813, 615);
            this.versionInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.versionInfo.Name = "versionInfo";
            this.versionInfo.Size = new System.Drawing.Size(346, 35);
            this.versionInfo.TabIndex = 14;
            this.versionInfo.Text = "Version 0.3.3";
            this.versionInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkForUpdate
            // 
            this.checkForUpdate.Location = new System.Drawing.Point(10, 615);
            this.checkForUpdate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkForUpdate.Name = "checkForUpdate";
            this.checkForUpdate.Size = new System.Drawing.Size(164, 35);
            this.checkForUpdate.TabIndex = 13;
            this.checkForUpdate.Text = "Check for update";
            this.checkForUpdate.UseVisualStyleBackColor = true;
            this.checkForUpdate.Click += new System.EventHandler(this.checkForUpdate_Click);
            // 
            // betaProgram
            // 
            this.betaProgram.AutoSize = true;
            this.betaProgram.Location = new System.Drawing.Point(198, 82);
            this.betaProgram.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.betaProgram.Name = "betaProgram";
            this.betaProgram.Size = new System.Drawing.Size(199, 24);
            this.betaProgram.TabIndex = 12;
            this.betaProgram.Text = "Join the beta program?";
            this.betaProgram.UseVisualStyleBackColor = true;
            this.betaProgram.CheckedChanged += new System.EventHandler(this.betaProgram_CheckedChanged);
            // 
            // fileEditedMargin
            // 
            this.fileEditedMargin.Location = new System.Drawing.Point(10, 218);
            this.fileEditedMargin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.fileEditedMargin.Name = "fileEditedMargin";
            this.fileEditedMargin.Size = new System.Drawing.Size(180, 26);
            this.fileEditedMargin.TabIndex = 11;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(10, 417);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(204, 35);
            this.button1.TabIndex = 15;
            this.button1.Text = "Do the setup guide again";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1200, 692);
            this.Controls.Add(this.mainPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
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
        private System.Windows.Forms.Button button1;
    }
}