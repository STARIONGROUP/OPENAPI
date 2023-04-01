// -------------------------------------------------------------------------------------------------
// <copyright file="RequestBodyDeSerializer.cs" company="RHEA System S.A.">
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
    /// The purpose of the <see cref="RequestBodyDeSerializer"/> is to deserialize the <see cref="RequestBody"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#request-body-object
    /// </remarks>
    internal class RequestBodyDeSerializer
    {
        /// <summary>
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<RequestBodyDeSerializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBodyDeSerializer"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal RequestBodyDeSerializer(ILoggerFactory loggerFactory = null)
        {
            this.loggerFactory = loggerFactory;

            this.logger = this.loggerFactory == null ? NullLogger<RequestBodyDeSerializer>.Instance : this.loggerFactory.CreateLogger<RequestBodyDeSerializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="RequestBody"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="RequestBody"/> json object
        /// </param>
        /// <returns></returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="RequestBody"/> object
        /// </exception>
        internal RequestBody DeSerialize(JsonElement jsonElement)
        {
            var requestBody = new RequestBody();
            
            if (jsonElement.TryGetProperty("description", out JsonElement descriptionProperty))
            {
                requestBody.Description = descriptionProperty.GetString();
            }
            else
            {
                this.logger.LogTrace("The optional RequestBody.description property is not provided in the OpenApi document");
            }

            if (!jsonElement.TryGetProperty("content", out JsonElement contentProperty))
            {
                throw new SerializationException("The REQUIRED RequestBody.content property is not available, this is an invalid OpenAPI document");
            }
            else
            {
                var mediaTypeDeSerializer = new MediaTypeDeSerializer(this.loggerFactory);

                foreach (var c in contentProperty.EnumerateObject())
                {
                    var mediaTypeName = c.Name;

                    var mediaType = mediaTypeDeSerializer.DeSerialize(c.Value);

                    requestBody.Content.Add(mediaTypeName, mediaType);
                }
            }
            
            if (jsonElement.TryGetProperty("required", out JsonElement requiredProperty))
            {
                requestBody.Required = requiredProperty.GetBoolean();
            }
            else
            {
                this.logger.LogTrace("The optional RequestBody.required property is not provided in the OpenApi document");
            }

            return requestBody;
        }
    }
}
