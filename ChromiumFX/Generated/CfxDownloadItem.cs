// Copyright (c) 2014-2017 Wolfgang Borgsmüller
// All rights reserved.
// 
// This software may be modified and distributed under the terms
// of the BSD license. See the License.txt file for details.

// Generated file. Do not edit.


using System;

namespace Chromium {
    /// <summary>
    /// Structure used to represent a download item.
    /// </summary>
    /// <remarks>
    /// See also the original CEF documentation in
    /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_download_item_capi.h">cef/include/capi/cef_download_item_capi.h</see>.
    /// </remarks>
    public class CfxDownloadItem : CfxBaseLibrary {

        private static readonly WeakCache weakCache = new WeakCache();

        internal static CfxDownloadItem Wrap(IntPtr nativePtr) {
            if(nativePtr == IntPtr.Zero) return null;
            lock(weakCache) {
                var wrapper = (CfxDownloadItem)weakCache.Get(nativePtr);
                if(wrapper == null) {
                    wrapper = new CfxDownloadItem(nativePtr);
                    weakCache.Add(wrapper);
                } else {
                    CfxApi.cfx_release(nativePtr);
                }
                return wrapper;
            }
        }


        internal CfxDownloadItem(IntPtr nativePtr) : base(nativePtr) {}

        /// <summary>
        /// Returns true (1) if this object is valid. Do not call any other functions
        /// if this function returns false (0).
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_download_item_capi.h">cef/include/capi/cef_download_item_capi.h</see>.
        /// </remarks>
        public bool IsValid {
            get {
                return 0 != CfxApi.DownloadItem.cfx_download_item_is_valid(NativePtr);
            }
        }

        /// <summary>
        /// Returns true (1) if the download is in progress.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_download_item_capi.h">cef/include/capi/cef_download_item_capi.h</see>.
        /// </remarks>
        public bool IsInProgress {
            get {
                return 0 != CfxApi.DownloadItem.cfx_download_item_is_in_progress(NativePtr);
            }
        }

        /// <summary>
        /// Returns true (1) if the download is complete.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_download_item_capi.h">cef/include/capi/cef_download_item_capi.h</see>.
        /// </remarks>
        public bool IsComplete {
            get {
                return 0 != CfxApi.DownloadItem.cfx_download_item_is_complete(NativePtr);
            }
        }

        /// <summary>
        /// Returns true (1) if the download has been canceled or interrupted.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_download_item_capi.h">cef/include/capi/cef_download_item_capi.h</see>.
        /// </remarks>
        public bool IsCanceled {
            get {
                return 0 != CfxApi.DownloadItem.cfx_download_item_is_canceled(NativePtr);
            }
        }

        /// <summary>
        /// Returns a simple speed estimate in bytes/s.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_download_item_capi.h">cef/include/capi/cef_download_item_capi.h</see>.
        /// </remarks>
        public long CurrentSpeed {
            get {
                return CfxApi.DownloadItem.cfx_download_item_get_current_speed(NativePtr);
            }
        }

        /// <summary>
        /// Returns the rough percent complete or -1 if the receive total size is
        /// unknown.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_download_item_capi.h">cef/include/capi/cef_download_item_capi.h</see>.
        /// </remarks>
        public int PercentComplete {
            get {
                return CfxApi.DownloadItem.cfx_download_item_get_percent_complete(NativePtr);
            }
        }

        /// <summary>
        /// Returns the total number of bytes.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_download_item_capi.h">cef/include/capi/cef_download_item_capi.h</see>.
        /// </remarks>
        public long TotalBytes {
            get {
                return CfxApi.DownloadItem.cfx_download_item_get_total_bytes(NativePtr);
            }
        }

        /// <summary>
        /// Returns the number of received bytes.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_download_item_capi.h">cef/include/capi/cef_download_item_capi.h</see>.
        /// </remarks>
        public long ReceivedBytes {
            get {
                return CfxApi.DownloadItem.cfx_download_item_get_received_bytes(NativePtr);
            }
        }

        /// <summary>
        /// Returns the time that the download started.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_download_item_capi.h">cef/include/capi/cef_download_item_capi.h</see>.
        /// </remarks>
        public CfxTime StartTime {
            get {
                return CfxTime.WrapOwned(CfxApi.DownloadItem.cfx_download_item_get_start_time(NativePtr));
            }
        }

        /// <summary>
        /// Returns the time that the download ended.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_download_item_capi.h">cef/include/capi/cef_download_item_capi.h</see>.
        /// </remarks>
        public CfxTime EndTime {
            get {
                return CfxTime.WrapOwned(CfxApi.DownloadItem.cfx_download_item_get_end_time(NativePtr));
            }
        }

        /// <summary>
        /// Returns the full path to the downloaded or downloading file.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_download_item_capi.h">cef/include/capi/cef_download_item_capi.h</see>.
        /// </remarks>
        public string FullPath {
            get {
                return StringFunctions.ConvertStringUserfree(CfxApi.DownloadItem.cfx_download_item_get_full_path(NativePtr));
            }
        }

        /// <summary>
        /// Returns the unique identifier for this download.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_download_item_capi.h">cef/include/capi/cef_download_item_capi.h</see>.
        /// </remarks>
        public uint Id {
            get {
                return CfxApi.DownloadItem.cfx_download_item_get_id(NativePtr);
            }
        }

        /// <summary>
        /// Returns the URL.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_download_item_capi.h">cef/include/capi/cef_download_item_capi.h</see>.
        /// </remarks>
        public string Url {
            get {
                return StringFunctions.ConvertStringUserfree(CfxApi.DownloadItem.cfx_download_item_get_url(NativePtr));
            }
        }

        /// <summary>
        /// Returns the original URL before any redirections.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_download_item_capi.h">cef/include/capi/cef_download_item_capi.h</see>.
        /// </remarks>
        public string OriginalUrl {
            get {
                return StringFunctions.ConvertStringUserfree(CfxApi.DownloadItem.cfx_download_item_get_original_url(NativePtr));
            }
        }

        /// <summary>
        /// Returns the suggested file name.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_download_item_capi.h">cef/include/capi/cef_download_item_capi.h</see>.
        /// </remarks>
        public string SuggestedFileName {
            get {
                return StringFunctions.ConvertStringUserfree(CfxApi.DownloadItem.cfx_download_item_get_suggested_file_name(NativePtr));
            }
        }

        /// <summary>
        /// Returns the content disposition.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_download_item_capi.h">cef/include/capi/cef_download_item_capi.h</see>.
        /// </remarks>
        public string ContentDisposition {
            get {
                return StringFunctions.ConvertStringUserfree(CfxApi.DownloadItem.cfx_download_item_get_content_disposition(NativePtr));
            }
        }

        /// <summary>
        /// Returns the mime type.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_download_item_capi.h">cef/include/capi/cef_download_item_capi.h</see>.
        /// </remarks>
        public string MimeType {
            get {
                return StringFunctions.ConvertStringUserfree(CfxApi.DownloadItem.cfx_download_item_get_mime_type(NativePtr));
            }
        }

        internal override void OnDispose(IntPtr nativePtr) {
            weakCache.Remove(nativePtr);
            base.OnDispose(nativePtr);
        }
    }
}
