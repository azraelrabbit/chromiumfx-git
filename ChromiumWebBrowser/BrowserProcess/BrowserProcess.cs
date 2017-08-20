// Copyright (c) 2014-2017 Wolfgang Borgsmüller
// All rights reserved.
// 
// This software may be modified and distributed under the terms
// of the BSD license. See the License.txt file for details.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
			app.OnBeforeCommandLineProcessing += (s, e) => ChromiumWebBrowserBase.RaiseOnBeforeCommandLineProcessing(e);
			app.OnRegisterCustomSchemes += (s, e) => ChromiumWebBrowserBase.RaiseOnRegisterCustomSchemes(e);

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
			    Application.ApplicationExit += Application_ApplicationExit;
                    break;

			}

			settings.NoSandbox = true;


			ChromiumWebBrowserBase.RaiseOnBeforeCfxInitialize(settings, processHandler);

		    ChromiumWebBrowserBase.WindowLess = settings.WindowlessRenderingEnabled;

			if(!CfxRuntime.Initialize(settings, app, RenderProcess.RenderProcessMain))
				throw new ChromiumWebBrowserException("Failed to initialize CEF library.");

			initialized = true;
		}
	    private static void Application_ApplicationExit(object sender, EventArgs e)
	    {

	        CfxRuntime.QuitMessageLoop();


	        //to kill subprocess in windows . because on windows when application exited,will left a subprocess can not auto shutdown.

	        var current = Process.GetCurrentProcess();

	        var processName = current.ProcessName;

	        var sublist = Process.GetProcessesByName(processName);

	        var realSublist = sublist.Where(p => p.Id != current.Id).ToList();

	        foreach (var process in realSublist)
	        {
	            process.Kill();
	        }

	        CfxRuntime.Shutdown();
	    }
        private static void ProcessHandler_OnBeforeChildProcessLaunch(object sender, CfxOnBeforeChildProcessLaunchEventArgs e)
		{
			//to fix that the  mono run in linux
 
			var programPath = new Uri(Assembly.GetEntryAssembly().CodeBase).LocalPath;
 
			if (e.CommandLine.GetSwitchValue("type") == "gpu-process")
			{
				e.CommandLine.Program= programPath;
			}
			else
			{
				if (_mono)
				{
					var currentP = System.Diagnostics.Process.GetCurrentProcess ();
 
					if (currentP.MainModule.FileName==currentP.MainModule.ModuleName) {

						Console.WriteLine ("------native mkbundle mode--------------");
						e.CommandLine.Program = currentP.MainModule.FileName;

					} else {

						Console.WriteLine ("------mono runtime mode--------------");
						e.CommandLine.Program=ProcessBundleClientHelper();
						 
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
