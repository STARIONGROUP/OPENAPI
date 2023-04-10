// -------------------------------------------------------------------------------------------------
// <copyright file="ReferenceInfo.cs" company="RHEA System S.A.">
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
    using OpenApi.Model;

    /// <summary>
    /// The purpose of the <see cref="ReferenceInfo"/> is to capture information regarding a
    /// <see cref="Model.Reference"/> required to resolve such <see cref="Model.Reference"/>
    /// </summary>
    public struct ReferenceInfo
    {
        /// <summary>
        /// Gets or sets the source object from which the <see cref="Model.Reference"/> is made
        /// </summary>
        public object Source { get; set; }

        /// <summary>
        /// Gets or sets the field name (property name) of the <see cref="Source"/> property that
        /// the reference represents
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the key of the key-value pair in case the referenced item will be added to a
        /// dictionary in the <see cref="Source"/> object
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the target object that the <see cref="Model.Reference"/> is pointing to
        /// </summary>
        public object Target { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Model.Reference"/> that this <see cref="ReferenceInfo"/>
        /// represents
        /// </summary>
        public Reference Reference { get; set; }
    }
}
