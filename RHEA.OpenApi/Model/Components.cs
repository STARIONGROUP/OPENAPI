// -------------------------------------------------------------------------------------------------
// <copyright file="Components.cs" company="RHEA System S.A.">
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
    /// Holds a set of reusable objects for different aspects of the OAS. All objects defined within the components object
    /// will have no effect on the API unless they are explicitly referenced from properties outside the components object.
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#componentsObject
    /// </remarks>
    public class Components
    {
        /// <summary>
        /// An object to hold reusable Schema Objects.
        /// </summary>
        public Dictionary<string, Schema> Schemas { get; set; } = new Dictionary<string, Schema>();

        /// <summary>
        /// An object to hold reusable Response Objects.
        /// </summary>
        public Dictionary<string, Response> Responses { get; set; } = new Dictionary<string, Response>();

        /// <summary>
        /// gets or sets a dictionary of <see cref="Reference"/> that can be used to populate the <see cref="Responses"/> Dictionary
        /// once the complete Open API document has been deserialized
        /// </summary>
        internal Dictionary<string, Reference> ResponsesReferences { get; set; } = new Dictionary<string, Reference>();

        /// <summary>
        /// An object to hold reusable Parameter Objects.
        /// </summary>
        public Dictionary<string, Parameter> Parameters { get; set; } = new Dictionary<string, Parameter>();

        /// <summary>
        /// gets or sets a dictionary of <see cref="Reference"/> that can be used to populate the <see cref="Parameters"/> Dictionary
        /// once the complete Open API document has been deserialized
        /// </summary>
        internal Dictionary<string, Reference> ParametersReferences { get; set; } = new Dictionary<string, Reference>();

        /// <summary>
        /// An object to hold reusable Example Objects.
        /// </summary>
        public Dictionary<string, Example> Examples { get; set; } = new Dictionary<string, Example>();

        /// <summary>
        /// gets or sets a dictionary of <see cref="Reference"/> that can be used to populate the <see cref="Examples"/> Dictionary
        /// once the complete Open API document has been deserialized
        /// </summary>
        internal Dictionary<string, Reference> ExamplesReferences { get; set; } = new Dictionary<string, Reference>();

        /// <summary>
        /// An object to hold reusable Request Body Objects
        /// </summary>
        public Dictionary<string, RequestBody> RequestBodies { get; set; } = new Dictionary<string, RequestBody>();

        /// <summary>
        /// gets or sets a dictionary of <see cref="Reference"/> that can be used to populate the <see cref="RequestBodies"/> Dictionary
        /// once the complete Open API document has been deserialized
        /// </summary>
        internal Dictionary<string, Reference> RequestBodiesReferences { get; set; } = new Dictionary<string, Reference>();

        /// <summary>
        /// An object to hold reusable Header Objects.
        /// </summary>
        public Dictionary<string, Header> Headers { get; set; } = new Dictionary<string, Header>();

        /// <summary>
        /// gets or sets a dictionary of <see cref="Reference"/> that can be used to populate the <see cref="Headers"/> Dictionary
        /// once the complete Open API document has been deserialized
        /// </summary>
        internal Dictionary<string, Reference> HeadersReferences { get; set; } = new Dictionary<string, Reference>();

        /// <summary>
        /// An object to hold reusable Security Scheme Objects.
        /// </summary>
        public Dictionary<string, SecurityScheme> SecuritySchemes { get; set; } = new Dictionary<string, SecurityScheme>();

        /// <summary>
        /// gets or sets a dictionary of <see cref="Reference"/> that can be used to populate the <see cref="SecuritySchemes"/> Dictionary
        /// once the complete Open API document has been deserialized
        /// </summary>
        internal Dictionary<string, Reference> SecuritySchemesReferences { get; set; } = new Dictionary<string, Reference>();

        /// <summary>
        /// An object to hold reusable Link Objects.
        /// </summary>
        public Dictionary<string, Link> Links { get; set; } = new Dictionary<string, Link>();

        /// <summary>
        /// gets or sets a dictionary of <see cref="Reference"/> that can be used to populate the <see cref="Links"/> Dictionary
        /// once the complete Open API document has been deserialized
        /// </summary>
        internal Dictionary<string, Reference> LinksReferences { get; set; } = new Dictionary<string, Reference>();

        /// <summary>
        /// An object to hold reusable Link Objects.
        /// </summary>
        public Dictionary<string, Callback> Callbacks { get; set; } = new Dictionary<string, Callback>();

        /// <summary>
        /// gets or sets a dictionary of <see cref="Reference"/> that can be used to populate the <see cref="Callbacks"/> Dictionary
        /// once the complete Open API document has been deserialized
        /// </summary>
        internal Dictionary<string, Reference> CallbacksReferences { get; set; } = new Dictionary<string, Reference>();

        /// <summary>
        /// An object to hold reusable Paths Item Object.
        /// </summary>
        public Dictionary<string, PathItem> PathItems { get; set; } = new Dictionary<string, PathItem>();

        /// <summary>
        /// gets or sets a dictionary of <see cref="Reference"/> that can be used to populate the <see cref="PathItems"/> Dictionary
        /// once the complete Open API document has been deserialized
        /// </summary>
        internal Dictionary<string, Reference> PathItemsReferences { get; set; } = new Dictionary<string, Reference>();
    }
}
