// Copyright (c) 2014-2017 Wolfgang Borgsmüller
// All rights reserved.
// 
// This software may be modified and distributed under the terms
// of the BSD license. See the License.txt file for details.

using System;
using System.Drawing;
using System.Windows.Forms;
using Chromium;
using Chromium.WebBrowser;

namespace Windowless
{
    /// <summary>
    /// A minimum and very incomplete implementation of a
    /// control with windowless browser.
    /// </summary>
    public  class ChromiumWebBrowserWindowless : ChromiumWebBrowserBase
    {
         
        

        protected override void Dispose(bool disposing)
        {
            //this.browser.Dispose();
            //this.client.Dispose();


            base.Dispose(disposing);
        }
 

    

        protected override void OnGotFocus(EventArgs e)
        {
            if (Browser != null)
            {
                Browser.Host.SendFocusEvent(true);
            }
        }

        // control overrides

        protected override void OnResize(EventArgs e)
        {
            if (Browser != null)
            {
                Browser.Host.WasResized();
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // do nothing
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            lock (pbLock)
            {
                if (pixelBuffer != null)
                    e.Graphics.DrawImage(pixelBuffer, e.ClipRectangle, e.ClipRectangle, GraphicsUnit.Pixel);
            }
        }

        // mouse events
        // this is not complete

        private readonly CfxMouseEvent mouseEventProxy = new CfxMouseEvent();

        private void SetMouseEvent(MouseEventArgs e)
        {
            mouseEventProxy.X = e.X;
            mouseEventProxy.Y = e.Y;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (Browser != null)
            {
                SetMouseEvent(e);
                Browser.Host.SendMouseMoveEvent(mouseEventProxy, false);
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (Browser != null)
            {
                Browser.Host.SendMouseMoveEvent(mouseEventProxy, true);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (Browser != null)
            {
                SetMouseEvent(e);
                CfxMouseButtonType t;
                switch (e.Button)
                {
                    case System.Windows.Forms.MouseButtons.Right:
                        t = CfxMouseButtonType.Left;
                        break;
                    case System.Windows.Forms.MouseButtons.Middle:
                        t = CfxMouseButtonType.Middle;
                        break;
                    default:
                        t = CfxMouseButtonType.Left;
                        break;
                }
                Browser.Host.SendFocusEvent(true);
                Browser.Host.SendMouseClickEvent(mouseEventProxy, t, false, e.Clicks);
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (Browser != null)
            {
                SetMouseEvent(e);
                CfxMouseButtonType t;
                switch (e.Button)
                {
                    case System.Windows.Forms.MouseButtons.Right:
                        t = CfxMouseButtonType.Left;
                        break;
                    case System.Windows.Forms.MouseButtons.Middle:
                        t = CfxMouseButtonType.Middle;
                        break;
                    default:
                        t = CfxMouseButtonType.Left;
                        break;
                }
                Browser.Host.SendMouseClickEvent(mouseEventProxy, t, true, e.Clicks);
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (Browser != null)
            {
                SetMouseEvent(e);
                Browser.Host.SendMouseWheelEvent(mouseEventProxy, 0, e.Delta);
            }
        }

        // key events
        // this is not complete

        protected override void OnKeyPress(KeyPressEventArgs e)
        {

            //Console.WriteLine (e.KeyChar);
            if (e.KeyChar == 7)
            {
                // ctrl+g - load google so we have a page with text input
                Browser.MainFrame.LoadUrl("https://www.baidu.com");
            }
            else
            {
                if (!e.Handled)
                {
                    var k = new CfxKeyEvent();
                    k.WindowsKeyCode = e.KeyChar;
                    k.Character = (short)e.KeyChar;
                    k.Type = CfxKeyEventType.Char;
                    k.UnmodifiedCharacter = (short)e.KeyChar;
                    Browser.Host.SendKeyEvent(k);

                    e.Handled = true;
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {

            var j = new CfxKeyEvent();
            j.WindowsKeyCode = e.KeyValue;
            j.Character = (short)e.KeyValue;
            j.Type = CfxKeyEventType.Keydown;

            Browser.Host.SendKeyEvent(j);
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            switch (keyData)
            {
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                    {
                        var j = new CfxKeyEvent();
                        j.WindowsKeyCode = (int)keyData;
                        j.Type = CfxKeyEventType.RawKeydown;
                        Browser.Host.SendKeyEvent(j);
                        return true;
                    }
                default:
                    {
                        return base.ProcessCmdKey(ref msg, keyData);
                    }
            }

        }

        public ChromiumWebBrowserWindowless(Control parent) : base(parent)
        {
            
        }

        public ChromiumWebBrowserWindowless(bool createImmediately, Control parent) : base(createImmediately, parent)
        {
            
        }

        public ChromiumWebBrowserWindowless(string initialUrl, Control parent) : base(initialUrl, parent)
        {
           
        }

        public ChromiumWebBrowserWindowless(string initialUrl, bool createImmediately, Control parent) : base(initialUrl, createImmediately, parent)
        {
             
        }
    }

}
