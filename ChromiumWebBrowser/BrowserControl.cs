// Copyright (c) 2014-2017 Wolfgang Borgsmüller
// All rights reserved.
// 
// This software may be modified and distributed under the terms
// of the BSD license. See the License.txt file for details.

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Chromium.WebBrowser
{
    /// <summary>
    /// A minimum and very incomplete implementation of a
    /// control with windowless browser.
    /// </summary>
    public class BrowserControl : Control
    {

        internal CfxBrowser browser;

        private CfxClient client;
        private CfxLifeSpanHandler lifeSpanHandler;
        private CfxLoadHandler loadHandler;
        private CfxRenderHandler renderHandler;

        private CfxKeyboardHandler keyboardHandler;

        private Bitmap pixelBuffer;
        private object pbLock = new object();

        private static bool _mono;
        public BrowserControl(Control parent)
        {
            _mono = Type.GetType("Mono.Runtime") != null;
            this.Parent = parent;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            Console.WriteLine("can enable ime:" + this.CanEnableIme);

            this.ImeMode = ImeMode.On;

            lifeSpanHandler = new CfxLifeSpanHandler();
            lifeSpanHandler.OnAfterCreated += lifeSpanHandler_OnAfterCreated;

            renderHandler = new CfxRenderHandler();

            renderHandler.GetRootScreenRect += renderHandler_GetRootScreenRect;
            renderHandler.GetScreenInfo += renderHandler_GetScreenInfo;
            renderHandler.GetScreenPoint += renderHandler_GetScreenPoint;
            renderHandler.GetViewRect += renderHandler_GetViewRect;
            renderHandler.OnCursorChange += renderHandler_OnCursorChange;
            renderHandler.OnPaint += renderHandler_OnPaint;

            //renderHandler.OnPopupShow += renderHandler_OnPopupShow;
            //renderHandler.OnPopupSize += renderHandler_OnPopupSize;
            //renderHandler.OnScrollOffsetChanged += renderHandler_OnScrollOffsetChanged;
            //renderHandler.StartDragging += renderHandler_StartDragging;
            //renderHandler.UpdateDragCursor += renderHandler_UpdateDragCursor;

            loadHandler = new CfxLoadHandler();

            loadHandler.OnLoadError += loadHandler_OnLoadError;


            keyboardHandler = new CfxKeyboardHandler();


            keyboardHandler.OnPreKeyEvent += KeyboardHandler_OnPreKeyEvent;

            client = new CfxClient();
            client.GetLifeSpanHandler += (sender, e) => e.SetReturnValue(lifeSpanHandler);
            client.GetRenderHandler += (sender, e) => e.SetReturnValue(renderHandler);
            client.GetLoadHandler += (sender, e) => e.SetReturnValue(loadHandler);
            client.GetKeyboardHandler += (sender, e) => e.SetReturnValue(keyboardHandler);

            var settings = new CfxBrowserSettings();

            var windowInfo = new CfxWindowInfo();

            windowInfo.SetAsWindowless(this.Handle);
            //windowInfo.SetAsWindowless(this.Parent.Handle);

            // Create handle now for InvokeRequired to work properly 
            // CreateHandle();
            //  CfxBrowserHost.CreateBrowser(windowInfo, client, "about:blank", settings, null);

            //browser = CfxBrowserHost.CreateBrowserSync(windowInfo, client, "about:blank", settings, null);

            if (!CfxBrowserHost.CreateBrowser(windowInfo, client, "about:blank", settings, null))
                throw new ChromiumWebBrowserException("Failed to create browser instance.");

        }

        protected string initialUrl;
        protected string m_loadUrlDeferred;
        protected string m_loadStringDeferred;

        internal readonly object browserSyncRoot = new object();
        public void LoadUrl(string url)
        {
            if (browser != null)
                browser.MainFrame.LoadUrl(url);
            else
            {
                initialUrl = url;
                lock (browserSyncRoot)
                {
                    if (browser != null)
                    {
                        browser.MainFrame.LoadUrl(url);
                    }
                    else
                    {
                        m_loadUrlDeferred = url;
                    }
                }
            }

            OnResize(null);
        }

        protected override void Dispose(bool disposing)
        {
            //this.browser.Dispose();
            //this.client.Dispose();


            base.Dispose(disposing);
        }

        void KeyboardHandler_OnPreKeyEvent(object sender, Chromium.Event.CfxOnPreKeyEventEventArgs e)
        {
            //	Console.WriteLine ((char)e.Event.UnmodifiedCharacter);


        }

        void loadHandler_OnLoadError(object sender, Chromium.Event.CfxOnLoadErrorEventArgs e)
        {
            if (e.ErrorCode == CfxErrorCode.Aborted)
            {
                // this seems to happen when calling LoadUrl and the browser is not yet ready
                var url = e.FailedUrl;
                var frame = e.Frame;
                System.Threading.ThreadPool.QueueUserWorkItem((state) =>
                {
                    System.Threading.Thread.Sleep(200);
                    frame.LoadUrl(url);
                });
            }
        }

        void renderHandler_UpdateDragCursor(object sender, Chromium.Event.CfxUpdateDragCursorEventArgs e)
        {
            throw new NotImplementedException();
        }

        void renderHandler_StartDragging(object sender, Chromium.Event.CfxStartDraggingEventArgs e)
        {
            throw new NotImplementedException();
        }

        void renderHandler_OnScrollOffsetChanged(object sender, Chromium.Event.CfxOnScrollOffsetChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void renderHandler_OnPopupSize(object sender, Chromium.Event.CfxOnPopupSizeEventArgs e)
        {
            throw new NotImplementedException();
        }

        void renderHandler_OnPopupShow(object sender, Chromium.Event.CfxOnPopupShowEventArgs e)
        {
            throw new NotImplementedException();
        }

        void renderHandler_OnPaint(object sender, Chromium.Event.CfxOnPaintEventArgs e)
        {

            lock (pbLock)
            {
                if (pixelBuffer == null || pixelBuffer.Width < e.Width || pixelBuffer.Height < e.Height)
                {
                    pixelBuffer = new Bitmap(e.Width, e.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                }
                using (var bm = new Bitmap(e.Width, e.Height, e.Width * 4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, e.Buffer))
                {
                    using (var g = Graphics.FromImage(pixelBuffer))
                    {
                        g.DrawImageUnscaled(bm, 0, 0);
                    }
                }
            }
            foreach (var r in e.DirtyRects)
            {
                Invalidate(new Rectangle(r.X, r.Y, r.Width, r.Height));
            }
        }

        void renderHandler_OnCursorChange(object sender, Chromium.Event.CfxOnCursorChangeEventArgs e)
        {
            Invoke((MethodInvoker)(() => {
                switch (e.Type)
                {
                    case CfxCursorType.Hand:
                        Cursor = Cursors.Hand;
                        break;
                    default:
                        Cursor = Cursors.Default;
                        break;
                }
            }));
            
        }

        void renderHandler_GetViewRect(object sender, Chromium.Event.CfxGetViewRectEventArgs e)
        {

            if (InvokeRequired)
            {
                Invoke((MethodInvoker)(() => renderHandler_GetViewRect(sender, e)));
                return;
            }

            if (!IsDisposed)
            {
                var origin = PointToScreen(new Point(0, 0));
                e.Rect.X = origin.X;
                e.Rect.Y = origin.Y;
                e.Rect.Width = Width;
                e.Rect.Height = Height;
                e.SetReturnValue(true);
            }
        }

        void renderHandler_GetScreenPoint(object sender, Chromium.Event.CfxGetScreenPointEventArgs e)
        {

            if (InvokeRequired)
            {
                Invoke((MethodInvoker)(() => renderHandler_GetScreenPoint(sender, e)));
                return;
            }

            if (!IsDisposed)
            {
                var origin = PointToScreen(new Point(e.ViewX, e.ViewY));
                e.ScreenX = origin.X;
                e.ScreenY = origin.Y;
                e.SetReturnValue(true);
            }
        }

        void renderHandler_GetScreenInfo(object sender, Chromium.Event.CfxGetScreenInfoEventArgs e)
        {
        }

        void renderHandler_GetRootScreenRect(object sender, Chromium.Event.CfxGetRootScreenRectEventArgs e)
        {
        }


        void lifeSpanHandler_OnAfterCreated(object sender, Chromium.Event.CfxOnAfterCreatedEventArgs e)
        {
            //  browser = e.Browser;
            //  browser.MainFrame.LoadUrl("about:version");
            //  if(Focused) {
            //      browser.Host.SendFocusEvent(true);
            //  }
            if (browser != null)
            {

            }
            else
            {
                browser = e.Browser;
                browser.MainFrame.LoadUrl("about:version");
                if (!string.IsNullOrEmpty(initialUrl))
                {
                    browser.MainFrame.LoadUrl(initialUrl);
                }
            }

            var br = e.Browser;
            // browser.MainFrame.LoadUrl("about:version");


            Invoke((MethodInvoker) (() =>
            {
                if (Focused)
                {
                    br.Host.SendFocusEvent(true);
                }
            }));
           
        }

        protected override void OnGotFocus(EventArgs e)
        {
            if (browser != null)
            {
                browser.Host.SendFocusEvent(true);
            }
        }

        // control overrides

        protected override void OnResize(EventArgs e)
        {
            if (browser != null)
            {
                browser.Host.WasResized();
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
            if (browser != null)
            {
                SetMouseEvent(e);
                browser.Host.SendMouseMoveEvent(mouseEventProxy, false);
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (browser != null)
            {
                browser.Host.SendMouseMoveEvent(mouseEventProxy, true);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (browser != null)
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
                browser.Host.SendFocusEvent(true);
                browser.Host.SendMouseClickEvent(mouseEventProxy, t, false, e.Clicks);
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (browser != null)
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
                browser.Host.SendMouseClickEvent(mouseEventProxy, t, true, e.Clicks);
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (browser != null)
            {
                SetMouseEvent(e);
                browser.Host.SendMouseWheelEvent(mouseEventProxy, 0, e.Delta);
            }
        }

        // key events
        // this is not complete

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (browser == null)
            {
                return;
            }
            //Console.WriteLine (e.KeyChar);
                if (e.KeyChar == 7)
            {
                // ctrl+g - load google so we have a page with text input
                browser.MainFrame.LoadUrl("https://www.baidu.com");
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
                    browser.Host.SendKeyEvent(k);

                    e.Handled = true;
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (browser != null)
            {
                var j = new CfxKeyEvent();
                j.WindowsKeyCode = e.KeyValue;
                j.Character = (short)e.KeyValue;
                j.Type = CfxKeyEventType.Keydown;

                browser.Host.SendKeyEvent(j);
            }
            
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
                        browser.Host.SendKeyEvent(j);
                        return true;
                    }
                default:
                    {
                        return base.ProcessCmdKey(ref msg, keyData);
                    }
            }

        }
    }

}
