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
    internal class OperationDeSerializer : ReferencerDeserializer
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
        /// <param name="referenceResolver">
        /// The <see cref="ReferenceResolver"/> that is used to register any <see cref="ReferenceInfo"/> objects
        /// and later resolve them
        /// </param>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal OperationDeSerializer(ReferenceResolver referenceResolver, ILoggerFactory loggerFactory = null) 
            : base(referenceResolver)
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

            if (jsonElement.TryGetProperty("summary"u8, out JsonElement summaryProperty))
            {
                operation.Summary = summaryProperty.GetString();
            }

            if (jsonElement.TryGetProperty("description"u8, out JsonElement descriptionProperty))
            {
                operation.Description = descriptionProperty.GetString();
            }

            if (jsonElement.TryGetProperty("externalDocs"u8, out JsonElement externalDocsProperty))
            {
                var externalDocumentationDeSerializer = new ExternalDocumentationDeSerializer(this.loggerFactory);
                operation.ExternalDocs = externalDocumentationDeSerializer.DeSerialize(externalDocsProperty, strict);
            }

            if (jsonElement.TryGetProperty("operationId"u8, out JsonElement operationIdProperty))
            {
                operation.OperationId = operationIdProperty.GetString();
            }

            this.DeserializeParameters(jsonElement, operation, strict);
            
            this.DeserializeRequestBody(jsonElement, operation, strict);

            if (jsonElement.TryGetProperty("responses"u8, out JsonElement responsesProperty))
            {
                var responsesDeSerializer = new ResponsesDeSerializer(this.referenceResolver, this.loggerFactory);
                operation.Responses = responsesDeSerializer.DeSerialize(responsesProperty, strict);
            }
            
            this.DeserializeCallbacks(jsonElement, operation, strict);

            if (jsonElement.TryGetProperty("deprecated"u8, out JsonElement deprecatedProperty))
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
            if (jsonElement.TryGetProperty("tags"u8, out JsonElement tagsProperty))
            {
                if (tagsProperty.ValueKind == JsonValueKind.Array)
                {
                    foreach (var arrayItem in tagsProperty.EnumerateArray())
                    {
                        operation.Tags.Add(arrayItem.GetString());
                    }
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
            if (jsonElement.TryGetProperty("parameters"u8, out JsonElement parametersProperty))
            {
                if (parametersProperty.ValueKind == JsonValueKind.Array)
                {
                    var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);
                    var parameterDeSerializer = new ParameterDeSerializer(this.referenceResolver, this.loggerFactory);

                    foreach (var arrayItem in parametersProperty.EnumerateArray())
                    {
                        if (arrayItem.TryGetProperty("$ref", out var referenceElement))
                        {
                            var reference = referenceDeSerializer.DeSerialize(arrayItem, strict);
                            operation.ParameterReferences.Add(reference);
                            this.Register(reference, operation, "Parameters");
                        }
                        else
                        {
                            var parameter = parameterDeSerializer.DeSerialize(arrayItem, strict);
                            operation.Parameters.Add(parameter);
                        }
                    }
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
            if (jsonElement.TryGetProperty("requestBody"u8, out JsonElement requestBodyProperty))
            {
                if (requestBodyProperty.ValueKind == JsonValueKind.Object)
                {
                    if (requestBodyProperty.TryGetProperty("$ref"u8, out var referenceElement))
                    {
                        var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);
                        var reference = referenceDeSerializer.DeSerialize(requestBodyProperty, strict);
                        operation.RequestBodyReference = reference;
                        this.Register(reference, operation.Deprecated, "RequestBody");
                    }
                    else 
                    {
                        var parameterDeSerializer = new RequestBodyDeSerializer(this.referenceResolver, this.loggerFactory);
                        var requestBody = parameterDeSerializer.DeSerialize(requestBodyProperty, strict);
                        operation.RequestBody = requestBody;
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
                    if (itemProperty.Value.TryGetProperty("$ref"u8, out var referenceElement))
                    {
                        var reference = referenceDeSerializer.DeSerialize(itemProperty.Value, strict);
                        operation.CallbacksReferences.Add(itemProperty.Name, reference);
                        this.Register(reference, operation, "Callbacks", itemProperty.Name);
                    }
                    else
                    {
                        var callback = callbackDeSerializer.DeSerialize(itemProperty.Value, strict);
                        operation.Callbacks.Add(itemProperty.Name, callback);
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
            if (jsonElement.TryGetProperty("security"u8, out JsonElement securityProperty))
            {
                if (securityProperty.ValueKind == JsonValueKind.Array)
                {
                    var securityRequirementDeSerializer = new SecurityRequirementDeSerializer(this.loggerFactory);

                    foreach (var arrayItem in securityProperty.EnumerateArray())
                    {
                        var securityRequirement = securityRequirementDeSerializer.DeSerialize(arrayItem);
                        operation.Security.Add(securityRequirement);
                    }
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
            if (jsonElement.TryGetProperty("servers"u8, out JsonElement serversProperty))
            {
                if (serversProperty.ValueKind == JsonValueKind.Array)
                {
                    var serverDeSerializer = new ServerDeSerializer(this.loggerFactory);

                    foreach (var arrayItem in serversProperty.EnumerateArray())
                    {
                        var server = serverDeSerializer.DeSerialize(arrayItem, strict);
                        operation.Servers.Add(server);
                    }
                }
            }
        }
    }
}
