using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace AssistantComputerControl {
    public partial class TestActionWindow : Form {
        private enum Images {
            loading,
            success,
            error
        }

        private bool browserLoaded = false;
        public static WebBrowser sWebBrowser;

        public TestActionWindow() {
            InitializeComponent();
            MaximizeBox = false;

            sWebBrowser = webBrowser;
            string listeningInText = "\n\nListening in: " + MainProgram.CheckPath() + "\nfor \"." + Properties.Settings.Default.ActionFileExtension + "\" extensions";
            actionTesterLabel.Text += listeningInText;

            //webBrowser.Url = new Uri(String.Format("file:///{0}/test.html", MainProgram.currentLocation));
            sWebBrowser.Url = new Uri("http://acc.albe.pw/success_error_listen.html");

            sWebBrowser.DocumentCompleted += delegate {
                browserLoaded = true;
                webBrowser.Document.InvokeScript("showLoader");
            };

            VisibleChanged += VisibilityChanged;
        }

        public void ActionExecuted(string successMessage, string errorMessage, string action, string parameters, string fullContent) {
            if (MainProgram.gettingStarted == null) {
                if (action is null) action = "<i>no action</i>";
                if (parameters is null) parameters = "<i>no parameters</i>";
                if (action is null) fullContent = "<i>didn't read</i>";

                if (successMessage != "") {
                    //Successful
                    string description = "<b>Success message:</b> " + successMessage + "<br><b>Parameters:</b> " + parameters + "<br><b>Full file content:</b> " + fullContent;
                    SetImage(Images.success, "Success; " + action, description);
                } else if (errorMessage != "") {
                    //Error
                    string description = "<b>Error message:</b> " + successMessage + "<br><b>Parameters:</b> " + parameters + "<br><b>Full file content:</b> " + fullContent;
                    SetImage(Images.error, "Error; " + action, description);
                }
            } else {
                //Is testing in "getting started" (less advanced)
                Object[] objArray = new Object[2];

                if (successMessage != "") {
                    objArray[0] = "success";
                    objArray[1] = "Success! \"" + action + "\" simulated";
                } else if (errorMessage != "") {
                    objArray[0] = "error";
                    objArray[1] = "Error doing \"" + action + "\" action";
                }
                MainProgram.gettingStarted.SendActionThrough(objArray);
            }
        }

        private void SetImage(Images img, string heading = "", string description = "") {
            Object[] objArray = new Object[2];
            objArray[0] = heading;
            objArray[1] = description;

            if (browserLoaded) {
                string function;
                switch (img) {
                    case Images.loading:
                        function = "showLoader";
                        break;
                    case Images.success:
                        function = "showSuccess";
                        break;
                    case Images.error:
                        function = "showError";
                        break;
                    default:
                        return;
                }
                MainProgram.DoDebug("Executing function; " + function);

                if (!this.IsHandleCreated) {
                    this.CreateHandle();
                }
                webBrowser.Invoke(new Action(() => {
                    sWebBrowser.Document.InvokeScript(function, objArray);
                }));
            }
        }

        private void VisibilityChanged(object sender, EventArgs e) {
            MainProgram.testingAction = Visible;
        }
    }
}
