namespace AssistantComputerControl {
    partial class TestActionWindow {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestActionWindow));
            this.actionTesterLabel = new System.Windows.Forms.Label();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // actionTesterLabel
            // 
            this.actionTesterLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.actionTesterLabel.Location = new System.Drawing.Point(12, 9);
            this.actionTesterLabel.Name = "actionTesterLabel";
            this.actionTesterLabel.Size = new System.Drawing.Size(300, 78);
            this.actionTesterLabel.TabIndex = 1;
            this.actionTesterLabel.Text = "Check if actions work - the actions won\'t be executed.";
            this.actionTesterLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // webBrowser
            // 
            this.webBrowser.AllowNavigation = false;
            this.webBrowser.AllowWebBrowserDrop = false;
            this.webBrowser.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser.Location = new System.Drawing.Point(12, 90);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.ScriptErrorsSuppressed = true;
            this.webBrowser.ScrollBarsEnabled = false;
            this.webBrowser.Size = new System.Drawing.Size(300, 348);
            this.webBrowser.TabIndex = 3;
            this.webBrowser.WebBrowserShortcutsEnabled = false;
            // 
            // TestActionWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(324, 450);
            this.Controls.Add(this.webBrowser);
            this.Controls.Add(this.actionTesterLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TestActionWindow";
            this.Text = "Action Simulator";
            this.ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Label actionTesterLabel;
        private System.Windows.Forms.WebBrowser webBrowser;
    }
}