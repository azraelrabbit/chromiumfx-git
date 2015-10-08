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


// cef_sslinfo

// cef_base_t base

// get_subject
static cef_sslcert_principal_t* cfx_sslinfo_get_subject(cef_sslinfo_t* self) {
    return self->get_subject(self);
}

// get_issuer
static cef_sslcert_principal_t* cfx_sslinfo_get_issuer(cef_sslinfo_t* self) {
    return self->get_issuer(self);
}

// get_serial_number
static cef_binary_value_t* cfx_sslinfo_get_serial_number(cef_sslinfo_t* self) {
    return self->get_serial_number(self);
}

// get_valid_start
static cef_time_t* cfx_sslinfo_get_valid_start(cef_sslinfo_t* self) {
    cef_time_t __retval_tmp = self->get_valid_start(self);
    return (cef_time_t*)cfx_copy_structure(&__retval_tmp, sizeof(cef_time_t));
}

// get_valid_expiry
static cef_time_t* cfx_sslinfo_get_valid_expiry(cef_sslinfo_t* self) {
    cef_time_t __retval_tmp = self->get_valid_expiry(self);
    return (cef_time_t*)cfx_copy_structure(&__retval_tmp, sizeof(cef_time_t));
}

// get_derencoded
static cef_binary_value_t* cfx_sslinfo_get_derencoded(cef_sslinfo_t* self) {
    return self->get_derencoded(self);
}

// get_pemencoded
static cef_binary_value_t* cfx_sslinfo_get_pemencoded(cef_sslinfo_t* self) {
    return self->get_pemencoded(self);
}


