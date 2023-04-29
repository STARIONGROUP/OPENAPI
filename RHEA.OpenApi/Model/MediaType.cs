// -------------------------------------------------------------------------------------------------
// <copyright file="MediaType.cs" company="RHEA System S.A.">
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
    /// Each Media Type Object provides schema and examples for the media type identified by its key.
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#media-type-object
    /// </remarks>
    public class MediaType
    {
        /// <summary>
        /// The schema defining the content of the request, response, or parameter.
        /// </summary>
        public Schema Schema { get; set; }

        /// <summary>
        /// gets or sets a <see cref="Reference"/> that can be used to populate the <see cref="Schema"/> property
        /// once the complete Open API document has been deserialized
        /// </summary>
        internal Reference SchemaReference { get; set; }

        /// <summary>
        /// Example of the media type. The example object SHOULD be in the correct format as specified by the media type.
        /// The example field is mutually exclusive of the examples field. Furthermore, if referencing a schema which contains an example,
        /// the example value SHALL override the example provided by the schema.
        /// </summary>
        public object Example { get; set; }

        /// <summary>
        /// Examples of the media type. Each example object SHOULD match the media type and specified schema if present.
        /// The examples field is mutually exclusive of the example field. Furthermore, if referencing a schema which contains an example,
        /// the examples value SHALL override the example provided by the schema.
        /// </summary>
        public Dictionary<string, Example> Examples { get; set; } = new Dictionary<string, Example>();

        /// <summary>
        /// gets or sets a dictionary of <see cref="Reference"/> that can be used to populate the <see cref="Example"/> Dictionary
        /// once the complete Open API document has been deserialized
        /// </summary>
        internal Dictionary<string, Reference> ExamplesReferences { get; set; } = new Dictionary<string, Reference>();

        /// <summary>
        /// A map between a property name and its encoding information. The key, being the property name, MUST exist in the schema as a property.
        /// The encoding object SHALL only apply to requestBody objects when the media type is multipart or application/x-www-form-urlencoded
        /// </summary>
        public Dictionary<string, Encoding> Encoding { get; set; } = new Dictionary<string, Encoding>();
    }
}
