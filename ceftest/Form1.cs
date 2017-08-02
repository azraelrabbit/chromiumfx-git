using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chromium;
using Chromium.WebBrowser;

namespace ceftest
{
    public partial class Form1 : Form
    {
        private Chromium.WebBrowser.ChromiumWebBrowser webBrowser;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs es)
        {
            //ChromiumWebBrowser.RemoteProcessCreated += (e) => {
            //    LogWriteLine("Remote render process created with process id {0}", CfxRemoteCallContext.CurrentContext.ProcessId, CfxRemoteCallContext.CurrentContext.ThreadId);
            //    e.RenderProcessHandler.OnRenderThreadCreated += (s, e1) => {
            //        LogWriteLine("RenderProcessHandler.OnRenderThreadCreated, process id = {0}", CfxRemoteCallContext.CurrentContext.ProcessId);
            //    };
            //    e.RenderProcessHandler.OnBrowserDestroyed += (s, e1) => {
            //        // this is never reached. 
            //        LogWriteLine("RenderProcessHandler.OnBrowserDestroyed, process id = {0}, browser id = {1}", CfxRemoteCallContext.CurrentContext.ProcessId, e1.Browser.Identifier);
            //    };
            //    e.RenderProcessHandler.OnBrowserCreated += (s, e1) => {
            //        LogWriteLine("RenderProcessHandler.OnBrowserCreated, process id = {0}, browser id = {1}", CfxRemoteCallContext.CurrentContext.ProcessId, e1.Browser.Identifier);
            //    };
            //};

            webBrowser =new ChromiumWebBrowser();
           webBrowser.Dock=DockStyle.Fill;



            //webBrowser.DisplayHandler.OnConsoleMessage += (s, e) => LogCallback(s, e);
            //webBrowser.DisplayHandler.OnTitleChange += (s, e) => LogCallback(s, e);
            //webBrowser.DisplayHandler.OnStatusMessage += (s, e) => LogCallback(s, e);

            //webBrowser.DisplayHandler.OnTitleChange += (s, e) => {
            //    var wb = ChromiumWebBrowser.FromCfxBrowser(e.Browser);
            //    LogWriteLine("ChromiumWebBrowser.FromCfxBrowser(e.Browser) == WebBrowser: {0}", wb == webBrowser);
            //    var title = e.Title;
            //    BeginInvoke((MethodInvoker)(() => Text = "ChromiumWebBrowser - " + title));
            //};

            //加上这句,弹出的窗口就会有标题栏,可以关闭,否则无法操作弹出的新窗口,无法关闭...
            webBrowser.LifeSpanHandler.OnBeforePopup += (s, e) =>
            {
                //LogCallback(s, e);
            };

            //webBrowser.LoadHandler.OnLoadingStateChange += (s, e) => {
            //    if (e.IsLoading) return;
            //    //BeginInvoke((MethodInvoker)(() => {
                    
            //    //}));
            //};

            //webBrowser.FindToolbar.Visible = true;

            panel1.Controls.Add(webBrowser);
            panel1.Refresh();
            //CfxRuntime.DoMessageLoopWork();
        }

        void LogCallback(params object[] parameters)
        {

            var callee = new StackFrame(1, false).GetMethod();
            //System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //sb.Append("Callback: ");
            //sb.Append(callee.Name);
            //sb.Append("(");
            var pm = callee.GetParameters();
            //for (var i = 0; i <= pm.Length - 1; i++)
            //{
            //    sb.Append(pm[i].Name);
            //    if (parameters.Length > i)
            //    {
            //        sb.Append(" = {");
            //        if (parameters[i] != null)
            //        {
            //            sb.Append(parameters[i].ToString());
            //        }
            //        else
            //        {
            //            sb.Append("null");
            //        }
            //        sb.Append("}");
            //    }
            //    if (i < pm.Length - 1)
            //    {
            //        sb.Append(", ");
            //    }
            //}
            //sb.Append(")");
            //LogWriteLine(sb.ToString());
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var url = textBox1.Text;

            webBrowser.LoadUrl(url);
            webBrowser.Refresh();
        }

        public void LogWriteLine()
        {
            LogWrite(Environment.NewLine);
        }

        public void LogWriteLine(string msg)
        {
            LogWrite(msg + Environment.NewLine);
        }

        public void LogWriteLine(string msg, params object[] parameters)
        {
            LogWrite(msg + Environment.NewLine, parameters);
        }

        public void LogWrite(string msg, params object[] parameters)
        {
            LogWrite(string.Format(msg, parameters));
        }

        public void LogWrite(string msg)
        {

            if (InvokeRequired)
            {
                Invoke((MethodInvoker)(() => { LogWrite("(*)" + msg); }));
                return;
            }

           Console.WriteLine(msg);

        }
    }
}
