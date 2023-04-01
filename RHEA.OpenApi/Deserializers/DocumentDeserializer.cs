﻿// -------------------------------------------------------------------------------------------------
// <copyright file="DocumentDeserializer.cs" company="RHEA System S.A.">
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
    /// The purpose of the <see cref="DocumentDeserializer"/> is to deserialize the <see cref="Document"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#openapi-object
    /// </remarks>
    internal class DocumentDeserializer
    {
        /// <summary>
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<DocumentDeserializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentDeserializer"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal DocumentDeserializer(ILoggerFactory loggerFactory = null)
        {
            this.loggerFactory = loggerFactory;

            this.logger = this.loggerFactory == null ? NullLogger<DocumentDeserializer>.Instance : this.loggerFactory.CreateLogger<DocumentDeserializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="Document"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Document"/> json object
        /// </param>
        /// <returns></returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Document"/>
        /// </exception>
        public Document DeSerialize(JsonElement jsonElement)
        {
            var document = new Document();

            if (!jsonElement.TryGetProperty("openapi", out JsonElement openapiProperty))
            {
                throw new SerializationException("The REQUIRED openapi property is not available, this is an invalid OpenAPI document");
            }

            document.OpenApi = openapiProperty.GetString();

            if (!jsonElement.TryGetProperty("info", out JsonElement infoProperty))
            {
                throw new SerializationException("The REQUIRED info property is not available, this is an invalid OpenAPI document");
            }

            var infoDeserializer = new InfoDeserializer(this.loggerFactory);
            document.Info = infoDeserializer.DeSerialize(infoProperty);

            if (jsonElement.TryGetProperty("jsonSchemaDialect", out JsonElement jsonSchemaDialectProperty))
            {
                var propertyValue = jsonSchemaDialectProperty.GetString();
                document.JsonSchemaDialect = propertyValue;
            }


            // servers

            // paths

            // webhooks

            // components

            // security

            // tags

            // externalDocs

            return document;
        }
    }
}