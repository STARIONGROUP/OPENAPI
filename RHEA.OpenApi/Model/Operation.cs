// -------------------------------------------------------------------------------------------------
// <copyright file="Operation.cs" company="RHEA System S.A.">
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
    /// Describes a single API operation on a path.
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#operation-object
    /// </remarks>
    public class Operation
    {
        /// <summary>
        /// A list of tags for API documentation control. Tags can be used for logical grouping of operations by resources or any other qualifier.
        /// </summary>
        public string[] Tags { get; set; } = Array.Empty<string>();

        /// <summary>
        /// A short summary of what the operation does.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// A verbose explanation of the operation behavior. CommonMark syntax MAY be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Additional external documentation for this operation.
        /// </summary>
        public ExternalDocumentation ExternalDocs { get; set; }

        /// <summary>
        /// Unique string used to identify the operation. The id MUST be unique among all operations described in the API.
        /// The operationId value is case-sensitive. Tools and libraries MAY use the operationId to uniquely identify an operation, therefore,
        /// it is RECOMMENDED to follow common programming naming conventions.
        /// </summary>
        public string OperationId { get; set; }

        /// <summary>
        /// A list of parameters that are applicable for this operation. If a parameter is already defined at the Path Item, the new definition will
        /// override it but can never remove it. The list MUST NOT include duplicated parameters.
        /// A unique parameter is defined by a combination of a name and location.
        /// The list can use the Reference Object to link to parameters that are defined at the OpenAPI Object’s components/parameters.
        /// </summary>
        public Parameter[] Parameters { get; set; } = Array.Empty<Parameter>();

        /// <summary>
        /// gets or sets an array of <see cref="Reference"/> that can be used to populate the <see cref="Parameter"/> array
        /// once the complete Open API document has been deserialized
        /// </summary>
        internal Reference[] ParameterReferences { get; set; } = Array.Empty<Reference>();

        /// <summary>
        /// The request body applicable for this operation. The requestBody is fully supported in HTTP methods where the HTTP 1.1 specification [RFC7231]
        /// has explicitly defined semantics for request bodies. In other cases where the HTTP spec is vague (such as GET, HEAD and DELETE),
        /// requestBody is permitted but does not have well-defined semantics and SHOULD be avoided if possible.
        /// </summary>
        public RequestBody RequestBody { get; set; }

        /// <summary>
        /// gets or sets a <see cref="Reference"/> that can be used to populate the <see cref="RequestBody"/> property
        /// once the complete Open API document has been deserialized
        /// </summary>
        internal Reference RequestBodyReference { get; set; }

        /// <summary>
        /// The list of possible responses as they are returned from executing this operation.
        /// </summary>
        public Responses Responses { get; set; }

        /// <summary>
        /// A map of possible out-of band callbacks related to the parent operation. The key is a unique identifier for the Callback Object.
        /// Each value in the map is a Callback Object that describes a request that may be initiated by the API provider and the expected responses.
        /// </summary>
        public Dictionary<string, Callback> Callbacks { get; set; } = new Dictionary<string, Callback>();

        /// <summary>
        /// gets or sets a dictionary of <see cref="Reference"/> that can be used to populate the <see cref="Callbacks"/> Dictionary
        /// once the complete Open API document has been deserialized
        /// </summary>
        internal Dictionary<string, Reference> CallbacksReferences { get; set; } = new Dictionary<string, Reference>();

        /// <summary>
        /// Declares this operation to be deprecated. Consumers SHOULD refrain from usage of the declared operation. Default value is false.
        /// </summary>
        public bool Deprecated { get; set; }

        /// <summary>
        /// A declaration of which security mechanisms can be used for this operation.
        /// The list of values includes alternative security requirement objects that can be used. Only one of the security requirement
        /// objects need to be satisfied to authorize a request. To make security optional, an empty security requirement ({}) can
        /// be included in the array. This definition overrides any declared top-level security. To remove a top-level security declaration, an empty array can be used.
        /// </summary>
        public SecurityRequirement[] Security { get; set; } = Array.Empty<SecurityRequirement>();

        /// <summary>
        /// An alternative server array to service this operation. If an alternative server object is specified at the Path Item Object or Root level,
        /// it will be overridden by this value.
        /// </summary>
        public Server[] Servers { get; set; } = Array.Empty<Server>();
    }
}
