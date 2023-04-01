// -------------------------------------------------------------------------------------------------
// <copyright file="OAuthFlow.cs" company="RHEA System S.A.">
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
    /// Configuration details for a supported OAuth Flow
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#oauth-flow-object
    /// </remarks>
    public class OAuthFlow
    {
        /// <summary>
        /// REQUIRED. The authorization URL to be used for this flow. This MUST be in the form of a URL. The OAuth2 standard requires the use of TLS.
        /// </summary>
        public string AuthorizationUrl { get; set; }

        /// <summary>
        /// REQUIRED. The token URL to be used for this flow. This MUST be in the form of a URL. The OAuth2 standard requires the use of TLS.
        /// </summary>
        public string TokenUrl { get; set; }

        /// <summary>
        /// The URL to be used for obtaining refresh tokens. This MUST be in the form of a URL. The OAuth2 standard requires the use of TLS.
        /// </summary>
        public string RefreshUrl { get; set; }

        /// <summary>
        /// REQUIRED. The available scopes for the OAuth2 security scheme. A map between the scope name and a short description for it. The map MAY be empty.
        /// </summary>
        public Dictionary<string, string> Scopes { get; set; }
    }
}
