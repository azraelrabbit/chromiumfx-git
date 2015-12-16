# ChromiumFX #

### .NET bindings for the [Chromium Embedded Framework](https://bitbucket.org/chromiumembedded/cef/). ###
----------

## Home ##
[https://bitbucket.org/chromiumfx/chromiumfx]()

## Overview ##

### ChromiumFX.dll ###

* **[Managed wrapper](https://bitbucket.org/chromiumfx/chromiumfx/wiki/Project) for the complete CEF API**
* **[Remote wrapper](https://bitbucket.org/chromiumfx/chromiumfx/wiki/Walkthrough_02) for full access to DOM and V8 from within the browser process**

### ChromiumWebBrowser.dll ###

* **[Windows Forms control](https://bitbucket.org/chromiumfx/chromiumfx/wiki/Walkthrough_01) based on ChromiumFX**

### See Also ###

* [Project Setup and Description](https://bitbucket.org/chromiumfx/chromiumfx/wiki/Project)
* [Getting started with the Browser Control](https://bitbucket.org/chromiumfx/chromiumfx/wiki/Walkthrough_01) 
* [Getting started with the Remoting Framework](https://bitbucket.org/chromiumfx/chromiumfx/wiki/Walkthrough_02)
* [API Reference](http://chromiumfx.bitbucket.org/api/)
* [Binary releases](https://bitbucket.org/chromiumfx/chromiumfx/downloads)

### Warning ###
The API of this project is not frozen and is subject to change. The overall structure will be kept though.

### Versioning ###

ChromiumFX version numbers have the format X.Y.Z where X is the CEF version (currently 3), Y is the CEF branch and Z is the ChromiumFX release number for a specific CEF version and branch. 

*"Within a release branch the CEF API is 'frozen' and generally only security/bug fixes are applied"*[[1](https://bitbucket.org/chromiumembedded/cef/wiki/BranchesAndBuilding#markdown-header-release-branches)]. So ChromiumFX binaries should work with any set of CEF binaries from the same branch. For example, ChromiumFX 3.2171.\* works with CEF 3.2171.1979 from the [CEF download page](http://www.magpcss.net/cef_downloads/), CEF 3.2171.2039 from [cefbuilds.com](https://cefbuilds.com/) or any other CEF 3.2171.\* build. 

From time to time, there might be an API change within the current release branch. This will be noted in the change log below. A list of CEF versions known to work with each ChromiumFX version will be maintained in CefVersion.txt.

At startup, API compatibility is verified by checking the API hash of the loaded libcef.dll, making sure ChromiumFX fails early when loading incompatible CEF binaries.

ChromiumFX exposes the following methods for querying version information: `CfxRuntime.GetCefVersion()`, `CfxRuntime.GetChromeVersion()`, `CfxRuntime.VersionInfo(entry)` and `CfxRuntime.ApiHash(entry)`. 

### Licensing and Credits ###

ChromiumFX is BSD licensed. The CEF project is BSD licensed. Please visit
"about:credits" in a CEF-based application for complete Chromium and third-party
licensing information. See also [cef/LICENSE.txt](https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/LICENSE.txt) and [cef/README.txt](https://bitbucket.org/chromiumfx/chromiumfx/src/tip/cef/README.txt).


## Changes ##

This is a summary of the most important changes and those relevant to embedders (API changes etc.).

### Version 3.2454.2 ###
- Expose the FindHandler through ChromiumWebBrowser.
- Implement a Find Toolbar for the ChromiumWebBrowser Control.
- Use BeginInvoke and Wait instead of Invoke for marshalled render process invokes.

### Version 3.2454.1 ###
- Update to cef version 3.2454.1344. The CEF API has changed since the last supported version, need updated cef binaries for this release.
- Resolved issue #35
- Fixed some smaller issues.

### Version 3.2454.0 ###
- Upgrade to CEF release branch 3.2454. This brings in some CEF API changes.
- All obsolete/deprecated functions have been deleted.

### Version 3.2171.19 ###
- This is a backport of ChromiumFX 3.2357.5 for the 3.2171 branch.   

### Version 3.2357.5 ###
- This is a bugfix release which addresses issues #27 and #29. The remote proxies of objects living in the render process did not release those objects due to a bug introduced in version 3.2357.3, causing a memory leak.

### Version 3.2171.18 ###
- This is a backport of ChromiumFX 3.2357.4 for the 3.2171 branch.   

### Version 3.2357.4 ###
- The platform toolset has been changed to v140, development is now done in VS 2015 Community. So in order to build libcef.dll, VS 2015 is required or change the toolset back to v120. The precompiled binaries should continue to work with previous versions of Visual Studio.
- CfxDragHandler.OnDraggableRegionsChanged was fixed.
- The deprecated functions in the remote layer now throw NotSupportedException because they were not reliable any more.
- Internal improvements without API changes and some bug fixes.

### Version 3.2171.17 ###
- This is a backport of ChromiumFX 3.2357.3 for the 3.2171 branch.   

### Version 3.2357.3 ###
- A 64-bit build is now available at cefbuilds.com for CEF 3.2357.1283 and it's compatible with this version. CEF 3.2357.1281 32-bit is still compatible as well.
- Instead of passing around a CfrRuntime object, the remote layer now uses remote process and thread contexts with thread-local and mostly transparent enter/exit semantics to keep track of remote callbacks. This implied a few changes in the public API of ChromiumFX and a lot of methods are deprecated now and will be deleted in a future release.
- Thanks to the new remote process and thread contexts, Cfr* static methods and constructors now work without passing in a remote runtime object. This allows for some useful features, e.g. implicit conversion from int, double, string and other primitive types to CfrV8Value.

### Version 3.2171.16 ###
- This is a back port of the current ChromiumFX version. It contains all ChromiumFX features from version 3.2357.2 but targets the CEF 3.2171 API. Besides new features, it contains a lot of important bug fixes so all users of the 3.2171 branch should upgrade.   

### Version 3.2357.2 ###
- Windows 64-bit builds on cefbuilds.com are still hanging so if you want to use this version in 64-bit mode, you have to build your own CEF binaries.
- JSObject now implements IDictionary.
- ChromiumWebBrowser: the global objects for the main frame and other named frames are now exposed through `public JSObject GlobalObject` and `public JSObject GlobalObjectForFrame(string frameName)`. The previous methods AddGlobalJSProperty, AddGlobalJSFunction and AddGlobalJSObject are deprecated.
- Some camelCase fixes for C# symbols, namely CfxSslCertPrincipal and CfxSslInfo.
- CfxGenerator was converted from VB to C# with some regular expressions, manual rewriting and the SharpDevelop 4.4 code converter.
- Implemented callback event ChromiumWebBrowser.OnRegisterCustomSchemes.
- Some bug fixes.

### Version 3.2357.1 Win32 ###
- This is an update based on CEF 3.2357.1280. There was an API change between the previous 3.2357.1 release (based on CEF 3.2357.1274) and this one, and there is no 64-bit build available at cefbuilds.com - 64-bit applications have to stick with the previous version.

### Version 3.2357.1 ###

* Fixed a race in the ipc layer introduced in 3.2171.15. Just as many concurrency bugs, this one becomes more visible under heavy usage. So basically 3.2171.15 and 
3.2357.0 are broken, with a bug causing the render process to freeze under some circumstances.

### Version 3.2357.0 ###

* Updated to CEF release branch 2357, the newest release branch considered production ready. If you update to this revision, you have to replace all CEF binary and resource files to the latest binary package found at [https://cefbuilds.com](). And you should test it with your use case to verify that there are no issues.
* As usual, the new release branch brings in a few API changes, see commit  393ad75 for an overview.
* The [API reference](http://chromiumfx.bitbucket.org/api/html/welcome.htm) has been updated to branch 2357 as well.

### Version 3.2171.15 ###

* Fixed a deadlock in the IPC code which caused the UI freeze after a renderer crash under some circumstances.
* Fixed a null reference bug unwrapping arrays of cef structs.
* Fixed a ref counting bug for arrays of cef structs.
* Issue #16 resolved; added test code for the use case described in that issue. 

### Version 3.2171.14 ###

* Enhanced javascript intgration: Additionally to the existing JSFunction, it is now possible to add javascript objects to the WebBrowser control and define functions and properties for those javascript objects, including other javascript objects and dynamic properties the application can handle at runtime. For a working example, see the test application (JsTestObject.cs).

### Version 3.2171.13 ###

* The `WindowStyles` enum was renamed to `WindowStyle`. The `CfxWindowInfo.Style` property type was changed to `WindowStyle`. Methods `CfxWindowInfo.SetAsChild()`, `CfxWindowInfo.SetAsPopup()` and `CfxWindowInfo.SetAsWindowless()` were added.
* Method `CfxRuntime.LoadLibraries()` was removed, the native libraries are now loaded transparently. To change locations for native libraries, use the `CfxRuntime.LibCefDirPath` and/or `CfxRuntime.LibCfxDirPath` properties before calling anything else in the Chromium namespaces.
* Method `CfxRemoting.ExecuteProcess()` was removed. `CfxRuntime.ExecuteProcess()` now checks transparently if a remote connection is required in the render process.

### Version 3.2171.12 ###

* The ChromiumWebBrowser control now exposes all client handlers as properties.
* ChromiumFX and ChromiumWebBrowser now support 64-bit CEF binaries. As a consequence, both assemblies are now compiled for platform AnyCPU. See [wiki](https://bitbucket.org/chromiumfx/chromiumfx/wiki/Project#markdown-header-application-layout) for details about application layouts.
* A small test application for windowless browsing was added [here](https://bitbucket.org/chromiumfx/chromiumfx/src/tip/Tests/Windowless/).
* `CfxRemoting.Initialize` was removed, applications now pass their render process startup callback directly through `CfxRuntime.Initialize(CfxSettings, CfxApp, CfxRenderProcessStartupDelegate)`.
* Some work has been done for linux support, but it's still broken. [Issue 9](https://bitbucket.org/chromiumfx/chromiumfx/issue/9/mono-support) is tracking this.

### Version 3.2171.11 ###

Fixed a bug introduced in 3.2171.10: for not refcounted cef types, the pinvoke layer would use the wrong native function as a dtor function, causing memory leaks and possibly corruption. Sorry for that :)

### Version 3.2171.10 ###

* libcfx.dll now loads libcef.dll function pointers explicitly instead of linking to libcef.lib, removing the link time dependency. This means that ChromiumFX is now compatible with any set of CEF binaries within the same branch.
* As a consequence, ChromiumFX version numbers now have the format X.Y.Z where X is the CEF version (currently 3), Y is the CEF branch and Z is the ChromiumFX release number for a specific CEF version and branch.
* At startup, API compatibility is verified by checking the API hash of the loaded libcef.dll, making sure ChromiumFX fails early when loading incompatible CEF binaries. 

### Version 3.2171.1979.9 ###

* The generator tool now extracts additional information about the public CEF C++ API to help translation of the C API:
 * The translation from C int (0/1) to C# bool (false/true) is now based on the signatures found in the C++ header files. This should have fixed the remaining issues with the int to bool translation.
 * Some CEF C functions have different names in the CEF C++ API. The C# translation now prefers the C++ names, including overloads like `CfxV8Value.HasValue()` for both `cef_v8value->has_value_bykey()` and `cef_v8value->has_value_byindex()`.

### Version 3.2171.1979.8 ###

* ChromiumFX and ChromiumWebBrowser assemblies are now built for platform x86 instead of AnyCPU, so the compiler will complain if you reference them from AnyCPU projects.
* Fixed int to bool conversion for `SetOsModalLoop` and `SetOsModalLoop` was removed from remote layer.
* Moved API delegate instantiation into static initializers of the types they belong to. This reduces work load at startup and avoids the allocation of unnecessary delegates for types never used by an application or secondary processes.
* CEF getter functions for string collections (`cef_string_list`, `cef_string_map`, `cef_string_multimap`) are now translated as functions with return value instead of functions with out paramenter. Example: `CfxBrowser.GetFrameNames(List<string> names)` ->  `List<string> CfxBrowser.GetFrameNames()`. This affects the public API of several classes.

### Version 3.2171.1979.7 ###

* Some of the framework callback container classes were not recognized as such by the code generator. This has been fixed. `CfxEndTracingCallback`, `CfxCompletionCallback`, `CfxRunFileDialogCallback` and `CfxSchemeHandlerFactory` are now correctly wrapped as framework callbacks.
* Removed browser-only `EndTracing` and `CfxEndTracingCallback` from remote layer.
* Fixed int to bool conversion for `GetBool` and `SetBool` functions in several classes.

### Version 3.2171.1979.6 ###

* Removed the `CfxRuntime.Initialize()` and `CfxRuntime.ExecuteProcess()` functions with sandbox parameter from the public API. Currently there is no way to start this within a sandbox. 
* Internally, the callback delegates and native function pointers are now created on demand, reducing the work load at startup. Most applications won't use all callbacks anyway. 
* Version information functions added to CfxRuntime.

### Version 3.2171.1979.5 ###

* The event handler and event arg classes for the framework callback events moved into their own namespaces. If you get errors, add `using Chromium.Event` and/or `using Chromium.Remote.Event` directives.
* A Sandcastle Help File Builder project was added and the xml documentation comments were improved for the new API reference.