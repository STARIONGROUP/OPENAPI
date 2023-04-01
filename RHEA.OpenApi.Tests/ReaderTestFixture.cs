// -------------------------------------------------------------------------------------------------
// <copyright file="ReaderTestFixture.cs" company="RHEA System S.A.">
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
    using System.Collections.Generic;
    
    using System.IO;
    using System.Linq;
    
    using Model;

    using NUnit.Framework;

    [TestFixture]
    public class ReaderTestFixture
    {
        private Reader reader;

        [SetUp]
        public void Setup()
        {
            this.reader = new Reader();
        }

        [Test]
        public void Verify_that_an_openapi_doc_can_be_deserialized()
        {
            var fileName = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Data", "sysml2.openapi.json");

            var document = this.reader.Read(fileName);

            Assert.That(document.Info.Title, Is.EqualTo("Systems Modeling API and Services"));
            Assert.That(document.Info.Description, Is.EqualTo("REST/HTTP platform specific model (PSM) for the Systems Modeling API and Services"));
            Assert.That(document.Info.Version, Is.EqualTo("1.0"));
            
            Assert.That(document.Tags.Length, Is.EqualTo(9));
            Assert.That(document.Tags.First().Name, Is.EqualTo("Project"));

            Assert.That(document.Paths.Count, Is.EqualTo(23));


            Assert.That(document.Paths.TryGetValue("/projects", out var allPprojectsPath), Is.True);

            Assert.That(allPprojectsPath.Get.Tags, Is.EqualTo( new string[]{"Project"} ));
            Assert.That(allPprojectsPath.Get.Summary, Is.EqualTo("Get projects"));
            Assert.That(allPprojectsPath.Get.OperationId, Is.EqualTo("getProjects"));

            var getPAgeAfterparameter = allPprojectsPath.Get.Parameters.First();
            
            var getProjectResponse = allPprojectsPath.Get.Responses.First();

            Assert.That(getProjectResponse.Key, Is.EqualTo("200"));

            Assert.That(getProjectResponse.Value.Description, Is.EqualTo("OK"));
            

        }
    }
}