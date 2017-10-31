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


        /// <summary>
        /// 是否开启windowless渲染模式,要配合chromiumwebbrowser 和 chromiumwebbrowserwindowless 使用
        /// </summary>
        public static bool RequireWindowLess {
            get { return ChromiumWebBrowserBase.WindowLess; }
            set { ChromiumWebBrowserBase.WindowLess = value; } }


        static string logPath;
        private static string cachePath;
        private static string userPath;

        private static string libCefDirPath;


        private static readonly Dictionary<int, ChromiumWebBrowserBase> browsers = new Dictionary<int, ChromiumWebBrowserBase>();
        internal static Dictionary<int, ChromiumWebBrowserBase> CurrentBrowsers
        {
            get
            {
                return browsers;
            }
        }

        internal static ChromiumWebBrowserBase GetBrowser(int id)
        {
            ChromiumWebBrowserBase wb;
            CurrentBrowsers.TryGetValue(id, out wb);
            return wb;
        }


        public static void Initialize(string domain = "local",bool enableDevtools=false,int remoteDevPort=10808,Action<OnCSBeforeCfxInitializeEventArgs> beforeInitsettingsCallback=null)
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
 
            var assemblyDir = System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            //rootPath = assemblyDir;
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

          

            var basePath = assemblyDir;//AppDomain.CurrentDomain.BaseDirectory;

              libCefDirPath = Path.Combine(basePath, "cef");

            if (!Environment.Is64BitProcess)
            {
                libCefDirPath = Path.Combine(basePath, "cef32");
            }

            ////var libCefLocalesPath = Path.Combine(libCefDirPath, "locales");
            CfxRuntime.LibCefDirPath = libCefDirPath;
            CfxRuntime.LibCfxDirPath = libCefDirPath;

            //if (CfxRuntime.PlatformArch == CfxPlatformArch.x64)
            //    CfxRuntime.LibCefDirPath = Path.Combine(libCefDirPath,"Release64");
            //else
            //    CfxRuntime.LibCefDirPath = Path.Combine(libCefDirPath, "Release");


            ChromiumWebBrowserBase.OnBeforeCfxInitialize += ChromiumWebBrowser_OnBeforeCfxInitialize;
            ChromiumWebBrowserBase.OnBeforeCommandLineProcessing += ChromiumWebBrowser_OnBeforeCommandLineProcessing;
            ChromiumWebBrowserBase.Initialize();

            RegisterLocalScheme();

            RegisterEmbeddedScheme(System.Reflection.Assembly.GetEntryAssembly(), domainName: domain);


            //if (CfxRuntime.PlatformOS != CfxPlatformOS.Windows)
            //{
            //    Application.Idle += Application_Idle;
            //}
            Application.ApplicationExit += Application_ApplicationExit;

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

            var basePath = System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);// AppDomain.CurrentDomain.BaseDirectory;

 
            var resourcePath = Path.Combine(libCefDirPath);

            var libCefLocalesPath = Path.Combine(libCefDirPath,"locales");
 

            if (enableDevTools)
            {
                e.Settings.RemoteDebuggingPort = devtoolPort;
            }
 
            e.Settings.Locale = "zh-CN";
 
            e.Settings.LocalesDirPath = libCefLocalesPath;
            e.Settings.ResourcesDirPath = resourcePath;
 
             
                e.Settings.WindowlessRenderingEnabled = ChromiumWebBrowserBase.WindowLess;
            
 
            e.Settings.CachePath = cachePath;
            e.Settings.LogFile = Path.Combine(basePath, "debug.log1");
            e.Settings.LogSeverity = CfxLogSeverity.Verbose;

            onBeforeCfxInitializeCallback?.Invoke(new OnCSBeforeCfxInitializeEventArgs(e.Settings, e.ProcessHandler));
 
        }

        /// <summary>
        /// Register your filesystem content schema,and the content folder muse be same as your application path.
        /// </summary>
        /// <param name="schemeName">prefix schema name, default: res, usage: local://</param>
        /// <param name="domainName">domain name ,default:local,useage with default schema: local://local</param>
        /// <remarks>usage: local://local/yourcontentfolder/yourviewpath/index.html</remarks>
        public static void RegisterLocalScheme(string schemeName = "local",string domainName="local")
        {
            if (string.IsNullOrEmpty(schemeName))
            {
                throw new ArgumentNullException("schemeName", "必须为scheme指定名称。");
            }
            var scheme =   new LocalSchemeHandlerFactory(schemeName);
 
            RegisterScheme(schemeName, domainName, scheme);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly">Resource Assembly</param>
        /// <param name="schemeName">prefix schema name, default: res, usage: res://</param>
        /// <param name="domainName">domain name ,default:local,useage with default schema: res://local</param>
        public static void RegisterEmbeddedScheme(System.Reflection.Assembly assembly, string schemeName = "res", string domainName = "local")
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
