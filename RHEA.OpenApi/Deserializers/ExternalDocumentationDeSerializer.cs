// -------------------------------------------------------------------------------------------------
// <copyright file="ExternalDocumentationDeSerializer.cs" company="RHEA System S.A.">
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
    /// The purpose of the <see cref="ExternalDocumentationDeSerializer"/> is to deserialize the <see cref="ExternalDocumentation"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#external-documentation-object
    /// </remarks>
    internal class ExternalDocumentationDeSerializer
    {
        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<ExternalDocumentationDeSerializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalDocumentationDeSerializer"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal ExternalDocumentationDeSerializer(ILoggerFactory loggerFactory = null)
        {
            this.logger = loggerFactory == null ? NullLogger<ExternalDocumentationDeSerializer>.Instance : loggerFactory.CreateLogger<ExternalDocumentationDeSerializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="ExternalDocumentation"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="ExternalDocumentation"/> json object
        /// </param>
        /// <returns></returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="ExternalDocumentation"/> object
        /// </exception>
        internal ExternalDocumentation DeSerialize(JsonElement jsonElement)
        {
            var externalDocumentation = new ExternalDocumentation();

            if (jsonElement.TryGetProperty("description", out JsonElement descriptionProperty))
            {
                externalDocumentation.Description = descriptionProperty.GetString();
            }
            else
            {
                this.logger.LogTrace("The optional ExternalDocumentation.description property is not provided in the OpenApi document");
            }

            if (!jsonElement.TryGetProperty("url", out JsonElement urlProperty))
            {
                throw new SerializationException("The REQUIRED ExternalDocumentation.url property is not available, this is an invalid OpenAPI document");
            }

            externalDocumentation.Url = urlProperty.GetString();

            return externalDocumentation;
        }
    }
}
