// -------------------------------------------------------------------------------------------------
// <copyright file="OAuthFlowDeSerializer.cs" company="RHEA System S.A.">
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
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text.Json;

    using Microsoft.Extensions.Logging;

    using Microsoft.Extensions.Logging.Abstractions;
    using OpenApi.Model;

    /// <summary>
    /// The purpose of the <see cref="OAuthFlowDeSerializer"/> is to deserialize the <see cref="OAuthFlow"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#oauth-flow-object
    /// </remarks>
    internal class OAuthFlowDeSerializer
    {
        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<OAuthFlowDeSerializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthFlowDeSerializer"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal OAuthFlowDeSerializer(ILoggerFactory loggerFactory = null)
        {
            this.logger = loggerFactory == null ? NullLogger<OAuthFlowDeSerializer>.Instance : loggerFactory.CreateLogger<OAuthFlowDeSerializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="Document"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Document"/> json object
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <returns>
        /// An instance of an Open Api <see cref="Document"/>
        /// </returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Document"/> object
        /// </exception>
        internal OAuthFlow DeSerialize(JsonElement jsonElement, bool strict)
        {
            this.logger.LogTrace("Start OAuthFlowsDeSerializer.DeSerialize");

            var oAuthFlow = new OAuthFlow();

            if (jsonElement.TryGetProperty("authorizationUrl", out JsonElement authorizationUrlProperty))
            {
                oAuthFlow.AuthorizationUrl = authorizationUrlProperty.GetString();
            }

            if (jsonElement.TryGetProperty("tokenUrl", out JsonElement tokenUrlProperty))
            {
                oAuthFlow.TokenUrl = tokenUrlProperty.GetString();
            }

            if (jsonElement.TryGetProperty("refreshUrl", out JsonElement refreshUrlProperty))
            {
                oAuthFlow.RefreshUrl = refreshUrlProperty.GetString();
            }

            if (jsonElement.TryGetProperty("scopes", out JsonElement scopesProperty))
            {
                foreach (var item in scopesProperty.EnumerateObject())
                {
                    oAuthFlow.Scopes.Add(item.Name, item.Value.GetString());
                }
            }

            foreach (var jsonProperty in jsonElement.EnumerateObject().Where(jsonProperty => jsonProperty.Name.StartsWith("x-")))
            {
                this.logger.LogWarning("extensions are not yet supported: {extension}", jsonProperty.Name);
            }

            this.logger.LogTrace("Finish OAuthFlowsDeSerializer.DeSerialize");

            return oAuthFlow;
        }
    }
}