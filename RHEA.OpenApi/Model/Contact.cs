// -------------------------------------------------------------------------------------------------
// <copyright file="Contact.cs" company="RHEA System S.A.">
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
    /// Contact information for the exposed API.
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#contact-object
    /// </remarks>
    public class Contact
    {
        /// <summary>
        /// The identifying name of the contact person/organization.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The URL pointing to the contact information. This MUST be in the form of a URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The email address of the contact person/organization. This MUST be in the form of an email address.
        /// </summary>
        public string Email { get; set; }
    }
}
