using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace Win7FTP
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            #region Check if OS = Win 7
            if (!Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.IsPlatformSupported)
            {
                MessageBox.Show("FTP 需要 Windows 7 才能正常运行. 程序即将退出.", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            #endregion

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmLogin());
        }


    }
}
