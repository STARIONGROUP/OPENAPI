// -------------------------------------------------------------------------------------------------
// <copyright file="InfoDeserializer.cs" company="RHEA System S.A.">
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
    /// The purpose of the <see cref="InfoDeserializer"/> is to deserialize the <see cref="Info"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#info-object
    /// </remarks>
    internal class InfoDeserializer
    {
        /// <summary>
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<InfoDeserializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentDeserializer"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal InfoDeserializer(ILoggerFactory loggerFactory = null)
        {
            this.loggerFactory = loggerFactory;

            this.logger = this.loggerFactory == null ? NullLogger<InfoDeserializer>.Instance : this.loggerFactory.CreateLogger<InfoDeserializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="Info"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Info"/> json object
        /// </param>
        /// <returns></returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Info"/> object
        /// </exception>
        internal Info DeSerialize(JsonElement jsonElement)
        {
            var info = new Info();

            if (!jsonElement.TryGetProperty("title", out JsonElement titleProperty))
            {
                throw new SerializationException("The REQUIRED Info.title property is not available, this is an invalid OpenAPI document");
            }
            
            info.Title= titleProperty.GetString();

            if (jsonElement.TryGetProperty("summary", out JsonElement summaryProperty))
            {
                info.Summary = summaryProperty.GetString();
            }
            
            if (jsonElement.TryGetProperty("description", out JsonElement descriptionProperty))
            {
                info.Description = descriptionProperty.GetString();
            }

            if (jsonElement.TryGetProperty("termsOfService", out JsonElement termsOfServiceProperty))
            {
                info.TermsOfService= termsOfServiceProperty.GetString();
            }
            
            if (jsonElement.TryGetProperty("contact", out JsonElement contactProperty))
            {
                var contactDeSerializer = new ContactDeSerializer(this.loggerFactory); 
                info.Contact = contactDeSerializer.DeSerialize(contactProperty);
            }
            
            if (jsonElement.TryGetProperty("license", out JsonElement licenseProperty))
            {
                var licenseDeSerializer = new LicenseDeSerializer(this.loggerFactory);
                info.License = licenseDeSerializer.DeSerialize(licenseProperty);
            }

            if (!jsonElement.TryGetProperty("version", out JsonElement versionProperty))
            {
                throw new SerializationException("The REQUIRED Info.version property is not available, this is an invalid OpenAPI document");
            }

            info.Version = versionProperty.GetString();

            return info;
        }
    }
}
