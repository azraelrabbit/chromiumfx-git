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
        

   
        /// <summary>
        /// For each new render process created, provides an opportunity to subscribe
        /// to CfrRenderProcessHandler remote callback events.
        /// </summary>
        public static event RemoteProcessCreatedEventHandler RemoteProcessCreated;
        internal static void RaiseRemoteProcessCreated(CfrRenderProcessHandler renderProcessHandler) {
            var ev = RemoteProcessCreated;
            if(ev != null) {
                ev(new RemoteProcessCreatedEventArgs(renderProcessHandler));
            }
        }

        [Obsolete("OnRemoteContextCreated is deprecated, use RemoteProcessCreated instead.")]
        public static event OnRemoteContextCreatedEventHandler OnRemoteContextCreated;
        internal static void RaiseOnRemoteContextCreated() {
            var handler = OnRemoteContextCreated;
            if(handler != null) {
                handler(EventArgs.Empty);
            }
        }


        /// <summary>
        /// The invoke mode for this browser. See also JSInvokeMode.
        /// Changes to the invoke mode will be effective after the next
        /// time the browser creates a V8 context. If this is set to
        /// "Inherit", then "Invoke" will be assumed. The invoke mode
        /// also applies to VisitDom and EvaluateJavascript.
        /// </summary>
        public JSInvokeMode RemoteCallbackInvokeMode { get; set; }

        /// <summary>
        /// Indicates whether render process callbacks on this browser
        /// will be executed on the thread that owns the 
        /// browser's underlying window handle.
        /// Depends on the invoke mode. If the invoke mode is set to
        /// "Inherit", then "Invoke" will be assumed.
        /// </summary>
        public bool RemoteCallbacksWillInvoke {
            get {
                return RemoteCallbackInvokeMode != JSInvokeMode.DontInvoke;
            }
        }

        internal RenderProcess remoteProcess;
        internal CfrBrowser remoteBrowser;

        /// <summary>
        /// Creates a ChromiumWebBrowser object with about:blank as initial URL.
        /// The underlying CfxBrowser is created immediately with the
        /// default CfxRequestContext.
        /// </summary>
		public ChromiumWebBrowser(Control parent) : base(parent) {
        }

        /// <summary>
        /// Creates a ChromiumWebBrowser object with about:blank as initial URL.
        /// If createImmediately is true, then the underlying CfxBrowser is 
        /// created immediately with the default CfxRequestContext.
        /// </summary>
        /// <param name="createImmediately"></param>
		public ChromiumWebBrowser(bool createImmediately,Control parent) : base(createImmediately, parent) {
        }

        /// <summary>
        /// Creates a ChromiumWebBrowser object with the given initial URL.
        /// The underlying CfxBrowser is created immediately with the
        /// default CfxRequestContext.
        /// </summary>
		public ChromiumWebBrowser(string initialUrl,Control parent) : base(initialUrl, parent) {
        }

        /// <summary>
        /// Creates a ChromiumWebBrowser object with the given initial URL.
        /// If createImmediately is true, then the underlying CfxBrowser is 
        /// created immediately with the default CfxRequestContext.
        /// </summary>
		public ChromiumWebBrowser(string initialUrl, bool createImmediately,Control parent) : base(initialUrl, createImmediately, parent)
        {
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
        /// Special Invoke for framework callbacks from the render process.
        /// Maintains the thread in the context of the calling remote thread.
        /// Use this instead of invoke when the following conditions are meat:
        /// 1) The current thread is executing in the scope of a framework
        ///    callback event from the render process (ex. CfrTask.Execute).
        /// 2) You need to Invoke on the webbrowser control and
        /// 3) The invoked code needs to call into the render process.
        /// </summary>
        public Object RenderThreadInvoke(Delegate method, params Object[] args) {

            if(!CfxRemoteCallContext.IsInContext) {
                throw new ChromiumWebBrowserException("Can't use RenderThreadInvoke without being in the scope of a render process callback.");
            }

            if(!InvokeRequired)
                return method.DynamicInvoke(args);

            object retval = null;
            var context = CfxRemoteCallContext.CurrentContext;

            // Use BeginInvoke and Wait instead of Invoke.
            // Invoke marshals exceptions back to the calling thread.
            // We want exceptions to be thrown in place.

            var waitLock = new object();
            lock(waitLock) {
                BeginInvoke((MethodInvoker)(() => {
                    context.Enter();
                    try {
                        retval = method.DynamicInvoke(args);
                    } finally {
                        context.Exit();
                        lock(waitLock) {
                            Monitor.PulseAll(waitLock);
                        }
                    }
                }));
                Monitor.Wait(waitLock);
            }
            return retval;
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
        public bool EvaluateJavascript(string code, Action<CfrV8Value, CfrV8Exception> callback)
        {
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
        public bool EvaluateJavascript(string code, JSInvokeMode invokeMode, Action<CfrV8Value, CfrV8Exception> callback)
        {
            var rb = remoteBrowser;
            if (rb == null) return false;
            try
            {
                var ctx = rb.CreateRemoteCallContext();
                ctx.Enter();
                try
                {
                    var taskRunner = CfrTaskRunner.GetForThread(CfxThreadId.Renderer);
                    var task = new ChromiumWebBrowser.EvaluateTask(this, code, invokeMode, callback);
                    taskRunner.PostTask(task);
                    return true;
                }
                finally
                {
                    ctx.Exit();
                }
            }
            catch (CfxRemotingException)
            {
                return false;
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
                        ((ChromiumWebBrowserBase) wb).RenderThreadInvoke((MethodInvoker)(() => { callback(e.Document, wb.remoteBrowser); }));
                    else
                        callback(e.Document, wb.remoteBrowser);
                };
            }

            void Task_Execute(object sender, CfrEventArgs e) {
                wb.remoteBrowser.MainFrame.VisitDom(visitor);
            }
        }

        // Callbacks from the associated render process handler

        /// <summary>
        /// Called immediately after the V8 context for a frame has been created. To
        /// retrieve the JavaScript 'window' object use the
        /// CfrV8Context.GetGlobal() function. V8 handles can only be accessed
        /// from the thread on which they are created. A task runner for posting tasks
        /// on the associated thread can be retrieved via the
        /// CfrV8Context.GetTaskRunner() function.
        /// 
        /// All javascript properties/functions defined through GlobalObject or GlobalObjectForFrame
        /// are made available before this event is executed.
        /// 
        /// If RemoteCallbackInvokeMode is set to Invoke, then this event is executed on the 
        /// thread that owns the browser's underlying window handle.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_render_process_handler_capi.h">cef/include/capi/cef_render_process_handler_capi.h</see>.
        /// </remarks>
        public event CfrOnContextCreatedEventHandler OnV8ContextCreated;

        internal void RaiseOnV8ContextCreated(CfrOnContextCreatedEventArgs e) {
            var eventHandler = OnV8ContextCreated;
            if(eventHandler == null) return;
            if(RemoteCallbacksWillInvoke)
                RenderThreadInvoke(() => eventHandler(this, e));
            else
                eventHandler(this, e);
        }


        /// <summary>
        /// Raised after the CfxBrowser object for this WebBrowser has been created.
        /// The event is executed on the thread that owns this browser control's 
        /// underlying window handle.
        /// </summary>
        public override event BrowserCreatedEventHandler BrowserCreated;

        /// <summary>
        /// Called after a remote browser has been created. When browsing cross-origin a new
        /// browser will be created before the old browser is destroyed.
        /// 
        /// Applications may keep a reference to the CfrBrowser object outside the scope 
        /// of this event, but you have to be aware that those objects become invalid as soon
        /// as the framework swaps render processes and/or recreates browsers.
        /// </summary>
        public event RemoteBrowserCreatedEventHandler RemoteBrowserCreated;


        internal void SetRemoteBrowser(CfrBrowser remoteBrowser, RenderProcess remoteProcess) {
            this.remoteBrowser = remoteBrowser;
            this.remoteProcess = remoteProcess;
            remoteProcess.AddBrowserReference(this);
            var h = RemoteBrowserCreated;
            if(h != null) {
                var e = new RemoteBrowserCreatedEventArgs(remoteBrowser);
                if(RemoteCallbacksWillInvoke && InvokeRequired) {
                    RenderThreadInvoke(() => { h(this, e); });
                } else {
                    h(this, e);
                }
            }
        }

        internal void RemoteProcessExited(RenderProcess process) {
            if(process == this.remoteProcess) {
                this.remoteBrowser = null;
                this.remoteProcess = null;
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
            base.OnResize(e);
            ResizeBrowserWindow();
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
					NativeWindow.SetStyle(browserWindowHandle, WindowStyle.WS_CHILD | WindowStyle.WS_CLIPCHILDREN | WindowStyle.WS_CLIPSIBLINGS | WindowStyle.WS_TABSTOP | WindowStyle.WS_VISIBLE);
					NativeWindow.SetPosition(browserWindowHandle, 0, 0, Width, h);
				}
			} else {
				if(browserWindowHandle != IntPtr.Zero)
					NativeWindow.SetStyle(browserWindowHandle, WindowStyle.WS_CHILD | WindowStyle.WS_CLIPCHILDREN | WindowStyle.WS_CLIPSIBLINGS | WindowStyle.WS_TABSTOP | WindowStyle.WS_DISABLED);
			}
			this.InvokeCallback(()=>this.Update());//.Update ();
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
