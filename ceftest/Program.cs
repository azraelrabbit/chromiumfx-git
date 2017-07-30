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

namespace ceftest
{
    static class Program
    {
        static string logPath;
        private static string cachePath;
        private static string userPath;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            var assemblyDir = System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);

            logPath = Path.Combine(assemblyDir, "debug.log");

            cachePath = Path.Combine(assemblyDir, "cache");
            if (!Directory.Exists(cachePath))
            {
                Directory.CreateDirectory(cachePath);
            }
            userPath= Path.Combine(assemblyDir, "temp");
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

           
            

            Chromium.WebBrowser.ChromiumWebBrowser.OnBeforeCfxInitialize += ChromiumWebBrowser_OnBeforeCfxInitialize;
            ChromiumWebBrowser.OnBeforeCommandLineProcessing += ChromiumWebBrowser_OnBeforeCommandLineProcessing;
            Chromium.WebBrowser.ChromiumWebBrowser.Initialize();


            //Walkthrough01.Main();
            //return;
            //CfxRuntime.RunMessageLoop();


            Application.ApplicationExit += Application_ApplicationExit;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            
            
            
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {

            CfxRuntime.QuitMessageLoop();
            CfxRuntime.Shutdown();
        }

        static void ChromiumWebBrowser_OnBeforeCommandLineProcessing(CfxOnBeforeCommandLineProcessingEventArgs e)
        {
            Console.WriteLine("ChromiumWebBrowser_OnBeforeCommandLineProcessing");
             e.CommandLine.AppendArgument("--disable-gpu");

            //e.CommandLine.AppendSwitchWithValue("renderer-cmd-prefix", "spawnscript");
            //e.CommandLine.AppendSwitchWithValue("utility-cmd-prefix", "spawnscript");
   
            e.CommandLine.AppendSwitch("disable-gpu-compositing");
            e.CommandLine.AppendSwitch("disable-gpu-vsync");
            Console.WriteLine(e.CommandLine.CommandLineString);
        }

        static void ChromiumWebBrowser_OnBeforeCfxInitialize(OnBeforeCfxInitializeEventArgs e)
        {
           // e.Settings.MultiThreadedMessageLoop = false;
           // e.Settings.ExternalMessagePump = true;//ExternalMessagePump= true;
            e.Settings.NoSandbox = true;
            e.Settings.CachePath = cachePath;
            e.Settings.UserDataPath = userPath;
            e.Settings.LogFile = logPath;
            
            e.Settings.LogSeverity=CfxLogSeverity.Verbose;

            e.Settings.LocalesDirPath = System.IO.Path.GetFullPath(@"cef/Resources/locales");
            e.Settings.ResourcesDirPath = System.IO.Path.GetFullPath(@"cef/Resources");
        }
    }
}
