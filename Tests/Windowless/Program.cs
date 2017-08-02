// Copyright (c) 2014-2017 Wolfgang Borgsmüller
// All rights reserved.
// 
// This software may be modified and distributed under the terms
// of the BSD license. See the License.txt file for details.

using System;
using System.Windows.Forms;
using Chromium;
using System.Diagnostics;
using System.Reflection;

namespace Windowless {
    static class Program {
		internal static CfxBrowserProcessHandler processHandler;
		private static bool _mono;
        [STAThread]
		static void Main(string[] args) {

			_mono = Type.GetType("Mono.Runtime") != null;
            var assemblyDir = System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);

            var projectRoot = assemblyDir;
            //while(!System.IO.File.Exists(System.IO.Path.Combine(projectRoot, "Readme.md")))
            //    projectRoot = System.IO.Path.GetDirectoryName(projectRoot);

            CfxRuntime.LibCefDirPath = System.IO.Path.Combine(projectRoot, "cef", "Release64");
            CfxRuntime.LibCfxDirPath = projectRoot;///System.IO.Path.Combine(projectRoot, "Build", "Release");

            var LD_LIBRARY_PATH = Environment.GetEnvironmentVariable("LD_LIBRARY_PATH");
			Console.WriteLine(LD_LIBRARY_PATH);
			if (string.IsNullOrEmpty (LD_LIBRARY_PATH)) {
				Environment.SetEnvironmentVariable ("LD_LIBRARY_PATH", CfxRuntime.LibCefDirPath);
			}
			 LD_LIBRARY_PATH = Environment.GetEnvironmentVariable("LD_LIBRARY_PATH");
			Console.WriteLine (LD_LIBRARY_PATH);
            var exitCode = CfxRuntime.ExecuteProcess(null);
            if(exitCode >= 0) {
                Environment.Exit(exitCode);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var settings = new CfxSettings();
            settings.WindowlessRenderingEnabled = true;
            settings.NoSandbox = true;

           // settings.SingleProcess = true;
			settings.BrowserSubprocessPath =System.IO.Path.Combine(assemblyDir, "windowless");

			settings.LogSeverity = CfxLogSeverity.Error;

            settings.ResourcesDirPath = System.IO.Path.Combine(projectRoot, "cef", "Resources");
            settings.LocalesDirPath = System.IO.Path.Combine(projectRoot, "cef", "Resources", "locales");

            var app = new CfxApp();
            app.OnBeforeCommandLineProcessing += (s, e) => {
                // optimizations following recommendations from issue #84

				if(args!=null && args.Length>0){
					e.CommandLine.AppendArgument(args);
				}
				else{
					//e.CommandLine.AppendSwitch("--type=zygote");
					e.CommandLine.AppendSwitch("disable-gpu");
					e.CommandLine.AppendSwitch("disable-gpu-compositing");
					e.CommandLine.AppendSwitch("disable-gpu-vsync");
				}

                
            };

			app.GetBrowserProcessHandler+= App_GetBrowserProcessHandler;

            if(!CfxRuntime.Initialize(settings, app))
                Environment.Exit(-1);


            var f = new Form();
            f.Width = 900;
            f.Height = 600;

            var c = new BrowserControl(f);
            c.Dock = DockStyle.Fill;
            //c.Parent = f;
			Application.ApplicationExit+=Application_Exit;
            Application.Idle += Application_Idle;
            Application.Run(f);
            
          
        }

        static void App_GetBrowserProcessHandler (object sender, Chromium.Event.CfxGetBrowserProcessHandlerEventArgs e)
        {
			processHandler = new CfxBrowserProcessHandler ();
			processHandler.OnBeforeChildProcessLaunch+= ProcessHandler_OnBeforeChildProcessLaunch;
			e.SetReturnValue (processHandler);
        }

        static void ProcessHandler_OnBeforeChildProcessLaunch (object sender, Chromium.Event.CfxOnBeforeChildProcessLaunchEventArgs e)
        {
			var programPath = new Uri(Assembly.GetEntryAssembly().CodeBase).LocalPath;

			//			Console.WriteLine (programPath);

			if (e.CommandLine.GetSwitchValue("type") == "gpu-process")
			{
				e.CommandLine.Program= programPath;
			}
			else
			{
				if (_mono)
				{
					//var monoExeName = Assembly.GetEntryAssembly ().GetName ().Name+".exe";
					var monoPath = "mono";
					e.CommandLine.Program=programPath;

					//e.CommandLine.PrependWrapper(" ");
					e.CommandLine.PrependWrapper("--llvm");
					e.CommandLine.Program=monoPath;
					//	e.CommandLine.Program="/usr/bin/mono-sgen "+programPath;
				}
			}

			Console.WriteLine("child cmdline:" + e.CommandLine.CommandLineString);
			Console.WriteLine("program: "+e.CommandLine.Program);
        }

        static void Application_Idle(object sender, EventArgs e) {
            CfxRuntime.DoMessageLoopWork();
        }

		static void Application_Exit(object sender,EventArgs e){
			CfxRuntime.Shutdown();
		
		}
    }
}
