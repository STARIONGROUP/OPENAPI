// -------------------------------------------------------------------------------------------------
// <copyright file="ExampleDeSerializer.cs" company="RHEA System S.A.">
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
    /// The purpose of the <see cref="ExampleDeSerializer"/> is to deserialize the <see cref="Example"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#example-object
    /// </remarks>
    internal class ExampleDeSerializer
    {
        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<ExampleDeSerializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleDeSerializer"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal ExampleDeSerializer(ILoggerFactory loggerFactory = null)
        {
            this.logger = loggerFactory == null ? NullLogger<ExampleDeSerializer>.Instance : loggerFactory.CreateLogger<ExampleDeSerializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="Example"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Example"/> json object
        /// </param>
        /// <returns></returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Example"/> object
        /// </exception>
        internal Example DeSerialize(JsonElement jsonElement)
        {
            var example = new Example();

            if (jsonElement.TryGetProperty("summary", out JsonElement summaryProperty))
            {
                example.Summary = summaryProperty.GetString();
            }
            else
            {
                this.logger.LogTrace("The optional Example.summary property is not provided in the OpenApi document");
            }

            if (jsonElement.TryGetProperty("description", out JsonElement descriptionProperty))
            {
                example.Description = descriptionProperty.GetString();
            }
            else
            {
                this.logger.LogTrace("The optional Example.description property is not provided in the OpenApi document");
            }
            
            if (jsonElement.TryGetProperty("value", out JsonElement valueProperty))
            {
                example.Value = valueProperty.ToString();
            }
            else
            {
                this.logger.LogTrace("The optional Example.value property is not provided in the OpenApi document");
            }

            if (jsonElement.TryGetProperty("externalValue", out JsonElement externalValueProperty))
            {
                example.ExternalValue = externalValueProperty.GetString();
            }
            else
            {
                this.logger.LogTrace("The optional Example.externalValue property is not provided in the OpenApi document");
            }

            return example;
        }
    }
}
