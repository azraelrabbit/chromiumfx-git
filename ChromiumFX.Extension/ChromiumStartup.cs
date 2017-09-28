using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chromium;
using Chromium.Event;
using Chromium.Remote;
using Chromium.WebBrowser;
using Chromium.WebBrowser.Event;
using ChromiumFX.Extension;

namespace Chromium.WebBrowser
{
    public static class ChromiumStartup
    {
        internal static List<GCHandle> SchemeHandlerGCHandles = new List<GCHandle>();

        //internal static CfxApp app;
        //internal static CfxBrowserProcessHandler processHandler;

        internal static bool initialized;

        internal static bool enableDevTools;

        internal static int devtoolPort;

        private static Action<OnCSBeforeCfxInitializeEventArgs> onBeforeCfxInitializeCallback;

        //public static Dictionary<int,ChromiumWebBrowser> BrowserDict=new Dictionary<int, ChromiumWebBrowser>();
        

            /// <summary>
            /// 根虚拟路径
            /// </summary>
       public static string VirtualPath { get; set; }
            /// <summary>
            /// 是否开启Master渲染
            /// </summary>
        public static bool EnableMaster { get; set; }

        /// <summary>
        /// Master Header 文件的相对路径
        /// </summary>
        public static string MasterHeaderFile { get; set; }


        /// <summary>
        /// Master Footer 文件的相对路径
        /// </summary>
        public static string MasterFooterFile { get; set; }

        /// <summary>
        /// 渲染Master的子路径名称
        /// </summary>
        public static string SubViewPathName { get; set; }


        private static readonly Dictionary<int, ChromiumWebBrowser> browsers = new Dictionary<int, ChromiumWebBrowser>();
        internal static Dictionary<int, ChromiumWebBrowser> CurrentBrowsers
        {
            get
            {
                return browsers;
            }
        }

        internal static ChromiumWebBrowser GetBrowser(int id)
        {
            ChromiumWebBrowser wb;
            CurrentBrowsers.TryGetValue(id, out wb);
            return wb;
        }


        public static void Initialize(string domain = "airbox.local",bool enableDevtools=false,int remoteDevPort=10808,Action<OnCSBeforeCfxInitializeEventArgs> beforeInitsettingsCallback=null)
        {
            if (initialized)
            {
                return;
            }

            enableDevTools = enableDevtools;
            devtoolPort = remoteDevPort;
            onBeforeCfxInitializeCallback = beforeInitsettingsCallback;
            var cachePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Application.ProductName, "Cache");
            if (!System.IO.Directory.Exists(cachePath))
                System.IO.Directory.CreateDirectory(cachePath);

            Application.ApplicationExit += Application_ApplicationExit;


            //if (CfxRuntime.PlatformArch == CfxPlatformArch.x64)
            //    CfxRuntime.LibCefDirPath = @"cef64/Release64";
            //else
            //    CfxRuntime.LibCefDirPath = @"cef64/Release";


            var basePath = AppDomain.CurrentDomain.BaseDirectory;

            var libCefDirPath = Path.Combine(basePath, "cef");

            if (!Environment.Is64BitProcess)
            {
                libCefDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cef32");
            }

            //var libCefLocalesPath = Path.Combine(libCefDirPath, "locales");
            CfxRuntime.LibCefDirPath = libCefDirPath;
            CfxRuntime.LibCfxDirPath = libCefDirPath;

            ChromiumWebBrowser.OnBeforeCfxInitialize += ChromiumWebBrowser_OnBeforeCfxInitialize;
            ChromiumWebBrowser.OnBeforeCommandLineProcessing += ChromiumWebBrowser_OnBeforeCommandLineProcessing;
            ChromiumWebBrowser.Initialize();

            RegisterLocalScheme();

            RegisterEmbeddedScheme(System.Reflection.Assembly.GetEntryAssembly(), domainName: domain);

            initialized = true;
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            foreach (var handle in SchemeHandlerGCHandles)
            {
                handle.Free();
            }

            CfxRuntime.Shutdown();
        }
 
        private static void ChromiumWebBrowser_OnBeforeCommandLineProcessing(Chromium.Event.CfxOnBeforeCommandLineProcessingEventArgs e)
        {
            Console.WriteLine("ChromiumWebBrowser_OnBeforeCommandLineProcessing");
            Console.WriteLine(e.CommandLine.CommandLineString);
        }

        private static void ChromiumWebBrowser_OnBeforeCfxInitialize(Chromium.WebBrowser.Event.OnBeforeCfxInitializeEventArgs e)
        {
            var cachePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Application.ProductName, "Cache");
            if (!System.IO.Directory.Exists(cachePath))
                System.IO.Directory.CreateDirectory(cachePath);

            var basePath = AppDomain.CurrentDomain.BaseDirectory;

            var libCefDirPath = Path.Combine(basePath, "cef");

            if (!Environment.Is64BitProcess)
            {
                libCefDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cef32");
            }


            var libCefLocalesPath = Path.Combine(libCefDirPath, "locales");
            //CfxRuntime.LibCefDirPath = libCefDirPath;



            if (enableDevTools)
            {
                e.Settings.RemoteDebuggingPort = devtoolPort;
            }
 
            e.Settings.Locale = "zh-CN";
            e.Settings.CachePath = cachePath;

            e.Settings.LocalesDirPath = libCefLocalesPath;
            e.Settings.ResourcesDirPath = libCefDirPath;
            e.Settings.ResourcesDirPath = libCefDirPath;//System.IO.Path.GetFullPath(@"cef64/Resources");
            //args.Settings.LocalesDirPath = ChromiumStartupSettings.LocalesDir;
            //args.Settings.ResourcesDirPath = ChromiumStartupSettings.ResourcesDir;
            e.Settings.Locale = "zh-CN";
            e.Settings.CachePath = cachePath;
            e.Settings.LogFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "debug.log1");
            e.Settings.LogSeverity = CfxLogSeverity.Error;

            onBeforeCfxInitializeCallback?.Invoke(new OnCSBeforeCfxInitializeEventArgs(e.Settings, e.ProcessHandler));
 
        }
        private static void RegisterLocalScheme(string schemeName = "local")
        {
            if (string.IsNullOrEmpty(schemeName))
            {
                throw new ArgumentNullException("schemeName", "必须为scheme指定名称。");
            }
            var scheme =   new LocalSchemeHandlerFactory();
 
            RegisterScheme(schemeName, null, scheme);
        }

        private static void RegisterEmbeddedScheme(System.Reflection.Assembly assembly, string schemeName = "http", string domainName = null)
        {
            if (string.IsNullOrEmpty(schemeName))
            {
                throw new ArgumentNullException("schemeName", "must set up the schema name。");
            }

            var embedded = new EmbeddedSchemeHandlerFactory(schemeName, domainName, assembly);
            

            RegisterScheme(embedded.SchemeName, domainName, embedded);
        }

        public static void RegisterScheme(string schemeName, string domain, CfxSchemeHandlerFactory factory)
        {
            var gchandle = GCHandle.Alloc(factory);
            
            SchemeHandlerGCHandles.Add(gchandle);

            if (string.IsNullOrEmpty(schemeName))
            {
                throw new ArgumentNullException("schemeName", "must set up the schema name.");
            }
 
            CfxRuntime.RegisterSchemeHandlerFactory(schemeName, domain, factory);
        }
 
    }
 
    public class OnCSBeforeCfxInitializeEventArgs : EventArgs
    {
        public CfxSettings Settings { get; private set; }
        public CfxBrowserProcessHandler ProcessHandler { get; private set; }
        internal OnCSBeforeCfxInitializeEventArgs(CfxSettings settings, CfxBrowserProcessHandler processHandler) 
        {
            Settings = settings;
            ProcessHandler = processHandler;
        }
    }
 
}
