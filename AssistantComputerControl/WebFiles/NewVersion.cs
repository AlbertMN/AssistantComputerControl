using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;

namespace AssistantComputerControl {
    public partial class NewVersion : Form {
        public NewVersion() {
            InitializeComponent();

            string fileName = Path.Combine(MainProgram.currentLocation, "WebFiles/AboutVersion.html");
            if (File.Exists(fileName)) {
                string fileLoc = "file:///" + fileName;
                Uri theUri = new Uri(fileLoc);
                webBrowser.Url = theUri;
            } else {
                webBrowser.Visible = false;
            }
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {

        }
    }
}
