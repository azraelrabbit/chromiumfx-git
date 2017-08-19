using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chromium.Remote;
using Chromium.Remote.Event;
using Chromium.WebBrowser;
using Chromium.WebBrowser.Event;
using RemoteBrowserCreatedEventHandler = Chromium.WebBrowser.RemoteBrowserCreatedEventHandler;

namespace ChromiumFX.Extension
{
    public class ChromiumBrowserEx:ChromiumWebBrowser
    {
        internal RenderProcess remoteProcess;
        internal CfrBrowser remoteBrowser;

        public ChromiumBrowserEx(string initialUrl) : base(initialUrl)
        {
        }

        public event RemoteBrowserCreatedEventHandler RemoteBrowserCreated;
        public void SetRemoteBrowser(CfrBrowser remoteBrowser, RenderProcess remoteProcess)
        {
            this.remoteBrowser = remoteBrowser;
            this.remoteProcess = remoteProcess;
            remoteProcess.OnExit += new Action<RenderProcess>(remoteProcess_OnExit);
            var h = RemoteBrowserCreated;
            if (h != null)
            {
                var e = new Chromium.WebBrowser.CSRemoteBrowserCreatedEventArgs(remoteBrowser);
                if (RemoteCallbacksWillInvoke && InvokeRequired)
                {
                    RenderThreadInvoke(() => { h(this, e); });
                }
                else
                {
                    h(this, e);
                }
            }

            base.BrowserCreated += ChromiumBrowserEx_BrowserCreated;
        }

        private void ChromiumBrowserEx_BrowserCreated(object sender, BrowserCreatedEventArgs e)
        {
            var browserId = e.Browser.Identifier;
            ChromiumStartup.CurrentBrowsers.Add(browserId,this);
        }

        void remoteProcess_OnExit(RenderProcess process)
        {
            if (process == this.remoteProcess)
            {
                this.remoteBrowser = null;
                this.remoteProcess = null;
            }
        }

        public event CfrOnContextCreatedEventHandler OnV8ContextCreated;

        public void RaiseOnV8ContextCreated(CfrOnContextCreatedEventArgs e)
        {
            var eventHandler = OnV8ContextCreated;
            if (eventHandler == null) return;
            if (RemoteCallbacksWillInvoke)
                RenderThreadInvoke(() => eventHandler(this, e));
            else
                eventHandler(this, e);
        }

    }
}
