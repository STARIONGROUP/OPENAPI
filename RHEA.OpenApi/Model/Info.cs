// -------------------------------------------------------------------------------------------------
// <copyright file="Info.cs" company="RHEA System S.A.">
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
    /// The object provides metadata about the API.
    /// The metadata MAY be used by the clients if needed, and MAY be presented in editing or documentation generation tools for convenience
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#info-object
    /// </remarks>
    public class Info
    {
        /// <summary>
        /// The title of the API.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A short summary of the API.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// A description of the API. CommonMark syntax MAY be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A URL to the Terms of Service for the API. This MUST be in the form of a URL.
        /// </summary>
        public string TermsOfService { get; set; }

        /// <summary>
        /// The contact information for the exposed API.
        /// </summary>
        public Contact Contact { get; set; }

        /// <summary>
        /// The license information for the exposed API.
        /// </summary>
        public License License { get; set; }

        /// <summary>
        /// The version of the OpenAPI document (which is distinct from the OpenAPI Specification version or the API implementation version).
        /// </summary>
        public string Version { get; set; }
    }
}