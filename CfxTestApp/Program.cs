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
        static string logPath;
        private static string cachePath;
        private static string userPath;
        private static string exePath;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            var assemblyDir = System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            exePath = assemblyDir;
            logPath = Path.Combine(assemblyDir, "debug.log");

            cachePath = Path.Combine(assemblyDir, "cache");
            if (!Directory.Exists(cachePath))
            {
                Directory.CreateDirectory(cachePath);
            }
            userPath = Path.Combine(assemblyDir, "temp");
            if (!Directory.Exists(userPath))
            {
                Directory.CreateDirectory(userPath);
            }


            Console.WriteLine(assemblyDir);
            Environment.CurrentDirectory = assemblyDir;//System.IO.Path.Combine(assemblyDir, @"..\..\");

            if (CfxRuntime.PlatformArch == CfxPlatformArch.x64)
                CfxRuntime.LibCefDirPath = @"cef/Release64";
            else
                CfxRuntime.LibCefDirPath = @"cef/Release";




            ChromiumWebBrowserBase.OnBeforeCfxInitialize += ChromiumWebBrowser_OnBeforeCfxInitialize;
            ChromiumWebBrowserBase.OnBeforeCommandLineProcessing += ChromiumWebBrowser_OnBeforeCommandLineProcessing;
            ChromiumWebBrowserBase.Initialize();

            //Walkthrough01.Main();
            //return;


            Application.ApplicationExit += Application_ApplicationExit;

                     Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

           
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
          CfxRuntime.Shutdown();
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


            if (CfxRuntime.PlatformOS == CfxPlatformOS.Linux)
            {
                e.Settings.WindowlessRenderingEnabled = true;
            }
            //settings.ResourcesDirPath = System.IO.Path.Combine(exePath, "cef", "Resources");
            //settings.LocalesDirPath = System.IO.Path.Combine(exePath, "cef", "Resources", "locales");
            e.Settings.LocalesDirPath = System.IO.Path.Combine(exePath, "cef", "Resources", "locales");
            e.Settings.ResourcesDirPath = System.IO.Path.Combine(exePath, "cef", "Resources");
        }
    }
}
