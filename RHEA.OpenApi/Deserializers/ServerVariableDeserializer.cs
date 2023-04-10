// -------------------------------------------------------------------------------------------------
// <copyright file="ServerVariableDeserializer.cs" company="RHEA System S.A.">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text.Json;

    using Microsoft.Extensions.Logging;

    using Microsoft.Extensions.Logging.Abstractions;
    using OpenApi.Model;

    /// <summary>
    /// The purpose of the <see cref="ServerVariableDeserializer"/> is to deserialize the <see cref="ServerVariable"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#server-variable-object
    /// </remarks> 
    internal class ServerVariableDeserializer
    {
        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<ServerVariableDeserializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerVariableDeserializer"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal ServerVariableDeserializer(ILoggerFactory loggerFactory = null)
        {
            this.logger = loggerFactory == null ? NullLogger<ServerVariableDeserializer>.Instance : loggerFactory.CreateLogger<ServerVariableDeserializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="ServerVariable"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="ServerVariable"/> json object
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <returns>
        /// an instance of <see cref="ServerVariable"/>
        /// </returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="ServerVariable"/> object
        /// </exception>
        internal ServerVariable DeSerialize(JsonElement jsonElement, bool strict)
        {
            this.logger.LogTrace("Start ServerDeSerializer.DeSerialize");

            var serverVariable = new ServerVariable();
            
            if (jsonElement.TryGetProperty("enum", out JsonElement enumProperty))
            {
                if (enumProperty.ValueKind != JsonValueKind.Array)
                {
                    if (strict)
                    {
                        throw new SerializationException("The ServerVariable.enum property must be an array");
                    }
                    else
                    {
                        this.logger.LogWarning("The ServerVariable.enum property must be an array");
                    }
                }

                var enums = new List<string>();

                foreach (var arrayItem in enumProperty.EnumerateArray())
                {
                    enums.Add(arrayItem.GetString());
                }

                if (!enums.Any())
                {
                    if (strict)
                    {
                        throw new SerializationException("The ServerVariable.enum must not be empty");
                    }
                    else
                    {
                        this.logger.LogWarning("The ServerVariable.enum must not be empty");
                    }
                }
                else
                {
                    serverVariable.Enum = enums;
                }
            }

            if (jsonElement.TryGetProperty("default", out JsonElement defaultProperty))
            {
                serverVariable.Default = defaultProperty.GetString();
            }

            if (!string.IsNullOrEmpty(serverVariable.Default))
            {
                if (serverVariable.Enum.Any() && !serverVariable.Enum.Contains(serverVariable.Default))
                {
                    if (strict)
                    {
                        throw new SerializationException("The ServerVariable.enum is specified and does not contain the ServerVariable.default value");
                    }
                    else
                    {
                        this.logger.LogWarning("The ServerVariable.enum is specified and does not contain the ServerVariable.default value");
                    }
                }
            }

            if (jsonElement.TryGetProperty("description", out JsonElement descriptionProperty))
            {
                serverVariable.Description = descriptionProperty.GetString();
            }

            this.logger.LogTrace("Finish ServerDeSerializer.DeSerialize");

            return serverVariable;
        }
    }
}
