// Copyright (c) 2014-2017 Wolfgang Borgsmüller
// All rights reserved.
// 
// This software may be modified and distributed under the terms
// of the BSD license. See the License.txt file for details.

using System;
using System.Windows.Forms;
using Chromium;
using System.Diagnostics;
using System.IO;
using System.Security;

namespace Windowless {
    static class ProgramWin {

        static string logPath;
        private static string cachePath;
        private static string userPath;
        private static string exePath;
        [STAThread]
        static void Main() {


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



            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var settings = new CfxSettings();
            settings.WindowlessRenderingEnabled = true;
            settings.NoSandbox = true;

			settings.LogFile = logPath;
            //settings.SingleProcess = true;
           // settings.BrowserSubprocessPath = System.IO.Path.Combine(assemblyDir, "Windowless.exe");

            //settings.LogSeverity = CfxLogSeverity.Disable;

            settings.ResourcesDirPath = System.IO.Path.Combine(exePath, "cef", "Resources");
            settings.LocalesDirPath = System.IO.Path.Combine(exePath, "cef", "Resources", "locales");
            //settings.MultiThreadedMessageLoop = true;
            var app = new CfxApp();
            app.OnBeforeCommandLineProcessing += (s, e) => {
                // optimizations following recommendations from issue #84
                //e.CommandLine.AppendSwitch("disable-gpu");
                //e.CommandLine.AppendSwitch("disable-gpu-compositing");
                //e.CommandLine.AppendSwitch("disable-gpu-vsync");
                
            };

            if(!CfxRuntime.Initialize(settings, app))
                Environment.Exit(-1);


            var f = new Form();
            f.Width = 900;
            f.Height = 600;

            var c = new BrowserControl(f);
            c.Dock = DockStyle.Fill;
            //c.Parent = f;

            Application.Idle += Application_Idle;
            Application.ApplicationExit += Application_ApplicationExit;
            Application.Run(f);
            
          
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            CfxRuntime.Shutdown();
        }

        static void Application_Idle(object sender, EventArgs e) {
            CfxRuntime.DoMessageLoopWork();
        }
    }
}
