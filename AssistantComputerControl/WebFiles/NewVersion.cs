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
using System.Diagnostics;
using System.Text.RegularExpressions;

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

            webBrowser.DocumentCompleted += BrowserDocumentCompleted;
            webBrowser.Navigating += BrowserNavigating;
            webBrowser.NewWindow += NewBrowserWindow;
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {

        }

        private void NewBrowserWindow(object sender, CancelEventArgs e) {
            e.Cancel = true;
        }

        private void BrowserNavigating(object sender, WebBrowserNavigatingEventArgs e) {
            if (!(e.Url.ToString().Equals("about:blank", StringComparison.InvariantCultureIgnoreCase))) {
                Process.Start(e.Url.ToString());
                e.Cancel = true;
                return;
            }

            e.Cancel = true;
            Process.Start(e.Url.ToString());
        }
        private void BrowserDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {
            string tagUpper = "";

            foreach (HtmlElement tag in (sender as WebBrowser).Document.All) {
                tagUpper = tag.TagName.ToUpper();

                if ((tagUpper == "AREA") || (tagUpper == "A")) {
                    tag.MouseUp += new HtmlElementEventHandler(this.link_MouseUp);
                }
            }
        }
        void link_MouseUp(object sender, HtmlElementEventArgs e) {
            Regex pattern = new Regex("href=\\\"(.+?)\\\"");
            Match match = pattern.Match((sender as HtmlElement).OuterHtml);
            if (match.Groups.Count >= 1) {
                string link = match.Groups[1].Value;

                if (link.Length > 0) {
                    if (link[0] != '#')
                        Process.Start(link);
                }
            }
        }
    }
}
