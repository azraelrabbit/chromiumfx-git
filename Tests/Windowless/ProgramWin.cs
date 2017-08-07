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
using System.Reflection;
using System.Security;
using System.Threading;

namespace Windowless {
    static class ProgramWin {

        static string logPath;
        private static string cachePath;
        private static string userPath;
        private static string exePath;

        internal static CfxBrowserProcessHandler processHandler;
        private static bool _mono;
        internal static bool helperInitialized;
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
            app.GetBrowserProcessHandler += App_GetBrowserProcessHandler;
            if (!CfxRuntime.Initialize(settings, app))
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

            CfxRuntime.Shutdown();
          
        }
        static void App_GetBrowserProcessHandler(object sender, Chromium.Event.CfxGetBrowserProcessHandlerEventArgs e)
        {
            processHandler = new CfxBrowserProcessHandler();
            processHandler.OnBeforeChildProcessLaunch += ProcessHandler_OnBeforeChildProcessLaunch;
            e.SetReturnValue(processHandler);
        }

        static void ProcessHandler_OnBeforeChildProcessLaunch(object sender, Chromium.Event.CfxOnBeforeChildProcessLaunchEventArgs e)
        {
            var programPath = new Uri(Assembly.GetEntryAssembly().CodeBase).LocalPath;

            if (!_mono)
            {
              //  SetJobObjectAsKillOnJobClose
              
            }
            //			Console.WriteLine (programPath);

            if (e.CommandLine.GetSwitchValue("type") == "gpu-process")
            {
                e.CommandLine.Program = programPath;
            }
            else
            {
                if (_mono)
                {
                    //var monoExeName = Assembly.GetEntryAssembly ().GetName ().Name+".exe";
                    var currentP = System.Diagnostics.Process.GetCurrentProcess();


                    //					Console.WriteLine ("Main CMDLINE : -- "+currentP.MainModule.FileName+"|| modulename: "+currentP.MainModule.ModuleName);
                    //					Console.WriteLine ("---------------------------------------------");

                    //System.Diagnostics.Process.GetCurrentProcess().StartInfo.FileName
                    if (currentP.MainModule.FileName == currentP.MainModule.ModuleName)
                    {

                        Console.WriteLine("------native mkbundle mode--------------");
                        e.CommandLine.Program = currentP.MainModule.FileName;

                    }
                    else
                    {

                        Console.WriteLine("------mono runtime mode--------------");
                        //e.CommandLine.Program = Path.Combine (new System.IO.FileInfo (programPath).Directory.FullName, "cef", "Release64", "cefclient");
                        e.CommandLine.Program = ProcessBundleClientHelper();
                        //
                        //					//e.CommandLine.AppendSwitch ("multi-threaded-message-loop");
                        ////					e.CommandLine.AppendSwitch ("off-screen-rendering-enabled");
                        //					e.CommandLine.AppendSwitch("renderer-cmd-prefix");


                        //e.CommandLine.AppendSwitch("disable-text-input-focus-manager");
                        //e.CommandLine.AppendSwitch("no-zygote");
                        //e.CommandLine.AppendSwitchWithValue("type","utility");
                        //e.CommandLine.AppendSwitch("use-views");

                        //--no-zygote
                    }
                }
            }

            Console.WriteLine("child cmdline:" + e.CommandLine.CommandLineString);
            Console.WriteLine("program: " + e.CommandLine.Program);
        }
        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            CfxRuntime.QuitMessageLoop();
            CfxRuntime.Shutdown();
        }

        static void Application_Idle(object sender, EventArgs e) {
            CfxRuntime.DoMessageLoopWork();
            Thread.Yield();
        }
    }
}
