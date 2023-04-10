// -------------------------------------------------------------------------------------------------
// <copyright file="HeaderDeSerializer.cs" company="RHEA System S.A.">
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
    /// The purpose of the <see cref="HeaderDeSerializer"/> is to deserialize the <see cref="Header"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#header-object
    /// </remarks>
    internal class HeaderDeSerializer : ReferencerDeserializer
    {
        /// <summary>
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<HeaderDeSerializer> logger;

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
        internal HeaderDeSerializer(ReferenceResolver referenceResolver, ILoggerFactory loggerFactory = null)
            : base(referenceResolver)
        {
            this.loggerFactory = loggerFactory;

            this.logger = this.loggerFactory == null ? NullLogger<HeaderDeSerializer>.Instance : this.loggerFactory.CreateLogger<HeaderDeSerializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="Header"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Header"/> json object
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <returns>
        /// An instance of an Open Api <see cref="Header"/>
        /// </returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Header"/> object
        /// </exception>
        internal Header DeSerialize(JsonElement jsonElement, bool strict)
        {
            this.logger.LogTrace("Start HeaderDeSerializer.DeSerialize");

            var header = new Header();

            if (jsonElement.TryGetProperty("name", out JsonElement nameProperty))
            {
                if (strict)
                {
                    throw new SerializationException("Header.name MUST NOT be specified, it is given in the corresponding headers map.");
                }
                else
                {
                    this.logger.LogWarning("Header.name MUST NOT be specified, it is given in the corresponding headers map.");
                }
            }

            if (jsonElement.TryGetProperty("in", out JsonElement inProperty))
            {
                if (strict)
                {
                    throw new SerializationException("Header.in MUST NOT be specified, it is implicitly in header.");
                }
                else
                {
                    this.logger.LogWarning("Header.in MUST NOT be specified, it is implicitly in header.");
                }
            }

            if (jsonElement.TryGetProperty("description", out JsonElement descriptionProperty))
            {
                header.Description = descriptionProperty.GetString();
            }

            if (jsonElement.TryGetProperty("required", out JsonElement requiredProperty))
            {
                header.Required = requiredProperty.GetBoolean();
            }

            if (jsonElement.TryGetProperty("deprecated", out JsonElement deprecatedProperty))
            {
                header.Deprecated = deprecatedProperty.GetBoolean();
            }

            if (jsonElement.TryGetProperty("allowEmptyValue", out JsonElement allowEmptyValueProperty))
            {
                header.AllowEmptyValue = allowEmptyValueProperty.GetBoolean();
            }

            if (jsonElement.TryGetProperty("style", out JsonElement styleProperty))
            {
                header.Style = styleProperty.GetString();
            }

            if (jsonElement.TryGetProperty("explode", out JsonElement explodeProperty))
            {
                header.Explode = explodeProperty.GetBoolean();
            }

            if (jsonElement.TryGetProperty("allowReserved", out JsonElement allowReservedProperty))
            {
                header.AllowReserved = allowReservedProperty.GetBoolean();
            }

            if (jsonElement.TryGetProperty("schema", out JsonElement schemaProperty))
            {
                var schemaDeSerializer = new SchemaDeSerializer(this.loggerFactory);
                header.Schema = schemaDeSerializer.DeSerialize(schemaProperty, strict);
            }

            if (jsonElement.TryGetProperty("example", out JsonElement exampleProperty))
            {
                header.Example = exampleProperty.ToString();
            }

            this.DeserializeExamples(jsonElement, header, strict);

            this.DeserializeContent(jsonElement, header, strict);

            this.logger.LogTrace("Finish HeaderDeSerializer.DeSerialize");

            return header;
        }

        /// <summary>
        /// Deserializes the header <see cref="Example"/>s from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Header"/> json object
        /// </param>
        /// <param name="header">
        /// The <see cref="Header"/> that is being deserialized
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Header"/> object
        /// </exception>
        private void DeserializeExamples(JsonElement jsonElement, Header header, bool strict)
        {
            if (jsonElement.TryGetProperty("examples", out JsonElement examplesProperty))
            {
                var exampleDeSerializer = new ExampleDeSerializer(this.loggerFactory);
                var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);

                foreach (var itemProperty in examplesProperty.EnumerateObject())
                {
                    var key = itemProperty.Name;

                    foreach (var value in itemProperty.Value.EnumerateObject())
                    {
                        if (value.Name == "$ref")
                        {
                            var reference = referenceDeSerializer.DeSerialize(itemProperty.Value, strict);
                            header.ExamplesReferences.Add(key, reference);

                            this.Register(reference, header, "Examples", key);
                        }
                        else
                        {
                            var example = exampleDeSerializer.DeSerialize(itemProperty.Value);
                            header.Examples.Add(key, example);
                        }
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
        /// <param name="header">
        /// The <see cref="Header"/> that is being deserialized
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Header"/> object
        /// </exception>
        private void DeserializeContent(JsonElement jsonElement, Header header, bool strict)
        {
            if (jsonElement.TryGetProperty("content", out JsonElement contentProperty))
            {
                var mediaTypeDeSerializer = new MediaTypeDeSerializer(this.referenceResolver, this.loggerFactory);

                foreach (var x in contentProperty.EnumerateObject())
                {
                    var mediaTypeName = x.Name;

                    var mediaType = mediaTypeDeSerializer.DeSerialize(x.Value, strict);

                    header.Content.Add(mediaTypeName, mediaType);
                }
            }
        }
    }
}
