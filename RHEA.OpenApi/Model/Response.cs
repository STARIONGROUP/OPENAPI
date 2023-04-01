// -------------------------------------------------------------------------------------------------
// <copyright file="Response.cs" company="RHEA System S.A.">
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
    /// Describes a single response from an API Operation, including design-time, static links to operations based on the response.
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#response-object
    /// </remarks>
    public class Response
    {
        /// <summary>
        /// REQUIRED. A description of the response. CommonMark syntax MAY be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Maps a header name to its definition. [RFC7230] states header names are case insensitive. If a response header is defined with
        /// the name "Content-Type", it SHALL be ignored.
        /// </summary>
        public Dictionary<string, Header> Headers { get; set; }

        /// <summary>
        /// A map containing descriptions of potential response payloads. The key is a media type or media type range and the value describes it.
        /// For responses that match multiple keys, only the most specific key is applicable. e.g. text/plain overrides text/*
        /// </summary>
        public Dictionary<string, MediaType> Content { get; set; }

        /// <summary>
        /// A map of operations links that can be followed from the response. The key of the map is a short name for the link,
        /// following the naming constraints of the names for Component Objects.
        /// </summary>
        public Dictionary<string, Link> Links { get; set; }
    }
}
