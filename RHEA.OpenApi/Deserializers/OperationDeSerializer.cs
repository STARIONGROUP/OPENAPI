// -------------------------------------------------------------------------------------------------
// <copyright file="OperationDeSerializer.cs" company="RHEA System S.A.">
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
    /// The purpose of the <see cref="OperationDeSerializer"/> is to deserialize the <see cref="Operation"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#path-item-object
    /// </remarks>
    public class OperationDeSerializer
    {
        /// <summary>
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<OperationDeSerializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationDeSerializer"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal OperationDeSerializer(ILoggerFactory loggerFactory = null)
        {
            this.loggerFactory = loggerFactory;

            this.logger = this.loggerFactory == null ? NullLogger<OperationDeSerializer>.Instance : this.loggerFactory.CreateLogger<OperationDeSerializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="Operation"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Operation"/> json object
        /// </param>
        /// <returns></returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Operation"/> object
        /// </exception>
        public Operation DeSerialize(JsonElement jsonElement)
        {
            var operation = new Operation();

            this.DeserializeTags(jsonElement, operation);

            if (jsonElement.TryGetProperty("summary", out JsonElement summaryProperty))
            {
                operation.Summary = summaryProperty.GetString();
            }
            else
            {
                this.logger.LogTrace("The optional Operation.summary property is not provided in the OpenApi document");
            }

            if (jsonElement.TryGetProperty("description", out JsonElement descriptionProperty))
            {
                operation.Description = descriptionProperty.GetString();
            }
            else
            {
                this.logger.LogTrace("The optional Operation.description property is not provided in the OpenApi document");
            }

            if (jsonElement.TryGetProperty("externalDocs", out JsonElement externalDocsProperty))
            {
                var externalDocumentationDeSerializer = new ExternalDocumentationDeSerializer(this.loggerFactory);
                operation.ExternalDocs = externalDocumentationDeSerializer.DeSerialize(externalDocsProperty);
            }
            else
            {
                this.logger.LogTrace("The optional Operation.externalDocs property is not provided in the OpenApi document");
            }

            if (jsonElement.TryGetProperty("operationId", out JsonElement operationIdProperty))
            {
                operation.OperationId = operationIdProperty.GetString();
            }
            else
            {
                this.logger.LogTrace("The optional Operation.operationId property is not provided in the OpenApi document");
            }

            this.DeserializeParameters(jsonElement, operation);
            
            this.DeserializeRequestBody(jsonElement, operation);

            // responses
            this.logger.LogWarning("TODO: the Operation.responses property is not yet supported");

            // callbacks
            this.logger.LogWarning("TODO: the Operation.callbacks property is not yet supported");

            if (jsonElement.TryGetProperty("deprecated", out JsonElement deprecatedProperty))
            {
                operation.Deprecated = deprecatedProperty.GetBoolean();
            }
            
            this.DeserializeSecurityRequirements(jsonElement, operation);
            
            this.DeserializeServers(jsonElement, operation);

            return operation;
        }

        /// <summary>
        /// Deserializes the <see cref="Tag"/>s from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Operation"/> json object
        /// </param>
        /// <param name="operation">
        /// The <see cref="Operation"/> that is being deserialized
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Operation"/> object
        /// </exception>
        private void DeserializeTags(JsonElement jsonElement, Operation operation)
        {
            if (jsonElement.TryGetProperty("tags", out JsonElement tagsProperty))
            {
                if (tagsProperty.ValueKind == JsonValueKind.Array)
                {
                    var tags = new List<string>();

                    foreach (var arrayItem in tagsProperty.EnumerateArray())
                    {
                        tags.Add(arrayItem.GetString());
                    }

                    operation.Tags = tags.ToArray();
                }
                else
                {
                    throw new SerializationException("the Operation.tags property shall be an array");
                }
            }
            else
            {
                this.logger.LogTrace("The optional Operation.tags property is not provided in the OpenApi document");
            }
        }

        /// <summary>
        /// Deserializes the <see cref="Parameter"/>s from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Operation"/> json object
        /// </param>
        /// <param name="operation">
        /// The <see cref="Operation"/> that is being deserialized
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Operation"/> object
        /// </exception>
        private void DeserializeParameters(JsonElement jsonElement, Operation operation)
        {
            if (jsonElement.TryGetProperty("parameters", out JsonElement parametersProperty))
            {
                if (parametersProperty.ValueKind == JsonValueKind.Array)
                {
                    var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);
                    var parameterReferences = new List<Reference>();

                    var parameterDeSerializer = new ParameterDeSerializer(this.loggerFactory);
                    var parameters = new List<Parameter>();

                    foreach (var arrayItem in parametersProperty.EnumerateArray())
                    {
                        foreach (var arrayItemProperty in arrayItem.EnumerateObject())
                        {
                            switch (arrayItemProperty.Name)
                            {
                                case "$ref":
                                    var reference = referenceDeSerializer.DeSerialize(arrayItem);
                                    parameterReferences.Add(reference);
                                    break;
                                case "name":
                                    var parameter = parameterDeSerializer.DeSerialize(arrayItem);
                                    parameters.Add(parameter);
                                    break;
                            }
                        }
                    }

                    operation.ParameterReferences = parameterReferences.ToArray();
                    operation.Parameters = parameters.ToArray();
                }
                else
                {
                    throw new SerializationException("the Operation.parameters property shall be an array");
                }
            }
            else
            {
                this.logger.LogTrace("The optional Operation.parameters property is not provided in the OpenApi document");
            }
        }

        /// <summary>
        /// Deserializes the <see cref="RequestBody"/>s from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Operation"/> json object
        /// </param>
        /// <param name="operation">
        /// The <see cref="Operation"/> that is being deserialized
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Operation"/> object
        /// </exception>
        private void DeserializeRequestBody(JsonElement jsonElement, Operation operation)
        {
            if (jsonElement.TryGetProperty("requestBody", out JsonElement requestBodyProperty))
            {
                if (requestBodyProperty.ValueKind == JsonValueKind.Object)
                {
                    var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);

                    var parameterDeSerializer = new RequestBodyDeSerializer(this.loggerFactory);

                    foreach (var itemProperty in requestBodyProperty.EnumerateObject())
                    {
                        switch (itemProperty.Name)
                        {
                            case "$ref":
                                var reference = referenceDeSerializer.DeSerialize(requestBodyProperty);
                                operation.RequestBodyReference = reference;
                                break;
                            case "content":
                                var requestBody = parameterDeSerializer.DeSerialize(requestBodyProperty);
                                operation.RequestBody = requestBody;
                                break;
                        }
                    }
                }
                else
                {
                    throw new SerializationException("the Operation.requestBody property shall be an object");
                }
            }
            else
            {
                this.logger.LogTrace("The optional Operation.requestBody property is not provided in the OpenApi document");
            }
        }

        /// <summary>
        /// Deserializes the <see cref="SecurityRequirement"/>s from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Operation"/> json object
        /// </param>
        /// <param name="operation">
        /// The <see cref="Operation"/> that is being deserialized
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Operation"/> object
        /// </exception>
        private void DeserializeSecurityRequirements(JsonElement jsonElement, Operation operation)
        {
            if (jsonElement.TryGetProperty("security", out JsonElement securityProperty))
            {
                if (securityProperty.ValueKind == JsonValueKind.Array)
                {
                    var securityRequirementDeSerializer = new SecurityRequirementDeSerializer(this.loggerFactory);

                    var securityRequirements = new List<SecurityRequirement>();

                    foreach (var arrayItem in securityProperty.EnumerateArray())
                    {
                        var securityRequirement = securityRequirementDeSerializer.DeSerialize(arrayItem);
                        securityRequirements.Add(securityRequirement);
                    }

                    operation.Security = securityRequirements.ToArray();
                }
                else
                {
                    throw new SerializationException("the Operation.security property shall be an array");
                }
            }
            else
            {
                this.logger.LogTrace("The optional Document.security property is not provided in the OpenApi document");
            }
        }

        /// <summary>
        /// Deserializes the <see cref="Server"/>s from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Operation"/> json object
        /// </param>
        /// <param name="operation">
        /// The <see cref="Operation"/> that is being deserialized
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Operation"/> object
        /// </exception>
        private void DeserializeServers(JsonElement jsonElement, Operation operation)
        {
            if (jsonElement.TryGetProperty("servers", out JsonElement serversProperty))
            {
                if (serversProperty.ValueKind == JsonValueKind.Array)
                {
                    var servers = new List<Server>();

                    var serverDeSerializer = new ServerDeSerializer(this.loggerFactory);

                    foreach (var arrayItem in serversProperty.EnumerateArray())
                    {
                        var server = serverDeSerializer.DeSerialize(arrayItem);
                        servers.Add(server);
                    }

                    operation.Servers = servers.ToArray();
                }
                else
                {
                    throw new SerializationException("the Operation.servers property shall be an array");
                }
            }
        }
    }
}
