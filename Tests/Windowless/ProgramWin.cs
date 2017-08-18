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
using System.Linq;
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
 
                    if (currentP.MainModule.FileName == currentP.MainModule.ModuleName)
                    {

                        Console.WriteLine("------native mkbundle mode--------------");
                        e.CommandLine.Program = currentP.MainModule.FileName;

                    }
                    else
                    {

                        Console.WriteLine("------mono runtime mode--------------");
                        
                        e.CommandLine.Program = ProcessBundleClientHelper();
              
                    }
                }
            }

            Console.WriteLine("child cmdline:" + e.CommandLine.CommandLineString);
            Console.WriteLine("program: " + e.CommandLine.Program);
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

        static void Application_Idle(object sender, EventArgs e) {
            CfxRuntime.DoMessageLoopWork();
            Thread.Yield();
        }





        static string ProcessBundleClientHelper()
        {
            var programPath = new Uri(Assembly.GetEntryAssembly().CodeBase).LocalPath;

            var progFi = new FileInfo(programPath);
            var progName = progFi.Name + "_helper";
            var helperPath = progFi.Directory.FullName;
            var helperFilePath = Path.Combine(helperPath, progName);

            if (helperInitialized)
            {
                return helperFilePath;
            }

            var mkbundleCmd = Which("mkbundle");

            //0:mkbundle command
            //1: mono_lib_path
            //2: output for client helper path and name
            //3: origin app name
            var mkbundleStr = "{0} --deps -L {1} -o {2} {3}";

            var mdCmd = string.Format(mkbundleStr, mkbundleCmd, Mono_Lib_Path, helperFilePath, programPath);

            //	var chExecCmd = "chmod +x " + helperFilePath;

            RunCmd(mdCmd);
            //RunCmd (chExecCmd);

            helperInitialized = true;
            return helperFilePath;
        }

        static void RunCmd(string cmd)
        {
            var process = new System.Diagnostics.Process();

            process.StartInfo = new System.Diagnostics.ProcessStartInfo("bash");
            process.StartInfo.Arguments = cmd;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();
            process.Dispose();
        }

        const string Mono_Lib_Path = "/usr/lib/mono/4.5";
        const string WhichCommand = "which {0}";

        static string Which(string programName)
        {

            try
            {

                var cmdStr = string.Format(WhichCommand, programName);

                var process = new System.Diagnostics.Process();
                process.StartInfo = new System.Diagnostics.ProcessStartInfo("bash");
                process.StartInfo.Arguments = cmdStr;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                process.WaitForExit();

                var result = process.StandardOutput.ReadToEnd();

                process.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return string.Empty;
        }
    }
}
