// -------------------------------------------------------------------------------------------------
// <copyright file="JsonSchemaType.cs" company="RHEA System S.A.">
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
    /// Specification of the data type for a Json Schema
    /// </summary>
    public enum JsonSchemaType
    {
        String,

        Number,

        Integer,

        Object,

        Array,

        Boolean,

        Null,

        /// <summary>
        /// value that is retunred when the schema contains a type that is unknown
        /// </summary>
        Unknown
    }
}

