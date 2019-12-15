namespace AssistantComputerControl {
    partial class UpdateProgress {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateProgress));
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.downloadingTitle = new System.Windows.Forms.Label();
            this.byteText = new System.Windows.Forms.Label();
            this.description = new System.Windows.Forms.Label();
            this.progressText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(8, 103);
            this.progressBar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(303, 15);
            this.progressBar.TabIndex = 0;
            // 
            // downloadingTitle
            // 
            this.downloadingTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadingTitle.Location = new System.Drawing.Point(8, 8);
            this.downloadingTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.downloadingTitle.Name = "downloadingTitle";
            this.downloadingTitle.Size = new System.Drawing.Size(303, 26);
            this.downloadingTitle.TabIndex = 1;
            this.downloadingTitle.Text = "Downloading new update...";
            this.downloadingTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // byteText
            // 
            this.byteText.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.byteText.Location = new System.Drawing.Point(8, 84);
            this.byteText.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.byteText.Name = "byteText";
            this.byteText.Size = new System.Drawing.Size(303, 14);
            this.byteText.TabIndex = 2;
            this.byteText.Text = "Downloaded x of x";
            this.byteText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // description
            // 
            this.description.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.description.Location = new System.Drawing.Point(8, 30);
            this.description.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(303, 27);
            this.description.TabIndex = 3;
            this.description.Text = "You can close this window - the installer will open when it\'s done downloading";
            this.description.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressText
            // 
            this.progressText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressText.Location = new System.Drawing.Point(8, 66);
            this.progressText.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.progressText.Name = "progressText";
            this.progressText.Size = new System.Drawing.Size(303, 18);
            this.progressText.TabIndex = 4;
            this.progressText.Text = "0%";
            this.progressText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UpdateProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(319, 127);
            this.Controls.Add(this.progressText);
            this.Controls.Add(this.description);
            this.Controls.Add(this.byteText);
            this.Controls.Add(this.downloadingTitle);
            this.Controls.Add(this.progressBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "UpdateProgress";
            this.Text = "Downloading Update";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label downloadingTitle;
        private System.Windows.Forms.Label byteText;
        private System.Windows.Forms.Label description;
        private System.Windows.Forms.Label progressText;
    }
}