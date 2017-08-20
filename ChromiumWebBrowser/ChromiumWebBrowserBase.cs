using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Chromium.Event;
using Chromium.WebBrowser.Event;

namespace Chromium.WebBrowser
{
    public abstract class ChromiumWebBrowserBase : Control
    {
        protected string initialUrl;
        protected string m_loadUrlDeferred;
        protected string m_loadStringDeferred;
        public static bool WindowLess { get; set; }


        internal readonly Dictionary<string, JSObject> frameGlobalObjects = new Dictionary<string, JSObject>();
        internal readonly Dictionary<string, WebResource> webResources = new Dictionary<string, WebResource>();

        /// <summary>
        /// Returns the CfxBrowser object for this ChromiumWebBrowser.
        /// Might be null if the browser has not yet been created.
        /// Wait for the BrowserCreated event before accessing this property.
        /// </summary>
        public CfxBrowser Browser { get; internal set; }

        /// <summary>
        /// Returns the CfxBrowserHost object for this ChromiumWebBrowser.
        /// Might be null if the browser has not yet been created.
        /// Wait for the BrowserCreated event before accessing this property.
        /// </summary>
        public CfxBrowserHost BrowserHost { get; internal set; }

        /// <summary>
        /// Returns the URL currently loaded in the main frame.
        /// </summary>
        public Uri Url {
            get {
                if(Browser == null) return null;
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



        internal readonly object browserSyncRoot = new object();

        /// <summary>
        /// Represents the main frame's global javascript object (window).
        /// Any changes to the global object's properties will be available 
        /// after the next time a V8 context is created in the render process
        /// for the main frame of this browser.
        /// </summary>
        public JSObject GlobalObject { get; internal set; }

        /// <summary>
        /// Navigate backwards.
        /// </summary>
        public void GoBack() { if(Browser != null) Browser.GoBack(); }

        /// <summary>
        /// Navigate forwards.
        /// </summary>
        public void GoForward() { if(Browser != null) Browser.GoForward(); }


        /// <summary>
        /// Initialize the ChromiumWebBrowser and ChromiumFX libraries.
        /// The application can change initialization settings by handling
        /// the OnBeforeCfxInitialize event.
        /// </summary>
        public static void Initialize()
        {
            BrowserProcess.Initialize();
        }


        /// <summary>
        /// Provides an opportunity to change initialization settings
        /// and subscribe to browser process handler events.
        /// </summary>
        public static event OnBeforeCfxInitializeEventHandler OnBeforeCfxInitialize;

        internal static void RaiseOnBeforeCfxInitialize(CfxSettings settings, CfxBrowserProcessHandler processHandler)
        {
            var handler = OnBeforeCfxInitialize;
            if (handler != null)
            {
                var e = new OnBeforeCfxInitializeEventArgs(settings, processHandler);
                handler(e);
            }
        }

        /// <summary>
        /// Provides an opportunity to view and/or modify command-line arguments before
        /// processing by CEF and Chromium. The |ProcessType| value will be NULL for
        /// the browser process. Do not keep a reference to the CfxCommandLine
        /// object passed to this function. The CfxSettings.CommandLineArgsDisabled
        /// value can be used to start with an NULL command-line object. Any values
        /// specified in CfxSettings that equate to command-line arguments will be set
        /// before this function is called. Be cautious when using this function to
        /// modify command-line arguments for non-browser processes as this may result
        /// in undefined behavior including crashes.
        /// </summary>
        public static event OnBeforeCommandLineProcessingEventHandler OnBeforeCommandLineProcessing;
        internal static void RaiseOnBeforeCommandLineProcessing(CfxOnBeforeCommandLineProcessingEventArgs e)
        {
            var handler = OnBeforeCommandLineProcessing;
            if (handler != null)
            {
                handler(e);
            }
        }

        /// <summary>
        /// Provides an opportunity to register custom schemes. Do not keep a reference
        /// to the |Registrar| object. This function is called on the main thread for
        /// each process and the registered schemes should be the same across all
        /// processes.
        /// </summary>
        public static event OnRegisterCustomSchemesEventHandler OnRegisterCustomSchemes;
        internal static void RaiseOnRegisterCustomSchemes(CfxOnRegisterCustomSchemesEventArgs e)
        {
            var handler = OnRegisterCustomSchemes;
            if (handler != null)
            {
                handler(e);
            }
        }

        /// <summary>
        /// Load the specified |url| into the main frame.
        /// </summary>
        public void LoadUrl(string url) {
            if(Browser != null)
                Browser.MainFrame.LoadUrl(url);
            else {
                lock(browserSyncRoot) {
                    if(Browser != null) {
                        Browser.MainFrame.LoadUrl(url);
                    } else {
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
        public void LoadString(string stringVal, string url) {
            if(Browser != null) {
                Browser.MainFrame.LoadString(stringVal, url);
            } else {
                lock(browserSyncRoot) {
                    if(Browser != null) {
                        Browser.MainFrame.LoadString(stringVal, url);
                    } else {
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
        public void LoadString(string stringVal) {
            LoadString(stringVal, "about:blank");
        }

        /// <summary>
        /// Execute a string of javascript code in the browser's main frame.
        /// Execution is asynchronous, this function returns immediately.
        /// Returns false if the browser has not yet been created.
        /// </summary>
        public bool ExecuteJavascript(string code) {
            if(Browser != null) {
                Browser.MainFrame.ExecuteJavaScript(code, null, 0);
                return true;
            } else {
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
        public bool ExecuteJavascript(string code, string scriptUrl, int startLine) {
            if(Browser != null) {
                Browser.MainFrame.ExecuteJavaScript(code, scriptUrl, startLine);
                
                return true;
            } else {
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

        /// <summary>
        /// Special Invoke for framework callbacks from the render process.
        /// Maintains the thread within the context of the calling remote thread.
        /// Use this instead of invoke when the following conditions are meat:
        /// 1) The current thread is executing in the scope of a framework
        ///    callback event from the render process (ex. CfrTask.Execute).
        /// 2) You need to Invoke on the webbrowser control and
        /// 3) The invoked code needs to call into the render process.
        /// </summary>
        public void RenderThreadInvoke(MethodInvoker method) {

            if(!CfxRemoteCallContext.IsInContext) {
                throw new ChromiumWebBrowserException("Can't use RenderThreadInvoke without being in the scope of a render process callback.");
            }

            if(!InvokeRequired) {
                method.Invoke();
                return;
            }

            var context = CfxRemoteCallContext.CurrentContext;

            // Use BeginInvoke and Wait instead of Invoke.
            // Invoke marshals exceptions back to the calling thread.
            // We want exceptions to be thrown in place.

            var waitLock = new object();
            lock(waitLock) {
                BeginInvoke((MethodInvoker)(() => {
                    context.Enter();
                    try {
                        method.Invoke();
                    } finally {
                        context.Exit();
                        lock(waitLock) {
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
        public JSObject GlobalObjectForFrame(string frameName) {
            JSObject obj;
            if(!frameGlobalObjects.TryGetValue(frameName, out obj)) {
                obj = new JSObject();
                obj.SetBrowser("window", this);
                frameGlobalObjects.Add(frameName, obj);
            }
            return obj;
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
        public void SetWebResource(string url, WebResource resource) {
            webResources[url] = resource;
        }

        /// <summary>
        /// Remove a resource previously set for the specified URL.
        /// </summary>
        /// <param name="url"></param>
        public void RemoveWebResource(string url) {
            webResources.Remove(url);
        }

        /// <summary>
        /// Raised after the CfxBrowser object for this WebBrowser has been created.
        /// The event is executed on the thread that owns this browser control's 
        /// underlying window handle.
        /// </summary>
        public  event BrowserCreatedEventHandler BrowserCreated;
      
        protected void InvokeCallback(MethodInvoker method) {

            if(!InvokeRequired) {
                method.Invoke();
                return;
            }

            // Use BeginInvoke and Wait instead of Invoke.
            // Invoke marshals exceptions back to the calling thread.
            // We want exceptions to be thrown in place.

            var waitLock = new object();
            lock(waitLock) {
                BeginInvoke((MethodInvoker)(() => {
                    try {
                        method.Invoke();
                    } finally {
                        lock(waitLock) {
                            Monitor.PulseAll(waitLock);
                        }
                    }
                }));
                Monitor.Wait(waitLock);
            }
        }
    }
}