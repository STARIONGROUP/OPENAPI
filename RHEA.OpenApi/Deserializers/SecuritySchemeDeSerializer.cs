// -------------------------------------------------------------------------------------------------
// <copyright file="SecuritySchemeDeSerializer.cs" company="RHEA System S.A.">
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
    /// The purpose of the <see cref="SecuritySchemeDeSerializer"/> is to deserialize the <see cref="SecurityScheme"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#security-scheme-object
    /// </remarks>
    internal class SecuritySchemeDeSerializer
    {
        /// <summary>
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<SecuritySchemeDeSerializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecuritySchemeDeSerializer"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal SecuritySchemeDeSerializer(ILoggerFactory loggerFactory = null)
        {
            this.loggerFactory = loggerFactory;

            this.logger = this.loggerFactory == null ? NullLogger<SecuritySchemeDeSerializer>.Instance : this.loggerFactory.CreateLogger<SecuritySchemeDeSerializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="SecurityScheme"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="SecurityScheme"/> json object
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <returns>
        /// An instance of an Open Api <see cref="SecurityScheme"/>
        /// </returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="SecurityScheme"/> object
        /// </exception>
        internal SecurityScheme DeSerialize(JsonElement jsonElement, bool strict)
        {
            this.logger.LogTrace("Start SecuritySchemeDeSerializer.DeSerialize");

            var securityScheme = new SecurityScheme();

            if (!jsonElement.TryGetProperty("type", out JsonElement typeProperty))
            {
                if (strict)
                {
                    throw new SerializationException("The REQUIRED SecurityScheme.type property is not available, this is an invalid OpenAPI document");
                }
                else
                {
                    this.logger.LogWarning("The REQUIRED SecurityScheme.type property is not available, this is an invalid OpenAPI document");
                }
            }
            else
            {
                securityScheme.Type = typeProperty.GetString();
            }

            if (jsonElement.TryGetProperty("description", out JsonElement descriptionProperty))
            {
                securityScheme.Description = descriptionProperty.GetString();
            }

            if (!jsonElement.TryGetProperty("name", out JsonElement nameProperty))
            {
                if (securityScheme.Type == "apiKey" && strict)
                {
                    throw new SerializationException("The REQUIRED SecurityScheme.name property is not available while the securityScheme.Type=apiKey, this is an invalid OpenAPI document");
                }
                else
                {
                    this.logger.LogWarning("The REQUIRED SecurityScheme.name property is not available while the securityScheme.Type=apiKey, this is an invalid OpenAPI document");
                }
            }
            else
            {
                securityScheme.Name = nameProperty.GetString();
            }

            if (!jsonElement.TryGetProperty("in", out JsonElement inProperty))
            {
                if (securityScheme.Type == "apiKey" && strict)
                {
                    throw new SerializationException("The REQUIRED SecurityScheme.in property is not available while the securityScheme.Type=apiKey, this is an invalid OpenAPI document");
                }
                else
                {
                    this.logger.LogWarning("The REQUIRED SecurityScheme.in property is not available while the securityScheme.Type=apiKey, this is an invalid OpenAPI document");
                }
            }
            else
            {
                securityScheme.In = inProperty.GetString();
            }

            if (!jsonElement.TryGetProperty("scheme", out JsonElement schemeProperty))
            {
                if (securityScheme.Type == "http" && strict)
                {
                    throw new SerializationException("The REQUIRED SecurityScheme.scheme property is not available while the securityScheme.Type=http, this is an invalid OpenAPI document");
                }
                else
                {
                    this.logger.LogWarning("The REQUIRED SecurityScheme.scheme property is not available, this is an invalid OpenAPI document");
                }
            }
            else
            {
                securityScheme.Scheme = schemeProperty.GetString();
            }

            if (jsonElement.TryGetProperty("bearerFormat", out JsonElement bearerFormatProperty))
            {
                securityScheme.BearerFormat = bearerFormatProperty.GetString();
            }

            if (!jsonElement.TryGetProperty("flows", out JsonElement flowsProperty))
            {
                if (securityScheme.Type == "oauth2" && strict)
                {
                    throw new SerializationException("The REQUIRED SecurityScheme.flows property is not available while the securityScheme.Type=oauth2, this is an invalid OpenAPI document");
                }
                else
                {
                    this.logger.LogWarning("The REQUIRED SecurityScheme.flows property is not available while the securityScheme.Type=oauth2, this is an invalid OpenAPI document");
                }
            }
            else
            {
                var authFlowsDeSerializer = new OAuthFlowsDeSerializer(this.loggerFactory);
                securityScheme.Flows = authFlowsDeSerializer.DeSerialize(flowsProperty);
            }

            if (!jsonElement.TryGetProperty("openIdConnectUrl", out JsonElement openIdConnectUrlProperty))
            {
                if (securityScheme.Type == "openIdConnect" && strict)
                {
                    throw new SerializationException("The REQUIRED SecurityScheme.openIdConnectUrl property is not available while the securityScheme.Type=openIdConnect, this is an invalid OpenAPI document");
                }
                else
                {
                    this.logger.LogWarning("The REQUIRED SecurityScheme.openIdConnectUrl property is not available while the securityScheme.Type=openIdConnect, this is an invalid OpenAPI document");
                }
            }
            else
            {
                securityScheme.OpenIdConnectUrl = openIdConnectUrlProperty.GetString();
            }

            this.logger.LogTrace("Finish SecuritySchemeDeSerializer.DeSerialize");

            return securityScheme;
        }
    }
}
