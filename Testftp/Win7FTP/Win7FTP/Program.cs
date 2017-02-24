using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace Win7FTP
{
    static class Program
    {
        public static string LoginFrmCmbValue = string.Empty;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        static void Main()
        {
            #region Check if OS = Win 7
            if (!Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.IsPlatformSupported)
            {
                MessageBox.Show("Win7FTP required Windows 7 to run. The Application will now Exit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            #endregion

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmLogin());
        }


    }
}
