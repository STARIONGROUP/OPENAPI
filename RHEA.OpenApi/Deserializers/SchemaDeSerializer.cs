// -------------------------------------------------------------------------------------------------
// <copyright file="SchemaDeSerializer.cs" company="RHEA System S.A.">
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
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text.Json;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;

    using OpenApi.JsonSchema;
    using OpenApi.Model;

    /// <summary>
    /// The purpose of the <see cref="SchemaDeSerializer"/> is to deserialize the <see cref="Schema"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#schema-object
    /// </remarks>
    internal class SchemaDeSerializer : ReferencerDeserializer
    {
        /// <summary>
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<SchemaDeSerializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaDeSerializer"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal SchemaDeSerializer(ReferenceResolver referenceResolver, ILoggerFactory loggerFactory = null)
            : base(referenceResolver)
        {
            this.loggerFactory = loggerFactory;

            this.logger = loggerFactory == null ? NullLogger<SchemaDeSerializer>.Instance : loggerFactory.CreateLogger<SchemaDeSerializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="Schema"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Schema"/> json object
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <returns>
        /// an instance of <see cref="Schema"/>
        /// </returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Schema"/> object
        /// </exception>
        internal Schema DeSerialize(JsonElement jsonElement, bool strict)
        {
            Schema schema = null;

            switch (jsonElement.ValueKind)
            {
                case JsonValueKind.True:
                    schema = new BooleanJsonSchema(true);
                    break;
                case JsonValueKind.False:
                    schema = new BooleanJsonSchema(false);
                    break;
                case JsonValueKind.Object:
                    schema = new JsonSchema();
                    this.DeSerialize(jsonElement, strict, (JsonSchema)schema);
                    break;
            }

            return schema;
        }

        /// <summary>
        /// Deserializes an instance of <see cref="Schema"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Schema"/> json object
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <param name="jsonSchema">
        /// The <see cref="JsonSchema"/> object that needs to be updated with the data from the <paramref name="jsonElement"/>
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Schema"/> object
        /// </exception>
        internal void DeSerialize(JsonElement jsonElement, bool strict, JsonSchema jsonSchema)
        {
            this.logger.LogTrace("Start SchemaDeSerializer.DeSerialize");

            if (jsonElement.TryGetProperty("discriminator", out JsonElement discriminatorProperty))
            {
                var discriminatorDeSerializer = new DiscriminatorDeSerializer(this.loggerFactory);
                jsonSchema.Discriminator = discriminatorDeSerializer.DeSerialize(discriminatorProperty, strict);
            }

            if (jsonElement.TryGetProperty("xml", out JsonElement xmlProperty))
            {
                var xmlDeSerializer = new XMLDeSerializer(this.loggerFactory);
                jsonSchema.XML = xmlDeSerializer.DeSerialize(xmlProperty);
            }

            if (jsonElement.TryGetProperty("externalDocs", out JsonElement externalDocsProperty))
            {
                var externalDocumentationDeSerializer = new ExternalDocumentationDeSerializer(this.loggerFactory);
                jsonSchema.ExternalDocs = externalDocumentationDeSerializer.DeSerialize(externalDocsProperty, strict);

            }

            if (jsonElement.TryGetProperty("example", out JsonElement exampleProperty))
            {
                jsonSchema.Example = exampleProperty.ToString();
            }

            if (jsonElement.TryGetProperty("$id", out JsonElement idProperty))
            {
                jsonSchema.Identifier = idProperty.ToString();
            }

            if (jsonElement.TryGetProperty("type", out JsonElement typeProperty))
            {
                switch (typeProperty.ValueKind)
                { 
                    case JsonValueKind.String:
                        jsonSchema.Type.Add(JsonSchemaTypeDeserializer.Deserialize(typeProperty.GetString()));
                        break;
                    case JsonValueKind.Array:
                        foreach (var arrayItem in typeProperty.EnumerateArray())
                        {
                            var jsonSchemaType = JsonSchemaTypeDeserializer.Deserialize(arrayItem.GetString());
                            jsonSchema.Type.Add(jsonSchemaType);
                        }
                        break;
                }
            }

            if (jsonSchema.Type.Count == 1 && jsonSchema.Type.Single() == JsonSchemaType.Array)
            {
                if (jsonElement.TryGetProperty("items", out JsonElement itemsProperty))
                {
                    if (itemsProperty.TryGetProperty("$ref", out var referenceElement))
                    {
                        var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);
                        var reference = referenceDeSerializer.DeSerialize(itemsProperty, strict);
                        jsonSchema.ItemsReference = reference;

                        this.Register(reference, jsonSchema, "items");
                    }
                    else
                    {
                        var schemaDeserializer = new SchemaDeSerializer(this.referenceResolver, this.loggerFactory);
                        var subJsonSchema = schemaDeserializer.DeSerialize(itemsProperty, strict);
                        jsonSchema.Items = subJsonSchema;
                    }
                }
            }

            if (jsonElement.TryGetProperty("properties", out JsonElement propertiesProperty))
            {
                var schemaDeSerializer = new SchemaDeSerializer(this.referenceResolver, this.loggerFactory);

                foreach (var item in propertiesProperty.EnumerateObject())
                {
                    var key = item.Name;
                    var schema = schemaDeSerializer.DeSerialize(item.Value, strict);
                    jsonSchema.Properties.Add(key, schema);
                }
            }

            if (jsonElement.TryGetProperty("allOf", out JsonElement allOfProperty))
            {
                foreach (var item in allOfProperty.EnumerateArray())
                {
                    if (item.TryGetProperty("$ref", out var referenceElement))
                    {
                        var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);
                        var reference = referenceDeSerializer.DeSerialize(item, strict);
                        jsonSchema.AllOfReferences.Add(reference);
                        this.Register(reference, jsonSchema, "allOf");
                    }
                    else
                    {
                        var schemaDeSerializer = new SchemaDeSerializer(this.referenceResolver, this.loggerFactory);
                        var allOfSchema = schemaDeSerializer.DeSerialize(item, strict);
                        jsonSchema.AllOf.Add(allOfSchema);
                    }
                }
            }

            if (jsonElement.TryGetProperty("anyOf", out JsonElement anyOfProperty))
            {
                foreach (var item in anyOfProperty.EnumerateArray())
                {
                    if (item.TryGetProperty("$ref", out var referenceElement))
                    {
                        var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);
                        var reference = referenceDeSerializer.DeSerialize(item, strict);
                        jsonSchema.AnyOfReferences.Add(reference);

                        this.Register(reference, jsonSchema, "anyOf");
                    }
                    else
                    {
                        var schemaDeSerializer = new SchemaDeSerializer(this.referenceResolver, this.loggerFactory);
                        var anyOfSchema = schemaDeSerializer.DeSerialize(item, strict);
                        jsonSchema.AnyOf.Add(anyOfSchema);
                    }
                }
            }

            if (jsonElement.TryGetProperty("oneOf", out JsonElement oneOfProperty))
            {
                foreach (var item in oneOfProperty.EnumerateArray())
                {
                    if (item.TryGetProperty("$ref", out var referenceElement))
                    {
                        var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);
                        var reference = referenceDeSerializer.DeSerialize(item, strict);
                        jsonSchema.OneOfReferences.Add(reference);

                        this.Register(reference, jsonSchema, "oneOf");
                    }
                    else
                    {
                        var schemaDeSerializer = new SchemaDeSerializer(this.referenceResolver, this.loggerFactory);
                        var oneOfSchema = schemaDeSerializer.DeSerialize(item, strict);
                        jsonSchema.OneOf.Add(oneOfSchema);
                    }
                }
            }

            if (jsonElement.TryGetProperty("not", out JsonElement notProperty))
            {
                if (notProperty.TryGetProperty("$ref", out var referenceElement))
                {
                    var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);
                    var reference = referenceDeSerializer.DeSerialize(notProperty, strict);
                    jsonSchema.NotReference = reference;

                    this.Register(reference, jsonSchema, "not");
                }
                else
                {
                    var schemaDeSerializer = new SchemaDeSerializer(this.referenceResolver, this.loggerFactory);
                    jsonSchema.Not = schemaDeSerializer.DeSerialize(notProperty, strict);
                }
            }

            if (jsonElement.TryGetProperty("required", out JsonElement requiredProperty))
            {
                foreach (var item in requiredProperty.EnumerateArray())
                {
                    jsonSchema.Required.Add(item.GetString());
                }
            }

            if (jsonElement.TryGetProperty("enum", out JsonElement enumProperty))
            {
                if (jsonSchema.Type.Contains(JsonSchemaType.String))
                {
                    foreach (var item in enumProperty.EnumerateArray())
                    {
                        jsonSchema.Enum.Add(item.GetString());
                    }
                }
                else 
                {
                    this.logger.LogWarning("non string enums are not yet supported");
                }
            }

            if (jsonElement.TryGetProperty("const", out JsonElement constProperty))
            {
                jsonSchema.Const = constProperty.ToString();
            }

            if (jsonElement.TryGetProperty("title", out JsonElement titleProperty))
            {
                jsonSchema.Title = titleProperty.ToString();
            }

            if (jsonElement.TryGetProperty("description", out JsonElement descriptionProperty))
            {
                jsonSchema.Description = descriptionProperty.ToString();
            }

            if (jsonElement.TryGetProperty("$comment", out JsonElement commentProperty))
            {
                jsonSchema.Comments = commentProperty.ToString();
            }

            if (jsonElement.TryGetProperty("deprecated", out JsonElement deprecatedProperty))
            {
                jsonSchema.Deprecated = deprecatedProperty.GetBoolean();
            }

            if (jsonElement.TryGetProperty("readOnly", out JsonElement readOnlyProperty))
            {
                jsonSchema.ReadOnly = readOnlyProperty.GetBoolean();
            }

            if (jsonElement.TryGetProperty("writeOnly", out JsonElement writeOnlyProperty))
            {
                jsonSchema.WriteOnly = writeOnlyProperty.GetBoolean();
            }

            if (jsonElement.TryGetProperty("contentEncoding", out JsonElement contentEncodingProperty))
            {
                jsonSchema.ContentEncoding = contentEncodingProperty.GetString();
            }

            if (jsonElement.TryGetProperty("contentMediaType", out JsonElement contentMediaTypeProperty))
            {
                jsonSchema.ContentMediaType = contentMediaTypeProperty.GetString();

                if (jsonElement.TryGetProperty("contentSchema", out JsonElement contentSchemaProperty))
                {
                    var schemaDeSerializer = new SchemaDeSerializer(this.referenceResolver, this.loggerFactory);

                    jsonSchema.ContentSchema = schemaDeSerializer.DeSerialize(contentSchemaProperty, strict);
                }
            }

            this.logger.LogTrace("Finish SchemaDeSerializer.DeSerialize");
        }
    }
}
