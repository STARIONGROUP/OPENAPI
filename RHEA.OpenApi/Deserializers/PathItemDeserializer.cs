// -------------------------------------------------------------------------------------------------
// <copyright file="PathItemDeserializer.cs" company="RHEA System S.A.">
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
    /// The purpose of the <see cref="PathItemDeserializer"/> is to deserialize the <see cref="PathItem"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#path-item-object
    /// </remarks>
    internal class PathItemDeserializer
    {
        /// <summary>
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<PathItemDeserializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PathItemDeserializer"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal PathItemDeserializer(ILoggerFactory loggerFactory = null)
        {
            this.loggerFactory = loggerFactory;

            this.logger = this.loggerFactory == null ? NullLogger<PathItemDeserializer>.Instance : this.loggerFactory.CreateLogger<PathItemDeserializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="PathItem"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="PathItem"/> json object
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <returns>
        /// an instance of <see cref="PathItem"/>
        /// </returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="PathItem"/> object
        /// </exception>
        internal PathItem DeSerialize(JsonElement jsonElement, bool strict)
        {
            this.logger.LogTrace("Start PathItemDeserializer.DeSerialize");

            var pathItem = new PathItem();

            var operationDeSerializer = new OperationDeSerializer(this.loggerFactory);

            foreach (var jsonProperty in jsonElement.EnumerateObject())
            {
                switch (jsonProperty.Name)
                {
                    case "$ref":
                        pathItem.Ref = jsonProperty.Value.GetString();
                        break;
                    case "summary":
                        pathItem.Summary = jsonProperty.Value.GetString();
                        break;
                    case "description":
                        pathItem.Description = jsonProperty.Value.GetString();
                        break;
                    case "get":
                        pathItem.Get = operationDeSerializer.DeSerialize(jsonProperty.Value, strict);
                        break;
                    case "put":
                        pathItem.Put = operationDeSerializer.DeSerialize(jsonProperty.Value, strict);
                        break;
                    case "post":
                        pathItem.Post = operationDeSerializer.DeSerialize(jsonProperty.Value, strict);
                        break;
                    case "delete":
                        pathItem.Delete = operationDeSerializer.DeSerialize(jsonProperty.Value, strict);
                        break;
                    case "options":
                        pathItem.Options = operationDeSerializer.DeSerialize(jsonProperty.Value, strict);
                        break;
                    case "head":
                        pathItem.Head = operationDeSerializer.DeSerialize(jsonProperty.Value, strict);
                        break;
                    case "patch":
                        pathItem.Patch = operationDeSerializer.DeSerialize(jsonProperty.Value, strict);
                        break;
                    case "trace":
                        pathItem.Trace = operationDeSerializer.DeSerialize(jsonProperty.Value, strict);
                        break;
                    case "servers":
                        if (jsonProperty.Value.ValueKind == JsonValueKind.Array)
                        {
                            var servers = new List<Server>();

                            var serverDeSerializer = new ServerDeSerializer(this.loggerFactory);

                            foreach (var arrayItem in jsonProperty.Value.EnumerateArray())
                            {
                                var server = serverDeSerializer.DeSerialize(arrayItem, strict);
                                servers.Add(server);
                            }

                            pathItem.Servers = servers.ToArray();
                        }
                        else
                        {
                            throw new SerializationException("the PathItem.servers property shall be an array");
                        }
                        break;
                    case "parameters":
                        if (jsonProperty.Value.ValueKind == JsonValueKind.Array)
                        {
                            var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);
                            var parameterReferences = new List<Reference>();

                            var parameterDeSerializer = new ParameterDeSerializer(this.loggerFactory);
                            var parameters = new List<Parameter>();

                            foreach (var arrayItem in jsonProperty.Value.EnumerateArray())
                            {
                                foreach (var arrayItemProperty in arrayItem.EnumerateObject())
                                {
                                    switch (arrayItemProperty.Name)
                                    {
                                        case "$ref":
                                            var reference = referenceDeSerializer.DeSerialize(arrayItem, strict);
                                            parameterReferences.Add(reference);
                                            break;
                                        case "name":
                                            var parameter = parameterDeSerializer.DeSerialize(arrayItem, strict);
                                            parameters.Add(parameter);
                                            break;
                                    }
                                }
                            }

                            pathItem.ParameterReferences = parameterReferences.ToArray();
                            pathItem.Parameters = parameters.ToArray();
                        }
                        else
                        {
                            throw new SerializationException("the PathItem.parameters property shall be an array");
                        }
                        break;
                    default:
                        if (jsonProperty.Name.StartsWith("x-"))
                        {
                            this.logger.LogWarning("extensions are not yet supported");
                        }
                        else
                        {
                            throw new SerializationException($"The {jsonProperty.Name} is an invalid property for the PathItem object, this is an invalid OpenAPI document");
                        }
                        break;
                }
            }

            this.logger.LogTrace("Finish PathItemDeserializer.DeSerialize");

            return pathItem;
        }
    }
}
