// -------------------------------------------------------------------------------------------------
// <copyright file="SchemaDeSerializer.cs" company="RHEA System S.A.">
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
    /// The purpose of the <see cref="SchemaDeSerializer"/> is to deserialize the <see cref="Schema"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#schema-object
    /// </remarks>
    internal class SchemaDeSerializer
    {
        /// <summary>
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<SchemaDeSerializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscriminatorDeSerializer"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal SchemaDeSerializer(ILoggerFactory loggerFactory = null)
        {
            this.loggerFactory = loggerFactory;

            this.logger = this.loggerFactory == null ? NullLogger<SchemaDeSerializer>.Instance : this.loggerFactory.CreateLogger<SchemaDeSerializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="Schema"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Schema"/> json object
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <returns>
        /// an instance of <see cref="Schema"/>
        /// </returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Schema"/> object
        /// </exception>
        internal Schema DeSerialize(JsonElement jsonElement, bool strict)
        {
            this.logger.LogTrace("Start SchemaDeSerializer.DeSerialize");

            var schema = new Schema();

            if (jsonElement.TryGetProperty("discriminator", out JsonElement discriminatorProperty))
            {
                var discriminatorDeSerializer = new DiscriminatorDeSerializer(this.loggerFactory);
                schema.Discriminator = discriminatorDeSerializer.DeSerialize(discriminatorProperty, strict);
            }

            if (jsonElement.TryGetProperty("xml", out JsonElement xmlProperty))
            {
                var xmlDeSerializer = new XMLDeSerializer(this.loggerFactory);
                schema.XML = xmlDeSerializer.DeSerialize(xmlProperty);
            }

            if (jsonElement.TryGetProperty("externalDocs", out JsonElement externalDocsProperty))
            {
                var externalDocumentationDeSerializer = new ExternalDocumentationDeSerializer(this.loggerFactory);
                schema.ExternalDocs = externalDocumentationDeSerializer.DeSerialize(externalDocsProperty, strict);

            }

            if (jsonElement.TryGetProperty("example", out JsonElement exampleProperty))
            {
                schema.Example = exampleProperty.ToString();
            }

            this.logger.LogTrace("Finish SchemaDeSerializer.DeSerialize");

            return schema;
        }
    }
}
