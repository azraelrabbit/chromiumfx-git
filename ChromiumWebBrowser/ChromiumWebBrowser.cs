// Copyright (c) 2014-2017 Wolfgang Borgsmüller
// All rights reserved.
// 
// This software may be modified and distributed under the terms
// of the BSD license. See the License.txt file for details.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Chromium.Event;
using Chromium.Remote;
using Chromium.Remote.Event;

using Chromium.WebBrowser.Event;

namespace Chromium.WebBrowser {


    /// <summary>
    /// Windows Forms webbrowser control based on ChromiumFX.
    /// </summary>
    [DesignerCategory("")]
    public class ChromiumWebBrowser : ChromiumWebBrowserBase {

        private static CfxBrowserSettings defaultBrowserSettings;


        /// <summary>
        /// The CfxBrowserSettings applied for new instances of ChromiumWebBrowser.
        /// Any changes to these settings will only apply to new browsers,
        /// leaving already created browsers unaffected.
        /// </summary>
        public static CfxBrowserSettings DefaultBrowserSettings {
            get {
                if(defaultBrowserSettings == null) {
                    defaultBrowserSettings = new CfxBrowserSettings();
                }
                return defaultBrowserSettings;
            }
        }


        

        /// <summary>
        /// This function should be called on the main application thread to shut down
        /// the CEF browser process before the application exits.
        /// </summary>
        public static void Shutdown() {
            CfxRuntime.Shutdown();
        }

        /// <summary>
        /// The CfxBrowserProcessHandler for this browser process.
        /// Do not access this property before calling ChromiumWebBrowser.Initialize()
        /// </summary>
        public static CfxBrowserProcessHandler BrowserProcessHandler {
            get {
                return BrowserProcess.processHandler;
            }
        }

   

      

        private BrowserClient client;

 

        /// <summary>
        /// Creates a ChromiumWebBrowser object with about:blank as initial URL.
        /// The underlying CfxBrowser is created immediately with the
        /// default CfxRequestContext.
        /// </summary>
		public ChromiumWebBrowser(Control parent) : this(null, true,parent) { }

        /// <summary>
        /// Creates a ChromiumWebBrowser object with about:blank as initial URL.
        /// If createImmediately is true, then the underlying CfxBrowser is 
        /// created immediately with the default CfxRequestContext.
        /// </summary>
        /// <param name="createImmediately"></param>
		public ChromiumWebBrowser(bool createImmediately,Control parent) : this(null, createImmediately,parent) { }

        /// <summary>
        /// Creates a ChromiumWebBrowser object with the given initial URL.
        /// The underlying CfxBrowser is created immediately with the
        /// default CfxRequestContext.
        /// </summary>
		public ChromiumWebBrowser(string initialUrl,Control parent) : this(initialUrl, true,parent) { }

        /// <summary>
        /// Creates a ChromiumWebBrowser object with the given initial URL.
        /// If createImmediately is true, then the underlying CfxBrowser is 
        /// created immediately with the default CfxRequestContext.
        /// </summary>
		public ChromiumWebBrowser(string initialUrl, bool createImmediately,Control parent) {

			this.Parent = parent;

			ImeMode = ImeMode.On;
            if(BrowserProcess.initialized) {
                //ControlStyles.ContainerControl
                // | ControlStyles.EnableNotifyMessage
                // | ControlStyles.UseTextForAccessibility
                //SetStyle(

                //     ControlStyles.ResizeRedraw
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

                //SetStyle(ControlStyles.UserPaint
                //    | ControlStyles.AllPaintingInWmPaint
                //    | ControlStyles.CacheText
                //    | ControlStyles.Selectable
                //    , true);


                base.SetStyle(ControlStyles.ContainerControl | ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.FixedWidth | ControlStyles.FixedHeight | ControlStyles.StandardClick | ControlStyles.UserMouse | ControlStyles.SupportsTransparentBackColor | ControlStyles.StandardDoubleClick | ControlStyles.EnableNotifyMessage | ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UseTextForAccessibility, false);
                //base.SetStyle(ControlStyles.UserPaint | ControlStyles.Selectable | ControlStyles.AllPaintingInWmPaint | ControlStyles.CacheText, true);

                SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                if (initialUrl == null)
                    this.initialUrl = "about:blank";
                else
                    this.initialUrl = initialUrl;

                client = new BrowserClient(this);

                GlobalObject = new JSObject();
                GlobalObject.SetBrowser("window", this);

                if(createImmediately)
                    CreateBrowser();

            } else {
                BackColor = System.Drawing.Color.White;
                Width = 200;
                Height = 160;
                var label = new Label();
                label.AutoSize = true;
                label.Text = "ChromiumWebBrowser";
                label.Parent = this;
            }
        }

        /// <summary>
        /// Creates the underlying CfxBrowser with the default CfxRequestContext.
        /// This method should only be called if this ChromiumWebBrowser
        /// was instanciated with createImmediately == false.
        /// </summary>
        public void CreateBrowser() {
            CreateBrowser((CfxRequestContext)null);
        }

        /// <summary>
        /// Creates the underlying CfxBrowser with the default CfxRequestContext
        /// and the given initial URL.
        /// This method should only be called if this ChromiumWebBrowser
        /// was instanciated with createImmediately == false.
        /// </summary>
        public void CreateBrowser(string initialUrl) {
            this.initialUrl = initialUrl;
            CreateBrowser((CfxRequestContext)null);
        }

        /// <summary>
        /// Creates the underlying CfxBrowser with the given 
        /// CfxRequestContext and initial URL.
        /// This method should only be called if this ChromiumWebBrowser
        /// was instanciated with createImmediately == false.
        /// </summary>
        public void CreateBrowser(string initialUrl, CfxRequestContext requestContext) {
            this.initialUrl = initialUrl;
            CreateBrowser(requestContext);
        }

        /// <summary>
        /// Creates the underlying CfxBrowser with the given CfxRequestContext.
        /// This method should only be called if this ChromiumWebBrowser
        /// was instanciated with createImmediately == false.
        /// </summary>
        public void CreateBrowser(CfxRequestContext requestContext) {

			 
            // avoid illegal cross-thread calls
            if(InvokeRequired) {
                Invoke((MethodInvoker)(() => CreateBrowser(requestContext)));
                return;
            }

			var parentHandle = this.Handle;

            //var rect = this.ClientRectangle;

            //if (this.Parent != null)
            //{
            //    parentHandle = this.Parent.Handle;
            //    rect = this.Parent.ClientRectangle;
            //}

            var windowInfo = new CfxWindowInfo();

            //this.ImeMode = ImeMode.Inherit;
            //if (WindowLess) {

            //	// in order to avoid focus issues when creating browsers offscreen,
            //	// the browser must be created with a disabled child window.
            //	windowInfo.SetAsDisabledChild(parentHandle);
            //	//windowInfo.SetAsChild ();
            //} else {
            //windowInfo.SetAsChild(parentHandle, rect.Left, rect.Top, rect.Width, rect.Height);
            //windowInfo.SetAsDisabledChild(parentHandle);
            //	//windowInfo.Style = WindowStyle.WS_CHILD;
            //}

            // this work in windows.
            windowInfo.SetAsDisabledChild(parentHandle);

            if (!CfxBrowserHost.CreateBrowser(windowInfo, client, initialUrl, DefaultBrowserSettings, requestContext))
                throw new ChromiumWebBrowserException("Failed to create browser instance.");
        }

        /// <summary>
        /// Returns the context menu handler for this browser. If this is never accessed the default
        /// implementation will be used.
        /// </summary>
        public CfxContextMenuHandler ContextMenuHandler { get { return client.ContextMenuHandler; } }

        /// <summary>
        /// Returns the life span handler for this browser.
        /// </summary>
        public CfxLifeSpanHandler LifeSpanHandler { get { return client.lifeSpanHandler; } }

        /// <summary>
        /// Returns the load handler for this browser.
        /// </summary>
        public CfxLoadHandler LoadHandler { get { return client.LoadHandler; } }

        /// <summary>
        /// Returns the request handler for this browser.
        /// Do not set the return value in the GetResourceHandler event for URLs
        /// with associated WebResources (see also SetWebResource).
        /// </summary>
        public CfxRequestHandler RequestHandler { get { return client.requestHandler; } }

        /// <summary>
        /// Returns the display handler for this browser.
        /// </summary>
        public CfxDisplayHandler DisplayHandler { get { return client.DisplayHandler; } }

        /// <summary>
        /// Returns the download handler for this browser. If this is never accessed
        /// downloads will not be allowed.
        /// </summary>
        public CfxDownloadHandler DownloadHandler { get { return client.DownloadHandler; } }

        /// <summary>
        /// Returns the drag handler for this browser.
        /// </summary>
        public CfxDragHandler DragHandler { get { return client.DragHandler; } }

        /// <summary>
        /// Returns the dialog handler for this browser. If this is never accessed the default
        /// implementation will be used.
        /// </summary>
        public CfxDialogHandler DialogHandler { get { return client.DialogHandler; } }

        /// <summary>
        /// Returns the find handler for this browser.
        /// </summary>
        public CfxFindHandler FindHandler { get { return client.FindHandler; } }

        /// <summary>
        /// Returns the focus handler for this browser.
        /// </summary>
        public CfxFocusHandler FocusHandler { get { return client.FocusHandler; } }

        /// <summary>
        /// Returns the geolocation handler for this browser. If this is never accessed
        /// geolocation access will be denied by default.
        /// </summary>
        public CfxGeolocationHandler GeolocationHandler { get { return client.GeolocationHandler; } }

        /// <summary>
        /// Returns the js dialog handler for this browser. If this is never accessed the default
        /// implementation will be used.
        /// </summary>
        public CfxJsDialogHandler JsDialogHandler { get { return client.JsDialogHandler; } }

        /// <summary>
        /// Returns the keyboard handler for this browser.
        /// </summary>
        public CfxKeyboardHandler KeyboardHandler { get { return client.KeyboardHandler; } }

        internal  override void OnBrowserCreated(CfxOnAfterCreatedEventArgs e)
        {

            Browser = e.Browser;
            BrowserHost = Browser.Host;
            browserWindowHandle = BrowserHost.WindowHandle;
            AddToBrowserCache(this);
            ResizeBrowserWindow();

            ReaiseBrowserCreated(e);

            System.Threading.ThreadPool.QueueUserWorkItem(AfterSetBrowserTasks);
        }

        private int findId;
        private string currentFindText;
        private bool currentMatchCase;

        /// <summary>
        /// Search for |searchText|. |forward| indicates whether to search forward or
        /// backward within the page. |matchCase| indicates whether the search should
        /// be case-sensitive.
        /// Returns the identifier for this find operation (see also CfxFindHandler),
        /// or -1 if the browser has not yet been created.
        /// </summary>
        public int Find(string searchText, bool forward, bool matchCase) {
            if(BrowserHost == null)
                return -1;
            var findNext = currentFindText == searchText && currentMatchCase == matchCase;
            if(!findNext) {
                currentFindText = searchText;
                currentMatchCase = matchCase;
                ++findId;
            }

            BrowserHost.Find(findId, searchText, forward, matchCase, findNext);
            return findId;
        }

        /// <summary>
        /// Search for |searchText|. |forward| indicates whether to search forward or
        /// backward within the page. The search will be case-insensitive.
        /// Returns the identifier for this find operation (see also CfxFindHandler),
        /// or -1 if the browser has not yet been created.
        /// </summary>
        public int Find(string searchText, bool forward) {
            return Find(searchText, forward, false);
        }

        /// <summary>
        /// Search for |searchText|. The search will be forward and case-insensitive.
        /// Returns the identifier for this find operation (see also CfxFindHandler),
        /// or -1 if the browser has not yet been created.
        /// </summary>
        public int Find(string searchText) {
            return Find(searchText, true, false);
        }


        private FindToolbar m_findToolbar;

        /// <summary>
        /// Get the find toolbar of this browser window.
        /// </summary>
        public FindToolbar FindToolbar {
            get {
                if(m_findToolbar == null)
                    m_findToolbar = new FindToolbar(this);
                return m_findToolbar;
            }
        }


        /// <summary>
        /// Evaluate a string of javascript code in the browser's main frame.
        /// Evaluation is done asynchronously in the render process.
        /// Returns false if the remote browser is currently unavailable.
        /// If this function returns false, then |callback| will not be called. Otherwise,
        /// |callback| will be called asynchronously in the context of the render thread and,
        /// if RemoteCallbackInvokeMode is set to Invoke, on the thread that owns the 
        /// browser's underlying window handle.
        /// 
        /// Use with care:
        /// The callback may never be called if the render process gets killed prematurely.
        /// On success the CfrV8Value argument of the callback will be set to the return value
        /// of the evaluated script, if any. On failure the CfrV8Exception argument of the callback
        /// will be set to the exception thrown by the evaluated script, if any.
        /// Do not block the callback since it blocks the render thread.
        /// 
        /// *** WARNING ***
        /// In CEF 3.2623 and higher, the return value of the evaluation 
        /// seems to be broken in some cases (see also issue #65).
        /// 
        /// </summary>
        public bool EvaluateJavascript(string code, Action<CfrV8Value, CfrV8Exception> callback) {
            return EvaluateJavascript(code, JSInvokeMode.Inherit, callback);
        }

        /// <summary>
        /// Evaluate a string of javascript code in the browser's main frame.
        /// Evaluation is done asynchronously in the render process.
        /// Returns false if the remote browser is currently unavailable.
        /// If this function returns false, then |callback| will not be called. Otherwise,
        /// |callback| will be called asynchronously in the context of the render thread.
        /// 
        /// If |invokeMode| is set to Invoke, |callback| will be called on the thread that 
        /// owns the browser's underlying window handle. If |invokeMode| is set to Inherit,
        /// |callback| will be called according to RemoteCallbackInvokeMode.
        /// 
        /// Use with care:
        /// The callback may never be called if the render process gets killed prematurely.
        /// On success the CfrV8Value argument of the callback will be set to the return value
        /// of the evaluated script, if any. On failure the CfrV8Exception argument of the callback
        /// will be set to the exception thrown by the evaluated script, if any.
        /// Do not block the callback since it blocks the render thread.
        /// 
        /// *** WARNING ***
        /// In CEF 3.2623 and higher, the return value of the evaluation 
        /// seems to be broken in some cases (see also issue #65).
        /// 
        /// </summary>
        public bool EvaluateJavascript(string code, JSInvokeMode invokeMode, Action<CfrV8Value, CfrV8Exception> callback) {
            var rb = remoteBrowser;
            if(rb == null) return false;
            try {
                var ctx = rb.CreateRemoteCallContext();
                ctx.Enter();
                try {
                    var taskRunner = CfrTaskRunner.GetForThread(CfxThreadId.Renderer);
                    var task = new EvaluateTask(this, code, invokeMode, callback);
                    taskRunner.PostTask(task);
                    return true;
                } finally {
                    ctx.Exit();
                }
            } catch(CfxRemotingException) {
                return false;
            }
        }

        private class EvaluateTask : CfrTask {

            ChromiumWebBrowser wb;
            string code;
            JSInvokeMode invokeMode;
            Action<CfrV8Value, CfrV8Exception> callback;

            internal EvaluateTask(ChromiumWebBrowser wb, string code, JSInvokeMode invokeMode, Action<CfrV8Value, CfrV8Exception> callback) {
                this.wb = wb;
                this.code = code;
                this.invokeMode = invokeMode;
                this.callback = callback;
                Execute += (s, e) => {
                    if(invokeMode == JSInvokeMode.Invoke || (invokeMode == JSInvokeMode.Inherit && wb.RemoteCallbacksWillInvoke))
                        wb.RenderThreadInvoke(() => Task_Execute(e));
                    else
                        Task_Execute(e);
                };
            }

            void Task_Execute(CfrEventArgs e) {
                CfrV8Context context;
                CfrV8Value retval;
                CfrV8Exception ex;
                bool result = false;
                try {
                    context = wb.remoteBrowser.MainFrame.V8Context;
                    result = context.Eval(code, null, 0, out retval, out ex);
                } catch {
                    callback(null, null);
                    return;
                }
                context.Enter();
                try {
                    if(result) {
                        callback(retval, null);
                    } else {
                        callback(null, ex);
                    }
                } finally {
                    context.Exit();
                }
            }
        }


        /// <summary>
        /// Visit the DOM in the remote browser's main frame.
        /// Returns false if the remote browser is currently unavailable.
        /// If this function returns false, then |callback| will not be called. Otherwise,
        /// |callback| will be called according to the InvokeMode setting.
        /// 
        /// The document object passed to the callback represents a snapshot 
        /// of the DOM at the time the callback is executed.
        /// DOM objects are only valid for the scope of the callback. Do not
        /// keep references to or attempt to access any DOM objects outside the scope
        /// of the callback.
        /// Use with care:
        /// The callback may never be called if the render process gets killed prematurely.
        /// Do not keep a reference to the remote DOM or remote browser object after returning from the callback.
        /// Do not block the callback since it blocks the renderer thread.
        /// Explicitly Dispose() all CfrDomNode objects, otherwise the render process may become unstable and crash.
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public bool VisitDom(Action<CfrDomDocument, CfrBrowser> callback) {
            var rb = remoteBrowser;
            if(rb == null) return false;
            try {
                var ctx = rb.CreateRemoteCallContext();
                ctx.Enter();
                try {
                    var taskRunner = CfrTaskRunner.GetForThread(CfxThreadId.Renderer);
                    var task = new VisitDomTask(this, callback);
                    taskRunner.PostTask(task);
                    return true;
                } finally {
                    ctx.Exit();
                }
            } catch(CfxRemotingException) {
                return false;
            }
        }

        private class VisitDomTask : CfrTask {

            ChromiumWebBrowser wb;
            Action<CfrDomDocument, CfrBrowser> callback;
            CfrDomVisitor visitor;

            internal VisitDomTask(ChromiumWebBrowser wb, Action<CfrDomDocument, CfrBrowser> callback) {
                this.wb = wb;
                this.callback = callback;
                this.Execute += Task_Execute;
                visitor = new CfrDomVisitor();
                visitor.Visit += (s, e) => {
                    if(wb.RemoteCallbacksWillInvoke)
                        wb.RenderThreadInvoke((MethodInvoker)(() => { callback(e.Document, wb.remoteBrowser); }));
                    else
                        callback(e.Document, wb.remoteBrowser);
                };
            }

            void Task_Execute(object sender, CfrEventArgs e) {
                wb.remoteBrowser.MainFrame.VisitDom(visitor);
            }
        }

        


        ///// <summary>
        ///// Raised after the CfxBrowser object for this WebBrowser has been created.
        ///// The event is executed on the thread that owns this browser control's 
        ///// underlying window handle.
        ///// </summary>
        //public   event BrowserCreatedEventHandler BrowserCreated;

       

        [Obsolete("OnLoadingStateChange is deprecated. Please use LoadHandler.OnLoadingStateChange and check for invalid cross-thread operations.")]
        public event CfxOnLoadingStateChangeEventHandler OnLoadingStateChange {
            add {
                lock(browserSyncRoot) {
                    if(m_OnLoadingStateChange == null)
                        client.LoadHandler.OnLoadingStateChange += RaiseOnLoadingStateChange;
                    m_OnLoadingStateChange += value;
                }
            }
            remove {
                lock(browserSyncRoot) {
                    m_OnLoadingStateChange -= value;
                    if(m_OnLoadingStateChange == null)
                        client.LoadHandler.OnLoadingStateChange -= RaiseOnLoadingStateChange;
                }
            }
        }

        private CfxOnLoadingStateChangeEventHandler m_OnLoadingStateChange;
        private void RaiseOnLoadingStateChange(object sender, CfxOnLoadingStateChangeEventArgs e) {
            var handler = m_OnLoadingStateChange;
            if(handler != null) {
                InvokeCallback(() => { handler(this, e); });
            }
        }


        [Obsolete("OnBeforeContextMenu is deprecated. Please use ContextMenuHandler.OnBeforeContextMenu and check for invalid cross-thread operations.")]
        public event CfxOnBeforeContextMenuEventHandler OnBeforeContextMenu {
            add {
                lock(browserSyncRoot) {
                    if(m_OnBeforeContextMenu == null)
                        client.ContextMenuHandler.OnBeforeContextMenu += RaiseOnBeforeContextMenu;
                    m_OnBeforeContextMenu += value;
                }
            }
            remove {
                lock(browserSyncRoot) {
                    m_OnBeforeContextMenu -= value;
                    if(m_OnBeforeContextMenu == null)
                        client.ContextMenuHandler.OnBeforeContextMenu -= RaiseOnBeforeContextMenu;
                }
            }
        }

        private CfxOnBeforeContextMenuEventHandler m_OnBeforeContextMenu;
        private void RaiseOnBeforeContextMenu(object sender, CfxOnBeforeContextMenuEventArgs e) {
            var handler = m_OnBeforeContextMenu;
            if(handler != null) {
                InvokeCallback(() => { handler(this, e); });
            }
        }


        [Obsolete("OnContextMenuCommand is deprecated. Please use ContextMenuHandler.OnContextMenuCommand and check for invalid cross-thread operations.")]
        public event CfxOnContextMenuCommandEventHandler OnContextMenuCommand {
            add {
                lock(browserSyncRoot) {
                    if(m_OnContextMenuCommand == null)
                        client.ContextMenuHandler.OnContextMenuCommand += RaiseOnContextMenuCommand;
                    m_OnContextMenuCommand += value;
                }
            }
            remove {
                lock(browserSyncRoot) {
                    m_OnContextMenuCommand -= value;
                    if(m_OnContextMenuCommand == null)
                        client.ContextMenuHandler.OnContextMenuCommand -= RaiseOnContextMenuCommand;
                }
            }
        }

        private CfxOnContextMenuCommandEventHandler m_OnContextMenuCommand;
        private void RaiseOnContextMenuCommand(object sender, CfxOnContextMenuCommandEventArgs e) {
            var handler = m_OnContextMenuCommand;
            if(handler != null) {
                InvokeCallback(() => { handler(this, e); });
            }
        }
 
        //protected override void WndProc(ref Message m) {
        //    base.WndProc(ref m);
        //    Debug.Print(m.ToString());
        //}

        protected override void OnVisibleChanged(EventArgs e) {
            base.OnVisibleChanged(e);
            ResizeBrowserWindow();
            if(Visible)
                Refresh();
        }

        protected override void OnGotFocus(System.EventArgs e) {
            base.OnGotFocus(e);
            if(BrowserHost != null) BrowserHost.SetFocus(true);
        }

        protected override void OnResize(System.EventArgs e) {
            ResizeBrowserWindow();
            base.OnResize(e);
        
        }



//        internal void ResizeBrowserWindow() {
//            if(browserWindowHandle == IntPtr.Zero) return;
//            if(Visible && Height > 0 && Width > 0) {
//                int h;
//                if(m_findToolbar == null || !m_findToolbar.Visible) {
//                    h = Height;
//                } else {
//                    if(InvokeRequired) {
//                        Invoke((MethodInvoker)(() => {
//                            m_findToolbar.Width = Width;
//                            m_findToolbar.Top = Height - m_findToolbar.Height;
//                        }));
//                    } else {
//                        m_findToolbar.Width = Width;
//                        m_findToolbar.Top = Height - m_findToolbar.Height;
//                    }
//                    h = m_findToolbar.Top;
//                }
//                SetWindowLong(browserWindowHandle, -16, (int)(WindowStyle.WS_CHILD | WindowStyle.WS_CLIPCHILDREN | WindowStyle.WS_CLIPSIBLINGS | WindowStyle.WS_TABSTOP | WindowStyle.WS_VISIBLE));
//                SetWindowPos(browserWindowHandle, IntPtr.Zero, 0, 0, Width, h, SWP_NOMOVE | SWP_NOZORDER | SWP_SHOWWINDOW | SWP_NOCOPYBITS | SWP_ASYNCWINDOWPOS);
//                //Debug.Print($"ResizeBrowserWindow: {Width} {h}");
//            } else {
//                SetWindowLong(browserWindowHandle, -16, (int)(WindowStyle.WS_CHILD | WindowStyle.WS_CLIPCHILDREN | WindowStyle.WS_CLIPSIBLINGS | WindowStyle.WS_TABSTOP | WindowStyle.WS_DISABLED));
//                SetWindowPos(browserWindowHandle, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_HIDEWINDOW | SWP_ASYNCWINDOWPOS);
//                //Debug.Print($"ResizeBrowserWindow: hide");
//            }
//        }
		internal void ResizeBrowserWindow() {
			if(Visible) {
				if(browserWindowHandle != IntPtr.Zero && this.Height > 0 && this.Width > 0) {
					int h;
					if(m_findToolbar == null || !m_findToolbar.Visible) {
						h = Height;
					} else {
						if(InvokeRequired) {
							Invoke((MethodInvoker)(() => {
								m_findToolbar.Width = Width;
								m_findToolbar.Top = Height - m_findToolbar.Height;
							}));
						} else {
							m_findToolbar.Width = Width;
							m_findToolbar.Top = Height - m_findToolbar.Height;
						}
						h = m_findToolbar.Top;
					}
					NativeWindow.SetStyle(browserWindowHandle, (int)(WindowStyle.WS_CHILD | WindowStyle.WS_CLIPCHILDREN | WindowStyle.WS_CLIPSIBLINGS | WindowStyle.WS_TABSTOP | WindowStyle.WS_VISIBLE));
					NativeWindow.SetPosition(browserWindowHandle, 0, 0, Width, h);
				}
			} else {
				if(browserWindowHandle != IntPtr.Zero)
					NativeWindow.SetStyle(browserWindowHandle, (int)(WindowStyle.WS_CHILD | WindowStyle.WS_CLIPCHILDREN | WindowStyle.WS_CLIPSIBLINGS | WindowStyle.WS_TABSTOP | WindowStyle.WS_DISABLED));
			}
			//this.InvokeCallback(()=>this.Update());//.Update ();
		}

//		protected override void OnPreviewKeyDown (PreviewKeyDownEventArgs e)
//		{
//			if (Browser != null) {
//				Browser.Host.SendKeyEvent (new CfxKeyEvent (){ NativeKeyCode = e.KeyValue });
//			}
//		}
//		protected override void OnKeyPress(KeyPressEventArgs e) {
//
//			Console.WriteLine (e.KeyChar);
//			if(e.KeyChar == 7) {
//				// ctrl+g - load google so we have a page with text input
//				Browser.MainFrame.LoadUrl("https://www.baidu.com");
//			} else {
//
//				var j = new CfxKeyEvent();
//				j.WindowsKeyCode = e.KeyChar;
//				j.Character = (short)e.KeyChar;
//				j.Type = CfxKeyEventType.Keydown;
//
//				Browser.Host.SendKeyEvent(j);
//
//				var k = new CfxKeyEvent();
//				k.WindowsKeyCode = e.KeyChar;
//				k.Character = (short)e.KeyChar;
//				k.Type = CfxKeyEventType.Char;
//				k.UnmodifiedCharacter = (short)e.KeyChar;
//				Browser.Host.SendKeyEvent(k);
//
//				base.OnKeyPress(e);
//			}
//		}
//
//
//		protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
//
//			switch (keyData)
//			{
//			case Keys.Down:
//			case Keys.Left:
//			case Keys.Right:
//			case Keys.Up:
//				{
//					var j = new CfxKeyEvent();
//					j.WindowsKeyCode = (int)keyData;
//					j.Type = CfxKeyEventType.RawKeydown;
//					Browser.Host.SendKeyEvent(j);
//					return true;
//				}
//			default:
//				{
//					return base.ProcessCmdKey(ref msg, keyData);
//				}
//			}
//
//		}

        [DllImport("user32", SetLastError = false)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32", SetLastError = false)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_SHOWWINDOW = 0x0040;
        private const uint SWP_HIDEWINDOW = 0x0080;
        private const uint SWP_NOCOPYBITS = 0x0100;
        private const uint SWP_ASYNCWINDOWPOS = 0x4000;
    }
}
