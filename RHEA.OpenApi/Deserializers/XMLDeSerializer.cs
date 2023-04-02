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
            var xml = new XML();

            if (jsonElement.TryGetProperty("name", out JsonElement nameProperty))
            {
                xml.Name = nameProperty.GetString();
            }
            else
            {
                this.logger.LogTrace("The optional XML.name property is not provided in the OpenApi document");
            }

            if (jsonElement.TryGetProperty("namespace", out JsonElement namespaceProperty))
            {
                xml.Namespace = namespaceProperty.GetString();
            }
            else
            {
                this.logger.LogTrace("The optional XML.namespace property is not provided in the OpenApi document");
            }

            if (jsonElement.TryGetProperty("prefix", out JsonElement prefixProperty))
            {
                xml.Prefix = prefixProperty.GetString();
            }
            else
            {
                this.logger.LogTrace("The optional XML.prefix property is not provided in the OpenApi document");
            }

            if (jsonElement.TryGetProperty("attribute", out JsonElement attributeProperty))
            {
                xml.Attribute = attributeProperty.GetBoolean();
            }
            else
            {
                this.logger.LogTrace("The optional XML.attribute property is not provided in the OpenApi document");
            }

            if (jsonElement.TryGetProperty("wrapped", out JsonElement wrappedProperty))
            {
                xml.Wrapped = wrappedProperty.GetBoolean();
            }
            else
            {
                this.logger.LogTrace("The optional XML.wrapped property is not provided in the OpenApi document");
            }

            return xml;
        }
    }
}
