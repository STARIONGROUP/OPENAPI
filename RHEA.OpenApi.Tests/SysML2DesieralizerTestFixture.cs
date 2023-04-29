// -------------------------------------------------------------------------------------------------
// <copyright file="SysML2DesieralizerTestFixture.cs" company="RHEA System S.A.">
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
    using System.IO;
    using System.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class SysML2DesieralizerTestFixture
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

            Assert.That(document.OpenApi, Is.EqualTo("3.1.0"));

            // Info object
            Assert.That(document.Info.Title, Is.EqualTo("Systems Modeling API and Services"));
            Assert.That(document.Info.Description, Is.EqualTo("REST/HTTP platform specific model (PSM) for the Systems Modeling API and Services"));
            Assert.That(document.Info.Version, Is.EqualTo("1.0"));

            // Tags
            Assert.That(document.Tags.Count, Is.EqualTo(9));
            var projectTag = document.Tags.Single(x => x.Name == "Project");

            // Paths
            Assert.That(document.Paths.Count, Is.EqualTo(23));

            Assert.That(document.Paths.TryGetValue("/projects", out var projectsPathItem), Is.True);
            Assert.That(projectsPathItem.Head, Is.Null);
            Assert.That(projectsPathItem.Options, Is.Null);
            Assert.That(projectsPathItem.Put, Is.Null);
            Assert.That(projectsPathItem.Patch, Is.Null);
            Assert.That(projectsPathItem.Trace, Is.Null);

            Assert.That(projectsPathItem.Get.Tags, Is.EquivalentTo(new[] { "Project" } ));
            Assert.That(projectsPathItem.Get.Summary, Is.EquivalentTo("Get projects"));
            Assert.That(projectsPathItem.Get.OperationId, Is.EquivalentTo("getProjects"));

            // Path GET Parameters
            Assert.That(projectsPathItem.Get.Parameters.Count, Is.EqualTo(3));

            var pageAfterParameter = projectsPathItem.Get.Parameters.Single(x => x.Name == "page[after]");
            Assert.That(pageAfterParameter.In, Is.EqualTo("query"));
            Assert.That(pageAfterParameter.Description, Is.EqualTo("Page after"));
            Assert.That(((JsonSchema.JsonSchema)pageAfterParameter.Schema).Type.Single(), Is.EqualTo(JsonSchema.JsonSchemaType.String));

            var pageSizeParameter = projectsPathItem.Get.Parameters.Single(x => x.Name == "page[size]");
            Assert.That(pageSizeParameter.In, Is.EqualTo("query"));
            Assert.That(pageSizeParameter.Description, Is.EqualTo("Page size"));
            Assert.That(((JsonSchema.JsonSchema)pageSizeParameter.Schema).Type.Single(), Is.EqualTo(JsonSchema.JsonSchemaType.Integer));

            // Path GET Responses
            Assert.That(projectsPathItem.Get.Responses.Response.Count, Is.EqualTo(2));

            Assert.That(projectsPathItem.Get.Responses.Response.TryGetValue("200", out var getProjectsResponseSuccess), Is.True);
            Assert.That(getProjectsResponseSuccess.Description, Is.EqualTo("OK"));

            Assert.That(getProjectsResponseSuccess.Content.TryGetValue("application/json", out var getProjectsResponseSuccessContent), Is.True);
            Assert.That(((JsonSchema.JsonSchema)getProjectsResponseSuccessContent.Schema).Type.Single(), Is.EqualTo(JsonSchema.JsonSchemaType.Array));
            Assert.That(((JsonSchema.JsonSchema)getProjectsResponseSuccessContent.Schema).Items, Is.Not.Null);

            var projectSchema = (JsonSchema.JsonSchema)((JsonSchema.JsonSchema)getProjectsResponseSuccessContent.Schema).Items;

            Assert.That(projectSchema.Identifier, Is.EqualTo("https://www.omg.org/spec/SystemsModelingAPI/20230201/Project"));
            Assert.That(projectSchema.Title, Is.EqualTo("Project"));
            Assert.That(projectSchema.Type.Single(), Is.EqualTo(JsonSchema.JsonSchemaType.Object));
            Assert.That(projectSchema.Required, Is.EquivalentTo(new[] { "@id", "@type", "created", "defaultBranch", "description", "name"}));

            Assert.That(projectSchema.Properties.TryGetValue("@id", out var idProperty), Is.True);
            var idPropertySchema = (JsonSchema.JsonSchema)idProperty;
            Assert.That(idPropertySchema.Type.Single, Is.EqualTo(JsonSchema.JsonSchemaType.String));

            Assert.That(projectSchema.Properties.TryGetValue("@type", out var typeProperty), Is.True);
            var typePropertySchema = (JsonSchema.JsonSchema)typeProperty;
            Assert.That(typePropertySchema.Type.Single, Is.EqualTo(JsonSchema.JsonSchemaType.String));
            Assert.That(typePropertySchema.Const, Is.EqualTo("Project"));

            Assert.That(projectSchema.Properties.TryGetValue("defaultBranch", out var defaultBranchProperty), Is.True);
            var defaultBranchPropertySchema = (JsonSchema.JsonSchema)defaultBranchProperty;
            //TODO: Assert.That(defaultBranchPropertySchema.Type.Single, Is.EqualTo(JsonSchema.JsonSchemaType.Object));

            Assert.That(defaultBranchPropertySchema.Comments, Is.EqualTo("https://www.omg.org/spec/SystemsModelingAPI/20230201/Branch"));

            Assert.That(projectSchema.Properties.TryGetValue("description", out var descriptionProperty), Is.True);
            var descriptionPropertySchema = (JsonSchema.JsonSchema)descriptionProperty;
            Assert.That(descriptionPropertySchema.OneOf.Count, Is.EqualTo(2));

            // Path POST Parameters
            Assert.That(projectsPathItem.Post.Parameters.Count, Is.EqualTo(0));

            Assert.That(projectsPathItem.Post.RequestBody.Content.TryGetValue("application/json", out var projectsPathItemPostMediaType), Is.True);

            var projectsPathItemPostMediaTypeJsonSchema = (JsonSchema.JsonSchema)projectsPathItemPostMediaType.Schema;
            Assert.That(projectsPathItemPostMediaTypeJsonSchema.Identifier, Is.EqualTo("https://www.omg.org/spec/SystemsModelingAPI/20230201/ProjectRequest"));
            Assert.That(projectsPathItemPostMediaTypeJsonSchema.Title, Is.EqualTo("ProjectRequest"));
            Assert.That(projectsPathItemPostMediaTypeJsonSchema.Type.Single(), Is.EqualTo(JsonSchema.JsonSchemaType.Object ));
        }
    }
}
