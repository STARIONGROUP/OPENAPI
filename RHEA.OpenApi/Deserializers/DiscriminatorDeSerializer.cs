// -------------------------------------------------------------------------------------------------
// <copyright file="DiscriminatorDeSerializer.cs" company="RHEA System S.A.">
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
    /// The purpose of the <see cref="DiscriminatorDeSerializer"/> is to deserialize the <see cref="Discriminator"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#discriminator-object
    /// </remarks>
    internal class DiscriminatorDeSerializer
    {
        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<DiscriminatorDeSerializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscriminatorDeSerializer"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal DiscriminatorDeSerializer(ILoggerFactory loggerFactory = null)
        {
            this.logger = loggerFactory == null ? NullLogger<DiscriminatorDeSerializer>.Instance : loggerFactory.CreateLogger<DiscriminatorDeSerializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="Discriminator"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Discriminator"/> json object
        /// </param>
        /// <returns></returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Discriminator"/> object
        /// </exception>
        internal Discriminator DeSerialize(JsonElement jsonElement)
        {
            var discriminator = new Discriminator();
            
            if (!jsonElement.TryGetProperty("propertyName", out JsonElement propertyNameProperty))
            {
                throw new SerializationException("The REQUIRED Discriminator.propertyName property is not available, this is an invalid OpenAPI document");
            }

            discriminator.PropertyName = propertyNameProperty.GetString();

            if (jsonElement.TryGetProperty("mapping", out JsonElement mappingProperty))
            {
                foreach (var item in mappingProperty.EnumerateObject())
                {
                    discriminator.Mapping.Add(item.Name, item.Value.GetString());
                }
            }
            else
            {
                this.logger.LogTrace("The optional Discriminator.mapping property is not provided in the OpenApi document");
            }

            return discriminator;
        }
    }
}
