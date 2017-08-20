using System;
using System.Collections.Generic;
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
        static void Main()
        {
            var assemblyDir = System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);

			Environment.CurrentDirectory = assemblyDir;// System.IO.Path.Combine(assemblyDir, @"..\..\");

            if (CfxRuntime.PlatformArch == CfxPlatformArch.x64)
                CfxRuntime.LibCefDirPath = @"cef64/Release64";
            else
                CfxRuntime.LibCefDirPath = @"cef64/Release";

            if (CfxRuntime.PlatformOS == CfxPlatformOS.Linux)
            {
                ChromiumWebBrowserBase.WindowLess = true;
            }

            ChromiumWebBrowserBase.OnBeforeCfxInitialize += ChromiumWebBrowser_OnBeforeCfxInitialize;
            ChromiumWebBrowserBase.OnBeforeCommandLineProcessing += ChromiumWebBrowser_OnBeforeCommandLineProcessing;
            ChromiumWebBrowserBase.Initialize();

            //Walkthrough01.Main();
            //return;

         

            CfxRuntime.Shutdown();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        static void ChromiumWebBrowser_OnBeforeCommandLineProcessing(CfxOnBeforeCommandLineProcessingEventArgs e)
        {
            Console.WriteLine("ChromiumWebBrowser_OnBeforeCommandLineProcessing");
            Console.WriteLine(e.CommandLine.CommandLineString);
        }

        static void ChromiumWebBrowser_OnBeforeCfxInitialize(OnBeforeCfxInitializeEventArgs e)
        {
            //e.Settings.MultiThreadedMessageLoop = false;
            e.Settings.LogSeverity=CfxLogSeverity.Error;
            e.Settings.LogFile = @"debug.log";
            e.Settings.LocalesDirPath = System.IO.Path.GetFullPath(@"cef64/Resources/locales");
            e.Settings.ResourcesDirPath = System.IO.Path.GetFullPath(@"cef64/Resources");
        }
    }
}
