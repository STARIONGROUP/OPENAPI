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
    internal class DocumentDeserializer : ReferencerDeserializer
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
        /// <param name="referenceResolver">
        /// The <see cref="ReferenceResolver"/> that is used to register any <see cref="ReferenceInfo"/> objects
        /// and later resolve them
        /// </param>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal DocumentDeserializer(ReferenceResolver referenceResolver, ILoggerFactory loggerFactory = null)
            : base(referenceResolver)
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
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <returns>
        /// An instance of an Open Api <see cref="Document"/>
        /// </returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Document"/> object
        /// </exception>
        internal Document DeSerialize(JsonElement jsonElement, bool strict)
        {
            this.logger.LogTrace("Start DocumentDeserializer.DeSerialize");

            var document = new Document();

            if (!jsonElement.TryGetProperty("openapi"u8, out JsonElement openapiProperty))
            {
                if (strict)
                {
                    throw new SerializationException("The REQUIRED Document.openapi property is not available, this is an invalid OpenAPI document");
                }
                else
                {
                    this.logger.LogWarning("The REQUIRED Document.openapi property is not available, this is an invalid OpenAPI document");
                }
            }
            else
            {
                document.OpenApi = openapiProperty.GetString();
            }

            this.DeserializeInfo(jsonElement, document, strict);
            
            if (jsonElement.TryGetProperty("jsonSchemaDialect"u8, out JsonElement jsonSchemaDialectProperty))
            {
                document.JsonSchemaDialect = jsonSchemaDialectProperty.GetString();
            }

            this.DeserializeServers(jsonElement, document, strict);

            this.DeserializePathItems(jsonElement, document, strict);

            this.DeserializeWebhooks(jsonElement, document, strict);

            this.DeserializeComponents(jsonElement, document, strict);
            
            this.DeserializeSecurityRequirements(jsonElement, document);

            this.DeserializeTags(jsonElement, document, strict);

            this.DeserializeExternalDocumentation(jsonElement, document, strict);

            this.logger.LogTrace("Finish DocumentDeserializer.DeSerialize");

            return document;
        }

        /// <summary>
        /// Deserializes the <see cref="Info"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Document"/> json object
        /// </param>
        /// <param name="document">
        /// The <see cref="Document"/> that is being deserialized
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Document"/> object
        /// </exception>
        private void DeserializeInfo(JsonElement jsonElement, Document document, bool strict)
        {
            if (!jsonElement.TryGetProperty("info"u8, out JsonElement infoProperty))
            {
                if (strict)
                {
                    throw new SerializationException("The REQUIRED info property is not available, this is an invalid OpenAPI document");
                }
                else
                {
                    this.logger.LogWarning("The REQUIRED info property is not available, this is an invalid OpenAPI document");
                }
            }
            else
            {
                var infoDeserializer = new InfoDeserializer(this.loggerFactory);
                document.Info = infoDeserializer.DeSerialize(infoProperty, strict);
            }
        }

        /// <summary>
        /// Deserializes the <see cref="Server"/>s from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Document"/> json object
        /// </param>
        /// <param name="document">
        /// The <see cref="Document"/> that is being deserialized
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Document"/> object
        /// </exception>
        private void DeserializeServers(JsonElement jsonElement, Document document, bool strict)
        {
            if (jsonElement.TryGetProperty("servers"u8, out JsonElement serversProperty) && serversProperty.ValueKind == JsonValueKind.Array)
            {
                var serverDeSerializer = new ServerDeSerializer(this.loggerFactory);

                foreach (var arrayItem in serversProperty.EnumerateArray())
                {
                    var server = serverDeSerializer.DeSerialize(arrayItem, strict);
                    document.Servers.Add(server);
                }
            }
        }

        /// <summary>
        /// Deserializes the <see cref="PathItem"/>s from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Document"/> json object
        /// </param>
        /// <param name="document">
        /// The <see cref="Document"/> that is being deserialized
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Document"/> object
        /// </exception>
        private void DeserializePathItems(JsonElement jsonElement, Document document, bool strict)
        {
            if (jsonElement.TryGetProperty("paths"u8, out JsonElement pathsProperty))
            {
                var pathItemDeserializer = new PathItemDeserializer(this.referenceResolver, this.loggerFactory);

                foreach (var p in pathsProperty.EnumerateObject())
                {
                    var pathItemName = p.Name;

                    var pathItem = pathItemDeserializer.DeSerialize(p.Value, strict);

                    document.Paths.Add(pathItemName, pathItem);
                }
            }
        }

        /// <summary>
        /// Deserializes the web hook <see cref="PathItem"/>s from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Document"/> json object
        /// </param>
        /// <param name="document">
        /// The <see cref="Document"/> that is being deserialized
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Document"/> object
        /// </exception>
        private void DeserializeWebhooks(JsonElement jsonElement, Document document, bool strict)
        {
            if (jsonElement.TryGetProperty("webhooks"u8, out JsonElement webhooksProperty))
            {
                var pathItemDeserializer = new PathItemDeserializer(this.referenceResolver, this.loggerFactory);
                var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);
                
                foreach (var itemProperty in webhooksProperty.EnumerateObject())
                {
                    var key = itemProperty.Name;

                    if (itemProperty.Value.TryGetProperty("$ref", out var referenceElement))
                    {
                        var reference = referenceDeSerializer.DeSerialize(itemProperty.Value, strict);
                        document.WebhooksReferences.Add(key, reference);
                        this.Register(reference, document, "Webhooks", key);
                    }
                    else
                    {
                        var pathItem = pathItemDeserializer.DeSerialize(itemProperty.Value, strict);
                        document.Webhooks.Add(key, pathItem);
                    }
                }
            }
        }

        /// <summary>
        /// Deserializes the <see cref="Components"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Document"/> json object
        /// </param>
        /// <param name="document">
        /// The <see cref="Document"/> that is being deserialized
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Document"/> object
        /// </exception>
        private void DeserializeComponents(JsonElement jsonElement, Document document, bool strict)
        {
            if (jsonElement.TryGetProperty("components"u8, out JsonElement componentsProperty))
            {
                var componentsDeSerializer = new ComponentsDeSerializer(this.referenceResolver, this.loggerFactory);

                document.Components = componentsDeSerializer.DeSerialize(componentsProperty, strict);
            }
        }

        /// <summary>
        /// Deserializes the <see cref="SecurityRequirement"/>s from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Document"/> json object
        /// </param>
        /// <param name="document">
        /// The <see cref="Document"/> that is being deserialized
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Document"/> object
        /// </exception>
        private void DeserializeSecurityRequirements(JsonElement jsonElement, Document document)
        {
            if (jsonElement.TryGetProperty("security"u8, out JsonElement securityProperty) && securityProperty.ValueKind == JsonValueKind.Array)
            {
                var securityRequirementDeSerializer = new SecurityRequirementDeSerializer(this.loggerFactory);

                foreach (var arrayItem in securityProperty.EnumerateArray())
                {
                    var securityRequirement = securityRequirementDeSerializer.DeSerialize(arrayItem);
                    document.Security.Add(securityRequirement);
                }
            }
        }

        /// <summary>
        /// Deserializes the <see cref="Tag"/>s from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Document"/> json object
        /// </param>
        /// <param name="document">
        /// The <see cref="Document"/> that is being deserialized
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Document"/> object
        /// </exception>
        private void DeserializeTags(JsonElement jsonElement, Document document, bool strict)
        {
            if (jsonElement.TryGetProperty("tags"u8, out JsonElement tagsProperty) && tagsProperty.ValueKind == JsonValueKind.Array)
            {
                var tagDeSerializer = new TagDeSerializer(this.loggerFactory);

                foreach (var arrayItem in tagsProperty.EnumerateArray())
                {
                    var tag = tagDeSerializer.DeSerialize(arrayItem, strict);
                    document.Tags.Add(tag);
                }
            }
        }

        /// <summary>
        /// Deserializes the <see cref="ExternalDocumentation"/>s from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Document"/> json object
        /// </param>
        /// <param name="document">
        /// The <see cref="Document"/> that is being deserialized
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Document"/> object
        /// </exception>
        private void DeserializeExternalDocumentation(JsonElement jsonElement, Document document, bool strict)
        {
            if (jsonElement.TryGetProperty("externalDocs"u8, out JsonElement externalDocsProperty))
            {
                var externalDocumentationDeSerializer = new ExternalDocumentationDeSerializer(this.loggerFactory);

                document.ExternalDocs = externalDocumentationDeSerializer.DeSerialize(externalDocsProperty, strict);
            }
        }
    }
}
