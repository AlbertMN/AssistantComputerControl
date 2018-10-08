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
            this.label1 = new System.Windows.Forms.Label();
            this.byteText = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.progressText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 158);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(454, 23);
            this.progressBar.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(454, 40);
            this.label1.TabIndex = 1;
            this.label1.Text = "Downloading new update...";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // byteText
            // 
            this.byteText.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.byteText.Location = new System.Drawing.Point(12, 130);
            this.byteText.Name = "byteText";
            this.byteText.Size = new System.Drawing.Size(454, 21);
            this.byteText.TabIndex = 2;
            this.byteText.Text = "Downloaded x of x";
            this.byteText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(454, 41);
            this.label2.TabIndex = 3;
            this.label2.Text = "You can close this window - the installer will open when it\'s done downloading";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressText
            // 
            this.progressText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressText.Location = new System.Drawing.Point(12, 101);
            this.progressText.Name = "progressText";
            this.progressText.Size = new System.Drawing.Size(454, 28);
            this.progressText.TabIndex = 4;
            this.progressText.Text = "0%";
            this.progressText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UpdateProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 195);
            this.Controls.Add(this.progressText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.byteText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UpdateProgress";
            this.Text = "Downloading Update";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label byteText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label progressText;
    }
}