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
            this.pickFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.expert = new System.Windows.Forms.TabPage();
            this.backToSetupGuide = new System.Windows.Forms.Button();
            this.disclaimerLabel = new System.Windows.Forms.Label();
            this.expertDoneButton = new System.Windows.Forms.Button();
            this.pickFolderBtn = new System.Windows.Forms.Button();
            this.customSetupInfo = new System.Windows.Forms.Label();
            this.customSetupTitle = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.actionFileExtension = new System.Windows.Forms.TextBox();
            this.actionFolderPath = new System.Windows.Forms.TextBox();
            this.actionFileExtensionLabel = new System.Windows.Forms.Label();
            this.actionFolderPathLabel = new System.Windows.Forms.Label();
            this.setupSelect = new System.Windows.Forms.TabPage();
            this.GettingStartedWebBrowser = new System.Windows.Forms.WebBrowser();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.ieTest = new System.Windows.Forms.TabPage();
            this.ieWebBrowser = new System.Windows.Forms.WebBrowser();
            this.Done = new System.Windows.Forms.TabPage();
            this.doneActionViewBrowser = new System.Windows.Forms.WebBrowser();
            this.expert.SuspendLayout();
            this.setupSelect.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.ieTest.SuspendLayout();
            this.Done.SuspendLayout();
            this.SuspendLayout();
            // 
            // expert
            // 
            this.expert.BackColor = System.Drawing.Color.White;
            this.expert.Controls.Add(this.backToSetupGuide);
            this.expert.Controls.Add(this.disclaimerLabel);
            this.expert.Controls.Add(this.expertDoneButton);
            this.expert.Controls.Add(this.pickFolderBtn);
            this.expert.Controls.Add(this.customSetupInfo);
            this.expert.Controls.Add(this.customSetupTitle);
            this.expert.Controls.Add(this.label1);
            this.expert.Controls.Add(this.actionFileExtension);
            this.expert.Controls.Add(this.actionFolderPath);
            this.expert.Controls.Add(this.actionFileExtensionLabel);
            this.expert.Controls.Add(this.actionFolderPathLabel);
            this.expert.Location = new System.Drawing.Point(4, 25);
            this.expert.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.expert.Name = "expert";
            this.expert.Size = new System.Drawing.Size(1047, 551);
            this.expert.TabIndex = 2;
            this.expert.Text = "Expert chosen";
            // 
            // backToSetupGuide
            // 
            this.backToSetupGuide.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.backToSetupGuide.Location = new System.Drawing.Point(928, 5);
            this.backToSetupGuide.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.backToSetupGuide.Name = "backToSetupGuide";
            this.backToSetupGuide.Size = new System.Drawing.Size(112, 52);
            this.backToSetupGuide.TabIndex = 14;
            this.backToSetupGuide.Text = "Back to setup guide";
            this.backToSetupGuide.UseVisualStyleBackColor = true;
            this.backToSetupGuide.Click += new System.EventHandler(this.backToSetupGuide_Click);
            // 
            // disclaimerLabel
            // 
            this.disclaimerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.disclaimerLabel.Location = new System.Drawing.Point(27, 446);
            this.disclaimerLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.disclaimerLabel.Name = "disclaimerLabel";
            this.disclaimerLabel.Size = new System.Drawing.Size(991, 26);
            this.disclaimerLabel.TabIndex = 13;
            this.disclaimerLabel.Text = "disclaimer";
            this.disclaimerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // expertDoneButton
            // 
            this.expertDoneButton.BackColor = System.Drawing.Color.LimeGreen;
            this.expertDoneButton.FlatAppearance.BorderSize = 0;
            this.expertDoneButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expertDoneButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.expertDoneButton.Location = new System.Drawing.Point(24, 475);
            this.expertDoneButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.expertDoneButton.Name = "expertDoneButton";
            this.expertDoneButton.Size = new System.Drawing.Size(993, 50);
            this.expertDoneButton.TabIndex = 12;
            this.expertDoneButton.Text = "Done";
            this.expertDoneButton.UseVisualStyleBackColor = false;
            this.expertDoneButton.Click += new System.EventHandler(this.expertDoneButton_Click);
            // 
            // pickFolderBtn
            // 
            this.pickFolderBtn.Location = new System.Drawing.Point(427, 336);
            this.pickFolderBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pickFolderBtn.Name = "pickFolderBtn";
            this.pickFolderBtn.Size = new System.Drawing.Size(33, 31);
            this.pickFolderBtn.TabIndex = 8;
            this.pickFolderBtn.Text = "...";
            this.pickFolderBtn.UseVisualStyleBackColor = true;
            this.pickFolderBtn.Click += new System.EventHandler(this.pickFolderBtn_Click);
            // 
            // customSetupInfo
            // 
            this.customSetupInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customSetupInfo.Location = new System.Drawing.Point(20, 57);
            this.customSetupInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.customSetupInfo.Name = "customSetupInfo";
            this.customSetupInfo.Size = new System.Drawing.Size(739, 230);
            this.customSetupInfo.TabIndex = 11;
            this.customSetupInfo.Text = "description";
            // 
            // customSetupTitle
            // 
            this.customSetupTitle.AutoSize = true;
            this.customSetupTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customSetupTitle.Location = new System.Drawing.Point(16, 27);
            this.customSetupTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.customSetupTitle.Name = "customSetupTitle";
            this.customSetupTitle.Size = new System.Drawing.Size(160, 29);
            this.customSetupTitle.TabIndex = 10;
            this.customSetupTitle.Text = "Custom setup";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(21, 405);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 29);
            this.label1.TabIndex = 9;
            this.label1.Text = ".";
            // 
            // actionFileExtension
            // 
            this.actionFileExtension.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actionFileExtension.Location = new System.Drawing.Point(41, 409);
            this.actionFileExtension.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.actionFileExtension.Name = "actionFileExtension";
            this.actionFileExtension.Size = new System.Drawing.Size(96, 26);
            this.actionFileExtension.TabIndex = 7;
            this.actionFileExtension.Text = "txt";
            // 
            // actionFolderPath
            // 
            this.actionFolderPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actionFolderPath.Location = new System.Drawing.Point(24, 337);
            this.actionFolderPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.actionFolderPath.Name = "actionFolderPath";
            this.actionFolderPath.Size = new System.Drawing.Size(401, 26);
            this.actionFolderPath.TabIndex = 5;
            // 
            // actionFileExtensionLabel
            // 
            this.actionFileExtensionLabel.AutoSize = true;
            this.actionFileExtensionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actionFileExtensionLabel.Location = new System.Drawing.Point(20, 384);
            this.actionFileExtensionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.actionFileExtensionLabel.Name = "actionFileExtensionLabel";
            this.actionFileExtensionLabel.Size = new System.Drawing.Size(165, 20);
            this.actionFileExtensionLabel.TabIndex = 8;
            this.actionFileExtensionLabel.Text = "action_file_extension";
            // 
            // actionFolderPathLabel
            // 
            this.actionFolderPathLabel.AutoSize = true;
            this.actionFolderPathLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actionFolderPathLabel.Location = new System.Drawing.Point(17, 313);
            this.actionFolderPathLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.actionFolderPathLabel.Name = "actionFolderPathLabel";
            this.actionFolderPathLabel.Size = new System.Drawing.Size(146, 20);
            this.actionFolderPathLabel.TabIndex = 6;
            this.actionFolderPathLabel.Text = "action_folder_path";
            // 
            // setupSelect
            // 
            this.setupSelect.BackColor = System.Drawing.Color.White;
            this.setupSelect.Controls.Add(this.GettingStartedWebBrowser);
            this.setupSelect.Location = new System.Drawing.Point(4, 25);
            this.setupSelect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.setupSelect.Name = "setupSelect";
            this.setupSelect.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.setupSelect.Size = new System.Drawing.Size(1047, 551);
            this.setupSelect.TabIndex = 0;
            this.setupSelect.Text = "Select setup";
            // 
            // GettingStartedWebBrowser
            // 
            this.GettingStartedWebBrowser.AllowNavigation = false;
            this.GettingStartedWebBrowser.AllowWebBrowserDrop = false;
            this.GettingStartedWebBrowser.IsWebBrowserContextMenuEnabled = false;
            this.GettingStartedWebBrowser.Location = new System.Drawing.Point(-1, 1);
            this.GettingStartedWebBrowser.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.GettingStartedWebBrowser.MinimumSize = new System.Drawing.Size(27, 25);
            this.GettingStartedWebBrowser.Name = "GettingStartedWebBrowser";
            this.GettingStartedWebBrowser.ScrollBarsEnabled = false;
            this.GettingStartedWebBrowser.Size = new System.Drawing.Size(1045, 544);
            this.GettingStartedWebBrowser.TabIndex = 5;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.ieTest);
            this.tabControl.Controls.Add(this.setupSelect);
            this.tabControl.Controls.Add(this.expert);
            this.tabControl.Controls.Add(this.Done);
            this.tabControl.Location = new System.Drawing.Point(16, 15);
            this.tabControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1055, 580);
            this.tabControl.TabIndex = 6;
            // 
            // ieTest
            // 
            this.ieTest.BackColor = System.Drawing.Color.White;
            this.ieTest.Controls.Add(this.ieWebBrowser);
            this.ieTest.Location = new System.Drawing.Point(4, 25);
            this.ieTest.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ieTest.Name = "ieTest";
            this.ieTest.Size = new System.Drawing.Size(1047, 551);
            this.ieTest.TabIndex = 4;
            this.ieTest.Text = "ieTest";
            // 
            // ieWebBrowser
            // 
            this.ieWebBrowser.AllowNavigation = false;
            this.ieWebBrowser.AllowWebBrowserDrop = false;
            this.ieWebBrowser.IsWebBrowserContextMenuEnabled = false;
            this.ieWebBrowser.Location = new System.Drawing.Point(-1, 0);
            this.ieWebBrowser.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ieWebBrowser.MinimumSize = new System.Drawing.Size(27, 25);
            this.ieWebBrowser.Name = "ieWebBrowser";
            this.ieWebBrowser.Size = new System.Drawing.Size(1045, 544);
            this.ieWebBrowser.TabIndex = 7;
            // 
            // Done
            // 
            this.Done.BackColor = System.Drawing.Color.White;
            this.Done.Controls.Add(this.doneActionViewBrowser);
            this.Done.Location = new System.Drawing.Point(4, 25);
            this.Done.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Done.Name = "Done";
            this.Done.Size = new System.Drawing.Size(1047, 551);
            this.Done.TabIndex = 3;
            this.Done.Text = "Done!";
            // 
            // doneActionViewBrowser
            // 
            this.doneActionViewBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.doneActionViewBrowser.Location = new System.Drawing.Point(0, 0);
            this.doneActionViewBrowser.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.doneActionViewBrowser.MinimumSize = new System.Drawing.Size(27, 25);
            this.doneActionViewBrowser.Name = "doneActionViewBrowser";
            this.doneActionViewBrowser.Size = new System.Drawing.Size(1047, 551);
            this.doneActionViewBrowser.TabIndex = 0;
            this.doneActionViewBrowser.Url = new System.Uri("", System.UriKind.Relative);
            this.doneActionViewBrowser.WebBrowserShortcutsEnabled = false;
            // 
            // GettingStarted
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1087, 609);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "GettingStarted";
            this.Text = "Getting Started | AssistantComputerControl setup";
            this.expert.ResumeLayout(false);
            this.expert.PerformLayout();
            this.setupSelect.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.ieTest.ResumeLayout(false);
            this.Done.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip tooltip;
        private System.Windows.Forms.FolderBrowserDialog pickFolder;
        private System.Windows.Forms.TabPage expert;
        private System.Windows.Forms.Label disclaimerLabel;
        private System.Windows.Forms.Button expertDoneButton;
        private System.Windows.Forms.Button pickFolderBtn;
        private System.Windows.Forms.Label customSetupInfo;
        private System.Windows.Forms.Label customSetupTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox actionFileExtension;
        private System.Windows.Forms.TextBox actionFolderPath;
        private System.Windows.Forms.Label actionFileExtensionLabel;
        private System.Windows.Forms.Label actionFolderPathLabel;
        private System.Windows.Forms.TabPage setupSelect;
        private System.Windows.Forms.WebBrowser GettingStartedWebBrowser;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage ieTest;
        private System.Windows.Forms.WebBrowser ieWebBrowser;
        private System.Windows.Forms.Button backToSetupGuide;
        private System.Windows.Forms.TabPage Done;
        private System.Windows.Forms.WebBrowser doneActionViewBrowser;
    }
}