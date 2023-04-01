// -------------------------------------------------------------------------------------------------
// <copyright file="Responses.cs" company="RHEA System S.A.">
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
    /// A container for the expected responses of an operation. The container maps a HTTP response code to the expected response.
    /// The documentation is not necessarily expected to cover all possible HTTP response codes because they may not be known in advance.However,
    /// documentation is expected to cover a successful operation response and any known errors.
    /// The default MAY be used as a default response object for all HTTP codes that are not covered individually by the Responses Object.
    /// The Responses Object MUST contain at least one response code, and if only one response code is provided it SHOULD be the response
    /// for a successful operation call.
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#responses-object
    /// </remarks>
    public class Responses : Dictionary<string, Response>
    {
        /// <summary>
        /// The documentation of responses other than the ones declared for specific HTTP response codes. Use this field to cover undeclared responses.
        /// </summary>
        public Response Default { get; set; }
    }
}
