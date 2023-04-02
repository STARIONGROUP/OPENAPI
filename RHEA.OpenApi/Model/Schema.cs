// -------------------------------------------------------------------------------------------------
// <copyright file="Schema.cs" company="RHEA System S.A.">
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

    /// <summary>
    /// The Schema Object allows the definition of input and output data types. These types can be objects, but also primitives and arrays.
    /// This object is a superset of the JSON Schema Specification Draft 2020-12.
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#schema-object
    /// </remarks>
    public class Schema
    {
        /// <summary>
        /// Adds support for polymorphism. The discriminator is an object name that is used to differentiate between other schemas which may
        /// satisfy the payload description. See Composition and Inheritance for more details.
        /// </summary>
        public Discriminator Discriminator { get; set; }

        /// <summary>
        /// This MAY be used only on properties schemas. It has no effect on root schemas. Adds additional metadata to describe the XML
        /// representation of this property.
        /// </summary>
        public XML XML { get; set; }

        /// <summary>
        /// Additional external documentation for this schema.
        /// </summary>
        public ExternalDocumentation ExternalDocs { get; set; }

        /// <summary>
        /// A free-form property to include an example of an instance for this schema. To represent examples that cannot be naturally represented in JSON or YAML,
        /// a string value can be used to contain the example with escaping where necessary.
        /// </summary>
        [Obsolete("The example property has been deprecated in favor of the JSON Schema examples keyword. Use of example is discouraged, and later versions of this specification may remove it.")]
        public object Example { get; set; }
    }
}
