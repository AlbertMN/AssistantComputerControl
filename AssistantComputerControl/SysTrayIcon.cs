using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

//Thanks to Johnny J., codeproject.com for (most of) the code
//https://www.codeproject.com/Tips/627796/Doing-a-NotifyIcon-program-the-right-way

namespace AssistantComputerControl {
    class SysTrayIcon : ApplicationContext {
        //Component declarations
        private NotifyIcon TrayIcon;
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
            TrayIcon = new NotifyIcon();
            TrayIcon.Text = "AssistantComputerControl";
            TrayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);
            
            //Add tray menu items
            trayMenu.MenuItems.Add("Open folder", new EventHandler(TrayOpenFolder));
            trayMenu.MenuItems.Add("Help", new EventHandler(TrayOpenHelp));
            trayMenu.MenuItems.Add("Exit", new EventHandler(TrayExit));
            TrayIcon.ContextMenu = trayMenu;
            TrayIcon.Icon = Properties.Resources.ACC_icon;
        }
        public void HideIcon() {
            TrayIcon.Visible = false;
        }
        private void TrayExit(object sender, EventArgs e) {
            TrayIcon.Visible = false;
            MainProgram.Exit();
        }
        private void TrayOpenHelp(object sender, EventArgs e) {
            Process.Start("https://github.com/AlbertMN/AssistantComputerControl/wiki");
        }
        private void TrayOpenFolder(object sender, EventArgs e) {
            Process.Start(Directory.GetCurrentDirectory());
        }

        private static void TrayCreateStartupLink(object sender, EventArgs e) {
            DialogResult dialogResult = MessageBox.Show("Do you want this software to automatically open when Windows starts (recommended)? Click \"Yes\"", "Open on startup? | " + MainProgram.messageBoxTitle, MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes) {
                MainProgram.CreateStartupLink();
                MessageBox.Show("Good choice! ACC will now start with Windows!", "Wuu! | " + MainProgram.messageBoxTitle + "");
            }
            else if (dialogResult == DialogResult.No)
            {
                MessageBox.Show("Alrighty. If you regret and want ACC to open automatically you always right-click on " + MainProgram.messageBoxTitle + " in the tray and click \"Open on startup\"!", "Aww | " + MainProgram.messageBoxTitle);
            }
        }
    }
}