using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chromium.Event;
using Chromium.Remote.Event;
using Chromium.WebBrowser;

namespace CfxTestApplication
{
    public partial class Form1 : Form
    {
		private Chromium.WebBrowser.ChromiumWebBrowser webBrowser;

        protected IntPtr BrowserHandle { get; private set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
              webBrowser=new ChromiumWebBrowser("local://local/www/views/dashboard/Index.html", this);//ChromiumWebBrowserWindowless("local://local/www/views/dashboard/Index.html", this);


            webBrowser.OnCGPress += WebBrowser_KeyPress;
 

            webBrowser.Dock=DockStyle.Fill;
            webBrowser.Visible = true;
           

            var mainWindow = new JSObject();

            mainWindow.AddFunction("doMinWindow").Execute += MinWindow;
            mainWindow.AddFunction("doMaxWindow").Execute += MaxWindow;
            mainWindow.AddFunction("doCloseWindow").Execute += CloseWindow;
 
            webBrowser.GlobalObject.Add("MainWindow", mainWindow);

            
 
            //BrowserHandle = webBrowser.Handle;

            this.Controls.Add(webBrowser);
 
        }

        private void WebBrowser_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 7)
            {
                // ctrl+g - load google so we have a page with text input
                webBrowser.LoadUrl("http://www.baidu.com");
            }
        }

 


        private void MaxWindow(object sender, CfrV8HandlerExecuteEventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
            }
            else
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
        }

        private void MinWindow(object sender, CfrV8HandlerExecuteEventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void CloseWindow(object sender, CfrV8HandlerExecuteEventArgs e)
        {
            this.Close();
        }


        private void LoadHandler_OnLoadEnd(object sender, CfxOnLoadEndEventArgs e)
        {
            webBrowser.ExecuteJavascript("alert('aaaa');");
        }
 
    }
}
