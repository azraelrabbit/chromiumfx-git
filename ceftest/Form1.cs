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

namespace ceftest
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
            webBrowser=new ChromiumWebBrowser();
 //加上这句,弹出的窗口就会有标题栏,可以关闭,否则无法操作弹出的新窗口,无法关闭...
            webBrowser.LifeSpanHandler.OnBeforePopup += (s, e) =>
            {
                //LogCallback(s, e);
            };
		
           webBrowser.Dock=DockStyle.Fill;
           
            
            panel1.Controls.Add(webBrowser);
            panel1.Refresh();
            //CfxRuntime.DoMessageLoopWork();
			//var callback=new CfxCompletionCallback();
			//callback.OnComplete += callbackComplete;
			//CfxRuntime.BeginTracing ("mytrace",callback);


        }
 
		void callbackComplete(object sender,CfxEventArgs e){

			 
		}

        private void button1_Click_1(object sender, EventArgs e)
        {
            var url = textBox1.Text;

            webBrowser.LoadUrl(url);
          
			webBrowser.Show ();
			webBrowser.BringToFront ();
			this.RaisePaintEvent (webBrowser, new PaintEventArgs (this.CreateGraphics (), webBrowser.ClientRectangle));
        }
    }
}
