// -------------------------------------------------------------------------------------------------
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
    using System.Collections.Generic;
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
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Document"/> object
        /// </exception>
        internal Document DeSerialize(JsonElement jsonElement)
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
                document.JsonSchemaDialect = jsonSchemaDialectProperty.GetString();
            }
            else
            {
                this.logger.LogTrace("The optional Document.jsonSchemaDialect property is not provided in the OpenApi document");
            }

            if (jsonElement.TryGetProperty("servers", out JsonElement serversProperty))
            {
                if (serversProperty.ValueKind == JsonValueKind.Array)
                {
                    var servers = new List<Server>();

                    var serverDeSerializer = new ServerDeSerializer(this.loggerFactory);

                    foreach (var arrayItem in serversProperty.EnumerateArray())
                    {
                        var server = serverDeSerializer.DeSerialize(arrayItem);
                        servers.Add(server);
                    }

                    document.Servers = servers.ToArray();
                }
                else
                {
                    throw new SerializationException("the servers property shall be an array");
                }
            }
            else
            {
                this.logger.LogTrace("The optional Document.servers property is not provided in the OpenApi document");
            }

            if (jsonElement.TryGetProperty("paths", out JsonElement pathsProperty))
            {
                var pathItemDeserializer = new PathItemDeserializer(this.loggerFactory);

                foreach (var p in pathsProperty.EnumerateObject())
                {
                    var pathItemName = p.Name;

                    var pathItem = pathItemDeserializer.DeSerialize(p.Value);

                    document.Paths.Add(pathItemName, pathItem);
                }
            }
            else
            {
                this.logger.LogWarning("The Document.paths property is not provided in the OpenApi document");
            }

            // webhooks
            if (jsonElement.TryGetProperty("webhooks", out JsonElement webhooksProperty))
            {
                this.logger.LogWarning("TODO: the Document.webhooks property is not yet supported");
            }
            else
            {
                this.logger.LogTrace("The optional Document.webhooks property is not provided in the OpenApi document");
            }

            if (jsonElement.TryGetProperty("components", out JsonElement componentsProperty))
            {
                var componentsDeSerializer = new ComponentsDeSerializer(this.loggerFactory);

                document.Components = componentsDeSerializer.DeSerialize(componentsProperty);
            }
            else
            {
                this.logger.LogTrace("The optional Document.components property is not provided in the OpenApi document");
            }

            // security
            if (jsonElement.TryGetProperty("security", out JsonElement securityProperty))
            {
                this.logger.LogWarning("TODO: the Document.security property is not yet supported");
            }
            else
            {
                this.logger.LogTrace("The optional Document.security property is not provided in the OpenApi document");
            }

            if (jsonElement.TryGetProperty("tags", out JsonElement tagsProperty))
            {
                if (tagsProperty.ValueKind == JsonValueKind.Array)
                {
                    var tags = new List<Tag>();

                    var tagDeSerializer = new TagDeSerializer(this.loggerFactory);

                    foreach (var arrayItem in tagsProperty.EnumerateArray())
                    {
                        var tag = tagDeSerializer.DeSerialize(arrayItem);
                        tags.Add(tag);
                    }

                    document.Tags = tags.ToArray();
                }
                else
                {
                    throw new SerializationException("the Document.tags property shall be an array");
                }
            }
            else
            {
                this.logger.LogTrace("The optional Document.tags property is not provided in the OpenApi document");
            }
            
            if (jsonElement.TryGetProperty("externalDocs", out JsonElement externalDocsProperty))
            {
                var externalDocumentationDeSerializer = new ExternalDocumentationDeSerializer(this.loggerFactory);

                document.externalDocs = externalDocumentationDeSerializer.DeSerialize(externalDocsProperty);
            }
            else
            {
                this.logger.LogTrace("The optional Document.externalDocs property is not provided in the OpenApi document");
            }

            return document;
        }
    }
}
