// -------------------------------------------------------------------------------------------------
// <copyright file="DeSerializer.cs" company="RHEA System S.A.">
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

namespace OpenApi
{
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Text. Json;
    
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;

    using OpenApi.Deserializers;
    using OpenApi.Model;

    /// <summary>
    /// The purpose of the <see cref="DeSerializer"/> is to deserialize a JSON <see cref="Stream"/> to
    /// an <see cref="Document"/>
    /// </summary>
    public class DeSerializer : IDeSerializer
    {
        /// <summary>
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<DeSerializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeSerializer"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        public DeSerializer(ILoggerFactory loggerFactory = null)
        {
            this.loggerFactory = loggerFactory;

            this.logger = this.loggerFactory == null ? NullLogger<DeSerializer>.Instance : this.loggerFactory.CreateLogger<DeSerializer>();
        }

        /// <summary>
        /// Deserializes the JSON stream to an <see cref="Document"/>
        /// </summary>
        /// <param name="stream">
        /// the JSON input stream
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <returns>
        /// an <see cref="Document"/>
        /// </returns>
        public Document DeSerialize(Stream stream, bool strict = true)
        {
            var sw = Stopwatch.StartNew();

            Document document;

            using (var jsonDocument = JsonDocument.Parse(stream))
            {
                var root = jsonDocument.RootElement;

                switch (root.ValueKind)
                {
                    case JsonValueKind.Object:
                        var documentDeserializer = new DocumentDeserializer(loggerFactory);
                        document = documentDeserializer.DeSerialize(root, strict);
                        break;
                    default:
                        throw new SerializationException("The provided stream does not contain valid open api JSON");
                }
            }

            this.logger.LogInformation("stream deserialized in {ElapsedTime} [ms]", sw.ElapsedMilliseconds);

            return document;
        }
    }
}
