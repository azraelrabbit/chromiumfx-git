// Copyright (c) 2016 Marshall A. Greenblatt. All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are
// met:
//
//    * Redistributions of source code must retain the above copyright
// notice, this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following disclaimer
// in the documentation and/or other materials provided with the
// distribution.
//    * Neither the name of Google Inc. nor the name Chromium Embedded
// Framework nor the names of its contributors may be used to endorse
// or promote products derived from this software without specific prior
// written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
// ---------------------------------------------------------------------------
//
// This file is generated by the make_version_header.py tool.
//

#ifndef CEF_INCLUDE_CEF_VERSION_H_
#define CEF_INCLUDE_CEF_VERSION_H_

#define CEF_VERSION "3.2526.1366.g8617e7c"
#define CEF_VERSION_MAJOR 3
#define CEF_COMMIT_NUMBER 1366
#define CEF_COMMIT_HASH "8617e7c22c1215e26aff2a594bb1165b8972b9b1"
#define COPYRIGHT_YEAR 2016

#define CHROME_VERSION_MAJOR 47
#define CHROME_VERSION_MINOR 0
#define CHROME_VERSION_BUILD 2526
#define CHROME_VERSION_PATCH 80

#define DO_MAKE_STRING(p) #p
#define MAKE_STRING(p) DO_MAKE_STRING(p)

#ifndef APSTUDIO_HIDDEN_SYMBOLS

#include "include/internal/cef_export.h"

#ifdef __cplusplus
extern "C" {
#endif

// The API hash is created by analyzing CEF header files for C API type
// definitions. The hash value will change when header files are modified
// in a way that may cause binary incompatibility with other builds. The
// universal hash value will change if any platform is affected whereas the
// platform hash values will change only if that particular platform is
// affected.
#define CEF_API_HASH_UNIVERSAL "d3a0414c40a4941f5c67005055e2c70599ad14ce"
#if defined(OS_WIN)
#define CEF_API_HASH_PLATFORM "1572ecbd47bba644fb71ada9451e128cd5087d12"
#elif defined(OS_MACOSX)
#define CEF_API_HASH_PLATFORM "f7d1855852e19149522d8b531325cb4e8a4daa66"
#elif defined(OS_LINUX)
#define CEF_API_HASH_PLATFORM "b0919d511659eb09d6a020f297a4ef446a37f0b5"
#endif

// Returns CEF version information for the libcef library. The |entry|
// parameter describes which version component will be returned:
// 0 - CEF_VERSION_MAJOR
// 1 - CEF_COMMIT_NUMBER
// 2 - CHROME_VERSION_MAJOR
// 3 - CHROME_VERSION_MINOR
// 4 - CHROME_VERSION_BUILD
// 5 - CHROME_VERSION_PATCH
///
CEF_EXPORT int cef_version_info(int entry);

///
// Returns CEF API hashes for the libcef library. The returned string is owned
// by the library and should not be freed. The |entry| parameter describes which
// hash value will be returned:
// 0 - CEF_API_HASH_PLATFORM
// 1 - CEF_API_HASH_UNIVERSAL
// 2 - CEF_COMMIT_HASH
///
CEF_EXPORT const char* cef_api_hash(int entry);

#ifdef __cplusplus
}
#endif

#endif  // APSTUDIO_HIDDEN_SYMBOLS

#endif  // CEF_INCLUDE_CEF_VERSION_H_
