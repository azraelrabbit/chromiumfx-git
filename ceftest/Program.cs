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
		static string rootPath;
        static string logPath;
        private static string cachePath;
        private static string userPath;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
		static void Main(string[] args)
        {
			Console.WriteLine ("init args:"+string.Join(",",args));
			
            var assemblyDir = System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);
			rootPath = assemblyDir;
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
			//Application.Idle += BrowserMessageLoopStep;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            
            
            
		}
		private static void BrowserMessageLoopStep(object sender, EventArgs e)
		{
			CfxRuntime.DoMessageLoopWork();
			//Thread.Yield();
		}

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {

           // CfxRuntime.QuitMessageLoop();
            CfxRuntime.Shutdown();
        }

        static void ChromiumWebBrowser_OnBeforeCommandLineProcessing(CfxOnBeforeCommandLineProcessingEventArgs e)
        {
            Console.WriteLine("ChromiumWebBrowser_OnBeforeCommandLineProcessing");

            e.CommandLine.AppendArgument("--disable-gpu"); 
//           e.CommandLine.AppendSwitch("disable-gpu-compositing"); 
//           e.CommandLine.AppendSwitch("disable-gpu-vsync");

			if (!e.CommandLine.HasSwitch ("renderer-cmd-prefix")) {
				e.CommandLine.AppendSwitch ("renderer-cmd-prefix");
			}
			//e.CommandLine.AppendSwitchWithValue ("renderer-process-limit", "1");

            Console.WriteLine(e.CommandLine.CommandLineString);

        }

        static void ChromiumWebBrowser_OnBeforeCfxInitialize(OnBeforeCfxInitializeEventArgs e)
        {
           // e.Settings.MultiThreadedMessageLoop = false;
           // e.Settings.ExternalMessagePump = true;//ExternalMessagePump= true;
			var lang=e.Settings.AcceptLanguageList;

			Console.WriteLine ("lang-list"+lang);
			Console.WriteLine ("ua"+e.Settings.UserAgent);
            e.Settings.NoSandbox = true;
            e.Settings.CachePath = cachePath;
            e.Settings.UserDataPath = userPath;
            e.Settings.LogFile = logPath;
			e.Settings.WindowlessRenderingEnabled = false;
			//e.Settings.ExternalMessagePump = true;
			e.Settings.LogSeverity=CfxLogSeverity.Error;
			//e.Settings.AcceptLanguageList = "*";
			e.Settings.IgnoreCertificateErrors = true;
			//e.Settings.SingleProcess = true;
			//e.Settings.EnableNetSecurityExpiration = false;
		//	e.Settings.PersistUserPreferences = false;
			//e.Settings.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36";

//			if (CfxRuntime.PlatformOS == CfxPlatformOS.Linux) {
//				e.Settings.BrowserSubprocessPath = Path.Combine (rootPath, "ceftest.exe");
//			}

		//	e.Settings.MultiThreadedMessageLoop = false;

            e.Settings.LocalesDirPath = System.IO.Path.GetFullPath(@"cef/Resources/locales");
            e.Settings.ResourcesDirPath = System.IO.Path.GetFullPath(@"cef/Resources");
        }
    }
}
