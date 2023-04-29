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
    internal class EncodingDeSerializer : ReferencerDeserializer
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
        /// <param name="referenceResolver">
        /// The <see cref="ReferenceResolver"/> that is used to register any <see cref="ReferenceInfo"/> objects
        /// and later resolve them
        /// </param>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal EncodingDeSerializer(ReferenceResolver referenceResolver, ILoggerFactory loggerFactory = null)
            : base(referenceResolver)
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
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <returns>
        /// An instance of an Open Api <see cref="Document"/>
        /// </returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Encoding"/> object
        /// </exception>
        internal Encoding DeSerialize(JsonElement jsonElement, bool strict)
        {
            this.logger.LogTrace("Start EncodingDeSerializer.DeSerialize");

            var encoding = new Encoding();

            if (jsonElement.TryGetProperty("contentType"u8, out JsonElement contentTypeProperty))
            {
                encoding.ContentType = contentTypeProperty.GetString();
            }

            this.DeserializeHeaders(jsonElement, encoding, strict);
            
            if (jsonElement.TryGetProperty("style"u8, out JsonElement styleProperty))
            {
                encoding.Style = styleProperty.GetString();
            }
            
            if (jsonElement.TryGetProperty("explode"u8, out JsonElement explodeProperty))
            {
                encoding.Explode = explodeProperty.GetBoolean();
            }

            if (jsonElement.TryGetProperty("allowReserved"u8, out JsonElement allowReservedProperty))
            {
                encoding.AllowReserved = allowReservedProperty.GetBoolean();
            }

            this.logger.LogTrace("Finish EncodingDeSerializer.DeSerialize");

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
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Encoding"/> object
        /// </exception>
        private void DeserializeHeaders(JsonElement jsonElement, Encoding encoding, bool strict)
        {
            if (jsonElement.TryGetProperty("headers"u8, out JsonElement headersProperty))
            {
                var headerDeSerializer = new HeaderDeSerializer(this.referenceResolver, this.loggerFactory);
                var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);

                foreach (var itemProperty in headersProperty.EnumerateObject())
                {
                    if (itemProperty.Value.TryGetProperty("$ref"u8, out var referenceElement))
                    {
                        var reference = referenceDeSerializer.DeSerialize(itemProperty.Value, strict);
                        encoding.HeadersReferences.Add(itemProperty.Name, reference);
                        this.Register(reference, encoding, "Headers", itemProperty.Name);
                    }
                    else
                    {
                        var header = headerDeSerializer.DeSerialize(itemProperty.Value, strict);
                        encoding.Headers.Add(itemProperty.Name, header);
                    }
                }
            }
        }
    }
}
