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


// cef_cursor_info

#ifdef __cplusplus
extern "C" {
#endif

CFX_EXPORT cef_cursor_info_t* cfx_cursor_info_ctor() {
    return (cef_cursor_info_t*)calloc(1, sizeof(cef_cursor_info_t));
}

CFX_EXPORT void cfx_cursor_info_dtor(cef_cursor_info_t* ptr) {
    free(ptr);
}

CFX_EXPORT void cfx_cursor_info_copy_to_native(cef_cursor_info_t* self, cef_point_t* hotspot, float image_scale_factor, void* buffer) {
    self->hotspot = *(hotspot);
    self->image_scale_factor = image_scale_factor;
    self->buffer = buffer;
}

CFX_EXPORT void cfx_cursor_info_copy_to_managed(cef_cursor_info_t* self, cef_point_t** hotspot, float* image_scale_factor, void** buffer) {
    *hotspot = &(self->hotspot);
    *image_scale_factor = self->image_scale_factor;
    *buffer = self->buffer;
}

#ifdef __cplusplus
} // extern "C"
#endif

