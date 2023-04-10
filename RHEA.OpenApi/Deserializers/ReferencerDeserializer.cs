// -------------------------------------------------------------------------------------------------
// <copyright file="ReferencerDeserializer.cs" company="RHEA System S.A.">
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

namespace OpenApi.Deserializers
{
    using System;
    using OpenApi.Model;

    /// <summary>
    /// abstract Deserializer from which deserializers shall derive that deserialize objects that may
    /// contain any <see cref="Reference"/> objects
    /// </summary>
    internal abstract class ReferencerDeserializer
    {
        /// <summary>
        /// The <see cref="ReferenceResolver"/> that is used to register any <see cref="ReferenceInfo"/> objects
        /// and later resolve them
        /// </summary>
        protected readonly ReferenceResolver referenceResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactDeSerializer"/> class.
        /// </summary>
        /// <param name="referenceResolver">
        /// The <see cref="ReferenceResolver"/> that is used to register any <see cref="ReferenceInfo"/> objects
        /// and later resolve them
        /// </param>
        protected ReferencerDeserializer(ReferenceResolver referenceResolver)
        {
            this.referenceResolver = referenceResolver;
        }

        /// <summary>
        /// Register the <see cref="Reference"/> with the <see cref="referenceResolver"/>
        /// </summary>
        /// <param name="reference">
        /// The <see cref="Reference"/> that is to be registered
        /// </param>
        /// <param name="source">
        /// The source object from which the <see cref="Reference"/> is made
        /// </param>
        /// <param name="field">
        /// The field name (property name) of the <paramref name="source"/> property that
        /// the reference represents
        /// </param>
        /// <param name="key">
        /// optional key value in case the registration is for a key-value pair
        /// </param>
        internal void Register(Reference reference, object source, string field, string key = "")
        {
            var referenceInfo = new ReferenceInfo
            {
                Source = source,
                Field = field,
                Key = key,
                Target = null,
                Reference = reference
            };

            this.referenceResolver.Register(referenceInfo);
        }
    }
}
