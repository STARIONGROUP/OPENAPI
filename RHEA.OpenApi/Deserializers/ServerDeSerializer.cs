// -------------------------------------------------------------------------------------------------
// <copyright file="ServerDeSerializer.cs" company="RHEA System S.A.">
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
    /// The purpose of the <see cref="ServerDeSerializer"/> is to deserialize the <see cref="Server"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#server-object
    /// </remarks>
    internal class ServerDeSerializer
    {
        /// <summary>
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<ServerDeSerializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerDeSerializer"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal ServerDeSerializer(ILoggerFactory loggerFactory = null)
        {
            this.loggerFactory = loggerFactory;

            this.logger = this.loggerFactory == null ? NullLogger<ServerDeSerializer>.Instance : this.loggerFactory.CreateLogger<ServerDeSerializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="Server"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Server"/> json object
        /// </param>
        /// <returns></returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Server"/> object
        /// </exception>
        internal Server DeSerialize(JsonElement jsonElement)
        {
            this.logger.LogTrace("Start ServerDeSerializer.DeSerialize");

            var server = new Server();

            if (!jsonElement.TryGetProperty("url", out JsonElement urlProperty))
            {
                throw new SerializationException("The REQUIRED Server.url property is not available, this is an invalid OpenAPI document");
            }

            server.Url = urlProperty.GetString();
            
            if (jsonElement.TryGetProperty("description", out JsonElement descriptionProperty))
            {
                server.Description = descriptionProperty.GetString();
            }

            if (jsonElement.TryGetProperty("variables", out JsonElement variablesProperty))
            {
                var serverVariableDeserializer = new ServerVariableDeserializer(this.loggerFactory);

                foreach (var v in variablesProperty.EnumerateObject())
                {
                    var serverVariableName = v.Name;

                    var serverVariable = serverVariableDeserializer.DeSerialize(v.Value);

                    server.Variables.Add(serverVariableName, serverVariable);
                }

                server.Description = descriptionProperty.GetString();
            }

            this.logger.LogTrace("Finish ServerDeSerializer.DeSerialize");

            return server;
        }
    }
}
