// -------------------------------------------------------------------------------------------------
// <copyright file="Header.cs" company="RHEA System S.A.">
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
    /// The Header Object follows the structure of the Parameter Object with the following changes:
    ///   1. name MUST NOT be specified, it is given in the corresponding headers map.
    ///   2. in MUST NOT be specified, it is implicitly in header.
    ///   3. All traits that are affected by the location MUST be applicable to a location of header (for example, style).
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#header-object
    /// </remarks>
    public class Header
    {
        /// <summary>
        /// A brief description of the parameter. This could contain examples of use. CommonMark syntax MAY be used for rich text representation
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Determines whether this parameter is mandatory. If the parameter location is "path", this property is REQUIRED and its value MUST be true.
        /// Otherwise, the property MAY be included and its default value is false.
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Specifies that a parameter is deprecated and SHOULD be transitioned out of usage. Default value is false.
        /// </summary>
        public bool Deprecated { get; set; }

        /// <summary>
        /// Sets the ability to pass empty-valued parameters. This is valid only for query parameters and allows sending a parameter with an empty value.
        /// Default value is false. If style is used, and if behavior is n/a (cannot be serialized), the value of allowEmptyValue SHALL be ignored.
        /// Use of this property is NOT RECOMMENDED, as it is likely to be removed in a later revision.
        /// </summary>
        public bool AllowEmptyValue { get; set; }

        /// <summary>
        /// Describes how the parameter value will be serialized depending on the type of the parameter value.
        /// Default values (based on value of in): for query - form; for path - simple; for header - simple; for cookie - form.
        /// </summary>
        public string Style { get; set; }

        /// <summary>
        /// When this is true, parameter values of type array or object generate separate parameters for each value of the array or key-value pair of the map.
        /// For other types of parameters this property has no effect. When style is form, the default value is true. For all other styles, the default value is false.
        /// </summary>
        public bool Explode { get; set; }

        /// <summary>
        /// Determines whether the parameter value SHOULD allow reserved characters, as defined by [RFC3986] :/?#[]@!$&'()*+,;= to be included without
        /// percent-encoding. This property only applies to parameters with an in value of query. The default value is false.
        /// </summary>
        public bool AllowReserved { get; set; }

        /// <summary>
        /// The schema defining the type used for the <see cref="Header"/>
        /// </summary>
        public Schema Schema { get; set; }

        /// <summary>
        /// Example of the parameter’s potential value. The example SHOULD match the specified schema and encoding properties if present.
        /// The example field is mutually exclusive of the examples field. Furthermore, if referencing a schema that contains an example,
        /// the example value SHALL override the example provided by the schema. To represent examples of media types that cannot naturally
        /// be represented in JSON or YAML, a string value can contain the example with escaping where necessary.
        /// </summary>
        public object Example { get; set; }

        /// <summary>
        /// Examples of the parameter’s potential value. Each example SHOULD contain a value in the correct format as specified in the parameter encoding.
        /// The examples field is mutually exclusive of the example field. Furthermore, if referencing a schema that contains an example,
        /// the examples value SHALL override the example provided by the schema.
        /// </summary>
        public Dictionary<string, Example> Examples { get; set; } = new Dictionary<string, Example>();

        /// <summary>
        /// gets or sets a dictionary of <see cref="Reference"/> that can be used to populate the <see cref="Example"/> Dictionary
        /// once the complete Open API document has been deserialized
        /// </summary>
        internal Dictionary<string, Reference> ExamplesReferences { get; set; } = new Dictionary<string, Reference>();

        /// <summary>
        /// A map containing the representations for the parameter. The key is the media type and the value describes it.
        /// The map MUST only contain one entry.
        /// </summary>
        public Dictionary<string, MediaType> Content { get; set; } = new Dictionary<string, MediaType>();
    }
}
