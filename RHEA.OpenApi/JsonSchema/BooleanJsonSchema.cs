// -------------------------------------------------------------------------------------------------
// <copyright file="BooleanJsonSchema.cs" company="RHEA System S.A.">
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

namespace OpenApi.JsonSchema
{
    /// <summary>
    /// The boolean schema values "true" and "false" are trivial schemas that
    /// always produce themselves as assertion results, regardless of the
    /// instance value.
    /// </summary>
    public class BooleanJsonSchema : OpenApi.Model.Schema
    {
        /// <summary>
        /// Initializes a new instannce of the <see cref="BooleanJsonSchema"/>
        /// </summary>
        /// <param name="value">
        /// the boolean value
        /// </param>
        public BooleanJsonSchema(bool value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the value of the <see cref="BooleanJsonSchema"/>
        /// </summary>
        public bool Value { get; set; }
    }
}

