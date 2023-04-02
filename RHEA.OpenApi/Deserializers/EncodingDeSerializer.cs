// -------------------------------------------------------------------------------------------------
// <copyright file="EncodingDeSerializer.cs" company="RHEA System S.A.">
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
    /// The purpose of the <see cref="EncodingDeSerializer"/> is to deserialize the <see cref="Encoding"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#encoding-object
    /// </remarks>
    internal class EncodingDeSerializer
    {
        /// <summary>
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<EncodingDeSerializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactDeSerializer"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal EncodingDeSerializer(ILoggerFactory loggerFactory = null)
        {
            this.loggerFactory = loggerFactory;

            this.logger = this.loggerFactory == null ? NullLogger<EncodingDeSerializer>.Instance : this.loggerFactory.CreateLogger<EncodingDeSerializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="Encoding"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Encoding"/> json object
        /// </param>
        /// <returns>
        /// An instance of an Open Api <see cref="Document"/>
        /// </returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Encoding"/> object
        /// </exception>
        internal Encoding DeSerialize(JsonElement jsonElement)
        {
            var encoding = new Encoding();

            if (jsonElement.TryGetProperty("contentType", out JsonElement contentTypeProperty))
            {
                encoding.ContentType = contentTypeProperty.GetString();
            }
            else
            {
                this.logger.LogTrace("The optional Encoding.contentType property is not provided in the OpenApi document");
            }

            this.DeserializeHeaders(jsonElement, encoding);
            
            if (jsonElement.TryGetProperty("style", out JsonElement styleProperty))
            {
                encoding.Style = styleProperty.GetString();
            }
            else
            {
                this.logger.LogTrace("The optional Encoding.style property is not provided in the OpenApi document");
            }
            
            if (jsonElement.TryGetProperty("explode", out JsonElement explodeProperty))
            {
                encoding.Explode = explodeProperty.GetBoolean();
            }
            else
            {
                this.logger.LogTrace("The optional Encoding.explode property is not provided in the OpenApi document");
            }

            if (jsonElement.TryGetProperty("allowReserved", out JsonElement allowReservedProperty))
            {
                encoding.AllowReserved = allowReservedProperty.GetBoolean();
            }
            else
            {
                this.logger.LogTrace("The optional Encoding.allowReserved property is not provided in the OpenApi document");
            }

            return encoding;
        }

        /// <summary>
        /// Deserializes the web hook <see cref="Encoding"/>s from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Encoding"/> json object
        /// </param>
        /// <param name="encoding">
        /// The <see cref="Encoding"/> that is being deserialized
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Encoding"/> object
        /// </exception>
        private void DeserializeHeaders(JsonElement jsonElement, Encoding encoding)
        {
            if (jsonElement.TryGetProperty("headers", out JsonElement headersProperty))
            {
                var headerDeSerializer = new HeaderDeSerializer(this.loggerFactory);
                var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);

                foreach (var itemProperty in headersProperty.EnumerateObject())
                {
                    var key = itemProperty.Name;

                    foreach (var value in itemProperty.Value.EnumerateObject())
                    {
                        if (value.Name == "$ref")
                        {
                            var reference = referenceDeSerializer.DeSerialize(itemProperty.Value);
                            encoding.HeadersReferences.Add(key, reference);
                        }
                        else
                        {
                            var header = headerDeSerializer.DeSerialize(itemProperty.Value);
                            encoding.Headers.Add(key, header);
                        }
                    }
                }
            }
            else
            {
                this.logger.LogTrace("The optional Encoding.headers property is not provided in the OpenApi document");
            }
        }
    }
}
