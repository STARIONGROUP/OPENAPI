// -------------------------------------------------------------------------------------------------
// <copyright file="ParameterDeSerializer.cs" company="RHEA System S.A.">
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
    /// The purpose of the <see cref="ParameterDeSerializer"/> is to deserialize the <see cref="Parameter"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#parameter-object
    /// </remarks>
    internal class ParameterDeSerializer : ReferencerDeserializer
    {
        /// <summary>
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<ParameterDeSerializer> logger;

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
        internal ParameterDeSerializer(ReferenceResolver referenceResolver, ILoggerFactory loggerFactory = null) 
            : base(referenceResolver)
        {
            this.loggerFactory = loggerFactory;

            this.logger = this.loggerFactory == null ? NullLogger<ParameterDeSerializer>.Instance : this.loggerFactory.CreateLogger<ParameterDeSerializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="Parameter"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Parameter"/> json object
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <returns>
        /// an instance of <see cref="Parameter"/>
        /// </returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Parameter"/> object
        /// </exception>
        internal Parameter DeSerialize(JsonElement jsonElement, bool strict)
        {
            this.logger.LogTrace("Start ParameterDeSerializer.DeSerialize");

            var parameter = new Parameter();

            if (!jsonElement.TryGetProperty("name", out JsonElement nameProperty))
            {
                if (strict)
                {
                    throw new SerializationException("The REQUIRED Parameter.name property is not available, this is an invalid Parameter object");
                }
                else
                {
                    this.logger.LogWarning("The REQUIRED Parameter.name property is not available, this is an invalid Parameter object");
                }
            }
            else
            {
                parameter.Name = nameProperty.GetString();
            }

            if (!jsonElement.TryGetProperty("in", out JsonElement inProperty))
            {
                if (strict)
                {
                    throw new SerializationException("The REQUIRED Parameter.in property is not available, this is an invalid Parameter object");
                }
                else
                {
                    this.logger.LogWarning("The REQUIRED Parameter.in property is not available, this is an invalid Parameter object");
                }
            }
            else
            {
                parameter.In = inProperty.GetString();
            }

            if (jsonElement.TryGetProperty("description", out JsonElement descriptionProperty))
            {
                parameter.Description = descriptionProperty.GetString();
            }

            if (jsonElement.TryGetProperty("required", out JsonElement requiredProperty))
            {
                parameter.Required = requiredProperty.GetBoolean();
            }

            if (jsonElement.TryGetProperty("deprecated", out JsonElement deprecatedProperty))
            {
                parameter.Deprecated = deprecatedProperty.GetBoolean();
            }

            if (jsonElement.TryGetProperty("allowEmptyValue", out JsonElement allowEmptyValueProperty))
            {
                parameter.AllowEmptyValue = allowEmptyValueProperty.GetBoolean();
            }

            if (jsonElement.TryGetProperty("style", out JsonElement styleProperty))
            {
                parameter.Style = styleProperty.GetString();
            }

            if (jsonElement.TryGetProperty("explode", out JsonElement explodeProperty))
            {
                parameter.Explode = explodeProperty.GetBoolean();
            }

            if (jsonElement.TryGetProperty("allowReserved", out JsonElement allowReservedProperty))
            {
                parameter.AllowReserved = allowReservedProperty.GetBoolean();
            }

            if (jsonElement.TryGetProperty("schema", out JsonElement schemaProperty))
            {
                var schemaDeSerializer = new SchemaDeSerializer(this.referenceResolver, this.loggerFactory);
                parameter.Schema = schemaDeSerializer.DeSerialize(schemaProperty, strict);
            }

            if (jsonElement.TryGetProperty("example", out JsonElement exampleProperty))
            {
                parameter.Example = exampleProperty.ToString();
            }

            this.DeserializeExamples(jsonElement, parameter, strict);

            this.DeserializeContent(jsonElement, parameter, strict);

            this.logger.LogTrace("Finish ParameterDeSerializer.DeSerialize");

            return parameter;
        }

        /// <summary>
        /// Deserializes the Parameter  <see cref="Example"/>s from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Parameter "/> json object
        /// </param>
        /// <param name="parameter">
        /// The <see cref="Parameter "/> that is being deserialized
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Parameter "/> object
        /// </exception>
        private void DeserializeExamples(JsonElement jsonElement, Parameter parameter, bool strict)
        {
            if (jsonElement.TryGetProperty("examples", out JsonElement examplesProperty))
            {
                var exampleDeSerializer = new ExampleDeSerializer(this.loggerFactory);
                var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);

                foreach (var itemProperty in examplesProperty.EnumerateObject())
                {
                    if (itemProperty.Value.TryGetProperty("$ref", out var referenceElement))
                    {
                        var reference = referenceDeSerializer.DeSerialize(itemProperty.Value, strict);
                        parameter.ExamplesReferences.Add(itemProperty.Name, reference);
                        this.Register(reference, parameter, "Examples", itemProperty.Name);
                    }
                    else
                    {
                        var example = exampleDeSerializer.DeSerialize(itemProperty.Value);
                        parameter.Examples.Add(itemProperty.Name, example);
                    }
                }
            }
        }

        /// <summary>
        /// Deserializes the Parameter.Content <see cref="MediaType"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Parameter"/> json object
        /// </param>
        /// <param name="parameter">
        /// The <see cref="Parameter"/> that is being deserialized
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Parameter"/> object
        /// </exception>
        private void DeserializeContent(JsonElement jsonElement, Parameter parameter, bool strict)
        {
            if (jsonElement.TryGetProperty("content", out JsonElement contentProperty))
            {
                var mediaTypeDeSerializer = new MediaTypeDeSerializer(this.referenceResolver, this.loggerFactory);

                foreach (var x in contentProperty.EnumerateObject())
                {
                    var mediaTypeName = x.Name;

                    var mediaType = mediaTypeDeSerializer.DeSerialize(x.Value, strict);

                    parameter.Content.Add(mediaTypeName, mediaType);
                }
            }
        }
    }
}
