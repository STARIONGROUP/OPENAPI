// -------------------------------------------------------------------------------------------------
// <copyright file="ResponseDeserializer.cs" company="RHEA System S.A.">
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
    /// The purpose of the <see cref="ResponseDeserializer"/> is to deserialize the <see cref="Response"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#response-object
    /// </remarks>
    internal class ResponseDeserializer : ReferencerDeserializer
    {
        /// <summary>
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<ResponseDeserializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseDeserializer"/> class.
        /// </summary>
        /// <param name="referenceResolver">
        /// The <see cref="ReferenceResolver"/> that is sed to register any <see cref="ReferenceInfo"/> objects
        /// and later resolve them
        /// </param>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal ResponseDeserializer(ReferenceResolver referenceResolver, ILoggerFactory loggerFactory = null) 
            : base(referenceResolver)
        {
            this.loggerFactory = loggerFactory;

            this.logger = this.loggerFactory == null ? NullLogger<ResponseDeserializer>.Instance : this.loggerFactory.CreateLogger<ResponseDeserializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="Response"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Response"/> json object
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <returns>
        /// An instance of an Open Api <see cref="Response"/>
        /// </returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Response"/> object
        /// </exception>
        internal Response DeSerialize(JsonElement jsonElement, bool strict)
        {
            this.logger.LogTrace("Start Response.DeSerialize");

            var response = new Response();

            if (!jsonElement.TryGetProperty("description"u8, out JsonElement descriptionProperty))
            {
                if (strict)
                {
                    throw new SerializationException("The REQUIRED Response.description property is not available, this is an invalid OpenAPI document");
                }
                else
                {
                    this.logger.LogWarning("The REQUIRED Response.description property is not available, this is an invalid OpenAPI document");
                }
            }
            else
            {
                response.Description = descriptionProperty.GetString();
            }
            
            this.DeserializeHeaders(jsonElement, response, strict);

            this.DeserializeContent(jsonElement, response, strict);
            
            this.DeserializeLinks(jsonElement, response, strict);

            this.logger.LogTrace("Finish Response.DeSerialize");

            return response;
        }

        /// <summary>
        /// Deserializes the Components.parameters from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Response"/> json object
        /// </param>
        /// <param name="response">
        /// The <see cref="Response"/> that is being deserialized
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Response"/> object
        /// </exception>
        private void DeserializeHeaders(JsonElement jsonElement, Response response, bool strict)
        {
            if (jsonElement.TryGetProperty("headers"u8, out JsonElement parametersProperty))
            {
                var headerDeSerializer = new HeaderDeSerializer(this.referenceResolver, this.loggerFactory);
                var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);

                foreach (var itemProperty in parametersProperty.EnumerateObject())
                {
                    if (itemProperty.Value.TryGetProperty("$ref", out var referenceElement))
                    {
                        var reference = referenceDeSerializer.DeSerialize(itemProperty.Value, strict);
                        response.HeadersReferences.Add(itemProperty.Name, reference);
                        this.Register(reference, response, "Headers", itemProperty.Name);
                    }
                    else
                    {
                        var header = headerDeSerializer.DeSerialize(itemProperty.Value, strict);
                        response.Headers.Add(itemProperty.Name, header);
                    }
                }
            }
        }

        /// <summary>
        /// Deserializes the Header.Content <see cref="MediaType"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Header"/> json object
        /// </param>
        /// <param name="response">
        /// The <see cref="Response"/> that is being deserialized
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Header"/> object
        /// </exception>
        private void DeserializeContent(JsonElement jsonElement, Response response, bool strict)
        {
            if (jsonElement.TryGetProperty("content"u8, out JsonElement contentProperty))
            {
                var mediaTypeDeSerializer = new MediaTypeDeSerializer(this.referenceResolver, this.loggerFactory);

                foreach (var x in contentProperty.EnumerateObject())
                {
                    var mediaTypeName = x.Name;

                    var mediaType = mediaTypeDeSerializer.DeSerialize(x.Value, strict);

                    response.Content.Add(mediaTypeName, mediaType);
                }
            }
        }

        /// <summary>
        /// Deserializes the Components.parameters from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Response"/> json object
        /// </param>
        /// <param name="response">
        /// The <see cref="Response"/> that is being deserialized
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Response"/> object
        /// </exception>
        private void DeserializeLinks(JsonElement jsonElement, Response response, bool strict)
        {
            if (jsonElement.TryGetProperty("links"u8, out JsonElement parametersProperty))
            {
                var linkDeSerializer = new LinkDeSerializer(this.loggerFactory);
                var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);

                foreach (var itemProperty in parametersProperty.EnumerateObject())
                {
                    if (itemProperty.Value.TryGetProperty("$ref"u8, out var referenceElement))
                    {
                        var reference = referenceDeSerializer.DeSerialize(itemProperty.Value, strict);
                        response.LinksReferences.Add(itemProperty.Name, reference);
                        this.Register(reference, response, "links", itemProperty.Name);
                    }
                    else
                    {
                        var link = linkDeSerializer.DeSerialize(itemProperty.Value, strict);
                        response.Links.Add(itemProperty.Name, link);
                    }
                }
            }
        }
    }
}
