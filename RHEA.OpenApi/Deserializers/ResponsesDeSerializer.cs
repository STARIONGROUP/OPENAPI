// -------------------------------------------------------------------------------------------------
// <copyright file="ResponsesDeSerializer.cs" company="RHEA System S.A.">
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
    /// The purpose of the <see cref="ResponsesDeSerializer"/> is to deserialize the <see cref="Responses"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#responses-object
    /// </remarks>
    internal class ResponsesDeSerializer : ReferencerDeserializer
    {
        /// <summary>
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<ResponsesDeSerializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponsesDeSerializer"/> class.
        /// </summary>
        /// <param name="referenceResolver">
        /// The <see cref="ReferenceResolver"/> that is used to register any <see cref="ReferenceInfo"/> objects
        /// and later resolve them
        /// </param>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal ResponsesDeSerializer(ReferenceResolver referenceResolver, ILoggerFactory loggerFactory = null)
            : base(referenceResolver)
        {
            this.loggerFactory = loggerFactory;

            this.logger = this.loggerFactory == null ? NullLogger<ResponsesDeSerializer>.Instance : this.loggerFactory.CreateLogger<ResponsesDeSerializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="Responses"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Responses"/> json object
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <returns>
        /// An instance of an Open Api <see cref="Responses"/>
        /// </returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Responses"/> object
        /// </exception>
        internal Responses DeSerialize(JsonElement jsonElement, bool strict)
        {
            this.logger.LogTrace("Start Responses.DeSerialize");

            var responses = new Responses();

            var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);
            var responseDeserializer = new ResponseDeserializer(this.referenceResolver, this.loggerFactory);

            foreach (var itemProperty in jsonElement.EnumerateObject())
            {
                var key = itemProperty.Name;
                
                if (itemProperty.Value.TryGetProperty("$ref"u8, out var referenceElement))
                {
                    var reference = referenceDeSerializer.DeSerialize(itemProperty.Value, strict);
                    responses.ResponseReferences.Add(key, reference);
                    this.Register(reference, responses, "responses", key);
                }
                else
                {
                    var response = responseDeserializer.DeSerialize(itemProperty.Value, strict);
                    responses.Response.Add(key, response);
                }
            }

            this.logger.LogTrace("Finish Responses.DeSerialize");

            return responses;
        }
    }
}
