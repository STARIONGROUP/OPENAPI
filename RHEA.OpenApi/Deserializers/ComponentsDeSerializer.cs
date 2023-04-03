// -------------------------------------------------------------------------------------------------
// <copyright file="ComponentsDeSerializer.cs" company="RHEA System S.A.">
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
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using System.Text.Json;

    using Microsoft.Extensions.Logging;

    using Microsoft.Extensions.Logging.Abstractions;
    using OpenApi.Model;

    /// <summary>
    /// The purpose of the <see cref="ComponentsDeSerializer"/> is to deserialize the <see cref="Components"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#componentsObject
    /// </remarks>
    internal class ComponentsDeSerializer
    {
        /// <summary>
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<ComponentsDeSerializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactDeSerializer"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal ComponentsDeSerializer(ILoggerFactory loggerFactory = null)
        {
            this.loggerFactory = loggerFactory;

            this.logger = this.loggerFactory == null ? NullLogger<ComponentsDeSerializer>.Instance : this.loggerFactory.CreateLogger<ComponentsDeSerializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="Components"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Components"/> json object
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <returns>
        /// an instance of <see cref="Components"/>
        /// </returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Components"/> object
        /// </exception>
        internal Components DeSerialize(JsonElement jsonElement, bool strict)
        {
            this.logger.LogTrace("Start ComponentsDeSerializer.DeSerialize");

            var components = new Components();

            this.DeserializeSchemas(jsonElement, components, strict);

            this.DeserializeResponses(jsonElement, components);
            
            this.DeserializeParameters(jsonElement, components, strict);

            this.DeserializeExamples(jsonElement, components, strict);

            this.DeserializeRequestBodies(jsonElement, components, strict);

            this.DeserializeHeaders(jsonElement, components, strict);

            this.DeserializeSecuritySchemes(jsonElement, components, strict);

            this.DeserializeLinks(jsonElement, components, strict);
            
            if (jsonElement.TryGetProperty("callbacks", out JsonElement callbacksProperty))
            {
                this.logger.LogWarning("callbacks are not yet supported");
            }
            
            if (jsonElement.TryGetProperty("pathItems", out JsonElement pathItemsProperty))
            {
                // TODO: add support for reference objects

                var pathItemsDeSerializer = new PathItemDeserializer(this.loggerFactory);

                foreach (var p in pathItemsProperty.EnumerateObject())
                {
                    var pathItemName = p.Name;

                    var pathItem = pathItemsDeSerializer.DeSerialize(p.Value, strict);

                    components.PathItems.Add(pathItemName, pathItem);
                }
            }

            this.logger.LogTrace("Finish ComponentsDeSerializer.DeSerialize");

            return components;
        }

        /// <summary>
        /// Deserializes the Components.Schemas from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Component"/> json object
        /// </param>
        /// <param name="components">
        /// The <see cref="Components"/> that is being deserialized
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Component"/> object
        /// </exception>
        private void DeserializeSchemas(JsonElement jsonElement, Components components, bool strict)
        {
            if (jsonElement.TryGetProperty("schemas", out JsonElement schemasProperty))
            {
                var schemaDeSerializer = new SchemaDeSerializer(this.loggerFactory);

                foreach (var item in schemasProperty.EnumerateObject())
                {
                    var key = item.Name;

                    var schema = schemaDeSerializer.DeSerialize(item.Value, strict);

                    components.Schemas.Add(key, schema);
                }
            }
        }

        /// <summary>
        /// Deserializes the Parameter.Content <see cref="MediaType"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Component"/> json object
        /// </param>
        /// <param name="components">
        /// The <see cref="Components"/> that is being deserialized
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Component"/> object
        /// </exception>
        private void DeserializeResponses(JsonElement jsonElement, Components components)
        {
            // TODO: implement responses

            if (jsonElement.TryGetProperty("responses", out JsonElement responsesProperty))
            {
                //var schemaDeSerializer = new ResponseDeSerializer(this.loggerFactory);

                //foreach (var item in schemasProperty.EnumerateObject())
                //{
                //    var key = item.Name;

                //    var schema = schemaDeSerializer.DeSerialize(item.Value);

                //    components.Schemas.Add(key, schema);
                //}
            }
        }

        /// <summary>
        /// Deserializes the Components.parameters from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Component"/> json object
        /// </param>
        /// <param name="components">
        /// The <see cref="Components"/> that is being deserialized
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Component"/> object
        /// </exception>
        private void DeserializeParameters(JsonElement jsonElement, Components components, bool strict)
        {
            if (jsonElement.TryGetProperty("parameters", out JsonElement parametersProperty))
            {
                var parameterDeSerializer = new ParameterDeSerializer(this.loggerFactory);
                var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);

                foreach (var itemProperty in parametersProperty.EnumerateObject())
                {
                    var key = itemProperty.Name;
                    var isRef = false;

                    foreach (var value in itemProperty.Value.EnumerateObject())
                    {
                        if (value.Name == "$ref")
                        {
                            isRef = true;

                            var reference = referenceDeSerializer.DeSerialize(itemProperty.Value, strict);
                            components.ParametersReferences.Add(key, reference);
                        }
                    }

                    if (!isRef)
                    {
                        var parameter = parameterDeSerializer.DeSerialize(itemProperty.Value, strict);
                        components.Parameters.Add(key, parameter);
                    }
                }
            }
        }

        /// <summary>
        /// Deserializes the Components.examples from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Component"/> json object
        /// </param>
        /// <param name="components">
        /// The <see cref="Components"/> that is being deserialized
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Component"/> object
        /// </exception>
        private void DeserializeExamples(JsonElement jsonElement, Components components, bool strict)
        {
            if (jsonElement.TryGetProperty("examples", out JsonElement parametersProperty))
            {
                var exampleDeSerializer = new ExampleDeSerializer(this.loggerFactory);
                var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);

                foreach (var itemProperty in parametersProperty.EnumerateObject())
                {
                    var key = itemProperty.Name;
                    var isRef = false;

                    foreach (var value in itemProperty.Value.EnumerateObject())
                    {
                        if (value.Name == "$ref")
                        {
                            isRef = true;

                            var reference = referenceDeSerializer.DeSerialize(itemProperty.Value, strict);
                            components.ExamplesReferences.Add(key, reference);
                        }
                    }

                    if (!isRef)
                    {
                        var example = exampleDeSerializer.DeSerialize(itemProperty.Value);
                        components.Examples.Add(key, example);
                    }
                }
            }
        }

        /// <summary>
        /// Deserializes the Components. requestBodies from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Component"/> json object
        /// </param>
        /// <param name="components">
        /// The <see cref="Components"/> that is being deserialized
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Component"/> object
        /// </exception>
        private void DeserializeRequestBodies(JsonElement jsonElement, Components components, bool strict)
        {
            if (jsonElement.TryGetProperty("requestBodies", out JsonElement requestBodiesProperty))
            {
                var requestBodyDeSerializer = new RequestBodyDeSerializer(this.loggerFactory);

                foreach (var r in requestBodiesProperty.EnumerateObject())
                {
                    var requestBodyName = r.Name;

                    var requestBody = requestBodyDeSerializer.DeSerialize(r.Value, strict);

                    components.RequestBodies.Add(requestBodyName, requestBody);
                }
            }
        }

        /// <summary>
        /// Deserializes the Components.parameters from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Component"/> json object
        /// </param>
        /// <param name="components">
        /// The <see cref="Components"/> that is being deserialized
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Component"/> object
        /// </exception>
        private void DeserializeHeaders(JsonElement jsonElement, Components components, bool strict)
        {
            if (jsonElement.TryGetProperty("headers", out JsonElement parametersProperty))
            {
                var headerDeSerializer = new HeaderDeSerializer(this.loggerFactory);
                var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);

                foreach (var itemProperty in parametersProperty.EnumerateObject())
                {
                    var key = itemProperty.Name;
                    var isRef = false;

                    foreach (var value in itemProperty.Value.EnumerateObject())
                    {
                        if (value.Name == "$ref")
                        {
                            isRef = true;

                            var reference = referenceDeSerializer.DeSerialize(itemProperty.Value, strict);
                            components.HeadersReferences.Add(key, reference);
                        }
                    }

                    if (!isRef)
                    {
                        var header = headerDeSerializer.DeSerialize(itemProperty.Value, strict);
                        components.Headers.Add(key, header);
                    }
                }
            }
        }

        /// <summary>
        /// Deserializes the Components.parameters from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Component"/> json object
        /// </param>
        /// <param name="components">
        /// The <see cref="Components"/> that is being deserialized
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Component"/> object
        /// </exception>
        private void DeserializeSecuritySchemes(JsonElement jsonElement, Components components, bool strict)
        {
            if (jsonElement.TryGetProperty("securitySchemes", out JsonElement parametersProperty))
            {
                var headerDeSerializer = new SecuritySchemeDeSerializer(this.loggerFactory);
                var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);

                foreach (var itemProperty in parametersProperty.EnumerateObject())
                {
                    var key = itemProperty.Name;
                    var isRef = false;

                    foreach (var value in itemProperty.Value.EnumerateObject())
                    {
                        if (value.Name == "$ref")
                        {
                            isRef = true;

                            var reference = referenceDeSerializer.DeSerialize(itemProperty.Value, strict);
                            components.SecuritySchemesReferences.Add(key, reference);
                        }
                    }

                    if (!isRef)
                    {
                        var securityScheme = headerDeSerializer.DeSerialize(itemProperty.Value, strict);
                        components.SecuritySchemes.Add(key, securityScheme);
                    }
                }
            }
        }

        /// <summary>
        /// Deserializes the Components.parameters from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Components"/> json object
        /// </param>
        /// <param name="components">
        /// The <see cref="Components"/> that is being deserialized
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Components"/> object
        /// </exception>
        private void DeserializeLinks(JsonElement jsonElement, Components components, bool strict)
        {
            if (jsonElement.TryGetProperty("links", out JsonElement parametersProperty))
            {
                var linkDeSerializer = new LinkDeSerializer(this.loggerFactory);
                var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);

                foreach (var itemProperty in parametersProperty.EnumerateObject())
                {
                    var key = itemProperty.Name;
                    var isRef = false;

                    foreach (var value in itemProperty.Value.EnumerateObject())
                    {
                        if (value.Name == "$ref")
                        {
                            isRef = true;

                            var reference = referenceDeSerializer.DeSerialize(itemProperty.Value, strict);
                            components.LinksReferences.Add(key, reference);
                        }
                    }

                    if (!isRef)
                    {
                        var link = linkDeSerializer.DeSerialize(itemProperty.Value, strict);
                        components.Links.Add(key, link);
                    }
                }
            }
        }
    }
}
