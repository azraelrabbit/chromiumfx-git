using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Chromium.Event;
using Chromium.Remote;
using Chromium.WebBrowser.Event;

namespace Chromium.WebBrowser
{
    public abstract class ChromiumWebBrowserBase : Control
    {
        public static bool WindowLess { get; set; }

        private static CfxBrowserSettings defaultBrowserSettings;
        private static readonly Dictionary<int, WeakReference> browsers = new Dictionary<int, WeakReference>();
        private readonly object browserSyncRoot = new object();
        protected IntPtr browserWindowHandle;
        internal readonly Dictionary<string, JSObject> frameGlobalObjects = new Dictionary<string, JSObject>();
        internal readonly Dictionary<string, WebResource> webResources = new Dictionary<string, WebResource>();
        private string initialUrl;
        private int findId;
        private string currentFindText;
        private bool currentMatchCase;
        private CfxOnLoadingStateChangeEventHandler m_OnLoadingStateChange;
        private CfxOnBeforeContextMenuEventHandler m_OnBeforeContextMenu;
        private CfxOnContextMenuCommandEventHandler m_OnContextMenuCommand;
        private string m_loadUrlDeferred;
        private string m_loadStringDeferred;

        protected Bitmap pixelBuffer;
        protected object pbLock = new object();

        private static bool _mono;

        private CfxRenderHandler renderHandler;

        private BrowserClient client;

        /// <summary>
        /// Creates a ChromiumWebBrowser object with about:blank as initial URL.
        /// The underlying CfxBrowser is created immediately with the
        /// default CfxRequestContext.
        /// </summary>
        public ChromiumWebBrowserBase(Control parent) : this(null, true,parent) { }

        /// <summary>
        /// Creates a ChromiumWebBrowser object with about:blank as initial URL.
        /// If createImmediately is true, then the underlying CfxBrowser is 
        /// created immediately with the default CfxRequestContext.
        /// </summary>
        /// <param name="createImmediately"></param>
        public ChromiumWebBrowserBase(bool createImmediately,Control parent) : this((string) null, createImmediately,parent) { }

        /// <summary>
        /// Creates a ChromiumWebBrowser object with the given initial URL.
        /// The underlying CfxBrowser is created immediately with the
        /// default CfxRequestContext.
        /// </summary>
        public ChromiumWebBrowserBase(string initialUrl,Control parent) : this(initialUrl, (bool) true,parent) { }

        /// <summary>
        /// Creates a ChromiumWebBrowser object with the given initial URL.
        /// If createImmediately is true, then the underlying CfxBrowser is 
        /// created immediately with the default CfxRequestContext.
        /// </summary>
        public ChromiumWebBrowserBase(string initialUrl, bool createImmediately,Control parent) {

            this.Parent = parent;

            if (!WindowLess)
            {
                //ImeMode = ImeMode.On;
                if (BrowserProcess.initialized)
                {
                    //ControlStyles.ContainerControl
                    // | ControlStyles.EnableNotifyMessage
                    // | ControlStyles.UseTextForAccessibility
                    SetStyle(

                        ControlStyles.ResizeRedraw
                        | ControlStyles.FixedWidth
                        | ControlStyles.FixedHeight
                        | ControlStyles.StandardClick
                        | ControlStyles.StandardDoubleClick
                        | ControlStyles.UserMouse
                        | ControlStyles.SupportsTransparentBackColor

                        | ControlStyles.DoubleBuffer
                        | ControlStyles.OptimizedDoubleBuffer

                        | ControlStyles.Opaque
                        , false);

                    //                SetStyle(ControlStyles.UserPaint
                    //                    | ControlStyles.AllPaintingInWmPaint
                    //                    | ControlStyles.CacheText
                    //                    | ControlStyles.Selectable
                    //                    , true);
                    SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                    if (initialUrl == null)
                        this.initialUrl = "about:blank";
                    else
                        this.initialUrl = initialUrl;

                    client = new BrowserClient(this);

                    GlobalObject = new JSObject();
                    GlobalObject.SetBrowser("window", this);

                    if (createImmediately)
                        CreateBrowser();

                }
                else
                {
                    BackColor = System.Drawing.Color.White;
                    Width = 200;
                    Height = 160;
                    var label = new Label();
                    label.AutoSize = true;
                    label.Text = "ChromiumWebBrowser";
                    label.Parent = this;
                }
            }
            else
            {
                _mono = Type.GetType("Mono.Runtime") != null;
                this.Parent = parent;
                SetStyle(ControlStyles.AllPaintingInWmPaint, true);

                Console.WriteLine("can enable ime:" + this.CanEnableIme);

                this.ImeMode = ImeMode.On;

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

         

                client = new BrowserClient(this);
                //client.GetLifeSpanHandler += (sender, e) => e.SetReturnValue(lifeSpanHandler);
                client.GetRenderHandler += (sender, e) => e.SetReturnValue(renderHandler);
                //client.GetLoadHandler += (sender, e) => e.SetReturnValue(loadHandler);
                //client.GetKeyboardHandler += (sender, e) => e.SetReturnValue(keyboardHandler);

                //var settings = new CfxBrowserSettings();

                //var windowInfo = new CfxWindowInfo();

                //windowInfo.SetAsWindowless(this.Handle);
                //windowInfo.SetAsWindowless(this.Parent.Handle);


                if (initialUrl == null)
                    this.initialUrl = "about:blank";
                else
                    this.initialUrl = initialUrl;

                client = new BrowserClient(this);

                GlobalObject = new JSObject();
                GlobalObject.SetBrowser("window", this);

                if (createImmediately)
                    CreateBrowser();


               

                //// Create handle now for InvokeRequired to work properly 
                //// CreateHandle();
                //CfxBrowserHost.CreateBrowser(windowInfo, client, "about:blank", settings, null);
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
        /// The CfxBrowserProcessHandler for this browser process.
        /// Do not access this property before calling ChromiumWebBrowser.Initialize()
        /// </summary>
        public static CfxBrowserProcessHandler BrowserProcessHandler {
            get {
                return BrowserProcess.processHandler;
            }
        }

        /// <summary>
        /// Returns the CfxBrowser object for this ChromiumWebBrowser.
        /// Might be null if the browser has not yet been created.
        /// Wait for the BrowserCreated event before accessing this property.
        /// </summary>
        public CfxBrowser Browser { get; private set; }

        /// <summary>
        /// Returns the CfxBrowserHost object for this ChromiumWebBrowser.
        /// Might be null if the browser has not yet been created.
        /// Wait for the BrowserCreated event before accessing this property.
        /// </summary>
        public CfxBrowserHost BrowserHost { get; private set; }

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

        /// <summary>
        /// Returns the URL currently loaded in the main frame.
        /// </summary>
        public System.Uri Url {
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

        /// <summary>
        /// Represents the main frame's global javascript object (window).
        /// Any changes to the global object's properties will be available 
        /// after the next time a V8 context is created in the render process
        /// for the main frame of this browser.
        /// </summary>
        public JSObject GlobalObject { get; private set; }

        /// <summary>
        /// Provides an opportunity to change initialization settings
        /// and subscribe to browser process handler events.
        /// </summary>
        public   static event OnBeforeCfxInitializeEventHandler OnBeforeCfxInitialize;

        internal static void RaiseOnBeforeCfxInitialize(CfxSettings settings, CfxBrowserProcessHandler processHandler) {
            var handler = ChromiumWebBrowser.OnBeforeCfxInitialize;
            if(handler != null) {
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
        public   static event OnBeforeCommandLineProcessingEventHandler OnBeforeCommandLineProcessing;

        internal static void RaiseOnBeforeCommandLineProcessing(CfxOnBeforeCommandLineProcessingEventArgs e) {
            var handler = ChromiumWebBrowser.OnBeforeCommandLineProcessing;
            if(handler != null) {
                handler(e);
            }
        }

        /// <summary>
        /// Provides an opportunity to register custom schemes. Do not keep a reference
        /// to the |Registrar| object. This function is called on the main thread for
        /// each process and the registered schemes should be the same across all
        /// processes.
        /// </summary>
        public   static event OnRegisterCustomSchemesEventHandler OnRegisterCustomSchemes;

        internal static void RaiseOnRegisterCustomSchemes(CfxOnRegisterCustomSchemesEventArgs e) {
            var handler = ChromiumWebBrowser.OnRegisterCustomSchemes;
            if(handler != null) {
                handler(e);
            }
        }

        /// <summary>
        /// Initialize the ChromiumWebBrowser and ChromiumFX libraries.
        /// The application can change initialization settings by handling
        /// the OnBeforeCfxInitialize event.
        /// </summary>
        public static void Initialize() {
            BrowserProcess.Initialize();
        }

        /// <summary>
        /// This function should be called on the main application thread to shut down
        /// the CEF browser process before the application exits.
        /// </summary>
        public static void Shutdown() {
            CfxRuntime.Shutdown();
        }

        /// <summary>
        /// Returns the ChromiumWebBrowser object associated with  the given CfxBrowser, or null
        /// if the CfxBrowser is not associated with any ChromiumWebBrowser object.
        /// </summary>
        public static ChromiumWebBrowserBase FromCfxBrowser(CfxBrowser cfxBrowser) {
            if(cfxBrowser == null) throw new ArgumentNullException("cfxBrowser");
            return GetBrowser(cfxBrowser.Identifier);
        }

        internal static ChromiumWebBrowserBase GetBrowser(int id) {
            lock(browsers) {
                WeakReference r;
                if(browsers.TryGetValue(id, out r)) {
                    return (ChromiumWebBrowserBase)r.Target;
                }
                return null;
            }
        }

        private static void AddToBrowserCache(ChromiumWebBrowserBase wb) {
            lock(browsers) {
                var deadRefs = new List<int>(browsers.Count);
                foreach(var b in browsers) {
                    if(!b.Value.IsAlive) deadRefs.Add(b.Key);
                }
                foreach(var r in deadRefs) {
                    browsers.Remove(r);
                }
                browsers[wb.Browser.Identifier] = new WeakReference(wb);
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

            var rect = this.ClientRectangle;

//			if (this.Parent != null) {
//				parentHandle = this.Parent.Handle;
//
//			}

            var windowInfo = new CfxWindowInfo();

            //this.ImeMode = ImeMode.Inherit;
            if (WindowLess) {
				
                // in order to avoid focus issues when creating browsers offscreen,
                // the browser must be created with a disabled child window.
                windowInfo.SetAsWindowless(parentHandle);
                //windowInfo.SetAsChild ();
            } else {
                windowInfo.SetAsChild (parentHandle,rect.Left,rect.Top,rect.Width,rect.Height);
                //windowInfo.Style = WindowStyle.WS_CHILD;
            }

            if(!CfxBrowserHost.CreateBrowser(windowInfo, client, initialUrl, DefaultBrowserSettings, requestContext))
                throw new ChromiumWebBrowserException("Failed to create browser instance.");
        }

        /// <summary>
        /// Navigate backwards.
        /// </summary>
        public void GoBack() { if(Browser != null) Browser.GoBack(); }

        /// <summary>
        /// Navigate forwards.
        /// </summary>
        public void GoForward() { if(Browser != null) Browser.GoForward(); }

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
        public virtual event BrowserCreatedEventHandler BrowserCreated;

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

        private void RaiseOnContextMenuCommand(object sender, CfxOnContextMenuCommandEventArgs e) {
            var handler = m_OnContextMenuCommand;
            if(handler != null) {
                InvokeCallback(() => { handler(this, e); });
            }
        }

        internal void OnBrowserCreated(CfxOnAfterCreatedEventArgs e) {

            Browser = e.Browser;
            BrowserHost = Browser.Host;
            browserWindowHandle = BrowserHost.WindowHandle;
            AddToBrowserCache(this);
           
            OnResize(null);

            var handler = BrowserCreated;
            if(handler != null) {
                var e1 = new BrowserCreatedEventArgs(e.Browser);
                handler(this, e1);
            }

            System.Threading.ThreadPool.QueueUserWorkItem(AfterSetBrowserTasks);
        }

        private void AfterSetBrowserTasks(object state) {
            lock(browserSyncRoot) {
                if(m_loadUrlDeferred != null) {
                    if(m_loadStringDeferred != null) {
                        Browser.MainFrame.LoadString(m_loadStringDeferred, m_loadUrlDeferred);
                    } else {
                        Browser.MainFrame.LoadUrl(m_loadUrlDeferred);
                    }
                }
            }
        }
    }
}