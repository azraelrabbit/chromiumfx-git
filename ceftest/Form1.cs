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
			this.AllowTransparency = true;
			//webBrowser.ImeMode = this.ImeMode;
			//panel1.ImeMode=ImeMode.Inherit;
			//webBrowser.ImeMode = ImeMode.Inherit;
			webBrowser=new ChromiumWebBrowser(this);
			webBrowser.KeyboardHandler.OnKeyEvent+= WebBrowser_KeyboardHandler_OnKeyEvent;

			webBrowser.KeyboardHandler.OnPreKeyEvent+= WebBrowser_KeyboardHandler_OnPreKeyEvent;
 //加上这句,弹出的窗口就会有标题栏,可以关闭,否则无法操作弹出的新窗口,无法关闭...
			webBrowser.LifeSpanHandler.OnBeforePopup += beforePop;
		
           webBrowser.Dock=DockStyle.Fill;
           
            
            panel1.Controls.Add(webBrowser);
            panel1.Refresh();
            //CfxRuntime.DoMessageLoopWork();
			//var callback=new CfxCompletionCallback();
			//callback.OnComplete += callbackComplete;
			//CfxRuntime.BeginTracing ("mytrace",callback);

//			this.Controls.Clear ();
//			this.Controls.Add (webBrowser);
        }

        void WebBrowser_KeyboardHandler_OnPreKeyEvent (object sender, Chromium.Event.CfxOnPreKeyEventEventArgs e)
        {
//			// setting to disable inner cfxbrowser keyboard event. this may be could to resolve input methods issue.  not completed.
//			e.IsKeyboardShortcut = true;
//			e.SetReturnValue (false);
 
        }

        void WebBrowser_KeyboardHandler_OnKeyEvent (object sender, Chromium.Event.CfxOnKeyEventEventArgs e)
        {
			Console.WriteLine ((char)e.Event.Character);
			//this.OnKeyPress (new KeyPressEventArgs ((char)e.Event.Character));
			//base.KeyPress.BeginInvoke (this, new KeyPressEventArgs ((char)e.Event.Character), null, null);
        }
 
		static void beforePop(object sender, EventArgs e){
		}

		void callbackComplete(object sender,CfxEventArgs e){

			 
		}
//		protected override void OnPreviewKeyDown (PreviewKeyDownEventArgs e)
//		{
//			base.OnPreviewKeyDown (e);
//			webBrowser.
//		}
//		protected override void OnPreviewTextInput(TextCompositionEventArgs e)
//		{
//			for (int i = 0; i < e.Text.Length; i++)
//			{
//				managedCefBrowserAdapter.SendKeyEvent((int)WM.CHAR, (int)e.Text[i], 0); // or WM.IME_CHAR?
//			}
//			base.OnPreviewTextInput(e); // maybe remove this?
//		}
        private void button1_Click_1(object sender, EventArgs e)
        {
            var url = textBox1.Text;

            webBrowser.LoadUrl(url);
          
		//	webBrowser.Show ();
		//	webBrowser.BringToFront ();
		//	this.RaisePaintEvent (webBrowser, new PaintEventArgs (this.CreateGraphics (), webBrowser.ClientRectangle));
        }
    }
}
