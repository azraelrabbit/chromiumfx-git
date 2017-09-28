using System.Runtime.InteropServices;
using Chromium;
using Chromium.Event;
using Chromium.WebBrowser;

namespace Chromium.WebBrowser
{
    internal class WebResourceHandler : CfxResourceHandler
    {

        private readonly WebResource webResource;
        private int bytesDone;
        private System.Runtime.InteropServices.GCHandle gcHandle;


        internal WebResourceHandler(WebResource webResource)
        {
            gcHandle = System.Runtime.InteropServices.GCHandle.Alloc(this);

           
            this.webResource = webResource;
            this.GetResponseHeaders += new CfxGetResponseHeadersEventHandler(ResourceHandler_GetResponseHeaders);
            this.ProcessRequest += new CfxProcessRequestEventHandler(ResourceHandler_ProcessRequest);
            this.ReadResponse += new CfxReadResponseEventHandler(ResourceHandler_ReadResponse);
        }

        void ResourceHandler_ProcessRequest(object sender, CfxProcessRequestEventArgs e)
        {
            bytesDone = 0;
           
            e.Callback.Continue();
            e.SetReturnValue(true);
        }

        void ResourceHandler_GetResponseHeaders(object sender, CfxGetResponseHeadersEventArgs e)
        {
            e.ResponseLength = e.Response;
            e.Response.MimeType = webResource.mimeType;
            e.Response.Status = 200;
            e.Response.StatusText = "OK";
        }

        void ResourceHandler_ReadResponse(object sender, CfxReadResponseEventArgs e)
        {
            int bytesToCopy = webResource.data.Length - bytesDone;
            if (bytesToCopy > e.BytesToRead)
                bytesToCopy = e.BytesToRead;
            Marshal.Copy(webResource.data, bytesDone, e.DataOut, bytesToCopy);
            e.BytesRead = bytesToCopy;
            bytesDone += bytesToCopy;
            e.SetReturnValue(true);

            if (bytesDone == webResource.data.Length)
            {
                gcHandle.Free();
            }
        }
    }
}