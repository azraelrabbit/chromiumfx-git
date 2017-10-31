using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chromium.Event;
using Chromium.WebBrowser;

namespace CfxTestApplication
{
    public partial class Form1 : Form
    {
        private Chromium.WebBrowser.ChromiumWebBrowser webBrowser;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser = new ChromiumWebBrowser(this);

            webBrowser.Dock = DockStyle.Fill;


            //加上这句,弹出的窗口就会有标题栏,可以关闭,否则无法操作弹出的新窗口,无法关闭...
            webBrowser.LifeSpanHandler.OnBeforePopup += beforePop;

   
            //webBrowser.ExecuteJavascript("alert('aaaa');");

              //webBrowser.LoadHandler.OnLoadEnd += LoadHandler_OnLoadEnd;

            webBrowser.LoadUrl("http://www.baidu.com");

        }

        private void LoadHandler_OnLoadEnd(object sender, CfxOnLoadEndEventArgs e)
        {
            webBrowser.ExecuteJavascript("alert('aaaa');");
        }


        private void beforePop(object sender, CfxOnBeforePopupEventArgs e)
        {

        }


    }
}
