using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

//Thanks to Johnny J., codeproject.com for the code
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

        public void addOpenOnStartupMenu() {
            trayMenu.MenuItems.Add("Open on startup", new EventHandler(MainProgram.trayCreateStartupLink));
        }
        private void InitializeComponent() {
            TrayIcon = new NotifyIcon();

            //System tray creation
            TrayIcon = new NotifyIcon();
            TrayIcon.Text = "AssistantComputerControl";
            TrayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);
            
            //Add tray menu items
            trayMenu.MenuItems.Add("Open folder", new EventHandler(trayOpenFolder));
            trayMenu.MenuItems.Add("Help", new EventHandler(trayOpenHelp));
            trayMenu.MenuItems.Add("Exit", new EventHandler(trayExit));
            TrayIcon.ContextMenu = trayMenu;
            TrayIcon.Icon = Properties.Resources.ACC_icon;
        }
        public void hideIcon() {
            TrayIcon.Visible = false;
        }
        private void trayExit(object sender, EventArgs e) {
            TrayIcon.Visible = false;
            MainProgram.exit();
        }
        private void trayOpenHelp(object sender, EventArgs e) {
            Process.Start("https://github.com/AlbertMN/AssistantComputerControl/wiki");
        }
        private void trayOpenFolder(object sender, EventArgs e) {
            Process.Start(Directory.GetCurrentDirectory());
        }
    }
}
