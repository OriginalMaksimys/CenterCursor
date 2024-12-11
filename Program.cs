using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace CenterCursor
{
    public class Program : Form
    {
        private NotifyIcon trayIcon;
        private System.Windows.Forms.Timer timer;

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        public Program()
        {
            // Create tray icon
            trayIcon = new NotifyIcon()
            {
                Icon = SystemIcons.Application,
                Text = "Cursor Centerer",
                Visible = true
            };

            // Create context menu
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Exit", null, Exit_Click);
            trayIcon.ContextMenuStrip = contextMenu;

            // Create timer
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 100; // After so many seconds the cursor will be in the center (in seconds)
            timer.Tick += Timer_Tick;
            timer.Start();

            // Hide form
            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Load += Program_Load;
        }

        private void Program_Load(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Get the screen dimensions
            Rectangle primaryScreen = Screen.PrimaryScreen.Bounds;
            
            // Calculate center position
            int centerX = primaryScreen.Width / 2;
            int centerY = primaryScreen.Height / 2;

            // Set cursor position
            SetCursorPos(centerX, centerY);
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Program());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                trayIcon?.Dispose();
                timer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
