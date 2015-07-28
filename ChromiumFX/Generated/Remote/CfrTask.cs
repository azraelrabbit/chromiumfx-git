// Copyright (c) 2014-2015 Wolfgang Borgsmüller
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions 
// are met:
// 
// 1. Redistributions of source code must retain the above copyright 
//    notice, this list of conditions and the following disclaimer.
// 
// 2. Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution.
// 
// 3. Neither the name of the copyright holder nor the names of its 
//    contributors may be used to endorse or promote products derived 
//    from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS 
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT 
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS 
// FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE 
// COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
// INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
// BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS 
// OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND 
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
// TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE 
// USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

// Generated file. Do not edit.


using System;

namespace Chromium.Remote {
    using Event;

    /// <summary>
    /// Implement this structure for asynchronous task execution. If the task is
    /// posted successfully and if the associated message loop is still running then
    /// the execute() function will be called on the target thread. If the task fails
    /// to post then the task object may be destroyed on the source thread instead of
    /// the target thread. For this reason be cautious when performing work in the
    /// task object destructor.
    /// </summary>
    /// <remarks>
    /// See also the original CEF documentation in
    /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_task_capi.h">cef/include/capi/cef_task_capi.h</see>.
    /// </remarks>
    public class CfrTask : CfrBase {

        private static readonly RemoteWeakCache weakCache = new RemoteWeakCache();

        internal static CfrTask Wrap(ulong proxyId) {
            if(proxyId == 0) return null;
            lock(weakCache) {
                var cfrObj = (CfrTask)weakCache.Get(proxyId);
                if(cfrObj == null) {
                    cfrObj = new CfrTask(proxyId);
                    weakCache.Add(proxyId, cfrObj);
                }
                return cfrObj;
            }
        }


        internal static ulong CreateRemote() {
            var call = new CfxTaskCtorRenderProcessCall();
            call.Execute();
            return call.__retval;
        }

        internal void raise_Execute(object sender, CfrEventArgs e) {
            var handler = m_Execute;
            if(handler == null) return;
            handler(this, e);
            e.m_isInvalid = true;
        }


        private CfrTask(ulong proxyId) : base(proxyId) {}
        [Obsolete("new CfrTask(CfrRuntime) is deprecated, please use new CfrTask() without CfrRuntime instead.")]
        public CfrTask(CfrRuntime remoteRuntime) : base(CreateRemote()) {
            weakCache.Add(this.proxyId, this);
        }
        public CfrTask() : base(CreateRemote()) {
            weakCache.Add(this.proxyId, this);
        }

        /// <summary>
        /// Method that will be executed on the target thread.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_task_capi.h">cef/include/capi/cef_task_capi.h</see>.
        /// </remarks>
        public event CfrEventHandler Execute {
            add {
                if(m_Execute == null) {
                    var call = new CfxTaskExecuteActivateRenderProcessCall();
                    call.sender = proxyId;
                    call.Execute();
                }
                m_Execute += value;
            }
            remove {
                m_Execute -= value;
                if(m_Execute == null) {
                    var call = new CfxTaskExecuteDeactivateRenderProcessCall();
                    call.sender = proxyId;
                    call.Execute();
                }
            }
        }

        CfrEventHandler m_Execute;


        internal override void OnDispose(ulong proxyId) {
            weakCache.Remove(proxyId);
        }
    }

    namespace Event {


    }
}
