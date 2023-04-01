// -------------------------------------------------------------------------------------------------
// <copyright file="Reference.cs" company="RHEA System S.A.">
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
    /// <summary>
    /// A simple object to allow referencing other components in the OpenAPI document, internally and externally.
    /// The $ref string value contains a URI [RFC3986], which identifies the location of the value being referenced.
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#reference-object
    /// </remarks>
    public class Reference
    {
        /// <summary>
        /// REQUIRED. The reference identifier. This MUST be in the form of a URI.
        /// </summary>
        public string Ref { get; set; }

        /// <summary>
        /// A short summary which by default SHOULD override that of the referenced component.
        /// If the referenced object-type does not allow a summary field, then this field has no effect.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// A description which by default SHOULD override that of the referenced component. CommonMark syntax MAY be
        /// used for rich text representation. If the referenced object-type does not allow a description field, then this field has no effect.
        /// </summary>
        public string Description { get; set; }
    }
}
