using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chromium;
using Chromium.Event;
using Chromium.WebBrowser;
using Chromium.WebBrowser.Event;

namespace CfxTestApplication
{
    static class Program
    {
 

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            
            ChromiumStartup.VirtualPath = "www";
            ChromiumStartup.EnableMaster = true;
            ChromiumStartup.SubViewPathName = "views";
            ChromiumStartup.MasterHeaderFile = "local://local/www/shared/header.html";
            ChromiumStartup.MasterFooterFile = "local://local/www/shared/footer.html";

            ChromiumStartup.RequireWindowLess = false;

            ChromiumStartup.Initialize();


            ChromiumStartup.RegisterLocalScheme();

          


            //CfxRuntime.RunMessageLoop();
            //Application.ApplicationExit += Application_ApplicationExit;
            //Application.Idle += BrowserMessageLoopStep;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            //



        }

    }
}
