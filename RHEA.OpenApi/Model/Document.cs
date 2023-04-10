// -------------------------------------------------------------------------------------------------
// <copyright file="Document.cs" company="RHEA System S.A.">
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
    using System.Collections.Generic;

    /// <summary>
    /// A self-contained or composite resource which defines or describes an API or elements of an API.
    /// The OpenAPI document MUST contain at least one paths field, a components field or a webhooks field.
    /// An OpenAPI document uses and conforms to the OpenAPI Specification.
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#openapi-object
    /// </remarks>
    public class Document
    {
        /// <summary>
        /// This string MUST be the version number of the OpenAPI Specification that the OpenAPI document uses.
        /// The openapi field SHOULD be used by tooling to interpret the OpenAPI document.
        /// This is not related to the API info.version string.
        /// </summary>
        public string OpenApi { get; set; }

        /// <summary>
        /// Provides metadata about the API. The metadata MAY be used by tooling as required.
        /// </summary>
        public Info Info { get; set; }

        /// <summary>
        /// The default value for the $schema keyword within Schema Objects contained within this OAS document. This MUST be in the form of a URI.
        /// </summary>
        public string JsonSchemaDialect { get; set; }

        /// <summary>
        /// An array of Server Objects, which provide connectivity information to a target server.
        /// If the servers property is not provided, or is an empty array, the default value would be a Server Object with a url value of /.
        /// </summary>
        public List<Server> Servers { get; set; } = new List<Server>();

        /// <summary>
        /// The available paths and operations for the API.
        /// </summary>
        public Dictionary<string, PathItem> Paths { get; set; } = new Dictionary<string, PathItem>();

        /// <summary>
        /// The incoming webhooks that MAY be received as part of this API and that the API consumer MAY choose to implement.
        /// Closely related to the callbacks feature, this section describes requests initiated other than by an API call,
        /// for example by an out of band registration. The key name is a unique string to refer to each webhook,
        /// while the (optionally referenced) Path Item Object describes a request that may be initiated by the API provider
        /// and the expected responses.
        /// </summary>
        public Dictionary<string, PathItem> Webhooks { get; set; } = new Dictionary<string, PathItem>();

        /// <summary>
        /// gets or sets a dictionary of <see cref="Reference"/> that can be used to populate the <see cref="Webhooks"/> Dictionary
        /// once the complete Open API document has been deserialized
        /// </summary>
        internal Dictionary<string, Reference> WebhooksReferences { get; set; } = new Dictionary<string, Reference>();

        /// <summary>
        /// An element to hold various schemas for the document.
        /// </summary>
        public Components Components { get; set; }

        /// <summary>
        /// A declaration of which security mechanisms can be used across the API. The list of values includes alternative security requirement objects
        /// that can be used. Only one of the security requirement objects need to be satisfied to authorize a request.
        /// Individual operations can override this definition. To make security optional, an empty security requirement ({}) can be included in the array.
        /// </summary>
        public List<SecurityRequirement> Security { get; set; } = new List<SecurityRequirement>();

        /// <summary>
        /// A list of tags used by the document with additional metadata.
        /// The order of the tags can be used to reflect on their order by the parsing tools. Not all tags that are used by the Operation Object must be declared.
        /// The tags that are not declared MAY be organized randomly or based on the tools’ logic. Each tag name in the list MUST be unique.
        /// </summary>
        public List<Tag> Tags { get; set; } = new List<Tag>();

        /// <summary>
        /// Additional external documentation.
        /// </summary>
        public ExternalDocumentation externalDocs { get; set; }
    }
}
