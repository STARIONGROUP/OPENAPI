// -------------------------------------------------------------------------------------------------
// <copyright file="ReferenceDeSerializer.cs" company="RHEA System S.A.">
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
    /// The purpose of the <see cref="ReferenceDeSerializer"/> is to deserialize the <see cref="Reference"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#reference-object
    /// </remarks>
    internal class ReferenceDeSerializer
    {
        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<ReferenceDeSerializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceDeSerializer"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal ReferenceDeSerializer(ILoggerFactory loggerFactory = null)
        {
            this.logger = loggerFactory == null ? NullLogger<ReferenceDeSerializer>.Instance : loggerFactory.CreateLogger<ReferenceDeSerializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="Reference"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Reference"/> json object
        /// </param>
        /// <returns></returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Reference"/> object
        /// </exception>
        internal Reference DeSerialize(JsonElement jsonElement)
        {
            var reference = new Reference();

            if (!jsonElement.TryGetProperty("$ref", out JsonElement rwfProperty))
            {
                throw new SerializationException("The REQUIRED Reference.$ref property is not available, this is an invalid Reference object");
            }

            reference.Ref = rwfProperty.GetString();

            if (jsonElement.TryGetProperty("summary", out JsonElement summaryProperty))
            {
                reference.Summary = summaryProperty.GetString();
            }
            else
            {
                this.logger.LogTrace("The optional Reference.summary property is not provided in the OpenApi document");
            }

            if (jsonElement.TryGetProperty("description", out JsonElement descriptionProperty))
            {
                reference.Description = descriptionProperty.GetString();
            }
            else
            {
                this.logger.LogTrace("The optional Reference.description property is not provided in the OpenApi document");
            }

            return reference;
        }
    }
}
