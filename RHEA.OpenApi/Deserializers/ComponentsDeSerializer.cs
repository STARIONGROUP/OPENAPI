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
        /// <returns></returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Components"/> object
        /// </exception>
        internal Components DeSerialize(JsonElement jsonElement)
        {
            this.logger.LogTrace("Start ComponentsDeSerializer.DeSerialize");

            var components = new Components();

            //  schemas
            if (jsonElement.TryGetProperty("schemas", out JsonElement schemasProperty))
            {
                // TODO: implement schemas
            }

            //  responses
            if (jsonElement.TryGetProperty("responses", out JsonElement responsesProperty))
            {
                // TODO: implement responses
            }

            if (jsonElement.TryGetProperty("parameters", out JsonElement parametersProperty))
            {
                var parameterDeSerializer = new ParameterDeSerializer(this.loggerFactory);

                foreach (var p in parametersProperty.EnumerateObject())
                {
                    var parameterName = p.Name;

                    var parameter = parameterDeSerializer.DeSerialize(p.Value);

                    components.Parameters.Add(parameterName, parameter);
                }
            }

            //  examples
            if (jsonElement.TryGetProperty("examples", out JsonElement examplesProperty))
            {
                // TODO: implement examples
            }

            if (jsonElement.TryGetProperty("requestBodies", out JsonElement requestBodiesProperty))
            {
                var requestBodyDeSerializer = new RequestBodyDeSerializer(this.loggerFactory);

                foreach (var r in requestBodiesProperty.EnumerateObject())
                {
                    var requestBodyName = r.Name;

                    var requestBody = requestBodyDeSerializer.DeSerialize(r.Value);

                    components.RequestBodies.Add(requestBodyName, requestBody);
                }
            }

            //  headers
            if (jsonElement.TryGetProperty("headers", out JsonElement headersProperty))
            {
                // TODO: implement headers
            }

            //  securitySchemes
            if (jsonElement.TryGetProperty("securitySchemes", out JsonElement securitySchemesProperty))
            {
                // TODO: implement securitySchemes
            }
            
            //  links
            if (jsonElement.TryGetProperty("links", out JsonElement linksProperty))
            {
                // TODO: implement links
            }
            
            if (jsonElement.TryGetProperty("callbacks", out JsonElement callbacksProperty))
            {
                this.logger.LogWarning("TODO: callbacks are not yet supported");
            }
            
            if (jsonElement.TryGetProperty("pathItems", out JsonElement pathItemsProperty))
            {
                // TODO: add support for reference objects

                var pathItemsDeSerializer = new PathItemDeserializer(this.loggerFactory);

                foreach (var p in pathItemsProperty.EnumerateObject())
                {
                    var pathItemName = p.Name;

                    var pathItem = pathItemsDeSerializer.DeSerialize(p.Value);

                    components.PathItems.Add(pathItemName, pathItem);
                }
            }

            this.logger.LogTrace("Finish ComponentsDeSerializer.DeSerialize");

            return components;
        }
    }
}
