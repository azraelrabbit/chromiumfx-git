using Chromium;
using Chromium.WebBrowser;

namespace ChromiumFX.Extension
{
    internal class LocalSchemeHandlerFactory : CfxSchemeHandlerFactory
    {
        internal LocalSchemeHandlerFactory()
        {
            this.Create += LocalSchemeHandlerFactory_Create;
        }

        private void LocalSchemeHandlerFactory_Create(object sender, Chromium.Event.CfxSchemeHandlerFactoryCreateEventArgs e)
        {
            if (e.SchemeName.Equals("local") && e.Browser != null)
            {
                var browser = ChromiumWebBrowser.FromCfxBrowser(e.Browser);
                var handler = new LocalResourceHandler(browser);
                e.SetReturnValue(handler);
            }
        }
    }
}
