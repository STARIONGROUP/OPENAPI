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
    internal class ComponentsDeSerializer : ReferencerDeserializer
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
        /// <param name="referenceResolver">
        /// The <see cref="ReferenceResolver"/> that is used to register any <see cref="ReferenceInfo"/> objects
        /// and later resolve them
        /// </param>
        internal ComponentsDeSerializer(ReferenceResolver referenceResolver, ILoggerFactory loggerFactory = null)
        : base(referenceResolver)
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

            this.DeserializeResponses(jsonElement, components, strict);
            
            this.DeserializeParameters(jsonElement, components, strict);

            this.DeserializeExamples(jsonElement, components, strict);

            this.DeserializeRequestBodies(jsonElement, components, strict);

            this.DeserializeHeaders(jsonElement, components, strict);

            this.DeserializeSecuritySchemes(jsonElement, components, strict);

            this.DeserializeLinks(jsonElement, components, strict);

            this.DeserializeCallbacks(jsonElement, components, strict);
            
            this.DeserializePathItems(jsonElement, components, strict);
            
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
            if (jsonElement.TryGetProperty("schemas"u8, out JsonElement schemasProperty))
            {
                var schemaDeSerializer = new SchemaDeSerializer(this.referenceResolver, this.loggerFactory);

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
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Component"/> object
        /// </exception>
        private void DeserializeResponses(JsonElement jsonElement, Components components, bool strict)
        {
            if (jsonElement.TryGetProperty("responses"u8, out JsonElement responsesProperty))
            {
                var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);
                var responseDeserializer = new ResponseDeserializer(this.referenceResolver,  this.loggerFactory);

                foreach (var itemProperty in responsesProperty.EnumerateObject())
                {
                    if (itemProperty.Value.TryGetProperty("$ref"u8, out var referenceElement))
                    {
                        var reference = referenceDeSerializer.DeSerialize(itemProperty.Value, strict);
                        components.ResponsesReferences.Add(itemProperty.Name, reference);
                        this.Register(reference, components, "Responses", itemProperty.Name);
                    }
                    else
                    {
                        var response = responseDeserializer.DeSerialize(itemProperty.Value, strict);
                        components.Responses.Add(itemProperty.Name, response);
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
        private void DeserializeParameters(JsonElement jsonElement, Components components, bool strict)
        {
            if (jsonElement.TryGetProperty("parameters"u8, out JsonElement parametersProperty))
            {
                var parameterDeSerializer = new ParameterDeSerializer(this.referenceResolver, this.loggerFactory);
                var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);

                foreach (var itemProperty in parametersProperty.EnumerateObject())
                {
                    if (itemProperty.Value.TryGetProperty("$ref"u8, out var referenceElement))
                    {
                        var reference = referenceDeSerializer.DeSerialize(itemProperty.Value, strict);
                        components.ParametersReferences.Add(itemProperty.Name, reference);
                        this.Register(reference, components, "Parameters", itemProperty.Name);
                    }
                    else
                    {
                        var parameter = parameterDeSerializer.DeSerialize(itemProperty.Value, strict);
                        components.Parameters.Add(itemProperty.Name, parameter);
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
            if (jsonElement.TryGetProperty("examples"u8, out JsonElement parametersProperty))
            {
                var exampleDeSerializer = new ExampleDeSerializer(this.loggerFactory);
                var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);

                foreach (var itemProperty in parametersProperty.EnumerateObject())
                {
                    if (itemProperty.Value.TryGetProperty("$ref"u8, out var referenceElement))
                    {
                        var reference = referenceDeSerializer.DeSerialize(itemProperty.Value, strict);
                        components.ExamplesReferences.Add(itemProperty.Name, reference);
                        this.Register(reference, components, "Examples", itemProperty.Name);
                    }
                    else
                    {
                        var example = exampleDeSerializer.DeSerialize(itemProperty.Value);
                        components.Examples.Add(itemProperty.Name, example);
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
            if (jsonElement.TryGetProperty("requestBodies"u8, out JsonElement requestBodiesProperty))
            {
                var requestBodyDeSerializer = new RequestBodyDeSerializer(this.referenceResolver, this.loggerFactory);
                var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);

                foreach (var itemProperty in requestBodiesProperty.EnumerateObject())
                {
                    if (itemProperty.Value.TryGetProperty("$ref"u8, out var referenceElement))
                    {
                        var reference = referenceDeSerializer.DeSerialize(itemProperty.Value, strict);
                        components.RequestBodiesReferences.Add(itemProperty.Name, reference);
                        this.Register(reference, components, "RequestBodies", itemProperty.Name);
                    }
                    else
                    {
                        var requestBody = requestBodyDeSerializer.DeSerialize(itemProperty.Value, strict);
                        components.RequestBodies.Add(itemProperty.Name, requestBody);
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
        private void DeserializeHeaders(JsonElement jsonElement, Components components, bool strict)
        {
            if (jsonElement.TryGetProperty("headers"u8, out JsonElement parametersProperty))
            {
                var headerDeSerializer = new HeaderDeSerializer(this.referenceResolver, this.loggerFactory);
                var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);

                foreach (var itemProperty in parametersProperty.EnumerateObject())
                {
                    if (itemProperty.Value.TryGetProperty("$ref"u8, out var referenceElement))
                    {
                        var reference = referenceDeSerializer.DeSerialize(itemProperty.Value, strict);
                        components.HeadersReferences.Add(itemProperty.Name, reference);
                        this.Register(reference, components, "Headers", itemProperty.Name);
                    }
                    else
                    {
                        var header = headerDeSerializer.DeSerialize(itemProperty.Value, strict);
                        components.Headers.Add(itemProperty.Name, header);
                    }
                }
            }
        }

        /// <summary>
        /// Deserializes the Components.securitySchemes  from the provided <paramref name="jsonElement"/>
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
            if (jsonElement.TryGetProperty("securitySchemes"u8, out JsonElement parametersProperty))
            {
                var headerDeSerializer = new SecuritySchemeDeSerializer(this.loggerFactory);
                var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);

                foreach (var itemProperty in parametersProperty.EnumerateObject())
                {
                    if (itemProperty.Value.TryGetProperty("$ref"u8, out var referenceElement))
                    {
                        var reference = referenceDeSerializer.DeSerialize(itemProperty.Value, strict);
                        components.SecuritySchemesReferences.Add(itemProperty.Name, reference);
                        this.Register(reference, components, "SecuritySchemes", itemProperty.Name);
                    }
                    else
                    {
                        var securityScheme = headerDeSerializer.DeSerialize(itemProperty.Value, strict);
                        components.SecuritySchemes.Add(itemProperty.Name, securityScheme);
                    }
                }
            }
        }

        /// <summary>
        /// Deserializes the Components <see cref="Link"/>s from the provided <paramref name="jsonElement"/>
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
            if (jsonElement.TryGetProperty("links"u8, out JsonElement parametersProperty))
            {
                var linkDeSerializer = new LinkDeSerializer(this.loggerFactory);
                var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);

                foreach (var itemProperty in parametersProperty.EnumerateObject())
                {
                    if (itemProperty.Value.TryGetProperty("$ref"u8, out var referenceElement))
                    {
                        var reference = referenceDeSerializer.DeSerialize(itemProperty.Value, strict);
                        components.LinksReferences.Add(itemProperty.Name, reference);
                        this.Register(reference, components, "Links", itemProperty.Name);
                    }
                    else
                    {
                        var link = linkDeSerializer.DeSerialize(itemProperty.Value, strict);
                        components.Links.Add(itemProperty.Name, link);
                    }
                }
            }
        }

        /// <summary>
        /// Deserializes the Components <see cref="Callback"/>s from the provided <paramref name="jsonElement"/>
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
        private void DeserializeCallbacks(JsonElement jsonElement, Components components, bool strict)
        {
            if (jsonElement.TryGetProperty("callbacks"u8, out JsonElement parametersProperty))
            {
                var callbackDeSerializer = new CallbackDeSerializer(this.loggerFactory);
                var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);

                foreach (var itemProperty in parametersProperty.EnumerateObject())
                {
                    if (itemProperty.Value.TryGetProperty("$ref"u8, out var referenceElement))
                    {
                        var reference = referenceDeSerializer.DeSerialize(itemProperty.Value, strict);
                        components.CallbacksReferences.Add(itemProperty.Name, reference);
                        this.Register(reference, components, "Callbacks", itemProperty.Name);
                    }
                    else
                    {
                        var callback = callbackDeSerializer.DeSerialize(itemProperty.Value, strict);
                        components.Callbacks.Add(itemProperty.Name, callback);
                    }
                }
            }
        }

        /// <summary>
        /// Deserializes the Components.<see cref="PathItem"/> from the provided <paramref name="jsonElement"/>
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
        private void DeserializePathItems(JsonElement jsonElement, Components components, bool strict)
        {
            if (jsonElement.TryGetProperty("pathItems"u8, out JsonElement pathItemsProperty))
            {
                var pathItemDeSerializer = new PathItemDeserializer(this.referenceResolver, this.loggerFactory);
                var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);

                foreach (var itemProperty in pathItemsProperty.EnumerateObject())
                {
                    if (itemProperty.Value.TryGetProperty("$ref"u8, out var referenceElement))
                    {
                        var reference = referenceDeSerializer.DeSerialize(itemProperty.Value, strict);
                        components.PathItemsReferences.Add(itemProperty.Name, reference);
                        this.Register(reference, components, "PathItems", itemProperty.Name);
                    }
                    else
                    {
                        var pathItem = pathItemDeSerializer.DeSerialize(itemProperty.Value, strict);
                        components.PathItems.Add(itemProperty.Name, pathItem);
                    }
                }
            }
        }
    }
}
