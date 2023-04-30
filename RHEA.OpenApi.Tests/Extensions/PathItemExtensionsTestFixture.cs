// -------------------------------------------------------------------------------------------------
// <copyright file="PathItemExtensionsTestFixture.cs" company="RHEA System S.A.">
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

namespace OpenApi.Tests.Extensions
{
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using NUnit.Framework;

    using OpenApi.Model;
    using OpenApi.Extensinos;

    [TestFixture]
    public class PathItemExtensionsTestFixture
    {
        private PathItem pathItem;

        [SetUp]
        public void SetUp()
        {
            this.pathItem = new PathItem();
        }

        [Test]
        public void Verify_That_Query_Operations_returns_All_Operations_When_all_are_set()
        {
            this.pathItem.Get = new Operation { OperationId = "GET" };
            this.pathItem.Put = new Operation { OperationId = "PUT" };
            this.pathItem.Post = new Operation { OperationId = "POST" };
            this.pathItem.Delete = new Operation { OperationId = "DELETE" };
            this.pathItem.Options = new Operation { OperationId = "OPTIONS" };
            this.pathItem.Head = new Operation { OperationId = "HEAD" };
            this.pathItem.Patch = new Operation { OperationId = "PATCH" };
            this.pathItem.Trace = new Operation { OperationId = "TRACE" };

            Assert.That (PathItemExtensions.QueryOperations(pathItem).Count(), Is.EqualTo(8));

            CollectionAssert.AllItemsAreUnique(PathItemExtensions.QueryOperations(pathItem));
        }

        [Test]
        public void Verify_That_Query_Operations_returns_All_Operations_When_one_is_set()
        {
            this.pathItem.Get = new Operation { OperationId = "GET" };

            Assert.That(PathItemExtensions.QueryOperations(pathItem).Count(), Is.EqualTo(1));
        }
    }
}
