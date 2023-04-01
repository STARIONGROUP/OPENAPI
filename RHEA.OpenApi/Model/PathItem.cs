// -------------------------------------------------------------------------------------------------
// <copyright file="PathItem.cs" company="RHEA System S.A.">
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
    using System;

    /// <summary>
    /// Describes the operations available on a single path. A Path Item MAY be empty, due to ACL constraints.
    /// The path itself is still exposed to the documentation viewer but they will not know which operations and parameters are available.
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#path-item-object
    /// </remarks>
    public class PathItem
    {
        /// <summary>
        /// Allows for a referenced definition of this path item. The referenced structure MUST be in the form of a Path Item Object.
        /// In case a Path Item Object field appears both in the defined object and the referenced object, the behavior is undefined.
        /// </summary>
        public string Ref { get; set; }

        /// <summary>
        /// An optional, string summary, intended to apply to all operations in this path.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// An optional, string description, intended to apply to all operations in this path. CommonMark syntax MAY be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A definition of a GET operation on this path.
        /// </summary>
        public Operation Get { get; set; }

        /// <summary>
        /// A definition of a PUT operation on this path.
        /// </summary>
        public Operation Put { get; set; }

        /// <summary>
        /// A definition of a POST operation on this path.
        /// </summary>
        public Operation Post { get; set; }

        /// <summary>
        /// A definition of a DELETE operation on this path.
        /// </summary>
        public Operation Delete { get; set; }

        /// <summary>
        /// A definition of a OPTIONS operation on this path.
        /// </summary>
        public Operation Options { get; set; }

        /// <summary>
        /// A definition of a HEAD operation on this path.
        /// </summary>
        public Operation Head { get; set; }

        /// <summary>
        /// A definition of a PATCH operation on this path.
        /// </summary>
        public Operation Patch { get; set; }

        /// <summary>
        /// A definition of a TRACE operation on this path.
        /// </summary>
        public Operation Trace { get; set; }

        /// <summary>
        /// An alternative server array to service all operations in this path.
        /// </summary>
        public Server[] Servers { get; set; } = Array.Empty<Server>();

        /// <summary>
        /// A list of parameters that are applicable for all the operations described under this path.
        /// These parameters can be overridden at the operation level, but cannot be removed there.
        /// The list MUST NOT include duplicated parameters. A unique parameter is defined by a combination of a name and location.
        /// The list can use the Reference Object to link to parameters that are defined at the OpenAPI Object’s components/parameters.
        /// </summary>
        public Parameter[] Parameters { get; set; } = Array.Empty<Parameter>();

        /// <summary>
        /// gets or sets an array of <see cref="Reference"/> that can be used to populate the <see cref="Parameter"/> array
        /// once the complete Open API document has been deserialized
        /// </summary>
        internal Reference[] ParameterReferences { get; set; } = Array.Empty<Reference>();
    }
}
