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
using System.IO;

namespace Windowless {
    static class Program {
		internal static CfxBrowserProcessHandler processHandler;
		private static bool _mono;
		internal static bool helperInitialized;

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
		//	settings.BrowserSubprocessPath =System.IO.Path.Combine(assemblyDir, "Windowless.exe");

			settings.LogSeverity = CfxLogSeverity.Error;

            settings.ResourcesDirPath = System.IO.Path.Combine(projectRoot, "cef", "Resources");
            settings.LocalesDirPath = System.IO.Path.Combine(projectRoot, "cef", "Resources", "locales");

            var app = new CfxApp();
            app.OnBeforeCommandLineProcessing += (s, e) => {
                // optimizations following recommendations from issue #84

				if(args!=null && args.Length>0){
					foreach(var arg in args){
						e.CommandLine.AppendArgument(arg);
					}

				}
				else{
					//e.CommandLine.AppendSwitch("--type=zygote");
					e.CommandLine.AppendSwitch("disable-gpu");
//					e.CommandLine.AppendSwitch("disable-gpu-compositing");
//					e.CommandLine.AppendSwitch("disable-gpu-vsync");
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
			c.ImeMode = ImeMode.Inherit;
            //c.Parent = f;
			f.KeyPreview=true;
			f.ImeMode = ImeMode.On;
			c.Enabled = true; 
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
					var currentP = System.Diagnostics.Process.GetCurrentProcess ();


					//					Console.WriteLine ("Main CMDLINE : -- "+currentP.MainModule.FileName+"|| modulename: "+currentP.MainModule.ModuleName);
					//					Console.WriteLine ("---------------------------------------------");

					//System.Diagnostics.Process.GetCurrentProcess().StartInfo.FileName
					if (currentP.MainModule.FileName==currentP.MainModule.ModuleName) {

						Console.WriteLine ("------native mkbundle mode--------------");
						e.CommandLine.Program = currentP.MainModule.FileName;

					} else {

						Console.WriteLine ("------mono runtime mode--------------");
						//e.CommandLine.Program = Path.Combine (new System.IO.FileInfo (programPath).Directory.FullName, "cef", "Release64", "cefclient");
						e.CommandLine.Program=ProcessBundleClientHelper();
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
			Console.WriteLine("program: "+e.CommandLine.Program);
        }

		static string ProcessBundleClientHelper(){
			var programPath = new Uri(Assembly.GetEntryAssembly().CodeBase).LocalPath;

			var progFi = new FileInfo (programPath);
			var progName = progFi.Name+"_helper";
			var helperPath = progFi.Directory.FullName;
			var helperFilePath = Path.Combine (helperPath, progName);

			if (helperInitialized) {
				return helperFilePath;
			}

			var mkbundleCmd = Which ("mkbundle");

			//0:mkbundle command
			//1: mono_lib_path
			//2: output for client helper path and name
			//3: origin app name
			var mkbundleStr = "{0} --deps -L {1} -o {2} {3}";

			var mdCmd = string.Format (mkbundleStr, mkbundleCmd, Mono_Lib_Path, helperFilePath, programPath);

		//	var chExecCmd = "chmod +x " + helperFilePath;

			RunCmd (mdCmd);
			//RunCmd (chExecCmd);

			helperInitialized = true;
			return helperFilePath;
		}

		static void RunCmd(string cmd){
			var process = new System.Diagnostics.Process ();

			process.StartInfo = new System.Diagnostics.ProcessStartInfo ("bash");
			process.StartInfo.Arguments=cmd;
			process.StartInfo.UseShellExecute=false;
			process.Start ();
			process.WaitForExit ();
			process.Dispose ();
		}

		const string Mono_Lib_Path = "/usr/lib/mono/4.5";
		const string WhichCommand = "which {0}";

		static string Which(string programName){

			try{

				var cmdStr = string.Format (WhichCommand, programName);

				var process = new System.Diagnostics.Process ();
				process.StartInfo = new System.Diagnostics.ProcessStartInfo ("bash");
				process.StartInfo.Arguments=cmdStr;
				process.StartInfo.RedirectStandardOutput=true;
				process.StartInfo.UseShellExecute=false;
				process.Start ();
				process.WaitForExit ();

				var result = process.StandardOutput.ReadToEnd ();

				process.Dispose ();
				return result;
			}catch(Exception ex){
				Console.WriteLine (ex.Message);
			}

			return string.Empty;
		}

        static void Application_Idle(object sender, EventArgs e) {
            CfxRuntime.DoMessageLoopWork();
        }

		static void Application_Exit(object sender,EventArgs e){
			CfxRuntime.Shutdown();
		
		}
    }
}
