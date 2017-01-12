// Copyright (c) 2014-2017 Wolfgang Borgsmüller
// All rights reserved.
// 
// This software may be modified and distributed under the terms
// of the BSD license. See the License.txt file for details.

// Generated file. Do not edit.


using System;

namespace Chromium.Remote {

    /// <summary>
    /// Structure used to represent a DOM document. The functions of this structure
    /// should only be called on the render process main thread thread.
    /// </summary>
    /// <remarks>
    /// See also the original CEF documentation in
    /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_dom_capi.h">cef/include/capi/cef_dom_capi.h</see>.
    /// </remarks>
    public class CfrDomDocument : CfrLibraryBase {

        internal static CfrDomDocument Wrap(RemotePtr remotePtr) {
            if(remotePtr == RemotePtr.Zero) return null;
            var weakCache = CfxRemoteCallContext.CurrentContext.connection.weakCache;
            lock(weakCache) {
                var cfrObj = (CfrDomDocument)weakCache.Get(remotePtr.ptr);
                if(cfrObj == null) {
                    cfrObj = new CfrDomDocument(remotePtr);
                    weakCache.Add(remotePtr.ptr, cfrObj);
                }
                return cfrObj;
            }
        }



        private CfrDomDocument(RemotePtr remotePtr) : base(remotePtr) {}

        /// <summary>
        /// Returns the document type.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_dom_capi.h">cef/include/capi/cef_dom_capi.h</see>.
        /// </remarks>
        public CfxDomDocumentType Type {
            get {
                var call = new CfxDomDocumentGetTypeRemoteCall();
                call.@this = RemotePtr.ptr;
                call.RequestExecution(RemotePtr.connection);
                return (CfxDomDocumentType)call.__retval;
            }
        }

        /// <summary>
        /// Returns the root document node.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_dom_capi.h">cef/include/capi/cef_dom_capi.h</see>.
        /// </remarks>
        public CfrDomNode Document {
            get {
                var call = new CfxDomDocumentGetDocumentRemoteCall();
                call.@this = RemotePtr.ptr;
                call.RequestExecution(RemotePtr.connection);
                return CfrDomNode.Wrap(new RemotePtr(call.__retval));
            }
        }

        /// <summary>
        /// Returns the BODY node of an HTML document.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_dom_capi.h">cef/include/capi/cef_dom_capi.h</see>.
        /// </remarks>
        public CfrDomNode Body {
            get {
                var call = new CfxDomDocumentGetBodyRemoteCall();
                call.@this = RemotePtr.ptr;
                call.RequestExecution(RemotePtr.connection);
                return CfrDomNode.Wrap(new RemotePtr(call.__retval));
            }
        }

        /// <summary>
        /// Returns the HEAD node of an HTML document.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_dom_capi.h">cef/include/capi/cef_dom_capi.h</see>.
        /// </remarks>
        public CfrDomNode Head {
            get {
                var call = new CfxDomDocumentGetHeadRemoteCall();
                call.@this = RemotePtr.ptr;
                call.RequestExecution(RemotePtr.connection);
                return CfrDomNode.Wrap(new RemotePtr(call.__retval));
            }
        }

        /// <summary>
        /// Returns the title of an HTML document.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_dom_capi.h">cef/include/capi/cef_dom_capi.h</see>.
        /// </remarks>
        public string Title {
            get {
                var call = new CfxDomDocumentGetTitleRemoteCall();
                call.@this = RemotePtr.ptr;
                call.RequestExecution(RemotePtr.connection);
                return call.__retval;
            }
        }

        /// <summary>
        /// Returns the node that currently has keyboard focus.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_dom_capi.h">cef/include/capi/cef_dom_capi.h</see>.
        /// </remarks>
        public CfrDomNode FocusedNode {
            get {
                var call = new CfxDomDocumentGetFocusedNodeRemoteCall();
                call.@this = RemotePtr.ptr;
                call.RequestExecution(RemotePtr.connection);
                return CfrDomNode.Wrap(new RemotePtr(call.__retval));
            }
        }

        /// <summary>
        /// Returns true (1) if a portion of the document is selected.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_dom_capi.h">cef/include/capi/cef_dom_capi.h</see>.
        /// </remarks>
        public bool HasSelection {
            get {
                var call = new CfxDomDocumentHasSelectionRemoteCall();
                call.@this = RemotePtr.ptr;
                call.RequestExecution(RemotePtr.connection);
                return call.__retval;
            }
        }

        /// <summary>
        /// Returns the selection offset within the start node.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_dom_capi.h">cef/include/capi/cef_dom_capi.h</see>.
        /// </remarks>
        public int SelectionStartOffset {
            get {
                var call = new CfxDomDocumentGetSelectionStartOffsetRemoteCall();
                call.@this = RemotePtr.ptr;
                call.RequestExecution(RemotePtr.connection);
                return call.__retval;
            }
        }

        /// <summary>
        /// Returns the selection offset within the end node.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_dom_capi.h">cef/include/capi/cef_dom_capi.h</see>.
        /// </remarks>
        public int SelectionEndOffset {
            get {
                var call = new CfxDomDocumentGetSelectionEndOffsetRemoteCall();
                call.@this = RemotePtr.ptr;
                call.RequestExecution(RemotePtr.connection);
                return call.__retval;
            }
        }

        /// <summary>
        /// Returns the contents of this selection as markup.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_dom_capi.h">cef/include/capi/cef_dom_capi.h</see>.
        /// </remarks>
        public string SelectionAsMarkup {
            get {
                var call = new CfxDomDocumentGetSelectionAsMarkupRemoteCall();
                call.@this = RemotePtr.ptr;
                call.RequestExecution(RemotePtr.connection);
                return call.__retval;
            }
        }

        /// <summary>
        /// Returns the contents of this selection as text.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_dom_capi.h">cef/include/capi/cef_dom_capi.h</see>.
        /// </remarks>
        public string SelectionAsText {
            get {
                var call = new CfxDomDocumentGetSelectionAsTextRemoteCall();
                call.@this = RemotePtr.ptr;
                call.RequestExecution(RemotePtr.connection);
                return call.__retval;
            }
        }

        /// <summary>
        /// Returns the base URL for the document.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_dom_capi.h">cef/include/capi/cef_dom_capi.h</see>.
        /// </remarks>
        public string BaseUrl {
            get {
                var call = new CfxDomDocumentGetBaseUrlRemoteCall();
                call.@this = RemotePtr.ptr;
                call.RequestExecution(RemotePtr.connection);
                return call.__retval;
            }
        }

        /// <summary>
        /// Returns the document element with the specified ID value.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_dom_capi.h">cef/include/capi/cef_dom_capi.h</see>.
        /// </remarks>
        public CfrDomNode GetElementById(string id) {
            var call = new CfxDomDocumentGetElementByIdRemoteCall();
            call.@this = RemotePtr.ptr;
            call.id = id;
            call.RequestExecution(RemotePtr.connection);
            return CfrDomNode.Wrap(new RemotePtr(call.__retval));
        }

        /// <summary>
        /// Returns a complete URL based on the document base URL and the specified
        /// partial URL.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_dom_capi.h">cef/include/capi/cef_dom_capi.h</see>.
        /// </remarks>
        public string GetCompleteUrl(string partialURL) {
            var call = new CfxDomDocumentGetCompleteUrlRemoteCall();
            call.@this = RemotePtr.ptr;
            call.partialURL = partialURL;
            call.RequestExecution(RemotePtr.connection);
            return call.__retval;
        }
    }
}
