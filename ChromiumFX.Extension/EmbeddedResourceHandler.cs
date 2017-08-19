using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Chromium;
using Chromium.WebBrowser;

namespace ChromiumFX.Extension
{
	internal class EmbeddedResourceHandler : CfxResourceHandler
	{

		private int readResponseStreamOffset;
		private Assembly resourceAssembly;

		//string requestFile = null;

		string requestUrl = null;

		private WebResource webResource;

	    private byte[] webResourceData;

	    private int webResourceLength;
	    private string webResourceMimeType;

		private ChromiumWebBrowser browser;




	    //public static Dictionary<string, WebResource> WebResources
	    //{
	    //    get
	    //    {
	    //        return webResources;
	    //    }
	    //}
	    //internal static readonly Dictionary<string, WebResource> webResources = new Dictionary<string, WebResource>();

        private System.Runtime.InteropServices.GCHandle gcHandle;

		private string domain = null;

		internal EmbeddedResourceHandler(Assembly resourceAssembly, ChromiumWebBrowser browser, string domain = null)
		{
			gcHandle = System.Runtime.InteropServices.GCHandle.Alloc(this);
			this.domain = domain;

			this.browser = browser;
		    //if (!ChromiumStartup.BrowserDict.ContainsKey(browser.Browser.Identifier))
		    //{
		    //    ChromiumStartup.BrowserDict.Add(browser.Browser.Identifier,browser);
		    //}

			this.resourceAssembly = resourceAssembly;
			this.GetResponseHeaders += EmbeddedResourceHandler_GetResponseHeaders;
			this.ProcessRequest += EmbeddedResourceHandler_ProcessRequest;
			this.ReadResponse += EmbeddedResourceHandler_ReadResponse;
			this.CanGetCookie += (s, e) => e.SetReturnValue(false);
			this.CanSetCookie += (s, e) => e.SetReturnValue(false);
		}


		private void EmbeddedResourceHandler_ProcessRequest(object sender, Chromium.Event.CfxProcessRequestEventArgs e)
		{

			readResponseStreamOffset = 0;
			var request = e.Request;
			var callback = e.Callback;

			//var uri = new Uri(request.Url);

		    var refurl = request.ReferrerUrl;

			requestUrl = request.Url;

		    Console.WriteLine("requrl: " + requestUrl);
		    Console.WriteLine("reffer url : " + refurl);

		    var uri = ResourceHelper.ResolveUri(requestUrl);
 
            string resourceMimeType;
		    string resourceName;
		    var ass = GetResourceInfo(uri, out resourceMimeType, out resourceName);

            if (!string.IsNullOrEmpty(resourceName) && ass.GetManifestResourceInfo(resourceName) != null)
			{
			    webResource=GetWebResource(uri, ass, resourceName, resourceMimeType);
			    browser.SetWebResource(uri.ToString(), webResource);


                Console.WriteLine($"[加载]:\t{requestUrl}");


				callback.Continue();
				e.SetReturnValue(true);
			}
			else
			{
			    webResource = GetNonFoundWebResource(uri, resourceMimeType);
			    browser.SetWebResource(uri.ToString(), webResource);


                Console.WriteLine($"[未找到]:\t{requestUrl}");

                //callback.Continue();
                //e.SetReturnValue(false);
			    callback.Continue();
			    e.SetReturnValue(true);

            }




		}
	    private WebResource GetNonFoundWebResource(Uri uri, string resourceMimeType)
	    {
	        webResourceMimeType = resourceMimeType;


            var nonFoundData = Encoding.UTF8.GetBytes($"Content {uri.ToString()} Not Found!");
	        return new WebResource(nonFoundData, webResourceMimeType);
	    }
        private WebResource GetWebResource(Uri uri,Assembly ass, string resourceName, string resourceMimeType)
	    {
	        byte[] headBuff;
	        byte[] footBuff;

	        var buff = GetResourceBytes(ass, resourceName);

	       // webResourceLength = buff.Length;
	        webResourceMimeType = resourceMimeType;
           // webResourceData = new byte[buff.Length]; ;

	        if (ChromiumStartup.EnableMaster && uri.ToString().Contains(ChromiumStartup.SubViewPathName))
	        {
	            string headMimeType;
	            string headResourceName;

	            var headAss = GetResourceInfo(new Uri(ChromiumStartup.MasterHeaderFile), out headMimeType, out headResourceName);

	            headBuff = GetResourceBytes(headAss, headResourceName);

	            string footMimeType;
	            string footResourceName;

	            var footAss = GetResourceInfo(new Uri(ChromiumStartup.MasterFooterFile),out footMimeType, out footResourceName);

	            footBuff = GetResourceBytes(footAss, footResourceName);

	            webResourceLength = buff.Length + headBuff.Length + footBuff.Length;
	            webResourceData = new byte[webResourceLength];

                Buffer.BlockCopy(headBuff,0,webResourceData,0,headBuff.Length);
                Buffer.BlockCopy(buff,0,webResourceData,headBuff.Length,buff.Length);
                Buffer.BlockCopy(footBuff,0,webResourceData,headBuff.Length+buff.Length,footBuff.Length);
	        }
	        else
	        {
	            webResourceLength = buff.Length;
	            webResourceData = new byte[buff.Length]; ;
	            Buffer.BlockCopy(buff, 0, webResourceData, 0, buff.Length);
            }

	        return new WebResource(webResourceData, webResourceMimeType);
	    }

	    private static byte[] GetResourceBytes(Assembly ass, string resourceName)
	    {
	        byte[] buff;
	        using (var reader = new System.IO.BinaryReader(ass.GetManifestResourceStream(resourceName)))
	        {
	            buff = reader.ReadBytes((int) reader.BaseStream.Length);


	            reader.Close();

	            //if (WebResources.ContainsKey(requestUrl))
	            //{
	            //SetWebResource(requestUrl, webResource);
	            //}
	        }
	        return buff;
	    }

	    private Assembly GetResourceInfo(Uri uri ,out string resourceMimeType, out string resourceName)
	    {
            var fileName = string.IsNullOrEmpty(domain)
	            ? string.Format("{0}{1}", uri.Authority, uri.AbsolutePath)
	            : uri.AbsolutePath;

	        //requestFile = uri.LocalPath;
	        if (fileName.StartsWith("/") && fileName.Length > 1)
	        {
	            fileName = fileName.Substring(1);
	        }

	        var ass = resourceAssembly;
	        var endTrimIndex = fileName.LastIndexOf('/');

	        if (endTrimIndex > -1)
	        {
	            var tmp = fileName.Substring(0, endTrimIndex);
	            tmp = tmp.Replace("-", "_");

	            fileName = string.Format("{0}{1}", tmp, fileName.Substring(endTrimIndex));
	        }

	        var resourcePath = string.Format("{0}.{1}", ass.GetName().Name, fileName.Replace('/', '.'));

	        resourceMimeType = MimeHelper.GetMimeType(System.IO.Path.GetExtension(fileName));

	        resourceName = ass.GetManifestResourceNames()
	            .SingleOrDefault(p => p.Equals(resourcePath, StringComparison.CurrentCultureIgnoreCase));
	        return ass;
	    }

	 
	    //public void SetWebResource(string url, WebResource resource)
	    //{
	    //    //if (!WebResources.ContainsKey(requestUrl))
	    //    //{
	    //    //    webResources.Add(url, resource);
	    //    //}

	    //}
        private void EmbeddedResourceHandler_GetResponseHeaders(object sender, Chromium.Event.CfxGetResponseHeadersEventArgs e)
		{ 
			if (webResource == null)
			{
				e.Response.Status = 404;
			}
			else
			{

                e.ResponseLength = webResourceLength;
                e.Response.MimeType = webResourceMimeType;
                e.Response.Status = 200;

				//if (!browser.WebResources.ContainsKey(requestUrl))
				//{
					 //SetWebResource(requestUrl, webResource);
				//}

			}

		}


		private void EmbeddedResourceHandler_ReadResponse(object sender, Chromium.Event.CfxReadResponseEventArgs e)
		{
			int bytesToCopy = webResourceLength - readResponseStreamOffset;
			if (bytesToCopy > e.BytesToRead)
				bytesToCopy = e.BytesToRead;
			System.Runtime.InteropServices.Marshal.Copy(webResourceData, readResponseStreamOffset, e.DataOut, bytesToCopy);
			e.BytesRead = bytesToCopy;
			readResponseStreamOffset += bytesToCopy;
			e.SetReturnValue(true);


			if (readResponseStreamOffset == webResourceData.Length)
			{
				gcHandle.Free();
				Console.WriteLine($"[完成]:\t{requestUrl}");
			}
		}
	}
}
