using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Chromium;
using Chromium.WebBrowser;

namespace ChromiumFX.Extension
{
    internal class LocalResourceHandler : CfxResourceHandler
    {
        private int readResponseStreamOffset;

        string requestFile = null;

        string requestUrl = null;

        private WebResource webResource;
        private ChromiumWebBrowser browser;
        private byte[] webResourceData;

        private int webResourceLength;
        private string webResourceMimeType;

        private System.Runtime.InteropServices.GCHandle gcHandle;

        internal LocalResourceHandler(ChromiumWebBrowser browser)
        {
            gcHandle = System.Runtime.InteropServices.GCHandle.Alloc(this);


            this.browser = browser;

            this.GetResponseHeaders += LocalResourceHandler_GetResponseHeaders;
            this.ProcessRequest += LocalResourceHandler_ProcessRequest;
            this.ReadResponse += LocalResourceHandler_ReadResponse;
            this.CanGetCookie += (s, e) => e.SetReturnValue(false);
            this.CanSetCookie += (s, e) => e.SetReturnValue(false);

        }

        private void LocalResourceHandler_ProcessRequest(object sender, Chromium.Event.CfxProcessRequestEventArgs e)
        {

            readResponseStreamOffset = 0;
            var request = e.Request;
            var callback = e.Callback;

            // var uri = new Uri(request.Url);

            requestUrl = request.Url;

            var uri = ResourceHelper.ResolveUri(requestUrl);

            var fileName = GetResourceFileName(uri);


            requestFile = request.Url;


            if (System.IO.File.Exists(fileName))
            {
             
               webResource = GetWebResource(uri, fileName);
                browser.SetWebResource(uri.ToString(), webResource);

                Console.WriteLine($"[加载]:\t{requestUrl}\t->\t{fileName}");
            }
            else
            {
                Console.WriteLine($"[未找到]:\t{requestUrl}");
                //callback.Continue();
                //e.SetReturnValue(false);

                webResource = GetNonFoundWebResource(uri, fileName);
                browser.SetWebResource(uri.ToString(), webResource);
            }

          

            callback.Continue();
            e.SetReturnValue(true);

        }

        private static string GetResourceFileName(Uri uri)
        {
            var localPath = uri.LocalPath;

            if (localPath.StartsWith("/"))
                localPath = $".{localPath}";

            var fileName = System.IO.Path.GetFullPath(localPath);
            return fileName;
        }

        private WebResource GetNonFoundWebResource(Uri uri, string fileName)
        {
            webResourceMimeType = MimeHelper.GetMimeType(System.IO.Path.GetExtension(fileName));
            byte[] headBuff;
            byte[] footBuff;
            var nonFoundData = Encoding.UTF8.GetBytes($"Content {uri.ToString()} Not Found!");

            if (ChromiumStartup.EnableMaster && uri.ToString().Contains(ChromiumStartup.SubViewPathName))
            {
                var headFile = GetResourceFileName(new Uri(ChromiumStartup.MasterHeaderFile));
                headBuff = File.ReadAllBytes(headFile);

                var footFile = GetResourceFileName(new Uri(ChromiumStartup.MasterFooterFile));
                footBuff = File.ReadAllBytes(footFile);

                webResourceLength = nonFoundData.Length + headBuff.Length + footBuff.Length;
                webResourceData = new byte[webResourceLength];

                Buffer.BlockCopy(headBuff, 0, webResourceData, 0, headBuff.Length);
                Buffer.BlockCopy(nonFoundData, 0, webResourceData, headBuff.Length, nonFoundData.Length);
                Buffer.BlockCopy(footBuff, 0, webResourceData, headBuff.Length + nonFoundData.Length, footBuff.Length);
            }
            else
            {

                webResourceLength = nonFoundData.Length;
                webResourceData = new byte[nonFoundData.Length];
                ;
                Buffer.BlockCopy(nonFoundData, 0, webResourceData, 0, nonFoundData.Length);
            }

            return new WebResource(webResourceData, webResourceMimeType);
        }

        private WebResource GetWebResource(Uri uri,string fileName)
        {
            byte[] headBuff;
            byte[] footBuff;

            var buff = File.ReadAllBytes(fileName);

            // webResourceLength = buff.Length;
            webResourceMimeType = MimeHelper.GetMimeType(System.IO.Path.GetExtension(fileName));
            // webResourceData = new byte[buff.Length]; ;

            if (ChromiumStartup.EnableMaster && uri.ToString().Contains(ChromiumStartup.SubViewPathName))
            {
              

                var headFile = GetResourceFileName(new Uri(ChromiumStartup.MasterHeaderFile));
                headBuff = File.ReadAllBytes(headFile);

                var footFile = GetResourceFileName(new Uri(ChromiumStartup.MasterFooterFile));
                footBuff = File.ReadAllBytes(footFile);

                webResourceLength = buff.Length + headBuff.Length + footBuff.Length;
                webResourceData = new byte[webResourceLength];

                Buffer.BlockCopy(headBuff, 0, webResourceData, 0, headBuff.Length);
                Buffer.BlockCopy(buff, 0, webResourceData, headBuff.Length, buff.Length);
                Buffer.BlockCopy(footBuff, 0, webResourceData, headBuff.Length + buff.Length, footBuff.Length);
            }
            else
            {
                webResourceLength = buff.Length;
                webResourceData = new byte[buff.Length]; ;
                Buffer.BlockCopy(buff, 0, webResourceData, 0, buff.Length);
            }

            return new WebResource(webResourceData, webResourceMimeType);
        }

        private void LocalResourceHandler_GetResponseHeaders(object sender, Chromium.Event.CfxGetResponseHeadersEventArgs e)
        {

            if (webResource == null)
            {
                e.Response.Status = 404;
            }
            else
            {
                e.ResponseLength = webResourceData.Length;
                e.Response.MimeType = webResourceMimeType;
                e.Response.Status = 200;

                //if (!WebResources.ContainsKey(requestUrl))
                //{
                browser.SetWebResource(requestUrl, webResource);
                //}

            }

        }
         

        private void LocalResourceHandler_ReadResponse(object sender, Chromium.Event.CfxReadResponseEventArgs e)
        {
            int bytesToCopy = webResourceData.Length - readResponseStreamOffset;
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