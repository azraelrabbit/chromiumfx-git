using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chromium;
using Chromium.WebBrowser;
using Windowless;

namespace CfxTestApplication
{
    public partial class Form1 : Form
    {
        private Chromium.WebBrowser.ChromiumWebBrowserBase webBrowser;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (CfxRuntime.PlatformOS == CfxPlatformOS.Windows)
            {
                webBrowser = new ChromiumWebBrowser(this);
            }
            else
            {
                webBrowser=new ChromiumWebBrowserWindowless(this);
            }
           webBrowser.Dock=DockStyle.Fill;
            this.Controls.Add(webBrowser);
            webBrowser.LoadUrl("http://www.baidu.com");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            webBrowser.LoadUrl("http://www.baidu.com");
        }
    }
}
