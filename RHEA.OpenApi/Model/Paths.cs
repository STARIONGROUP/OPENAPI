// -------------------------------------------------------------------------------------------------
// <copyright file="Paths.cs" company="RHEA System S.A.">
// 
//   Copyright 2022-2023 RHEA System S.A.
// 
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace OpenApi.Model
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Holds the relative paths to the individual endpoints and their operations. The path is appended to the URL from the Server Object in order to construct the full URL.
    /// The Paths MAY be empty, due to Access Control List (ACL) constraints.
    /// 
    /// A relative path to an individual endpoint. The field name MUST begin with a forward slash (/). The path is appended (no relative URL resolution) to the
    /// expanded URL from the Server Object’s url field in order to construct the full URL. Path templating is allowed. When matching URLs,
    /// concrete (non-templated) paths would be matched before their templated counterparts. Templated paths with the same hierarchy
    /// but different templated names MUST NOT exist as they are identical. In case of ambiguous matching, it’s up to the tooling to decide which one to use.
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#paths-object
    /// </remarks>
    public class Paths : Dictionary<string, PathItem>
    {
    }
}
