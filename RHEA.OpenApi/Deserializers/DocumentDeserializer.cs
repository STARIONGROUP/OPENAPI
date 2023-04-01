// -------------------------------------------------------------------------------------------------
// <copyright file="DocumentDeserializer.cs" company="RHEA System S.A.">
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
    using System;
    using System.Runtime.Serialization;
    using System.Text.Json;

    using OpenApi.Model;

    /// <summary>
    /// The purpose of the <see cref="DocumentDeserializer"/> is to deserialize the <see cref="Document"/> object
    /// </summary>
    public class DocumentDeserializer
    {
        public Document DeSerialize(JsonElement jsonElement)
        {
            var document = new Document();

            if (!jsonElement.TryGetProperty("openapi", out JsonElement openapiProperty))
            {
                throw new SerializationException("The REQUIRED openapi property is not available, this is an invalid OpenAPI document");
            }

            document.OpenApi = openapiProperty.GetString();

            if (!jsonElement.TryGetProperty("info", out JsonElement infoProperty))
            {
                throw new SerializationException("The REQUIRED info property is not available, this is an invalid OpenAPI document");
            }

            var info = new Info();

            return document;

            // jsonSchemaDialect

            // servers

            // paths

            // webhooks

            // components

            // security

            // tags

            // externalDocs
        }
    }
}
