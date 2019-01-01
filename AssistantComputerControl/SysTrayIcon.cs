using System;
using System.Windows.Forms;
using System.Diagnostics;

//Thanks to Johnny J., codeproject.com for (most of) the code
//https://www.codeproject.com/Tips/627796/Doing-a-NotifyIcon-program-the-right-way

namespace AssistantComputerControl {
    class SysTrayIcon : ApplicationContext {
        //Component declarations
        public NotifyIcon TrayIcon;
        //private ContextMenuStrip TrayIconContextMenu;
        //private ToolStripMenuItem CloseMenuItem;
        private ContextMenu trayMenu = new ContextMenu();

        public SysTrayIcon() {
            InitializeComponent();
            TrayIcon.Visible = true;
        }

        public void AddOpenOnStartupMenu() {
            trayMenu.MenuItems.Add("Open on startup", new EventHandler(TrayCreateStartupLink));
        }
        private void InitializeComponent() {
            TrayIcon = new NotifyIcon();

            //System tray creation
            TrayIcon = new NotifyIcon() {
                Text = MainProgram.appName + " v" + MainProgram.softwareVersion,
                Icon = Properties.Resources.ACC_loading_icon
            };
            
            //Add tray menu items
            trayMenu.MenuItems.Add("Settings", delegate { MainProgram.ShowSettings(); });
            trayMenu.MenuItems.Add("Help", new EventHandler(TrayOpenHelp));
            trayMenu.MenuItems.Add("Exit", new EventHandler(TrayExit));
            TrayIcon.ContextMenu = trayMenu;
        }
        public void HideIcon() {
            TrayIcon.Visible = false;
        }
        private void TrayExit(object sender, EventArgs e) {
            TrayIcon.Visible = false;
            MainProgram.Exit();
        }
        private void TrayOpenHelp(object sender, EventArgs e) {
            Process.Start("https://assistantcomputercontrol.com/#get-in-touch");
        }

        private static void TrayCreateStartupLink(object sender, EventArgs e) {
            DialogResult dialogResult = MessageBox.Show("Do you want this software to automatically open when Windows starts (recommended)? Click \"Yes\"", "Open on startup? | " + MainProgram.messageBoxTitle, MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes) {
                MainProgram.SetStartup(true);
                MessageBox.Show("Good choice! ACC will now start with Windows!", "Wuu! | " + MainProgram.messageBoxTitle + "");
            } else if (dialogResult == DialogResult.No) {
                MessageBox.Show("Alrighty. If you regret and want ACC to open automatically you always right-click on " + MainProgram.messageBoxTitle + " in the tray and click \"Open on startup\"!", "Aww | " + MainProgram.messageBoxTitle);
            }
        }
    }
}