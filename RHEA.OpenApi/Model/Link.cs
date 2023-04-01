// -------------------------------------------------------------------------------------------------
// <copyright file="Link.cs" company="RHEA System S.A.">
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
    /// The Link object represents a possible design-time link for a response. The presence of a link does not guarantee the caller’s
    /// ability to successfully invoke it, rather it provides a known relationship and traversal mechanism between responses and other operations.
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#link-object
    /// </remarks>
    public class Link
    {
        /// <summary>
        /// A relative or absolute URI reference to an OAS operation. This field is mutually exclusive of the operationId field,
        /// and MUST point to an Operation Object. Relative operationRef values MAY be used to locate an existing Operation Object
        /// in the OpenAPI definition. See the rules for resolving Relative References.
        /// </summary>
        public string OperationRef { get; set; }

        /// <summary>
        /// The name of an existing, resolvable OAS operation, as defined with a unique operationId. This field is mutually exclusive of the operationRef field.
        /// </summary>
        public string OperationId { get; set; }

        /// <summary>
        /// A map representing parameters to pass to an operation as specified with operationId or identified via operationRef.
        /// The key is the parameter name to be used, whereas the value can be a constant or an expression to be evaluated and
        /// passed to the linked operation. The parameter name can be qualified using the parameter location [{in}.]{name}
        /// for operations that use the same parameter name in different locations (e.g. path.id).
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; }

        /// <summary>
        /// A literal value or {expression} to use as a request body when calling the target operation.
        /// </summary>
        public object RequestBody { get; set; }

        /// <summary>
        /// A description of the link. CommonMark syntax MAY be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A server object to be used by the target operation.
        /// </summary>
        public Server Server { get; set; }
    }
}
