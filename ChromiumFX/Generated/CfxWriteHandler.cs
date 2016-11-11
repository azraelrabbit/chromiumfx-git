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

namespace Chromium {
    using Event;

    /// <summary>
    /// Structure the client can implement to provide a custom stream writer. The
    /// functions of this structure may be called on any thread.
    /// </summary>
    /// <remarks>
    /// See also the original CEF documentation in
    /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_stream_capi.h">cef/include/capi/cef_stream_capi.h</see>.
    /// </remarks>
    public class CfxWriteHandler : CfxClientBase {

        internal static CfxWriteHandler Wrap(IntPtr nativePtr) {
            if(nativePtr == IntPtr.Zero) return null;
            var handlePtr = CfxApi.WriteHandler.cfx_write_handler_get_gc_handle(nativePtr);
            return (CfxWriteHandler)System.Runtime.InteropServices.GCHandle.FromIntPtr(handlePtr).Target;
        }


        private static object eventLock = new object();

        internal static void SetNativeCallbacks() {
            write_native = write;
            seek_native = seek;
            tell_native = tell;
            flush_native = flush;
            may_block_native = may_block;

            write_native_ptr = System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(write_native);
            seek_native_ptr = System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(seek_native);
            tell_native_ptr = System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(tell_native);
            flush_native_ptr = System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(flush_native);
            may_block_native_ptr = System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(may_block_native);
        }

        // write
        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall, SetLastError = false)]
        private delegate void write_delegate(IntPtr gcHandlePtr, out UIntPtr __retval, IntPtr ptr, UIntPtr size, UIntPtr n);
        private static write_delegate write_native;
        private static IntPtr write_native_ptr;

        internal static void write(IntPtr gcHandlePtr, out UIntPtr __retval, IntPtr ptr, UIntPtr size, UIntPtr n) {
            var self = (CfxWriteHandler)System.Runtime.InteropServices.GCHandle.FromIntPtr(gcHandlePtr).Target;
            if(self == null || self.CallbacksDisabled) {
                __retval = default(UIntPtr);
                return;
            }
            var e = new CfxWriteEventArgs(ptr, size, n);
            var eventHandler = self.m_Write;
            if(eventHandler != null) eventHandler(self, e);
            e.m_isInvalid = true;
            __retval = (UIntPtr)e.m_returnValue;
        }

        // seek
        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall, SetLastError = false)]
        private delegate void seek_delegate(IntPtr gcHandlePtr, out int __retval, long offset, int whence);
        private static seek_delegate seek_native;
        private static IntPtr seek_native_ptr;

        internal static void seek(IntPtr gcHandlePtr, out int __retval, long offset, int whence) {
            var self = (CfxWriteHandler)System.Runtime.InteropServices.GCHandle.FromIntPtr(gcHandlePtr).Target;
            if(self == null || self.CallbacksDisabled) {
                __retval = default(int);
                return;
            }
            var e = new CfxSeekEventArgs(offset, whence);
            var eventHandler = self.m_Seek;
            if(eventHandler != null) eventHandler(self, e);
            e.m_isInvalid = true;
            __retval = e.m_returnValue;
        }

        // tell
        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall, SetLastError = false)]
        private delegate void tell_delegate(IntPtr gcHandlePtr, out long __retval);
        private static tell_delegate tell_native;
        private static IntPtr tell_native_ptr;

        internal static void tell(IntPtr gcHandlePtr, out long __retval) {
            var self = (CfxWriteHandler)System.Runtime.InteropServices.GCHandle.FromIntPtr(gcHandlePtr).Target;
            if(self == null || self.CallbacksDisabled) {
                __retval = default(long);
                return;
            }
            var e = new CfxTellEventArgs();
            var eventHandler = self.m_Tell;
            if(eventHandler != null) eventHandler(self, e);
            e.m_isInvalid = true;
            __retval = e.m_returnValue;
        }

        // flush
        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall, SetLastError = false)]
        private delegate void flush_delegate(IntPtr gcHandlePtr, out int __retval);
        private static flush_delegate flush_native;
        private static IntPtr flush_native_ptr;

        internal static void flush(IntPtr gcHandlePtr, out int __retval) {
            var self = (CfxWriteHandler)System.Runtime.InteropServices.GCHandle.FromIntPtr(gcHandlePtr).Target;
            if(self == null || self.CallbacksDisabled) {
                __retval = default(int);
                return;
            }
            var e = new CfxFlushEventArgs();
            var eventHandler = self.m_Flush;
            if(eventHandler != null) eventHandler(self, e);
            e.m_isInvalid = true;
            __retval = e.m_returnValue;
        }

        // may_block
        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall, SetLastError = false)]
        private delegate void may_block_delegate(IntPtr gcHandlePtr, out int __retval);
        private static may_block_delegate may_block_native;
        private static IntPtr may_block_native_ptr;

        internal static void may_block(IntPtr gcHandlePtr, out int __retval) {
            var self = (CfxWriteHandler)System.Runtime.InteropServices.GCHandle.FromIntPtr(gcHandlePtr).Target;
            if(self == null || self.CallbacksDisabled) {
                __retval = default(int);
                return;
            }
            var e = new CfxMayBlockEventArgs();
            var eventHandler = self.m_MayBlock;
            if(eventHandler != null) eventHandler(self, e);
            e.m_isInvalid = true;
            __retval = e.m_returnValue ? 1 : 0;
        }

        internal CfxWriteHandler(IntPtr nativePtr) : base(nativePtr) {}
        public CfxWriteHandler() : base(CfxApi.WriteHandler.cfx_write_handler_ctor) {}

        /// <summary>
        /// Write raw binary data.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_stream_capi.h">cef/include/capi/cef_stream_capi.h</see>.
        /// </remarks>
        public event CfxWriteEventHandler Write {
            add {
                lock(eventLock) {
                    if(m_Write == null) {
                        CfxApi.WriteHandler.cfx_write_handler_set_callback(NativePtr, 0, write_native_ptr);
                    }
                    m_Write += value;
                }
            }
            remove {
                lock(eventLock) {
                    m_Write -= value;
                    if(m_Write == null) {
                        CfxApi.WriteHandler.cfx_write_handler_set_callback(NativePtr, 0, IntPtr.Zero);
                    }
                }
            }
        }

        private CfxWriteEventHandler m_Write;

        /// <summary>
        /// Seek to the specified offset position. |Whence| may be any one of SEEK_CUR,
        /// SEEK_END or SEEK_SET. Return zero on success and non-zero on failure.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_stream_capi.h">cef/include/capi/cef_stream_capi.h</see>.
        /// </remarks>
        public event CfxSeekEventHandler Seek {
            add {
                lock(eventLock) {
                    if(m_Seek == null) {
                        CfxApi.WriteHandler.cfx_write_handler_set_callback(NativePtr, 1, seek_native_ptr);
                    }
                    m_Seek += value;
                }
            }
            remove {
                lock(eventLock) {
                    m_Seek -= value;
                    if(m_Seek == null) {
                        CfxApi.WriteHandler.cfx_write_handler_set_callback(NativePtr, 1, IntPtr.Zero);
                    }
                }
            }
        }

        private CfxSeekEventHandler m_Seek;

        /// <summary>
        /// Return the current offset position.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_stream_capi.h">cef/include/capi/cef_stream_capi.h</see>.
        /// </remarks>
        public event CfxTellEventHandler Tell {
            add {
                lock(eventLock) {
                    if(m_Tell == null) {
                        CfxApi.WriteHandler.cfx_write_handler_set_callback(NativePtr, 2, tell_native_ptr);
                    }
                    m_Tell += value;
                }
            }
            remove {
                lock(eventLock) {
                    m_Tell -= value;
                    if(m_Tell == null) {
                        CfxApi.WriteHandler.cfx_write_handler_set_callback(NativePtr, 2, IntPtr.Zero);
                    }
                }
            }
        }

        private CfxTellEventHandler m_Tell;

        /// <summary>
        /// Flush the stream.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_stream_capi.h">cef/include/capi/cef_stream_capi.h</see>.
        /// </remarks>
        public event CfxFlushEventHandler Flush {
            add {
                lock(eventLock) {
                    if(m_Flush == null) {
                        CfxApi.WriteHandler.cfx_write_handler_set_callback(NativePtr, 3, flush_native_ptr);
                    }
                    m_Flush += value;
                }
            }
            remove {
                lock(eventLock) {
                    m_Flush -= value;
                    if(m_Flush == null) {
                        CfxApi.WriteHandler.cfx_write_handler_set_callback(NativePtr, 3, IntPtr.Zero);
                    }
                }
            }
        }

        private CfxFlushEventHandler m_Flush;

        /// <summary>
        /// Return true (1) if this handler performs work like accessing the file
        /// system which may block. Used as a hint for determining the thread to access
        /// the handler from.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_stream_capi.h">cef/include/capi/cef_stream_capi.h</see>.
        /// </remarks>
        public event CfxMayBlockEventHandler MayBlock {
            add {
                lock(eventLock) {
                    if(m_MayBlock == null) {
                        CfxApi.WriteHandler.cfx_write_handler_set_callback(NativePtr, 4, may_block_native_ptr);
                    }
                    m_MayBlock += value;
                }
            }
            remove {
                lock(eventLock) {
                    m_MayBlock -= value;
                    if(m_MayBlock == null) {
                        CfxApi.WriteHandler.cfx_write_handler_set_callback(NativePtr, 4, IntPtr.Zero);
                    }
                }
            }
        }

        private CfxMayBlockEventHandler m_MayBlock;

        internal override void OnDispose(IntPtr nativePtr) {
            if(m_Write != null) {
                m_Write = null;
                CfxApi.WriteHandler.cfx_write_handler_set_callback(NativePtr, 0, IntPtr.Zero);
            }
            if(m_Seek != null) {
                m_Seek = null;
                CfxApi.WriteHandler.cfx_write_handler_set_callback(NativePtr, 1, IntPtr.Zero);
            }
            if(m_Tell != null) {
                m_Tell = null;
                CfxApi.WriteHandler.cfx_write_handler_set_callback(NativePtr, 2, IntPtr.Zero);
            }
            if(m_Flush != null) {
                m_Flush = null;
                CfxApi.WriteHandler.cfx_write_handler_set_callback(NativePtr, 3, IntPtr.Zero);
            }
            if(m_MayBlock != null) {
                m_MayBlock = null;
                CfxApi.WriteHandler.cfx_write_handler_set_callback(NativePtr, 4, IntPtr.Zero);
            }
            base.OnDispose(nativePtr);
        }
    }


    namespace Event {

        /// <summary>
        /// Write raw binary data.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_stream_capi.h">cef/include/capi/cef_stream_capi.h</see>.
        /// </remarks>
        public delegate void CfxWriteEventHandler(object sender, CfxWriteEventArgs e);

        /// <summary>
        /// Write raw binary data.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_stream_capi.h">cef/include/capi/cef_stream_capi.h</see>.
        /// </remarks>
        public class CfxWriteEventArgs : CfxEventArgs {

            internal IntPtr m_ptr;
            internal UIntPtr m_size;
            internal UIntPtr m_n;

            internal ulong m_returnValue;
            private bool returnValueSet;

            internal CfxWriteEventArgs(IntPtr ptr, UIntPtr size, UIntPtr n) {
                m_ptr = ptr;
                m_size = size;
                m_n = n;
            }

            /// <summary>
            /// Get the Ptr parameter for the <see cref="CfxWriteHandler.Write"/> callback.
            /// </summary>
            public IntPtr Ptr {
                get {
                    CheckAccess();
                    return m_ptr;
                }
            }
            /// <summary>
            /// Get the Size parameter for the <see cref="CfxWriteHandler.Write"/> callback.
            /// </summary>
            public ulong Size {
                get {
                    CheckAccess();
                    return (ulong)m_size;
                }
            }
            /// <summary>
            /// Get the N parameter for the <see cref="CfxWriteHandler.Write"/> callback.
            /// </summary>
            public ulong N {
                get {
                    CheckAccess();
                    return (ulong)m_n;
                }
            }
            /// <summary>
            /// Set the return value for the <see cref="CfxWriteHandler.Write"/> callback.
            /// Calling SetReturnValue() more then once per callback or from different event handlers will cause an exception to be thrown.
            /// </summary>
            public void SetReturnValue(ulong returnValue) {
                CheckAccess();
                if(returnValueSet) {
                    throw new CfxException("The return value has already been set");
                }
                returnValueSet = true;
                this.m_returnValue = returnValue;
            }

            public override string ToString() {
                return String.Format("Ptr={{{0}}}, Size={{{1}}}, N={{{2}}}", Ptr, Size, N);
            }
        }



        /// <summary>
        /// Flush the stream.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_stream_capi.h">cef/include/capi/cef_stream_capi.h</see>.
        /// </remarks>
        public delegate void CfxFlushEventHandler(object sender, CfxFlushEventArgs e);

        /// <summary>
        /// Flush the stream.
        /// </summary>
        /// <remarks>
        /// See also the original CEF documentation in
        /// <see href="https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/include/capi/cef_stream_capi.h">cef/include/capi/cef_stream_capi.h</see>.
        /// </remarks>
        public class CfxFlushEventArgs : CfxEventArgs {


            internal int m_returnValue;
            private bool returnValueSet;

            internal CfxFlushEventArgs() {
            }

            /// <summary>
            /// Set the return value for the <see cref="CfxWriteHandler.Flush"/> callback.
            /// Calling SetReturnValue() more then once per callback or from different event handlers will cause an exception to be thrown.
            /// </summary>
            public void SetReturnValue(int returnValue) {
                CheckAccess();
                if(returnValueSet) {
                    throw new CfxException("The return value has already been set");
                }
                returnValueSet = true;
                this.m_returnValue = returnValue;
            }
        }


    }
}
