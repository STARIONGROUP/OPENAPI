// -------------------------------------------------------------------------------------------------
// <copyright file="LicenseDeSerializer.cs" company="RHEA System S.A.">
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
    /// The purpose of the <see cref="LicenseDeSerializer"/> is to deserialize the <see cref="License"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#license-object
    /// </remarks>
    internal class LicenseDeSerializer
    {
        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<LicenseDeSerializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseDeSerializer"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal LicenseDeSerializer(ILoggerFactory loggerFactory = null)
        {
            this.logger = loggerFactory == null ? NullLogger<LicenseDeSerializer>.Instance : loggerFactory.CreateLogger<LicenseDeSerializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="License"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="License"/> json object
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <returns>
        /// an instance of <see cref="License"/>
        /// </returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="License"/> object
        /// </exception>
        internal License DeSerialize(JsonElement jsonElement, bool strict)
        {
            this.logger.LogTrace("Start LicenseDeSerializer.DeSerialize");

            var license = new License();
            
            if (!jsonElement.TryGetProperty("name", out JsonElement nameProperty))
            {
                if (strict)
                {
                    throw new SerializationException("The REQUIRED License.name property is not available, this is an invalid OpenAPI document");
                }
                else
                {
                    this.logger.LogWarning("The REQUIRED License.name property is not available, this is an invalid OpenAPI document");
                }
            }
            else
            {
                license.Name = nameProperty.GetString();
            }

            if (jsonElement.TryGetProperty("identifier", out JsonElement identifierProperty))
            {
                license.Identifier = identifierProperty.GetString();
            }

            if (jsonElement.TryGetProperty("url", out JsonElement urlProperty))
            {
                license.Url = urlProperty.GetString();
            }

            this.logger.LogTrace("Finish LicenseDeSerializer.DeSerialize");

            return license;
        }
    }
}
