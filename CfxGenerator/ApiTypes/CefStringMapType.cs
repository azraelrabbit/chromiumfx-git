// Copyright (c) 2014-2017 Wolfgang Borgsmüller
// All rights reserved.
// 
// This software may be modified and distributed under the terms
// of the BSD license. See the License.txt file for details.

public class CefStringMapType : StringCollectionType {

    public CefStringMapType()
        : base("cef_string_map") {
    }

    public override string PublicSymbol {
        get { return "System.Collections.Generic.List<string[]>"; }
    }
}