// -------------------------------------------------------------------------------------------------
// <copyright file="ReferenceResolver.cs" company="RHEA System S.A.">
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
    using System;
    using System.Collections.Generic;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;

    using OpenApi.Model;

    /// <summary>
    /// The purpose of the <see cref="ReferenceResolver"/> is to resolve all <see cref="Reference"/> objects
    /// to the referenced item
    /// </summary>
    internal class ReferenceResolver
    {
        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<ReferenceResolver> logger;

        /// <summary>
        /// The list of registered <see cref="ReferenceInfo"/> instances
        /// </summary>
        private readonly List<ReferenceInfo> referenceInfos;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceResolver"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal ReferenceResolver(ILoggerFactory loggerFactory = null)
        {
            this.logger = loggerFactory == null ? NullLogger<ReferenceResolver>.Instance : loggerFactory.CreateLogger<ReferenceResolver>();

            this.referenceInfos = new List<ReferenceInfo>();
        }

        /// <summary>
        /// Registers the <see cref="ReferenceInfo"/> with the <see cref="ReferenceResolver"/>
        /// </summary>
        /// <param name="referenceInfo"></param>
        internal void Register(ReferenceInfo referenceInfo)
        {
            this.referenceInfos.Add(referenceInfo);
        }

        /// <summary>
        /// Resolves the <see cref="Reference"/>s contained in the provided <paramref name="root"/> open api <see cref="Document"/>
        /// </summary>
        /// <param name="root">
        /// The OpenAPI <see cref="Document"/> and its constituents for which the <see cref="Reference"/>s
        /// need to be resolved.
        /// </param>
        internal void Resolve(Document root)
        {
            foreach (var referenceInfo in referenceInfos)
            {
                if (!referenceInfo.Reference.Ref.StartsWith("#"))
                {
                    this.logger.LogWarning("relative references are not yet supported");
                    continue;
                }

                var uri = referenceInfo.Reference.Ref.Replace("#/components/", "");
                var uriComponents = uri.Split('/');
                var uriPropertyName = uriComponents[0];
                var uriPropertyValue = uriComponents[1];

                switch (referenceInfo.Source)
                {
                    case Document document:
                        this.Resolve(document, root.Components, uriPropertyName, uriPropertyValue, referenceInfo);
                        break;
                    case Components components:
                        this.Resolve(components, root.Components, uriPropertyName, uriPropertyValue, referenceInfo);
                        break;
                    case PathItem pathItem:
                        this.Resolve(pathItem, root.Components, uriPropertyName, uriPropertyValue);
                        break;
                    case Operation operation:
                        this.Resolve(operation, root.Components, uriPropertyName, uriPropertyValue, referenceInfo);
                        break;
                    case Parameter parameter:
                        this.Resolve(parameter, root.Components, uriPropertyName, uriPropertyValue, referenceInfo);
                        break;
                    case MediaType mediaType:
                        this.Resolve(mediaType, root.Components, uriPropertyName, uriPropertyValue, referenceInfo);
                        break;
                    case Encoding encoding:
                        this.Resolve(encoding, root.Components, uriPropertyName, uriPropertyValue, referenceInfo);
                        break;
                    case Responses responses:
                        this.Resolve(responses, root.Components, uriPropertyName, uriPropertyValue, referenceInfo);
                        break;
                    case Response response:
                        this.Resolve(response, root.Components, uriPropertyName, uriPropertyValue, referenceInfo);
                        break;
                    case Callback callback:
                        this.Resolve(callback, root.Components, uriPropertyName, uriPropertyValue, referenceInfo);
                        break;
                    case Header header:
                        this.Resolve(header, root.Components, uriPropertyName, uriPropertyValue, referenceInfo);
                        break;
                    case JsonSchema.JsonSchema jsonSchema:
                        this.Resolve(jsonSchema, root.Components, uriPropertyName, uriPropertyValue, referenceInfo);
                        break;
                    default:
                        this.logger.LogWarning("The {type} should not contain any $ref properties", referenceInfo.Source.GetType());
                        break;
                }
            }
        }

        /// <summary>
        /// Resolves the <see cref="Reference"/>s that are contained in the <see cref="Document"/> object
        /// </summary>
        /// <param name="document">
        /// The <see cref="Document"/> for which the contained <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="targetComponents">
        /// The <see cref="Components"/> that contains the referenced items
        /// </param>
        /// <param name="uriPropertyName">
        /// The name of the property for which the <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="uriPropertyValue">
        /// The value of the property for which the <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="referenceInfo">
        /// The <see cref="ReferenceInfo"/> that contains information to resolve the <see cref="Reference"/>s
        /// </param>
        private void Resolve(Document document, Components targetComponents, string uriPropertyName, string uriPropertyValue, ReferenceInfo referenceInfo)
        {
            if (uriPropertyName == "webhooks")
            {
                if (targetComponents.PathItems.TryGetValue(uriPropertyValue, out var referencedPathItem))
                {
                    document.Webhooks.Add(referenceInfo.Key, referencedPathItem);
                }
                else
                {
                    this.logger.LogWarning("The {uriPropertyValue} PathItem was not found in the Components.PathItems", uriPropertyValue);
                }
            }
            else
            {
                this.logger.LogWarning("The {uriPropertyName} reference property is not a valid reference for the Open API (Document) type", uriPropertyName);
            }
        }

        /// <summary>
        /// Resolves the <see cref="Reference"/>s that are contained in the <see cref="Components"/> object
        /// </summary>
        /// <param name="components">
        /// The <see cref="Components"/> for which the contained <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="targetComponents">
        /// The <see cref="Components"/> that contains the referenced items
        /// </param>
        /// <param name="uriPropertyName">
        /// The name of the property for which the <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="uriPropertyValue">
        /// The value of the property for which the <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="referenceInfo">
        /// The <see cref="ReferenceInfo"/> that contains information to resolve the <see cref="Reference"/>s
        /// </param>
        private void Resolve(Components components, Components targetComponents, string uriPropertyName, string uriPropertyValue, ReferenceInfo referenceInfo)
        {
            switch (uriPropertyName)
            {
                case "responses":
                    if (targetComponents.Responses.TryGetValue(uriPropertyValue, out var referencedResponse))
                    {
                        components.Responses.Add(referenceInfo.Key, referencedResponse);
                    }
                    else
                    {
                        this.logger.LogWarning("The {uriPropertyValue} Response was not found in the Components.Responses", uriPropertyValue);
                    }
                    break;
                case "parameters":
                    if (targetComponents.Parameters.TryGetValue(uriPropertyValue, out var referencedParameter))
                    {
                        components.Parameters.Add(referenceInfo.Key, referencedParameter);
                    }
                    else
                    {
                        this.logger.LogWarning("The {uriPropertyValue} Parameter was not found in the Components.Parameters", uriPropertyValue);
                    }
                    break;
                case "examples":
                    if (targetComponents.Examples.TryGetValue(uriPropertyValue, out var referencedExample))
                    {
                        components.Examples.Add(referenceInfo.Key, referencedExample);
                    }
                    else
                    {
                        this.logger.LogWarning("The {uriPropertyValue} Example was not found in the Components.Examples", uriPropertyValue);
                    }
                    break;
                case "requestBodies":
                    if (targetComponents.RequestBodies.TryGetValue(uriPropertyValue, out var referencedRequestBody))
                    {
                        components.RequestBodies.Add(referenceInfo.Key, referencedRequestBody);
                    }
                    else
                    {
                        this.logger.LogWarning("The {uriPropertyValue} RequestBody was not found in the Components.RequestBodies", uriPropertyValue);
                    }
                    break;
                case "headers":
                    if (targetComponents.Headers.TryGetValue(uriPropertyValue, out var referencedHeader))
                    {
                        components.Headers.Add(referenceInfo.Key, referencedHeader);
                    }
                    else
                    {
                        this.logger.LogWarning("The {uriPropertyValue} Header was not found in the Components.Headers", uriPropertyValue);
                    }
                    break;
                case "securitySchemes":
                    if (targetComponents.SecuritySchemes.TryGetValue(uriPropertyValue, out var referencedSecurityScheme))
                    {
                        components.SecuritySchemes.Add(referenceInfo.Key, referencedSecurityScheme);
                    }
                    else
                    {
                        this.logger.LogWarning("The {uriPropertyValue} Header was not found in the Components.Headers", uriPropertyValue);
                    }
                    break;
                case "links":
                    if (targetComponents.Links.TryGetValue(uriPropertyValue, out var referencedLink))
                    {
                        components.Links.Add(referenceInfo.Key, referencedLink);
                    }
                    else
                    {
                        this.logger.LogWarning("The {uriPropertyValue} Link was not found in the Components.Links", uriPropertyValue);
                    }
                    break;
                case "callbacks":
                    if (targetComponents.Callbacks.TryGetValue(uriPropertyValue, out var referencedCallback))
                    {
                        components.Callbacks.Add(referenceInfo.Key, referencedCallback);
                    }
                    else
                    {
                        this.logger.LogWarning("The {uriPropertyValue} Callback was not found in the Components.Callbacks", uriPropertyValue);
                    }
                    break;
                case "pathItems":
                    if (targetComponents.PathItems.TryGetValue(uriPropertyValue, out var referencedPathItem))
                    {
                        components.PathItems.Add(referenceInfo.Key, referencedPathItem);
                    }
                    else
                    {
                        this.logger.LogWarning("The {uriPropertyValue} PathItem was not found in the Components.PathItems", uriPropertyValue);
                    }
                    break;
                default:
                    this.logger.LogWarning("The {uriPropertyName} reference property is not a valid reference for the Components type", uriPropertyName);
                    break;
            }
        }

        /// <summary>
        /// Resolves the <see cref="Reference"/>s that are contained in the <see cref="PathItem"/> object
        /// </summary>
        /// <param name="pathItem">
        /// The <see cref="PathItem"/> for which the contained <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="targetComponents">
        /// The <see cref="Components"/> that contains the referenced items
        /// </param>
        /// <param name="uriPropertyName">
        /// The name of the property for which the <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="uriPropertyValue">
        /// The value of the property for which the <see cref="Reference"/>s need to be resolved
        /// </param>
        private void Resolve(PathItem pathItem, Components targetComponents, string uriPropertyName, string uriPropertyValue)
        {
            if (uriPropertyName == "parameters")
            {
                if (targetComponents.Parameters.TryGetValue(uriPropertyValue, out var referencedParameter))
                {
                    pathItem.Parameters.Add(referencedParameter);
                }
                else
                {
                    this.logger.LogWarning("The {uriPropertyValue} Parameter was not found in the Components.Parameters", uriPropertyValue);
                }
            }
            else
            {
                this.logger.LogWarning("The {uriPropertyName} reference property is not a valid reference for the PathItem type", uriPropertyName);
            }
        }

        /// <summary>
        /// Resolves the <see cref="Reference"/>s that are contained in the <see cref="Operation"/> object
        /// </summary>
        /// <param name="operation">
        /// The <see cref="Operation"/> for which the contained <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="targetComponents">
        /// The <see cref="Components"/> that contains the referenced items
        /// </param>
        /// <param name="uriPropertyName">
        /// The name of the property for which the <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="uriPropertyValue">
        /// The value of the property for which the <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="referenceInfo">
        /// The <see cref="ReferenceInfo"/> that contains information to resolve the <see cref="Reference"/>s
        /// </param>
        private void Resolve(Operation operation, Components targetComponents, string uriPropertyName, string uriPropertyValue, ReferenceInfo referenceInfo)
        {
            switch (uriPropertyName)
            {
                case "parameters":

                    if (targetComponents.Parameters.TryGetValue(uriPropertyValue, out var referencedParameter))
                    {
                        operation.Parameters.Add(referencedParameter);
                    }
                    else
                    {
                        this.logger.LogWarning("The {uriPropertyValue} Parameter was not found in the Components.Parameters", uriPropertyValue);
                    }

                    break;
                case "requestBodies":

                    if (targetComponents.RequestBodies.TryGetValue(uriPropertyValue, out var referencedRequestBody))
                    {
                        operation.RequestBody = referencedRequestBody;
                    }
                    else
                    {
                        this.logger.LogWarning("The {uriPropertyValue} RequestBody was not found in the Components.RequestBodies", uriPropertyValue);
                    }

                    break;
                case "callbacks":

                    if (targetComponents.Callbacks.TryGetValue(uriPropertyValue, out var referencedCallback))
                    {
                        operation.Callbacks.Add(referenceInfo.Key, referencedCallback);
                    }
                    else
                    {
                        this.logger.LogWarning("The {uriPropertyValue} Callback was not found in the Components.Callbacks", uriPropertyValue);
                    }

                    break;
                default:
                    this.logger.LogWarning("The {uriPropertyName} reference property is not a valid reference for the Operation type", uriPropertyName);
                    break;
            }
        }

        /// <summary>
        /// Resolves the <see cref="Reference"/>s that are contained in the <see cref="Parameter"/> object
        /// </summary>
        /// <param name="parameter">
        /// The <see cref="Parameter"/> for which the contained <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="targetComponents">
        /// The <see cref="Components"/> that contains the referenced items
        /// </param>
        /// <param name="uriPropertyName">
        /// The name of the property for which the <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="uriPropertyValue">
        /// The value of the property for which the <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="referenceInfo">
        /// The <see cref="ReferenceInfo"/> that contains information to resolve the <see cref="Reference"/>s
        /// </param>
        private void Resolve(Parameter parameter, Components targetComponents, string uriPropertyName, string uriPropertyValue, ReferenceInfo referenceInfo)
        {
            if (uriPropertyName == "examples")
            {
                if (targetComponents.Examples.TryGetValue(uriPropertyValue, out var referencedExample))
                {
                    parameter.Examples.Add(referenceInfo.Key, referencedExample);
                }
                else
                {
                    this.logger.LogWarning("The {uriPropertyValue} Example was not found in the Components.Examples", uriPropertyValue);
                }
            }
            else
            {
                this.logger.LogWarning("The {uriPropertyName} reference property is not a valid reference for the Parameter type", uriPropertyName);
            }
        }

        /// <summary>
        /// Resolves the <see cref="Reference"/>s that are contained in the <see cref="MediaType"/> object
        /// </summary>
        /// <param name="mediaType">
        /// The <see cref="MediaType"/> for which the contained <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="targetComponents">
        /// The <see cref="Components"/> that contains the referenced items
        /// </param>
        /// <param name="uriPropertyName">
        /// The name of the property for which the <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="uriPropertyValue">
        /// The value of the property for which the <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="referenceInfo">
        /// The <see cref="ReferenceInfo"/> that contains information to resolve the <see cref="Reference"/>s
        /// </param>
        private void Resolve(MediaType mediaType, Components targetComponents, string uriPropertyName, string uriPropertyValue, ReferenceInfo referenceInfo)
        {
            switch (uriPropertyName)
            {
                case "examples":
                    if (targetComponents.Examples.TryGetValue(uriPropertyValue, out var referencedExample))
                    {
                        mediaType.Examples.Add(referenceInfo.Key, referencedExample);
                    }
                    else
                    {
                        this.logger.LogWarning("The {uriPropertyValue} Example was not found in the Components.Examples", uriPropertyValue);
                    }
                    break;
                case "schemas":
                    if (targetComponents.Schemas.TryGetValue(uriPropertyValue, out var referencedSchema))
                    {
                        switch (referenceInfo.Field)
                        {
                            case "schema":
                                mediaType.Schema = referencedSchema;
                                break;
                            default:
                                this.logger.LogWarning("The {field} Field is not yet supported", referenceInfo.Field);
                                break;
                        }
                    }
                    else
                    {
                        this.logger.LogWarning("The {uriPropertyValue} Schema was not found in the Components.Schemas", uriPropertyValue);
                    }
                    break;
                default:
                    this.logger.LogWarning("The {uriPropertyName} reference property is not a valid reference for the MediaType type", uriPropertyName);
                    break;
            }
        }

        /// <summary>
        /// Resolves the <see cref="Reference"/>s that are contained in the <see cref="Encoding"/> object
        /// </summary>
        /// <param name="encoding">
        /// The <see cref="Encoding"/> for which the contained <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="targetComponents">
        /// The <see cref="Components"/> that contains the referenced items
        /// </param>
        /// <param name="uriPropertyName">
        /// The name of the property for which the <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="uriPropertyValue">
        /// The value of the property for which the <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="referenceInfo">
        /// The <see cref="ReferenceInfo"/> that contains information to resolve the <see cref="Reference"/>s
        /// </param>
        private void Resolve(Encoding encoding, Components targetComponents, string uriPropertyName, string uriPropertyValue, ReferenceInfo referenceInfo)
        {
            if (uriPropertyName == "headers")
            {
                if (targetComponents.Headers.TryGetValue(uriPropertyValue, out var referencedHeader))
                {
                    encoding.Headers.Add(referenceInfo.Key, referencedHeader);
                }
                else
                {
                    this.logger.LogWarning("The {uriPropertyValue} Header was not found in the Components.Headers", uriPropertyValue);
                }
            }
            else
            {
                this.logger.LogWarning("The {uriPropertyName} reference property is not a valid reference for the Encoding type", uriPropertyName);
            }
        }

        /// <summary>
        /// Resolves the <see cref="Reference"/>s that are contained in the <see cref="Responses"/> object
        /// </summary>
        /// <param name="responses">
        /// The <see cref="Responses"/> for which the contained <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="targetComponents">
        /// The <see cref="Components"/> that contains the referenced items
        /// </param>
        /// <param name="uriPropertyName">
        /// The name of the property for which the <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="uriPropertyValue">
        /// The value of the property for which the <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="referenceInfo">
        /// The <see cref="ReferenceInfo"/> that contains information to resolve the <see cref="Reference"/>s
        /// </param>
        private void Resolve(Responses responses, Components targetComponents, string uriPropertyName, string uriPropertyValue, ReferenceInfo referenceInfo)
        {
            if (uriPropertyName == "responses")
            {
                if (targetComponents.Responses.TryGetValue(uriPropertyValue, out var referencedResponse))
                {
                    responses.Response.Add(referenceInfo.Key, referencedResponse);
                }
                else
                {
                    this.logger.LogWarning("The {uriPropertyValue} Response was not found in the Components.Responses", uriPropertyValue);
                }
            }
            else
            {
                this.logger.LogWarning("The {uriPropertyName} reference property is not a valid reference for the Responses type", uriPropertyName);
            }
        }

        /// <summary>
        /// Resolves the <see cref="Reference"/>s that are contained in the <see cref="Response"/> object
        /// </summary>
        /// <param name="response">
        /// The <see cref="Response"/> for which the contained <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="targetComponents">
        /// The <see cref="Components"/> that contains the referenced items
        /// </param>
        /// <param name="uriPropertyName">
        /// The name of the property for which the <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="uriPropertyValue">
        /// The value of the property for which the <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="referenceInfo">
        /// The <see cref="ReferenceInfo"/> that contains information to resolve the <see cref="Reference"/>s
        /// </param>
        private void Resolve(Response response, Components targetComponents, string uriPropertyName, string uriPropertyValue, ReferenceInfo referenceInfo)
        {
            switch (uriPropertyName)
            {
                case "headers":
                    if (targetComponents.Headers.TryGetValue(uriPropertyValue, out var referencedHeader))
                    {
                        response.Headers.Add(referenceInfo.Key, referencedHeader);
                    }
                    else
                    {
                        this.logger.LogWarning("The {uriPropertyValue} Header was not found in the Components.Headers", uriPropertyValue);
                    }
                    break;
                case "links":
                    if (targetComponents.Links.TryGetValue(uriPropertyValue, out var referencedLink))
                    {
                        response.Links.Add(referenceInfo.Key, referencedLink);
                    }
                    else
                    {
                        this.logger.LogWarning("The {uriPropertyValue} Link was not found in the Components.Links", uriPropertyValue);
                    }
                    break;
                default:
                    this.logger.LogWarning("The {uriPropertyName} reference property is not a valid reference for the Response type", uriPropertyName);
                    break;
            }
        }

        /// <summary>
        /// Resolves the <see cref="Reference"/>s that are contained in the <see cref="Callback"/> object
        /// </summary>
        /// <param name="callback">
        /// The <see cref="Callback"/> for which the contained <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="targetComponents">
        /// The <see cref="Components"/> that contains the referenced items
        /// </param>
        /// <param name="uriPropertyName">
        /// The name of the property for which the <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="uriPropertyValue">
        /// The value of the property for which the <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="referenceInfo">
        /// The <see cref="ReferenceInfo"/> that contains information to resolve the <see cref="Reference"/>s
        /// </param>
        private void Resolve(Callback callback, Components targetComponents, string uriPropertyName, string uriPropertyValue, ReferenceInfo referenceInfo)
        {
            if (uriPropertyName == "pathItems")
            {
                if (targetComponents.PathItems.TryGetValue(uriPropertyValue, out var referencedPathItem))
                {
                    callback.PathItems.Add(referenceInfo.Key, referencedPathItem);
                }
                else
                {
                    this.logger.LogWarning("The {uriPropertyValue} PathItem was not found in the Components.PathItems", uriPropertyValue);
                }
            }
            else
            {
                this.logger.LogWarning("The {uriPropertyName} reference property is not a valid reference for the Callback type", uriPropertyName);
            }
        }

        /// <summary>
        /// Resolves the <see cref="Reference"/>s that are contained in the <see cref="Header"/> object
        /// </summary>
        /// <param name="header">
        /// The <see cref="Header"/> for which the contained <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="targetComponents">
        /// The <see cref="Components"/> that contains the referenced items
        /// </param>
        /// <param name="uriPropertyName">
        /// The name of the property for which the <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="uriPropertyValue">
        /// The value of the property for which the <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="referenceInfo">
        /// The <see cref="ReferenceInfo"/> that contains information to resolve the <see cref="Reference"/>s
        /// </param>
        private void Resolve(Header header, Components targetComponents, string uriPropertyName, string uriPropertyValue, ReferenceInfo referenceInfo)
        {
            if (uriPropertyName == "examples")
            {
                if (targetComponents.Examples.TryGetValue(uriPropertyValue, out var referencedExample))
                {
                    header.Examples.Add(referenceInfo.Key, referencedExample);
                }
                else
                {
                    this.logger.LogWarning("The {uriPropertyValue} Example was not found in the Components.Examples", uriPropertyValue);
                }
            }
            else
            {
                this.logger.LogWarning("The {uriPropertyName} reference property is not a valid reference for the Header type", uriPropertyName);
            }
        }

        /// <summary>
        /// Resolves the <see cref="Reference"/>s that are contained in the <see cref="Header"/> object
        /// </summary>
        /// <param name="jsonSchema">
        /// The <see cref="JsonSchema.JsonSchema"/> for which the contained <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="targetComponents">
        /// The <see cref="Components"/> that contains the referenced items
        /// </param>
        /// <param name="uriPropertyName">
        /// The name of the property for which the <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="uriPropertyValue">
        /// The value of the property for which the <see cref="Reference"/>s need to be resolved
        /// </param>
        /// <param name="referenceInfo">
        /// The <see cref="ReferenceInfo"/> that contains information to resolve the <see cref="Reference"/>s
        /// </param>
        private void Resolve(JsonSchema.JsonSchema jsonSchema, Components targetComponents, string uriPropertyName, string uriPropertyValue, ReferenceInfo referenceInfo)
        {
            if (uriPropertyName == "schemas")
            {
                if (targetComponents.Schemas.TryGetValue(uriPropertyValue, out var referencedSchema))
                {
                    switch (referenceInfo.Field)
                    {
                        case "items":
                            jsonSchema.Items = referencedSchema;
                            break;
                        case "properties":
                            jsonSchema.Properties.Add(referenceInfo.Key, referencedSchema);
                            break;
                        case "allOf":
                            jsonSchema.AllOf.Add(referencedSchema);
                            break;
                        case "anyOf":
                            jsonSchema.AnyOf.Add(referencedSchema);
                            break;
                        case "oneOf":
                            jsonSchema.OneOf.Add(referencedSchema);
                            break;
                        case "not":
                            jsonSchema.Not = referencedSchema;
                            break;
                        default:
                            this.logger.LogWarning("The {field} Field is not yet supported", referenceInfo.Field);
                            break;
                    }
                }
                else
                {
                    this.logger.LogWarning("The {uriPropertyValue} Schema was not found in the Components.Schemas", uriPropertyValue);
                }
            }
            else
            {
                this.logger.LogWarning("The {uriPropertyName} reference property is not a valid reference for the JsonSchema type", uriPropertyName);
            }
        }
    }
}
