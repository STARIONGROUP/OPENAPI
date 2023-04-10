// -------------------------------------------------------------------------------------------------
// <copyright file="ServerVariable.cs" company="RHEA System S.A.">
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
    /// An object representing a Server Variable for server URL template substitution.
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#server-variable-object
    /// </remarks> 
    public class ServerVariable
    {
        /// <summary>
        /// An enumeration of string values to be used if the substitution options are from a limited set. The array MUST NOT be empty.
        /// </summary>
        public List<string> Enum { get; set; } = new List<string>();

        /// <summary>
        /// REQUIRED. The default value to use for substitution, which SHALL be sent if an alternate value is not supplied.
        /// Note this behavior is different than the Schema Object’s treatment of default values, because in those cases parameter values are optional.
        /// If the enum is defined, the value MUST exist in the enum’s values.
        /// </summary>
        public string Default { get; set; }

        /// <summary>
        /// An optional description for the server variable. CommonMark syntax MAY be used for rich text representation.
        /// </summary>
        public string Description { get; set; }
    }
}
