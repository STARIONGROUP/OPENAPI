// -------------------------------------------------------------------------------------------------
// <copyright file="DeSerializerTestFixture.cs" company="RHEA System S.A.">
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

namespace OpenApi.Tests
{
    using NUnit.Framework;
    using System.IO;

    [TestFixture]
    public class DeSerializerTestFixture
    {
        private DeSerializer deSerializer;

        [SetUp]
        public void SetUp()
        {
            this.deSerializer = new DeSerializer();
        }

        [Test]
        public void Verify_that_the_SysML2_openapi_spec_can_be_read()
        {
            var fileName = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Data", "sysml2.openapi.json");

            using var fs = File.OpenRead(fileName);
            var document = this.deSerializer.DeSerialize(fs);

            Assert.That(document, Is.Not.Null);
        }

        [Test]
        public void Verify_that_the_petstore_1_openapi_spec_can_be_read()
        {
            var fileName = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Data", "petstore_1.openapi.json");

            using var fs = File.OpenRead(fileName);
            var document = this.deSerializer.DeSerialize(fs);

            Assert.That(document, Is.Not.Null);
        }

        [Test]
        public void Verify_that_the_petstore_2_openapi_spec_can_be_read()
        {
            var fileName = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Data", "petstore_2.openapi.json");

            using var fs = File.OpenRead(fileName);
            var document = this.deSerializer.DeSerialize(fs, false);

            Assert.That(document, Is.Not.Null);
        }
    }
}
