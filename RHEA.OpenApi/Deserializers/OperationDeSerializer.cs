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
    using System;
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
    /// https://spec.openapis.org/oas/latest.html#operation-object
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
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <returns>
        /// an instance of <see cref="Operation"/>
        /// </returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Operation"/> object
        /// </exception>
        public Operation DeSerialize(JsonElement jsonElement, bool strict)
        {
            this.logger.LogTrace("Start OperationDeSerializer.DeSerialize");

            var operation = new Operation();

            this.DeserializeTags(jsonElement, operation);

            if (jsonElement.TryGetProperty("summary", out JsonElement summaryProperty))
            {
                operation.Summary = summaryProperty.GetString();
            }

            if (jsonElement.TryGetProperty("description", out JsonElement descriptionProperty))
            {
                operation.Description = descriptionProperty.GetString();
            }

            if (jsonElement.TryGetProperty("externalDocs", out JsonElement externalDocsProperty))
            {
                var externalDocumentationDeSerializer = new ExternalDocumentationDeSerializer(this.loggerFactory);
                operation.ExternalDocs = externalDocumentationDeSerializer.DeSerialize(externalDocsProperty, strict);
            }

            if (jsonElement.TryGetProperty("operationId", out JsonElement operationIdProperty))
            {
                operation.OperationId = operationIdProperty.GetString();
            }

            this.DeserializeParameters(jsonElement, operation, strict);
            
            this.DeserializeRequestBody(jsonElement, operation, strict);

            this.logger.LogWarning("TODO: the Operation.responses property is not yet supported");
            if (jsonElement.TryGetProperty("responses", out JsonElement responsesProperty))
            {
                var responsesDeSerializer = new ResponsesDeSerializer(this.loggerFactory);
                operation.Responses = responsesDeSerializer.DeSerialize(responsesProperty, strict);
            }
            
            this.DeserializeCallbacks(jsonElement, operation, strict);

            if (jsonElement.TryGetProperty("deprecated", out JsonElement deprecatedProperty))
            {
                operation.Deprecated = deprecatedProperty.GetBoolean();
            }
            
            this.DeserializeSecurityRequirements(jsonElement, operation);
            
            this.DeserializeServers(jsonElement, operation, strict);

            this.logger.LogTrace("Finish OperationDeSerializer.DeSerialize");

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
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Operation"/> object
        /// </exception>
        private void DeserializeParameters(JsonElement jsonElement, Operation operation, bool strict)
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

                    operation.ParameterReferences = parameterReferences.ToArray();
                    operation.Parameters = parameters.ToArray();
                }
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
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Operation"/> object
        /// </exception>
        private void DeserializeRequestBody(JsonElement jsonElement, Operation operation, bool strict)
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
                                var reference = referenceDeSerializer.DeSerialize(requestBodyProperty, strict);
                                operation.RequestBodyReference = reference;
                                break;
                            case "content":
                                var requestBody = parameterDeSerializer.DeSerialize(requestBodyProperty, strict);
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
        }

        /// <summary>
        /// Deserializes the <see cref="Callback"/>s from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Operation"/> json object
        /// </param>
        /// <param name="operation">
        /// The <see cref="Operation"/> that is being deserialized
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Operation"/> object
        /// </exception>
        private void DeserializeCallbacks(JsonElement jsonElement, Operation operation, bool strict)
        {
            if (jsonElement.TryGetProperty("callbacks", out JsonElement parametersProperty))
            {
                var callbackDeSerializer = new CallbackDeSerializer(this.loggerFactory);
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
                            operation.CallbacksReferences.Add(key, reference);
                        }
                    }

                    if (!isRef)
                    {
                        var callback = callbackDeSerializer.DeSerialize(itemProperty.Value, strict);
                        operation.Callbacks.Add(key, callback);
                    }
                }
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
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Operation"/> object
        /// </exception>
        private void DeserializeServers(JsonElement jsonElement, Operation operation, bool strict)
        {
            if (jsonElement.TryGetProperty("servers", out JsonElement serversProperty))
            {
                if (serversProperty.ValueKind == JsonValueKind.Array)
                {
                    var servers = new List<Server>();

                    var serverDeSerializer = new ServerDeSerializer(this.loggerFactory);

                    foreach (var arrayItem in serversProperty.EnumerateArray())
                    {
                        var server = serverDeSerializer.DeSerialize(arrayItem, strict);
                        servers.Add(server);
                    }

                    operation.Servers = servers.ToArray();
                }
            }
        }
    }
}
