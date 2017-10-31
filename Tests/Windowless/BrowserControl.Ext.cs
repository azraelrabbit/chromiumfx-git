using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Chromium;
 

namespace Windowless
{
    public partial class BrowserControl
    { 
        /// <summary>
        /// Represents the main frame's global javascript object (window).
        /// Any changes to the global object's properties will be available 
        /// after the next time a V8 context is created in the render process
        /// for the main frame of this browser.
        /// </summary>
        public JSObject GlobalObject { get; private set; }

        internal readonly Dictionary<string, JSObject> frameGlobalObjects = new Dictionary<string, JSObject>();
        internal readonly Dictionary<string, WebResource> webResources = new Dictionary<string, WebResource>();

        /// <summary>
        /// Returns the URL currently loaded in the main frame.
        /// </summary>
        public System.Uri Url
        {
            get
            {
                if (Browser == null) return null;
                Uri retval;
                Uri.TryCreate(Browser.MainFrame.Url, UriKind.RelativeOrAbsolute, out retval);
                return retval;
            }
        }

        /// <summary>
        /// Returns true if the browser is currently loading.
        /// </summary>
        public bool IsLoading { get { return Browser == null ? false : Browser.IsLoading; } }

        /// <summary>
        /// Returns true if the browser can navigate backwards.
        /// </summary>
        public bool CanGoBack { get { return Browser == null ? false : Browser.CanGoBack; } }

        /// <summary>
        /// Returns true if the browser can navigate forwards.
        /// </summary>
        public bool CanGoForward { get { return Browser == null ? false : Browser.CanGoForward; } }

        /// <summary>
        /// Navigate backwards.
        /// </summary>
        public void GoBack() { if (Browser != null) Browser.GoBack(); }

        /// <summary>
        /// Navigate forwards.
        /// </summary>
        public void GoForward() { if (Browser != null) Browser.GoForward(); }



        private string m_loadUrlDeferred;
        private string m_loadStringDeferred;


        /// <summary>
        /// Load the specified |url| into the main frame.
        /// </summary>
        public void LoadUrl(string url)
        {
            if (Browser != null)
                Browser.MainFrame.LoadUrl(url);
            else
            {
                lock (browserSyncRoot)
                {
                    if (Browser != null)
                    {
                        Browser.MainFrame.LoadUrl(url);
                    }
                    else
                    {
                        m_loadUrlDeferred = url;
                    }
                }
            }

            OnResize(null);
        }

        /// <summary>
        /// Load the contents of |stringVal| with the specified dummy |url|. |url|
        /// should have a standard scheme (for example, http scheme) or behaviors like
        /// link clicks and web security restrictions may not behave as expected.
        /// </summary>
        public void LoadString(string stringVal, string url)
        {
            if (Browser != null)
            {
                Browser.MainFrame.LoadString(stringVal, url);
            }
            else
            {
                lock (browserSyncRoot)
                {
                    if (Browser != null)
                    {
                        Browser.MainFrame.LoadString(stringVal, url);
                    }
                    else
                    {
                        m_loadUrlDeferred = url;
                        m_loadStringDeferred = stringVal;
                    }
                }
            }
     
            OnResize(null);
        }

        /// <summary>
        /// Load the contents of |stringVal| with dummy url about:blank.
        /// </summary>
        public void LoadString(string stringVal)
        {
            LoadString(stringVal, "about:blank");
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
        public int Find(string searchText, bool forward, bool matchCase)
        {
            if (BrowserHost == null)
                return -1;
            var findNext = currentFindText == searchText && currentMatchCase == matchCase;
            if (!findNext)
            {
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
        public int Find(string searchText, bool forward)
        {
            return Find(searchText, forward, false);
        }

        /// <summary>
        /// Search for |searchText|. The search will be forward and case-insensitive.
        /// Returns the identifier for this find operation (see also CfxFindHandler),
        /// or -1 if the browser has not yet been created.
        /// </summary>
        public int Find(string searchText)
        {
            return Find(searchText, true, false);
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
        public bool RemoteCallbacksWillInvoke
        {
            get
            {
                return RemoteCallbackInvokeMode != JSInvokeMode.DontInvoke;
            }
        }



        /// <summary>
        /// Execute a string of javascript code in the browser's main frame.
        /// Execution is asynchronous, this function returns immediately.
        /// Returns false if the browser has not yet been created.
        /// </summary>
        public bool ExecuteJavascript(string code)
        {
            if (Browser != null)
            {
                Browser.MainFrame.ExecuteJavaScript(code, null, 0);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Execute a string of javascript code in the browser's main frame. The |scriptUrl|
        /// parameter is the URL where the script in question can be found, if any. The
        /// renderer may request this URL to show the developer the source of the
        /// error.  The |startLine| parameter is the base line number to use for error
        /// reporting.
        /// Execution is asynchronous, this function returns immediately.
        /// Returns false if the browser has not yet been created.
        /// </summary>
        public bool ExecuteJavascript(string code, string scriptUrl, int startLine)
        {
            if (Browser != null)
            {
                Browser.MainFrame.ExecuteJavaScript(code, scriptUrl, startLine);
                return true;
            }
            else
            {
                return false;
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
        public Object RenderThreadInvoke(Delegate method, params Object[] args)
        {

            if (!CfxRemoteCallContext.IsInContext)
            {
                throw new ChromiumWebBrowserException("Can't use RenderThreadInvoke without being in the scope of a render process callback.");
            }

            if (!InvokeRequired)
                return method.DynamicInvoke(args);

            object retval = null;
            var context = CfxRemoteCallContext.CurrentContext;

            // Use BeginInvoke and Wait instead of Invoke.
            // Invoke marshals exceptions back to the calling thread.
            // We want exceptions to be thrown in place.

            var waitLock = new object();
            lock (waitLock)
            {
                BeginInvoke((MethodInvoker)(() => {
                    context.Enter();
                    try
                    {
                        retval = method.DynamicInvoke(args);
                    }
                    finally
                    {
                        context.Exit();
                        lock (waitLock)
                        {
                            Monitor.PulseAll(waitLock);
                        }
                    }
                }));
                Monitor.Wait(waitLock);
            }
            return retval;
        }


        /// <summary>
        /// Special Invoke for framework callbacks from the render process.
        /// Maintains the thread within the context of the calling remote thread.
        /// Use this instead of invoke when the following conditions are meat:
        /// 1) The current thread is executing in the scope of a framework
        ///    callback event from the render process (ex. CfrTask.Execute).
        /// 2) You need to Invoke on the webbrowser control and
        /// 3) The invoked code needs to call into the render process.
        /// </summary>
        public void RenderThreadInvoke(MethodInvoker method)
        {

            if (!CfxRemoteCallContext.IsInContext)
            {
                throw new ChromiumWebBrowserException("Can't use RenderThreadInvoke without being in the scope of a render process callback.");
            }

            if (!InvokeRequired)
            {
                method.Invoke();
                return;
            }

            var context = CfxRemoteCallContext.CurrentContext;

            // Use BeginInvoke and Wait instead of Invoke.
            // Invoke marshals exceptions back to the calling thread.
            // We want exceptions to be thrown in place.

            var waitLock = new object();
            lock (waitLock)
            {
                BeginInvoke((MethodInvoker)(() => {
                    context.Enter();
                    try
                    {
                        method.Invoke();
                    }
                    finally
                    {
                        context.Exit();
                        lock (waitLock)
                        {
                            Monitor.PulseAll(waitLock);
                        }
                    }
                }));
                Monitor.Wait(waitLock);
            }
        }


        /// <summary>
        /// Represents a named frame's global javascript object (window).
        /// Any changes to the global object's properties will be available 
        /// after the next time a V8 context is created in the render process
        /// of this browser for a frame with this name.
        /// </summary>
        public JSObject GlobalObjectForFrame(string frameName)
        {
            JSObject obj;
            if (!frameGlobalObjects.TryGetValue(frameName, out obj))
            {
                obj = new JSObject();
                obj.SetBrowser("window", this);
                frameGlobalObjects.Add(frameName, obj);
            }
            return obj;
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
        public bool EvaluateJavascript(string code, Action<CfxV8Value, CfxV8Exception> callback)
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
        public bool EvaluateJavascript(string code, JSInvokeMode invokeMode, Action<CfxV8Value, CfxV8Exception> callback)
        {
            var rb = Browser;
            if (rb == null) return false;
            try
            {
                var ctx = rb.MainFrame.V8Context;
                ctx.Enter();
                try
                {
                    var taskRunner = CfxTaskRunner.GetForThread(CfxThreadId.Renderer);
                    var task = new EvaluateTask(this, code, invokeMode, callback);
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

        private class EvaluateTask : CfxTask
        {

            BrowserControl wb;
            string code;
            JSInvokeMode invokeMode;
            Action<CfxV8Value, CfxV8Exception> callback;

            internal EvaluateTask(BrowserControl wb, string code, JSInvokeMode invokeMode, Action<CfxV8Value, CfxV8Exception> callback)
            {
                this.wb = wb;
                this.code = code;
                this.invokeMode = invokeMode;
                this.callback = callback;
                Execute += (s, e) => {
                    if (invokeMode == JSInvokeMode.Invoke || (invokeMode == JSInvokeMode.Inherit && wb.RemoteCallbacksWillInvoke))
                        wb.RenderThreadInvoke(() => Task_Execute(e));
                    else
                        Task_Execute(e);
                };
            }

            void Task_Execute(CfxEventArgs e)
            {
                CfxV8Context context;
                CfxV8Value retval;
                CfxV8Exception ex;
                bool result = false;
                try
                {
                    context = wb.Browser.MainFrame.V8Context;
                    result = context.Eval(code, null, 0, out retval, out ex);
                }
                catch
                {
                    callback(null, null);
                    return;
                }
                context.Enter();
                try
                {
                    if (result)
                    {
                        callback(retval, null);
                    }
                    else
                    {
                        callback(null, ex);
                    }
                }
                finally
                {
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
        public bool VisitDom(Action<CfxDomDocument, CfxBrowser> callback)
        {
            var rb = Browser;
            if (rb == null) return false;
            try
            {
                var ctx = rb.MainFrame.V8Context;
                ctx.Enter();
                try
                {
                    var taskRunner = CfxTaskRunner.GetForThread(CfxThreadId.Renderer);
                    var task = new VisitDomTask(this, callback);
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

        private class VisitDomTask : CfxTask
        {

            BrowserControl wb;
            Action<CfxDomDocument, CfxBrowser> callback;
            CfxDomVisitor visitor;

            internal VisitDomTask(BrowserControl wb, Action<CfxDomDocument, CfxBrowser> callback)
            {
                this.wb = wb;
                this.callback = callback;
                this.Execute += Task_Execute;
                visitor = new CfxDomVisitor();
                visitor.Visit += (s, e) => {
                    if (wb.RemoteCallbacksWillInvoke)
                        wb.RenderThreadInvoke((MethodInvoker)(() => { callback(e.Document, wb.Browser); }));
                    else
                        callback(e.Document, wb.Browser);
                };
            }

            void Task_Execute(object sender, CfxEventArgs e)
            {
                wb.Browser.MainFrame.VisitDom(visitor);
            }
        }

        /// <summary>
        /// Set a resource to be used for the specified URL.
        /// Note that these resources are kept in the memory.
        /// If you need to handle a lot of custom web resources,
        /// subscribing to RequestHandler.GetResourceHandler
        /// and loading from disk on demand
        /// might be a better choice.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="resource"></param>
        public void SetWebResource(string url, WebResource resource)
        {
            webResources[url] = resource;
        }

        /// <summary>
        /// Remove a resource previously set for the specified URL.
        /// </summary>
        /// <param name="url"></param>
        public void RemoveWebResource(string url)
        {
            webResources.Remove(url);
        }


        private void InvokeCallback(MethodInvoker method)
        {

            if (!InvokeRequired)
            {
                method.Invoke();
                return;
            }

            // Use BeginInvoke and Wait instead of Invoke.
            // Invoke marshals exceptions back to the calling thread.
            // We want exceptions to be thrown in place.

            var waitLock = new object();
            lock (waitLock)
            {
                BeginInvoke((MethodInvoker)(() => {
                    try
                    {
                        method.Invoke();
                    }
                    finally
                    {
                        lock (waitLock)
                        {
                            Monitor.PulseAll(waitLock);
                        }
                    }
                }));
                Monitor.Wait(waitLock);
            }
        }

    }
}
