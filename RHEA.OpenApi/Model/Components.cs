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
        public Dictionary<string, Schema> Schemas { get; set; }

        /// <summary>
        /// An object to hold reusable Response Objects.
        /// </summary>
        public Dictionary<string, Response> Responses { get; set; }

        /// <summary>
        /// An object to hold reusable Parameter Objects.
        /// </summary>
        public Dictionary<string, Parameter> Parameters { get; set; }

        /// <summary>
        /// An object to hold reusable Example Objects.
        /// </summary>
        public Dictionary<string, Example> Examples { get; set; }

        /// <summary>
        /// An object to hold reusable Request Body Objects
        /// </summary>
        public Dictionary<string, RequestBody> RequestBodies { get; set; }

        /// <summary>
        /// An object to hold reusable Header Objects.
        /// </summary>
        public Dictionary<string, Header> Headers { get; set; }

        /// <summary>
        /// An object to hold reusable Security Scheme Objects.
        /// </summary>
        public Dictionary<string, SecurityScheme> SecuritySchemes { get; set; }

        /// <summary>
        /// An object to hold reusable Link Objects.
        /// </summary>
        public Dictionary<string, Link> Links { get; set; }

        /// <summary>
        /// An object to hold reusable Link Objects.
        /// </summary>
        public Dictionary<string, Callback> Callbacks { get; set; }

        /// <summary>
        /// An object to hold reusable Paths Item Object.
        /// </summary>
        public Dictionary<string, PathItem> PathItems { get; set; }
    }
}
