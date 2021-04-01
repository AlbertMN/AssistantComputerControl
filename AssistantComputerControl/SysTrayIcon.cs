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
            trayMenu.MenuItems.Add(Translator.__("open_on_startup", "tray_menu"), new EventHandler(TrayCreateStartupLink));
        }
        public void InitializeComponent() {
            TrayIcon = new NotifyIcon();

            //System tray creation
            TrayIcon = new NotifyIcon() {
                Text = MainProgram.appName + " v" + MainProgram.softwareVersion,
                Icon = Properties.Resources.ACC_loading_light_icon
            };
            
            //Add tray menu items
            trayMenu.MenuItems.Add(Translator.__("settings_title", "tray_menu"), delegate { MainProgram.ShowSettings(); });
            trayMenu.MenuItems.Add(Translator.__("help_title", "tray_menu"), new EventHandler(TrayOpenHelp));
            trayMenu.MenuItems.Add(Translator.__("exit_title", "tray_menu"), new EventHandler(TrayExit));
            TrayIcon.ContextMenu = trayMenu;

            TrayIcon.DoubleClick += new System.EventHandler(this.TrayShowSettings);
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
        private void TrayShowSettings(object sender, EventArgs e) {
            MainProgram.ShowSettings();
        }

        private static void TrayCreateStartupLink(object sender, EventArgs e) {
            DialogResult dialogResult = MessageBox.Show(Translator.__("start_with_pc_desc", "general"), MainProgram.messageBoxTitle, MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes) {
                MainProgram.SetStartup(true);
                MessageBox.Show(Translator.__("start_with_pc_yes", "general"), MainProgram.messageBoxTitle + "");
            } else if (dialogResult == DialogResult.No) {
                MessageBox.Show(Translator.__("start_with_pc_no", "general"), MainProgram.messageBoxTitle);
            }
        }
    }
}