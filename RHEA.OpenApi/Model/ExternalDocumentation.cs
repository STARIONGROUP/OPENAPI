// -------------------------------------------------------------------------------------------------
// <copyright file="ExternalDocumentation.cs" company="RHEA System S.A.">
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
    /// Allows referencing an external resource for extended documentation.
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#external-documentation-object
    /// </remarks>
    public class ExternalDocumentation
    {
        /// <summary>
        /// A description of the target documentation. CommonMark syntax MAY be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// REQUIRED. The URL for the target documentation. This MUST be in the form of a URL.
        /// </summary>
        public string Url { get; set; }
    }
}
