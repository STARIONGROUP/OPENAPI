// -------------------------------------------------------------------------------------------------
// <copyright file="OAuthFlows.cs" company="RHEA System S.A.">
// 
//   Copyright 2022-2023 RHEA System S.A.
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
    /// <summary>
    /// Allows configuration of the supported OAuth Flows.
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#oauth-flows-object
    /// </remarks>
    public class OAuthFlows
    {
        /// <summary>
        /// Configuration for the OAuth Implicit flow
        /// </summary>
        public OAuthFlow Implicit { get; set; }

        /// <summary>
        /// Configuration for the OAuth Resource Owner Password flow
        /// </summary>
        public OAuthFlow Password { get; set; }

        /// <summary>
        /// Configuration for the OAuth Client Credentials flow. Previously called application in OpenAPI 2.0.
        /// </summary>
        public OAuthFlow ClientCredentials { get; set; }

        /// <summary>
        /// Configuration for the OAuth Authorization Code flow. Previously called accessCode in OpenAPI 2.0.
        /// </summary>
        public OAuthFlow AuthorizationCode { get; set; }
    }
}
