// Copyright (c) 2014-2017 Wolfgang Borgsmüller
// All rights reserved.
// 
// This software may be modified and distributed under the terms
// of the BSD license. See the License.txt file for details.

using System;
using System.IO;
using System.Reflection;
using Chromium;
using Chromium.Event;
using System.Threading;
using System.Windows.Forms;

namespace Chromium.WebBrowser {
	public static class BrowserProcess {

		internal static CfxApp app;
		internal static CfxBrowserProcessHandler processHandler;
		private static bool _mono;
		internal static bool initialized;
		internal static bool helperInitialized;

		internal static void Initialize() {
			_mono = Type.GetType("Mono.Runtime") != null;

			if (initialized)
				throw new ChromiumWebBrowserException("ChromiumWebBrowser library already initialized.");


			int retval = CfxRuntime.ExecuteProcess();
			if(retval >= 0)
				Environment.Exit(retval);


			app = new CfxApp();
			processHandler = new CfxBrowserProcessHandler();
			processHandler.OnBeforeChildProcessLaunch += ProcessHandler_OnBeforeChildProcessLaunch;


			app.GetBrowserProcessHandler += (s, e) => e.SetReturnValue(processHandler);
			app.OnBeforeCommandLineProcessing += (s, e) => ChromiumWebBrowser.RaiseOnBeforeCommandLineProcessing(e);
			app.OnRegisterCustomSchemes += (s, e) => ChromiumWebBrowser.RaiseOnRegisterCustomSchemes(e);

			var settings = new CfxSettings();
			//FIXED different default settings based on platform

			switch (CfxRuntime.PlatformOS)

			{

			case CfxPlatformOS.Linux:

				settings.MultiThreadedMessageLoop = false;


				//TODO less demanding way of using DoMessageLoopWork, ExernalMessageLoop = true doesn't seem to work

				Application.Idle += BrowserMessageLoopStep;

				break;

			default:

				settings.MultiThreadedMessageLoop = true;

				break;

			}

			settings.NoSandbox = true;


			ChromiumWebBrowser.RaiseOnBeforeCfxInitialize(settings, processHandler);

			ChromiumWebBrowser.WindowLess = settings.WindowlessRenderingEnabled;

			if(!CfxRuntime.Initialize(settings, app, RenderProcess.RenderProcessMain))
				throw new ChromiumWebBrowserException("Failed to initialize CEF library.");

			initialized = true;
		}

		private static void ProcessHandler_OnBeforeChildProcessLaunch(object sender, CfxOnBeforeChildProcessLaunchEventArgs e)
		{
			//to fix that the  mono run in linux
			//Console.WriteLine("child cmdline:"+e.CommandLine.CommandLineString);
//			Console.WriteLine("child cmdline:" + e.CommandLine.CommandLineString);
//			Console.WriteLine("program: "+e.CommandLine.Program);

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
					 
//				    var monoPath = "mono";
//					e.CommandLine.Program=programPath;
//				 
//					e.CommandLine.PrependWrapper("--llvm");
//					e.CommandLine.Program=monoPath;
			 
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

			//var chExecCmd = "chmod +x " + helperFilePath;

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

		private static void BrowserMessageLoopStep(object sender, EventArgs e)
		{
			CfxRuntime.DoMessageLoopWork();
			Thread.Yield();
		}

	}
}
