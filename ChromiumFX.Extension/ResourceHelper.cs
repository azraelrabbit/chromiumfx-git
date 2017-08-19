using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chromium.WebBrowser;

namespace ChromiumFX.Extension
{
    public static class ResourceHelper
    {
        public static Uri ResolveUri(string url)
        {
            var uri = new Uri(url, UriKind.RelativeOrAbsolute);

            if (url.Contains("~/"))
            {
                var newUri = string.Empty;
                var rootPath = uri.Scheme + "://" + uri.Host;
                if (!string.IsNullOrEmpty(ChromiumStartup.VirtualPath))
                {
                    newUri = rootPath + "/" + ChromiumStartup.VirtualPath + "/";
                }
                var subPath = url.Substring(url.LastIndexOf("~/"));

                newUri += subPath.Replace("~/", string.Empty);
                uri = new Uri(newUri, UriKind.RelativeOrAbsolute);
            }
            if (!string.IsNullOrEmpty(ChromiumStartup.VirtualPath))
            {
                var tmpUrl = uri.ToString();
                if (!tmpUrl.Contains(ChromiumStartup.VirtualPath))
                {
                    var newUri = string.Empty;
                    var rootPath = uri.Scheme + "://" + uri.Host;
                    if (!string.IsNullOrEmpty(ChromiumStartup.VirtualPath))
                    {
                        newUri = rootPath + "/" + ChromiumStartup.VirtualPath;
                    }

                    newUri += uri.PathAndQuery;
                    uri = new Uri(newUri, UriKind.RelativeOrAbsolute);
                }
            }
            return uri;
        }
    }
}
