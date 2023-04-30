// -------------------------------------------------------------------------------------------------
// <copyright file="PathItemExtensions.cs" company="RHEA System S.A.">
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

namespace OpenApi.Extensinos
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using OpenApi.Model;

    /// <summary>
    /// Extension methods for the <see cref="PathItem"/> class
    /// </summary>
    public static class PathItemExtensions
    {
        /// <summary>
        /// Queries the <see cref="Operation"/> objects from the <paramref name="pathItem"/>
        /// that are not null
        /// </summary>
        /// <param name="pathItem">
        /// The subject <see cref="PathItem"/> that is to be queried
        /// </param>
        /// <returns>
        /// An <see cref="IEnumerable{Operation}"/>
        /// </returns>
        public static IEnumerable<Operation> QueryOperations(this PathItem pathItem)
        {
            if (pathItem.Get != null)
            {
                yield return pathItem.Get;
            }

            if (pathItem.Put != null)
            {
                yield return pathItem.Put;
            }

            if (pathItem.Post != null)
            {
                yield return pathItem.Post;
            }

            if (pathItem.Delete != null)
            {
                yield return pathItem.Delete;
            }

            if (pathItem.Options != null)
            {
                yield return pathItem.Options;
            }

            if (pathItem.Head != null)
            {
                yield return pathItem.Head;
            }

            if (pathItem.Patch != null)
            {
                yield return pathItem.Patch;
            }

            if (pathItem.Trace != null)
            {
                yield return pathItem.Trace;
            }
        }
    }
}
