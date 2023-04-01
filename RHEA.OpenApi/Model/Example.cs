// -------------------------------------------------------------------------------------------------
// <copyright file="Example.cs" company="RHEA System S.A.">
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
    /// <summary>
    /// NA
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#example-object
    /// </remarks>
    public class Example
    {
        /// <summary>
        /// Short description for the example.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Long description for the example. CommonMark syntax MAY be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Embedded literal example. The value field and externalValue field are mutually exclusive. To represent examples of media types
        /// that cannot naturally represented in JSON or YAML, use a string value to contain the example, escaping where necessary.
        /// </summary>
        public object Any { get; set; }

        /// <summary>
        /// A URI that points to the literal example. This provides the capability to reference examples that cannot easily be included in JSON or YAML documents.
        /// The value field and externalValue field are mutually exclusive. See the rules for resolving Relative References.
        /// </summary>
        public string ExternalValue { get; set; }
    }
}