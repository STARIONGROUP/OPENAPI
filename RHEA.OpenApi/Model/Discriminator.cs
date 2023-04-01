// -------------------------------------------------------------------------------------------------
// <copyright file="Discriminator.cs" company="RHEA System S.A.">
// 
//   Copyright 2022-2023 RHEA System S.A.
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
    /// When request bodies or response payloads may be one of a number of different schemas, a discriminator object can be
    /// used to aid in serialization, deserialization, and validation. The discriminator is a specific object in
    /// a schema which is used to inform the consumer of the document of an alternative schema based on the value associated with it.
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#discriminator-object
    /// </remarks>
    public class Discriminator
    {
        /// <summary>
        /// REQUIRED. The name of the property in the payload that will hold the discriminator value.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// An object to hold mappings between payload values and schema names or references.
        /// </summary>
        public Dictionary<string, string> Mapping { get; set; }
    }
}
