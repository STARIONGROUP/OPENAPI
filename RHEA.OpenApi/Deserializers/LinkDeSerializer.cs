// -------------------------------------------------------------------------------------------------
// <copyright file="LinkDeSerializer.cs" company="RHEA System S.A.">
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
    /// The purpose of the <see cref="LinkDeSerializer"/> is to deserialize the <see cref="Link"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#link-object
    /// </remarks>
    internal class LinkDeSerializer
    {
        /// <summary>
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<LinkDeSerializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkDeSerializer"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal LinkDeSerializer(ILoggerFactory loggerFactory = null)
        {
            this.loggerFactory = loggerFactory;

            this.logger = this.loggerFactory == null ? NullLogger<LinkDeSerializer>.Instance : this.loggerFactory.CreateLogger<LinkDeSerializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="Link"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Link"/> json object
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <returns>
        /// An instance of an Open Api <see cref="Link"/>
        /// </returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Link"/> object
        /// </exception>
        internal Link DeSerialize(JsonElement jsonElement, bool strict)
        {
            this.logger.LogTrace("Start Link.DeSerialize");

            var link = new Link();

            if (jsonElement.TryGetProperty("operationRef"u8, out JsonElement operationRefProperty))
            {
                link.OperationRef = operationRefProperty.GetString();
            }

            if (jsonElement.TryGetProperty("operationId"u8, out JsonElement operationIdProperty))
            {
                link.OperationId = operationIdProperty.GetString();
            }

            // parameters
            this.logger.LogWarning("TODO: implement Link.parameters");

            if (jsonElement.TryGetProperty("requestBody"u8, out JsonElement requestBodyProperty))
            {
                link.RequestBody = requestBodyProperty.GetString();
            }

            if (jsonElement.TryGetProperty("description"u8, out JsonElement descriptionProperty))
            {
                link.Description = descriptionProperty.GetString();
            }

            if (jsonElement.TryGetProperty("server"u8, out JsonElement serverProperty))
            {
                var serverDeSerializer = new ServerDeSerializer(this.loggerFactory);

                link.Server = serverDeSerializer.DeSerialize(serverProperty, strict);
            }

            this.logger.LogTrace("Finish Link.DeSerialize");

            return link;
        }
    }
}
