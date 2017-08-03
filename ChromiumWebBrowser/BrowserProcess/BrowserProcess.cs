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
			 

				
					//e.CommandLine.Program = Path.Combine (new System.IO.FileInfo (programPath).Directory.FullName, "cef", "Release64", "cefclient");
					e.CommandLine.Program=programPath;

					//e.CommandLine.AppendSwitch ("multi-threaded-message-loop");
//					e.CommandLine.AppendSwitch ("off-screen-rendering-enabled");
					e.CommandLine.AppendSwitch("renderer-cmd-prefix");

					e.CommandLine.AppendSwitch("disable-text-input-focus-manager");
					e.CommandLine.AppendSwitch("no-zygote");
					e.CommandLine.AppendSwitch("use-views");

					//--no-zygote

				}
			}

			Console.WriteLine("child cmdline:" + e.CommandLine.CommandLineString);
			Console.WriteLine("program: "+e.CommandLine.Program);
		}

		private static void BrowserMessageLoopStep(object sender, EventArgs e)
		{
			CfxRuntime.DoMessageLoopWork();
			Thread.Yield();
		}

	}
}
