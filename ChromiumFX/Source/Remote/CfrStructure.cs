// Copyright (c) 2014-2017 Wolfgang Borgsmüller
// All rights reserved.
// 
// This software may be modified and distributed under the terms
// of the BSD license. See the License.txt file for details.

using System;

namespace Chromium.Remote {
    /// <summary>
    /// Base class for all remote wrapper classes for CEF structs without refcount.
    /// </summary>
    public abstract class CfrStructure : CfrObject {

        private readonly DtorRemoteCall dtor;

        // Case 1) User created structure: 
        // allocate native on creation, free native on dispose.
        internal CfrStructure(CtorRemoteCall ctor, DtorRemoteCall dtor) {
            this.dtor = dtor;
            ctor.RequestExecution();
            SetRemotePtr(new RemotePtr(ctor.__retval));
            RemotePtr.connection.weakCache.Add(RemotePtr.ptr, this);
        }

        // Case 2) struct pointer passed in from framework in callback function
        // wrap native pointer on creation, do not free native pointer
        internal CfrStructure(RemotePtr remotePtr) {
            SetRemotePtr(remotePtr);
        }

        internal override sealed void OnDispose(RemotePtr remotePtr) {
            if(dtor != null) {
                dtor.nativePtr = remotePtr.ptr;
                dtor.RequestExecution(remotePtr.connection);
            }
        }
    }
}
