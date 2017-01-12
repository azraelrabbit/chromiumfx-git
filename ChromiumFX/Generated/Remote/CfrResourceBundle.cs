// Copyright (c) 2014-2017 Wolfgang Borgsmüller
// All rights reserved.
// 
// This software may be modified and distributed under the terms
// of the BSD license. See the License.txt file for details.

// Generated file. Do not edit.


using System;

namespace Chromium.Remote {

    /// <summary>
    /// Structure used for retrieving resources from the resource bundle (*.pak)
    /// files loaded by CEF during startup or via the CfrResourceBundleHandler
    /// returned from CfrApp.GetResourceBundleHandler. See CfrSettings for
    /// additional options related to resource bundle loading. The functions of this
    /// structure may be called on any thread unless otherwise indicated.
    /// </summary>
    /// <remarks>
    /// See also the original CEF documentation in
    /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_resource_bundle_capi.h">cef/include/capi/cef_resource_bundle_capi.h</see>.
    /// </remarks>
    public class CfrResourceBundle : CfrLibraryBase {

        internal static CfrResourceBundle Wrap(RemotePtr remotePtr) {
            if(remotePtr == RemotePtr.Zero) return null;
            var weakCache = CfxRemoteCallContext.CurrentContext.connection.weakCache;
            lock(weakCache) {
                var cfrObj = (CfrResourceBundle)weakCache.Get(remotePtr.ptr);
                if(cfrObj == null) {
                    cfrObj = new CfrResourceBundle(remotePtr);
                    weakCache.Add(remotePtr.ptr, cfrObj);
                }
                return cfrObj;
            }
        }


        /// <summary>
        /// Returns the global resource bundle instance.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_resource_bundle_capi.h">cef/include/capi/cef_resource_bundle_capi.h</see>.
        /// </remarks>
        public static CfrResourceBundle GetGlobal() {
            var call = new CfxResourceBundleGetGlobalRemoteCall();
            call.RequestExecution();
            return CfrResourceBundle.Wrap(new RemotePtr(call.__retval));
        }


        private CfrResourceBundle(RemotePtr remotePtr) : base(remotePtr) {}

        /// <summary>
        /// Returns the localized string for the specified |stringId| or an NULL
        /// string if the value is not found. Include cef_pack_strings.h for a listing
        /// of valid string ID values.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_resource_bundle_capi.h">cef/include/capi/cef_resource_bundle_capi.h</see>.
        /// </remarks>
        public string GetLocalizedString(int stringId) {
            var call = new CfxResourceBundleGetLocalizedStringRemoteCall();
            call.@this = RemotePtr.ptr;
            call.stringId = stringId;
            call.RequestExecution(RemotePtr.connection);
            return call.__retval;
        }

        /// <summary>
        /// Retrieves the contents of the specified scale independent |resourceId|. If
        /// the value is found then |data| and |dataSize| will be populated and this
        /// function will return true (1). If the value is not found then this function
        /// will return false (0). The returned |data| pointer will remain resident in
        /// memory and should not be freed. Include cef_pack_resources.h for a listing
        /// of valid resource ID values.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_resource_bundle_capi.h">cef/include/capi/cef_resource_bundle_capi.h</see>.
        /// </remarks>
        public bool GetDataResource(int resourceId, RemotePtr data, ulong dataSize) {
            var call = new CfxResourceBundleGetDataResourceRemoteCall();
            call.@this = RemotePtr.ptr;
            call.resourceId = resourceId;
            call.RequestExecution(RemotePtr.connection);
            data = new RemotePtr(connection, call.data);
            dataSize = call.dataSize;
            return call.__retval;
        }

        /// <summary>
        /// Retrieves the contents of the specified |resourceId| nearest the scale
        /// factor |scaleFactor|. Use a |scaleFactor| value of SCALE_FACTOR_NONE for
        /// scale independent resources or call GetDataResource instead. If the value
        /// is found then |data| and |dataSize| will be populated and this function
        /// will return true (1). If the value is not found then this function will
        /// return false (0). The returned |data| pointer will remain resident in
        /// memory and should not be freed. Include cef_pack_resources.h for a listing
        /// of valid resource ID values.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_resource_bundle_capi.h">cef/include/capi/cef_resource_bundle_capi.h</see>.
        /// </remarks>
        public bool GetDataResourceForScale(int resourceId, CfxScaleFactor scaleFactor, RemotePtr data, ulong dataSize) {
            var call = new CfxResourceBundleGetDataResourceForScaleRemoteCall();
            call.@this = RemotePtr.ptr;
            call.resourceId = resourceId;
            call.scaleFactor = (int)scaleFactor;
            call.RequestExecution(RemotePtr.connection);
            data = new RemotePtr(connection, call.data);
            dataSize = call.dataSize;
            return call.__retval;
        }
    }
}
