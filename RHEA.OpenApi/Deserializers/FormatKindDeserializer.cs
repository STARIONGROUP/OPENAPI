// -------------------------------------------------------------------------------------------------
// <copyright file="FormatKindDeserializer.cs" company="RHEA System S.A.">
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

namespace OpenApi.Deserializers
{
    using OpenApi.JsonSchema;

    /// <summary>
    /// The purpose of the <see cref="FormatKindDeserializer"/> is to provide deserialization capabilities
    /// for the <see cref="FormatKind"/> Enum
    /// </summary>
    internal static class FormatKindDeserializer
    {
        /// <summary>
        /// Deserializes a string value to a <see cref="FormatKind"/>
        /// </summary>
        /// <param name="value">
        /// The string representation of the <see cref="FormatKind"/>
        /// </param>
        /// <returns>
        /// The value of the <see cref="FormatKind"/>
        /// </returns>
        internal static FormatKind Deserialize(string value)
        {
            switch (value)
            {
                case "date-time":
                    return FormatKind.DateTime;
                case "date":
                    return FormatKind.Date;
                case "time":
                    return FormatKind.Time;
                case "duration":
                    return FormatKind.Duration;
                case "email":
                    return FormatKind.Email;
                case "idn-email":
                    return FormatKind.IdnEmail;
                case "hostname":
                    return FormatKind.Hostnane;
                case "idn-hostname":
                    return FormatKind.IdnHostname;
                case "ipv4":
                    return FormatKind.Ipv4;
                case "ipv6":
                    return FormatKind.Ipv6;
                case "uri":
                    return FormatKind.Uri;
                case "uri-reference":
                    return FormatKind.UriReference;
                case "iri":
                    return FormatKind.Iri;
                case "iri-reference":
                    return FormatKind.IriReference;
                case "uuid":
                    return FormatKind.Uuid;
                case "uri-template":
                    return FormatKind.UriTemplate;
                case "json-pointer":
                    return FormatKind.JsonPointer;
                case "relative-json-pointer":
                    return FormatKind.RelativeJsonPointer;
                case "regex":
                    return FormatKind.Regex;
                default:
                    return FormatKind.Unknown;
            }
        }
    }
}
