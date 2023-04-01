// -------------------------------------------------------------------------------------------------
// <copyright file="Encoding.cs" company="RHEA System S.A.">
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
    /// A single encoding definition applied to a single schema property.
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#encoding-object
    /// </remarks>
    public class Encoding
    {
        /// <summary>
        /// The Content-Type for encoding a specific property. Default value depends on the property type: for object - application/json;
        /// for array – the default is defined based on the inner type; for all other cases the default is application/octet-stream.
        /// The value can be a specific media type (e.g. application/json), a wildcard media type (e.g. image/*), or a comma-separated list of the two types.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// A map allowing additional information to be provided as headers, for example Content-Disposition.
        /// Content-Type is described separately and SHALL be ignored in this section.
        /// This property SHALL be ignored if the request body media type is not a multipart.
        /// </summary>
        public Dictionary<string, Header> Headers { get; set; }

        /// <summary>
        /// Describes how a specific property value will be serialized depending on its type.
        /// See Parameter Object for details on the style property.
        /// The behavior follows the same values as query parameters, including default values. This property SHALL be ignored if
        /// the request body media type is not application/x-www-form-urlencoded or multipart/form-data.
        /// If a value is explicitly defined, then the value of contentType (implicit or explicit) SHALL be ignored.
        /// </summary>
        public string Style { get; set; }

        /// <summary>
        /// When this is true, property values of type array or object generate separate parameters for each value of the array,
        /// or key-value-pair of the map. For other types of properties this property has no effect. When style is form, the
        /// default value is true. For all other styles, the default value is false. This property SHALL be ignored if the request
        /// body media type is not application/x-www-form-urlencoded or multipart/form-data. If a value is explicitly defined, then
        /// the value of contentType (implicit or explicit) SHALL be ignored.
        /// </summary>
        public bool Explode { get; set; }

        /// <summary>
        /// Determines whether the parameter value SHOULD allow reserved characters, as defined by [RFC3986] :/?#[]@!$&'()*+,;= to be included without
        /// percent-encoding. The default value is false. This property SHALL be ignored if the request body media type is not
        /// application/x-www-form-urlencoded or multipart/form-data. If a value is explicitly defined, then the value of contentType
        /// (implicit or explicit) SHALL be ignored.
        /// </summary>
        public bool AllowReserved { get; set; }
    }
}
