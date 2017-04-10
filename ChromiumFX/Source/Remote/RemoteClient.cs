// Copyright (c) 2014-2017 Wolfgang Borgsmüller
// All rights reserved.
// 
// This software may be modified and distributed under the terms
// of the BSD license. See the License.txt file for details.

using System;

namespace Chromium.Remote {

    internal class RemoteClient {

        /// <summary>
        /// Remote connection in the render process.
        /// If null, then this is the browser process.
        /// </summary>
        internal static RemoteConnection connection;

        internal static int ExecuteProcess(string pipeName) {

            //System.Diagnostics.Debugger.Launch();

            var pipeIn = PipeFactory.Instance.CreateClientPipeInputStream(pipeName + "so");
            var pipeOut = PipeFactory.Instance.CreateClientPipeOutputStream(pipeName + "si");

            connection = new RemoteConnection(pipeIn, pipeOut, true);

            var call = new RenderProcessMainRemoteCall();
            call.RequestExecution(connection);
            return call.__retval;

        }
    }

    internal class RenderProcessMainRemoteCall : RemoteCall {

        internal int __retval;

        internal RenderProcessMainRemoteCall() 
            : base(RemoteCallId.RenderProcessMainRemoteCall) { }

        protected override void WriteReturn(StreamHandler h) { h.Write(__retval); }
        protected override void ReadReturn(StreamHandler h) { h.Read(out __retval); }

        protected override void RemoteProcedure() {
            __retval = RemoteService.renderProcessMainCallback();
        }
    }
}
