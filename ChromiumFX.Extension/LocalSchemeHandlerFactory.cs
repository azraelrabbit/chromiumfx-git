using Chromium;
using Chromium.WebBrowser;

namespace ChromiumFX.Extension
{
    internal class LocalSchemeHandlerFactory : CfxSchemeHandlerFactory
    {
        private string _schemaName;
        internal LocalSchemeHandlerFactory(string schemaName="local")
        {
            this.Create += LocalSchemeHandlerFactory_Create;
            _schemaName = schemaName;
        }

        private void LocalSchemeHandlerFactory_Create(object sender, Chromium.Event.CfxSchemeHandlerFactoryCreateEventArgs e)
        {
            if (e.SchemeName.Equals(_schemaName.Trim()) && e.Browser != null)
            {
                var browser = ChromiumWebBrowserBase.FromCfxBrowser(e.Browser);
                var handler = new LocalResourceHandler(browser);
                e.SetReturnValue(handler);
            }
        }
    }
}
