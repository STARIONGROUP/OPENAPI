// -------------------------------------------------------------------------------------------------
// <copyright file="XMLDeSerializer.cs" company="RHEA System S.A.">
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
    using System.Runtime.Serialization;
    using System.Text.Json;

    using Microsoft.Extensions.Logging;

    using Microsoft.Extensions.Logging.Abstractions;
    using OpenApi.Model;

    /// <summary>
    /// The purpose of the <see cref="XMLDeSerializer"/> is to deserialize the <see cref="XML"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#xml-object
    /// </remarks>
    internal class XMLDeSerializer
    {
        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<XMLDeSerializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="XMLDeSerializer"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal XMLDeSerializer(ILoggerFactory loggerFactory = null)
        {
            this.logger = loggerFactory == null ? NullLogger<XMLDeSerializer>.Instance : loggerFactory.CreateLogger<XMLDeSerializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="XML"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="XML"/> json object
        /// </param>
        /// <returns></returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="XML"/> object
        /// </exception>
        internal XML DeSerialize(JsonElement jsonElement)
        {
            this.logger.LogTrace("Start XMLDeSerializer.DeSerialize");

            var xml = new XML();

            if (jsonElement.TryGetProperty("name"u8, out JsonElement nameProperty))
            {
                xml.Name = nameProperty.GetString();
            }

            if (jsonElement.TryGetProperty("namespace"u8, out JsonElement namespaceProperty))
            {
                xml.Namespace = namespaceProperty.GetString();
            }

            if (jsonElement.TryGetProperty("prefix"u8, out JsonElement prefixProperty))
            {
                xml.Prefix = prefixProperty.GetString();
            }

            if (jsonElement.TryGetProperty("attribute"u8, out JsonElement attributeProperty))
            {
                xml.Attribute = attributeProperty.GetBoolean();
            }

            if (jsonElement.TryGetProperty("wrapped"u8, out JsonElement wrappedProperty))
            {
                xml.Wrapped = wrappedProperty.GetBoolean();
            }

            this.logger.LogTrace("Finish XMLDeSerializer.DeSerialize");

            return xml;
        }
    }
}
