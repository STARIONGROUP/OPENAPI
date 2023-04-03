// -------------------------------------------------------------------------------------------------
// <copyright file="Callback.cs" company="RHEA System S.A.">
// 
//   Copyright 2023 RHEA System S.A.
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

    /// <summary>
    /// A map of possible out-of band callbacks related to the parent operation.
    /// Each value in the map is a Path Item Object that describes a set of requests that may be initiated by the
    /// API provider and the expected responses. The key value used to identify the path item object is an expression,
    /// evaluated at runtime, that identifies a URL to use for the callback operation.
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#callback-object
    /// </remarks>
    public class Callback
    {
        /// <summary>
        /// Gets or sets the <see cref="PathItem"/> objects in this <see cref="Callback"/>
        /// </summary>
        public Dictionary<string, PathItem> PathItems = new Dictionary<string, PathItem>();

        /// <summary>
        /// gets or sets a dictionary of <see cref="Reference"/> that can be used to populate the <see cref="PathItems"/> Dictionary
        /// once the complete Open API document has been deserialized
        /// </summary>
        internal Dictionary<string, Reference> PathItemsReferences = new Dictionary<string, Reference>();
    }
}
