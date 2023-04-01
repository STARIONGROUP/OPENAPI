// -------------------------------------------------------------------------------------------------
// <copyright file="SecurityRequirement.cs" company="RHEA System S.A.">
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

namespace OpenApi.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Lists the required security schemes to execute this operation. The name used for each property MUST correspond to a security scheme declared
    /// in the Security Schemes under the Components Object.
    /// Security Requirement Objects that contain multiple schemes require that all schemes MUST be satisfied for a request to be authorized.
    /// This enables support for scenarios where multiple query parameters or HTTP headers are required to convey security information.
    /// When a list of Security Requirement Objects is defined on the OpenAPI Object or Operation Object,
    /// only one of the Security Requirement Objects in the list needs to be satisfied to authorize the request.
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#security-requirement-object
    /// </remarks>
    public class SecurityRequirement : Dictionary<string, string[]>
    {
    }
}
