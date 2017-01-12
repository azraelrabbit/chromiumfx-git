// Copyright (c) 2014-2017 Wolfgang Borgsmüller
// All rights reserved.
// 
// This software may be modified and distributed under the terms
// of the BSD license. See the License.txt file for details.

// Generated file. Do not edit.


using System;

namespace Chromium {
    /// <summary>
    /// Callback structure used for continuation of custom context menu display.
    /// </summary>
    /// <remarks>
    /// See also the original CEF documentation in
    /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_context_menu_handler_capi.h">cef/include/capi/cef_context_menu_handler_capi.h</see>.
    /// </remarks>
    public class CfxRunContextMenuCallback : CfxLibraryBase {

        private static readonly WeakCache weakCache = new WeakCache();

        internal static CfxRunContextMenuCallback Wrap(IntPtr nativePtr) {
            if(nativePtr == IntPtr.Zero) return null;
            lock(weakCache) {
                var wrapper = (CfxRunContextMenuCallback)weakCache.Get(nativePtr);
                if(wrapper == null) {
                    wrapper = new CfxRunContextMenuCallback(nativePtr);
                    weakCache.Add(wrapper);
                } else {
                    CfxApi.cfx_release(nativePtr);
                }
                return wrapper;
            }
        }


        internal CfxRunContextMenuCallback(IntPtr nativePtr) : base(nativePtr) {}

        /// <summary>
        /// Complete context menu display by selecting the specified |commandId| and
        /// |eventFlags|.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_context_menu_handler_capi.h">cef/include/capi/cef_context_menu_handler_capi.h</see>.
        /// </remarks>
        public void Continue(int commandId, CfxEventFlags eventFlags) {
            CfxApi.RunContextMenuCallback.cfx_run_context_menu_callback_cont(NativePtr, commandId, (int)eventFlags);
        }

        /// <summary>
        /// Cancel context menu display.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_context_menu_handler_capi.h">cef/include/capi/cef_context_menu_handler_capi.h</see>.
        /// </remarks>
        public void Cancel() {
            CfxApi.RunContextMenuCallback.cfx_run_context_menu_callback_cancel(NativePtr);
        }

        internal override void OnDispose(IntPtr nativePtr) {
            weakCache.Remove(nativePtr);
            base.OnDispose(nativePtr);
        }
    }
}
