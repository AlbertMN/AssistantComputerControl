using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssistantComputerControl {
    public partial class UpdateProgress : Form {
        public UpdateProgress() {
            InitializeComponent();

            FormClosed += delegate { MainProgram.updateProgressWindow = null; };
            Shown += delegate { Thread.CurrentThread.Priority = ThreadPriority.Highest; };

            downloadingTitle.Text = Translator.__("title", "update_downloading");
            description.Text = Translator.__("description", "update_downloading");
        }

        public void SetProgress(DownloadProgressChangedEventArgs e) {
            this.BeginInvoke((MethodInvoker)delegate {
                double bytesIn = double.Parse(e.BytesReceived.ToString());
                double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
                double percentage = bytesIn / totalBytes * 100;
                progressText.Text = int.Parse(Math.Truncate(percentage).ToString()) + "%";

                var transString = Translator.__("downloaded_bytes", "update_downloading");
                transString = transString.Replace("{x}", e.BytesReceived.ToString());
                transString = transString.Replace("{y}", e.TotalBytesToReceive.ToString());

                //byteText.Text = "Downloaded " + e.BytesReceived + " of " + e.TotalBytesToReceive + " bytes";
                byteText.Text = transString;

                progressBar.Value = int.Parse(Math.Truncate(percentage).ToString());
            });
        }
    }
}
