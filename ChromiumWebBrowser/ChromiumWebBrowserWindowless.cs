// Copyright (c) 2014-2017 Wolfgang Borgsmüller
// All rights reserved.
// 
// This software may be modified and distributed under the terms
// of the BSD license. See the License.txt file for details.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Chromium.Event;
using Chromium.WebBrowser.Event;

namespace Chromium.WebBrowser
{
    /// <summary>
    /// A minimum and very incomplete implementation of a
    /// control with windowless browser.
    /// </summary>
    [DesignerCategory("")]
    public class ChromiumWebBrowserWindowless : ChromiumWebBrowserBase
    {

        //internal CfxBrowser browser;

        private BrowserClient client;
        private CfxLifeSpanHandler lifeSpanHandler;
        private CfxLoadHandler loadHandler;
        private CfxRenderHandler renderHandler;

        private CfxKeyboardHandler keyboardHandler;

        private Bitmap pixelBuffer;
        private object pbLock = new object();

        private static bool _mono;


        public ChromiumWebBrowserWindowless(string initUrl, Control parent)
        {
            this.initialUrl = initUrl;
            MyCtor(parent);
        }
        public ChromiumWebBrowserWindowless(Control parent)
        {
            MyCtor(parent);
        }

        private void MyCtor(Control parent)
        {
            _mono = Type.GetType("Mono.Runtime") != null;
            this.Parent = parent;

            //SetStyle(

            //    ControlStyles.ResizeRedraw
            //    | ControlStyles.FixedWidth
            //    | ControlStyles.FixedHeight
            //    | ControlStyles.StandardClick
            //    | ControlStyles.StandardDoubleClick
            //    | ControlStyles.UserMouse
            //    | ControlStyles.SupportsTransparentBackColor

            //    | ControlStyles.DoubleBuffer
            //    | ControlStyles.OptimizedDoubleBuffer

            //    | ControlStyles.Opaque
            //    , false);

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            Console.WriteLine("can enable ime:" + this.CanEnableIme);

            this.ImeMode = ImeMode.On;


            client = new BrowserClient(this);
            //client.GetLifeSpanHandler += (sender, e) => e.SetReturnValue(lifeSpanHandler);
            //client.GetRenderHandler += (sender, e) => e.SetReturnValue(renderHandler);
            //client.GetLoadHandler += (sender, e) => e.SetReturnValue(loadHandler);
            //client.GetKeyboardHandler += (sender, e) => e.SetReturnValue(keyboardHandler);

            //lifeSpanHandler = new CfxLifeSpanHandler();
            //lifeSpanHandler.OnAfterCreated += lifeSpanHandler_OnAfterCreated;

            renderHandler = client.RenderHandler;// new CfxRenderHandler();

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

            loadHandler = client.LoadHandler;// new CfxLoadHandler();

            loadHandler.OnLoadError += loadHandler_OnLoadError;


            keyboardHandler = new CfxKeyboardHandler();


            keyboardHandler.OnPreKeyEvent += KeyboardHandler_OnPreKeyEvent;

            lifeSpanHandler = client.lifeSpanHandler;
            lifeSpanHandler.OnAfterCreated += lifeSpanHandler_OnAfterCreated;



            var settings = new CfxBrowserSettings();

            var windowInfo = new CfxWindowInfo();



            if (CfxRuntime.PlatformOS == CfxPlatformOS.Windows)
            {
                windowInfo.SetAsWindowless(this.Parent.Handle);
            }
            else
            {
                windowInfo.SetAsWindowless(this.Parent.Handle);
            }
         
            //windowInfo.SetAsWindowless(this.Parent.Handle);

            // Create handle now for InvokeRequired to work properly 
            // CreateHandle();

            if (string.IsNullOrEmpty(initialUrl))
            {
                initialUrl = "about:version";
            }


            GlobalObject = new JSObject();
            GlobalObject.SetBrowser("window", this);



            // CreateHandle();
            //  CfxBrowserHost.CreateBrowser(windowInfo, client, "about:blank", settings, null);

            //Browser = CfxBrowserHost.CreateBrowserSync(windowInfo, client, initialUrl, settings, null);

            //if (!CfxBrowserHost.CreateBrowser(windowInfo, client, initialUrl, settings, null))
            //    throw new ChromiumWebBrowserException("Failed to create browser instance.");

            CfxBrowserHost.CreateBrowser(windowInfo, client, initialUrl, settings, null);
        }

        /// <summary>
        /// Returns the load handler for this browser.
        /// </summary>
        public CfxLoadHandler LoadHandler {
            get { return loadHandler; }
        }

        

        protected string initialUrl;
        protected string m_loadUrlDeferred;
        protected string m_loadStringDeferred;

        internal readonly object browserSyncRoot = new object();


        //public void LoadUrl(string url)
        //{
        //    if (Browser != null)
        //        Browser.MainFrame.LoadUrl(url);
        //    else
        //    {
        //        initialUrl = url;
        //        lock (browserSyncRoot)
        //        {
        //            if (Browser != null)
        //            {
        //                Browser.MainFrame.LoadUrl(url);
        //            }
        //            else
        //            {
        //                m_loadUrlDeferred = url;
        //            }
        //        }
        //    }

        //    OnResize(null);
        //}


        ///// <summary>
        ///// Initialize the ChromiumWebBrowser and ChromiumFX libraries.
        ///// The application can change initialization settings by handling
        ///// the OnBeforeCfxInitialize event.
        ///// </summary>
        //public static void Initialize()
        //{
        //    BrowserProcess.Initialize();
        //}


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
            if (InvokeRequired)
            {
                Invoke((MethodInvoker) (() =>
                {
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
            else
            {
                switch (e.Type)
                {
                    case CfxCursorType.Hand:
                        Cursor = Cursors.Hand;
                        break;
                    default:
                        Cursor = Cursors.Default;
                        break;
                }
            }
            
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
            try
            {
                if (InvokeRequired)
                {
                    Invoke((MethodInvoker) (() => renderHandler_GetScreenPoint(sender, e)));
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
            catch (Exception ex)
            {
              //ignore   
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

            //if (Browser != null)
            //{
            //    //OnBrowserCreated(e);
            //}
            //else
            //{
            //    Browser = e.Browser;
            //    Browser.MainFrame.LoadUrl("about:version");
            //    //if (!string.IsNullOrEmpty(initialUrl))
            //    //{
            //    //    Browser.MainFrame.LoadUrl(initialUrl);
            //    //}

            //    //OnBrowserCreated(e);
            //}


            //Invoke((MethodInvoker)(() =>
            //{
            //    if (Focused)
            //    {
            //        var br = e.Browser;
            //        br.Host.SendFocusEvent(true);
            //    }
            //}));



        }



        /// <summary>
        /// Raised after the CfxBrowser object for this WebBrowser has been created.
        /// The event is executed on the thread that owns this browser control's 
        /// underlying window handle.
        /// </summary>
        public new event BrowserCreatedEventHandler BrowserCreated;

        //internal override void OnBrowserCreated(CfxOnAfterCreatedEventArgs e)
        //{

        //    Browser = e.Browser;
        //    BrowserHost = Browser.Host;
        //    //browserWindowHandle = BrowserHost.WindowHandle;
        //    AddToBrowserCache(this);
        ////ResizeBrowserWindow();

        //    var handler = BrowserCreated;
        //    if (handler != null)
        //    {
        //        var e1 = new BrowserCreatedEventArgs(e.Browser);
        //        handler(this, e1);
        //    }

        //    System.Threading.ThreadPool.QueueUserWorkItem(AfterSetBrowserTasks);
        //}

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

 

        protected override void OnKeyPress(KeyPressEventArgs e)
        {

            Console.WriteLine(e.KeyChar);
            if (e.KeyChar == 7)
            {
                //// ctrl+g - load google so we have a page with text input
                //Browser.MainFrame.LoadUrl("https://www.baidu.com");
                OnOnCgPress(e);
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
 
    }

}
