// -------------------------------------------------------------------------------------------------
// <copyright file="ContactDeSerializer.cs" company="RHEA System S.A.">
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
    /// The purpose of the <see cref="ContactDeSerializer"/> is to deserialize the <see cref="Contact"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#contact-object
    /// </remarks>
    internal class ContactDeSerializer
    {
        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<ContactDeSerializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactDeSerializer"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal ContactDeSerializer(ILoggerFactory loggerFactory = null)
        {
            this.logger = loggerFactory == null ? NullLogger<ContactDeSerializer>.Instance : loggerFactory.CreateLogger<ContactDeSerializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="Contact"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Contact"/> json object
        /// </param>
        /// <returns></returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Contact"/> object
        /// </exception>
        internal Contact DeSerialize(JsonElement jsonElement)
        {
            this.logger.LogTrace("Start ContactDeSerializer.DeSerialize");

            var contact = new Contact();

            if (jsonElement.TryGetProperty("name", out JsonElement nameProperty))
            {
                contact.Name = nameProperty.GetString();
            }

            if (jsonElement.TryGetProperty("url", out JsonElement urlProperty))
            {
                contact.Url = urlProperty.GetString();
            }

            if (jsonElement.TryGetProperty("email", out JsonElement emailProperty))
            {
                contact.Email = emailProperty.GetString();
            }

            this.logger.LogTrace("Finish ContactDeSerializer.DeSerialize");

            return contact;
        }
    }
}
