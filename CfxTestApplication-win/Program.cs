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


            //on windows use ChromiumfxWebBrowser set to false
            ChromiumStartup.RequireWindowLess = false;

            //on linux use chromiumfxWebBrowserWindowless set to true
            //ChromiumStartup.RequireWindowLess = true;

            // you can use your custome cef lib path 
            //var basePath = System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            //ChromiumStartup.LibCefPath = Path.Combine(basePath, "cef");
            //ChromiumStartup.LibCfxPath = Path.Combine(basePath, "cef");
            //ChromiumStartup.CefResourcePath = Path.Combine(basePath, "cef");// or Path.Combine(basePath, "cef","Resource");
 

            ChromiumStartup.Initialize(beforeInitsettings:initSettings,beforeCommandLine:initCommandlineArgs);


            ChromiumStartup.RegisterLocalScheme();

          


            //CfxRuntime.RunMessageLoop();
            //Application.ApplicationExit += Application_ApplicationExit;
            //Application.Idle += BrowserMessageLoopStep;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            //



        }

        private static void initCommandlineArgs(CfxOnBeforeCommandLineProcessingEventArgs e)
        {
            //
            //					//e.CommandLine.AppendSwitch ("multi-threaded-message-loop");
            ////					e.CommandLine.AppendSwitch ("off-screen-rendering-enabled");
            //					e.CommandLine.AppendSwitch("renderer-cmd-prefix");


            //e.CommandLine.AppendSwitch("disable-text-input-focus-manager");
            //e.CommandLine.AppendSwitch("no-zygote");
            //e.CommandLine.AppendSwitchWithValue("type","utility");
            //e.CommandLine.AppendSwitch("use-views");

            e.CommandLine.AppendArgument("--disable-gpu");
        }

        private static void initSettings(OnCSBeforeCfxInitializeEventArgs e)
        {
            
        }
    }
}
