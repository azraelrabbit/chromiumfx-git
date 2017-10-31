using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chromium;
using Chromium.Event;
using Chromium.WebBrowser;
using Chromium.WebBrowser.Event;

namespace CfxTestApplication
{
    static class Program
    {
		static string rootPath;
		static string logPath;
		private static string cachePath;
		private static string userPath;
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{

            // // you can  custome cef lib path 
            //      var basePath =   System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            //      ChromiumStartup.LibCefPath = Path.Combine(basePath,"cef");
            //ChromiumStartup.LibCfxPath = Path.Combine(basePath, "cef");
            //ChromiumStartup.CefResourcePath = Path.Combine(basePath, "cef");// or Path.Combine(basePath, "cef","Resource");

		    //on windows use ChromiumfxWebBrowser set to false
		    ChromiumStartup.RequireWindowLess = false;

		    //on linux use chromiumfxWebBrowserWindowless set to true
		    //ChromiumStartup.RequireWindowLess = true;


            ChromiumStartup.Initialize();

 
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
 

		}
		 
    }
}
