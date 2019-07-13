namespace AssistantComputerControl {
    partial class AdvancedSettings {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvancedSettings));
            this.label1 = new System.Windows.Forms.Label();
            this.actionFolderPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.saveActionPath = new System.Windows.Forms.SaveFileDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.actionFileExtension = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.shouldIEditLink = new System.Windows.Forms.LinkLabel();
            this.pickFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.pickFolderBtn = new System.Windows.Forms.Button();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.saveActionFolder = new System.Windows.Forms.Button();
            this.mainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Advanced Settings";
            // 
            // actionFolderPath
            // 
            this.actionFolderPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.actionFolderPath.Location = new System.Drawing.Point(7, 117);
            this.actionFolderPath.Name = "actionFolderPath";
            this.actionFolderPath.Size = new System.Drawing.Size(341, 20);
            this.actionFolderPath.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Action folder path";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 184);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Action file extension";
            // 
            // actionFileExtension
            // 
            this.actionFileExtension.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.actionFileExtension.Location = new System.Drawing.Point(7, 200);
            this.actionFileExtension.Name = "actionFileExtension";
            this.actionFileExtension.Size = new System.Drawing.Size(73, 20);
            this.actionFileExtension.TabIndex = 3;
            this.actionFileExtension.Text = "txt";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(246, 52);
            this.label4.TabIndex = 5;
            this.label4.Text = "Tampering with these settings will change the way \r\n AssistantComputerControl wor" +
    "ks completely.\r\nIt\'s only for those who do not wish to use Dropbox\r\n(the only bu" +
    "ilt-in solution at the moment)";
            // 
            // shouldIEditLink
            // 
            this.shouldIEditLink.AutoSize = true;
            this.shouldIEditLink.Location = new System.Drawing.Point(3, 76);
            this.shouldIEditLink.Name = "shouldIEditLink";
            this.shouldIEditLink.Size = new System.Drawing.Size(100, 13);
            this.shouldIEditLink.TabIndex = 6;
            this.shouldIEditLink.TabStop = true;
            this.shouldIEditLink.Text = "Should I edit this...?";
            this.shouldIEditLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.shouldIEditLink_LinkClicked);
            // 
            // pickFolderBtn
            // 
            this.pickFolderBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.pickFolderBtn.Location = new System.Drawing.Point(348, 116);
            this.pickFolderBtn.Name = "pickFolderBtn";
            this.pickFolderBtn.Size = new System.Drawing.Size(23, 22);
            this.pickFolderBtn.TabIndex = 7;
            this.pickFolderBtn.Text = "...";
            this.pickFolderBtn.UseVisualStyleBackColor = true;
            this.pickFolderBtn.Click += new System.EventHandler(this.pickFolderBtn_Click);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.saveActionFolder);
            this.mainPanel.Controls.Add(this.label1);
            this.mainPanel.Controls.Add(this.pickFolderBtn);
            this.mainPanel.Controls.Add(this.label4);
            this.mainPanel.Controls.Add(this.actionFileExtension);
            this.mainPanel.Controls.Add(this.label3);
            this.mainPanel.Controls.Add(this.shouldIEditLink);
            this.mainPanel.Controls.Add(this.label2);
            this.mainPanel.Controls.Add(this.actionFolderPath);
            this.mainPanel.Location = new System.Drawing.Point(12, 12);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(382, 301);
            this.mainPanel.TabIndex = 8;
            // 
            // saveActionFolder
            // 
            this.saveActionFolder.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.saveActionFolder.Location = new System.Drawing.Point(7, 143);
            this.saveActionFolder.Name = "saveActionFolder";
            this.saveActionFolder.Size = new System.Drawing.Size(342, 23);
            this.saveActionFolder.TabIndex = 8;
            this.saveActionFolder.Text = "Save action folder";
            this.saveActionFolder.UseVisualStyleBackColor = true;
            this.saveActionFolder.Click += new System.EventHandler(this.saveActionFolder_Click);
            // 
            // AdvancedSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(406, 325);
            this.Controls.Add(this.mainPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AdvancedSettings";
            this.Text = "Advanced Settings | ACC";
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox actionFolderPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.SaveFileDialog saveActionPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox actionFileExtension;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel shouldIEditLink;
        private System.Windows.Forms.FolderBrowserDialog pickFolder;
        private System.Windows.Forms.Button pickFolderBtn;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Button saveActionFolder;
    }
}