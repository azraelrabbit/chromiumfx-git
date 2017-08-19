using System;
using System.Drawing;
using System.Drawing.Imaging;
using Chromium;

namespace ChromiumFX.Extension
{
    /// <summary>
    /// Custom web resource for registration with a
    /// ChromiumWebBrowser control.
    /// </summary>
    public class WebResource
    {

        internal readonly byte[] data;
        internal readonly string mimeType;

        /// <summary>
        /// Creates a WebResource for registration with a
        /// ChromiumWebBrowser control.
        /// </summary>
        public WebResource(byte[] data, string mimeType)
        {
            this.data = data;
            this.mimeType = mimeType;
        }

        /// <summary>
        /// Creates a WebResource from the given image
        /// for registration with a ChromiumWebBrowser control.
        /// The mime type will be image/png 
        /// </summary>
        /// <param name="image"></param>
        public WebResource(Image image)
        {
            mimeType = "image/png";
            var pngData = new System.IO.MemoryStream();
            image.Save(pngData, ImageFormat.Png);
            data = pngData.ToArray();
        }

        /// <summary>
        /// Creates a WebResource from the given image
        /// for registration with a ChromiumWebBrowser control.
        /// The mime type will be set according to the image format.
        /// </summary>
        public WebResource(Image image, ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (var c in codecs)
            {
                if (c.FormatID == format.Guid)
                {
                    mimeType = c.MimeType;
                    var imgData = new System.IO.MemoryStream();
                    image.Save(imgData, format);
                    data = imgData.ToArray();
                    return;
                }
            }
            throw new Exception("No mime type for the given image format.");
        }

        /// <summary>
        /// Creates a WebResource from the given text
        /// for registration with a ChromiumWebBrowser control.
        /// The mime type will be text/html
        /// </summary>
        /// <param name="html"></param>
        public WebResource(string html)
        {
            mimeType = "text/html";
            data = System.Text.Encoding.UTF8.GetBytes(html);
        }

        public CfxResourceHandler GetResourceHandler()
        {
            return new WebResourceHandler(this);
        }
    }
}