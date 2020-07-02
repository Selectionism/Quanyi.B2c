using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quanyi.B2c.Yaofangwang
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FormMain());
            IsRunning();
        }

        static void IsRunning()
        {
            bool createdNew;
            //同步基元变量
            System.Threading.Mutex instance = new System.Threading.Mutex(true, "Quanyi.B2c.Yaofangwang", out createdNew);
            if(createdNew)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain());
                instance.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("已经启动了一个程序，请先退出");
                Application.Exit();
            }
        }
    }
}
