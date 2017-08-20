// Copyright (c) 2014-2017 Wolfgang Borgsmüller
// All rights reserved.
// 
// This software may be modified and distributed under the terms
// of the BSD license. See the License.txt file for details.

using System;

namespace Parser {

    [Serializable()]
    public class TypeNode {
        public string Name;
        public string Indirection;

        public override string ToString() {
            return Name + " " + Indirection;
        }
    }
}
