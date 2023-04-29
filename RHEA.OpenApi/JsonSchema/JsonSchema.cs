// -------------------------------------------------------------------------------------------------
// <copyright file="JsonSchema.cs" company="RHEA System S.A.">
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
    using System.Collections.Generic;

    using OpenApi.Model;

    /// <summary>
    /// Definition of the <see cref="JsonSchema"/> interface as defined by the JSON Schema Specification Draft 2020-12
    /// </summary>
    /// <remarks>
    ///  https://datatracker.ietf.org/doc/html/draft-bhutton-json-schema-00#section-4.2.1
    /// </remarks>
    public class JsonSchema : OpenApi.Model.Schema
    {
        /// <summary>
        /// identifies a schema resource with its canonical[RFC6596] URI.
        /// </summary>
        /// <remarks
        /// https://datatracker.ietf.org/doc/html/draft-bhutton-json-schema-00#section-8.2.1
        /// </remarks>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the data type for a schema. The array may not contain duplicated types
        /// </summary>
        /// <remarks>
        /// https://datatracker.ietf.org/doc/html/draft-bhutton-json-schema-validation-00#section-6.1.1 
        /// </remarks>
        public List<JsonSchemaType> Type { get; set; } = new List<JsonSchemaType>();

        /// <summary>
        /// Gets or sets the <see cref="OpenApi.Model.Schema"/> that is used to validate the items
        /// in the array population
        /// </summary>
        public OpenApi.Model.Schema Items { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="FormatKind"/>, this is only valid in case the <see cref="Type"/>
        /// is equal to <see cref="JsonSchemaType.String"/>
        /// </summary>
        public FormatKind Format { get; set; } = FormatKind.Unknown;

        /// <summary>
        /// gets or sets a <see cref="Reference"/> that can be used to populate the <see cref="Items"/> property
        /// once the complete Open API document has been deserialized
        /// </summary>
        internal Reference ItemsReference { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="OpenApi.Model.Schema"/>, an instance must validate against
        /// all schema in this list
        /// </summary>
        public List<OpenApi.Model.Schema> AllOf { get; set; } = new List<OpenApi.Model.Schema>();

        /// <summary>
        /// gets or sets a dictionary of <see cref="Reference"/> that can be used to populate the <see cref="AllOf"/> List
        /// once the complete Open API document has been deserialized
        /// </summary>
        public List<Reference> AllOfReferences { get; set; } = new List<Reference>();

        /// <summary>
        /// Gets or sets a list of <see cref="OpenApi.Model.Schema"/>, an instance must validate against
        /// at least one schema in this list
        /// </summary>
        public List<OpenApi.Model.Schema> AnyOf { get; set; } = new List<OpenApi.Model.Schema>();

        /// <summary>
        /// gets or sets a dictionary of <see cref="Reference"/> that can be used to populate the <see cref="AnyOf"/> List
        /// once the complete Open API document has been deserialized
        /// </summary>
        public List<Reference> AnyOfReferences { get; set; } = new List<Reference>();

        /// <summary>
        /// Gets or sets a list of <see cref="OpenApi.Model.Schema"/>, an instance must validate against
        /// exatlye one schema in this list
        /// </summary>
        public List<OpenApi.Model.Schema> OneOf { get; set; } = new List<OpenApi.Model.Schema>();

        /// <summary>
        /// gets or sets a dictionary of <see cref="Reference"/> that can be used to populate the <see cref="OneOf"/> List
        /// once the complete Open API document has been deserialized
        /// </summary>
        public List<Reference> OneOfReferences { get; set; } = new List<Reference>();

        /// <summary>
        /// Gets or sets a  <see cref="OpenApi.Model.Schema"/>, an instance must not validate against
        /// this schema
        /// </summary>
        public OpenApi.Model.Schema Not { get; set; }

        /// <summary>
        /// gets or sets a dictionary of <see cref="Reference"/> that can be used to populate the <see cref="Not"/> Dictionary
        /// once the complete Open API document has been deserialized
        /// </summary>
        public Reference NotReference { get; set; }

        /// <summary>
        /// a list of named <see cref=ISchema"/> objects (in the form of a dictionary) where the
        /// name represents the name of the property and the <see cref="ISchema"/> the type of
        /// the property
        /// </summary>
        public Dictionary<string, OpenApi.Model.Schema> Properties { get; set; } = new Dictionary<string, Model.Schema>();

        /// <summary>
        /// gets or sets a dictionary of <see cref="Reference"/> that can be used to populate the <see cref="Properties"/> Dictionary
        /// once the complete Open API document has been deserialized
        /// </summary>
        internal Dictionary<string, Reference> PropertiesReferences { get; set; } = new Dictionary<string, Reference>();

        /// <summary>
        /// Gets or sets the list of required properties
        /// </summary>
        public List<string> Required { get; set; } = new List<string>();

        /// <summary>
        /// The enum is used to restrict a value to a fixed set of values. It must be an array with at least one element, where each element is unique.
        /// </summary>
        /// <remarks>
        /// https://datatracker.ietf.org/doc/html/draft-bhutton-json-schema-validation-00#section-6.1.2
        /// </remarks>
        public List<object> Enum { get; set; } = new List<object>();

        /// <summary>
        /// Gets or sets a value that is used to restrict a value to a single value.
        /// </summary>
        /// <remarks>
        /// https://datatracker.ietf.org/doc/html/draft-bhutton-json-schema-validation-00#section-6.1.3
        /// </remarks>
        public string Const { get; set; }

        /// <summary>
        /// Gets or sets the title that can be used to decorate a user interface with
        /// information about the data produced by this user interface.  A title
        /// will preferably be short.
        /// </summary>
        /// <remarks>
        /// https://datatracker.ietf.org/doc/html/draft-bhutton-json-schema-validation-00#section-9.1
        /// </remarks>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description that can be used to decorate a user interface with
        /// information about the data produced by this user interface. A description will provide
        /// explanation about the purpose of the instance described by this schema. 
        /// </summary>
        /// <remarks>
        /// https://datatracker.ietf.org/doc/html/draft-bhutton-json-schema-validation-00#section-9.1
        /// </remarks>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a string that is not to be interpreted by schema inplementations, they are useful 
        /// for leaving notes to future editors of a JSON schema, but should not be used to communicate
        /// to users of the schema.
        /// </summary>
        /// <remarks>
        /// https://datatracker.ietf.org/doc/html/draft-bhutton-json-schema-00#section-8.3
        /// </remarks>
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating  that
        /// applications SHOULD refrain from usage of the declared property.It
        /// MAY mean the property is going to be removed in the future.
        /// </summary>
        /// <remarks>
        /// https://datatracker.ietf.org/doc/html/draft-bhutton-json-schema-validation-00#section-9.3
        /// </remarks>
        public bool Deprecated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the
        /// value of the instance is managed exclusively by the owning authority,
        ///and attempts by an application to modify the value of this property
        /// are expected to be ignored or rejected by that owning authority.
        /// </summary>
        /// <remarks>
        /// https://datatracker.ietf.org/doc/html/draft-bhutton-json-schema-validation-00#section-9.4
        /// </remarks>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the value is never present when the instance is retrieved from the owning
        /// authority.It can be present when sent to the owning authority to
        /// update or create the document(or the resource it represents), but it
        /// will not be included in any updated or newly created version of the instance.
        /// </summary>
        /// <remarks>
        /// https://datatracker.ietf.org/doc/html/draft-bhutton-json-schema-validation-00#section-9.4
        /// </remarks>
        public bool WriteOnly { get; set; }

        /// <summary>
        /// Gets or sets a value, that in case the instance value is a string, that the
        /// string SHOULD be interpreted as binary data and decoded using the
        /// encoding named by this property.
        /// </summary>
        /// <remarks>
        /// https://datatracker.ietf.org/doc/html/draft-bhutton-json-schema-validation-00#section-8.3
        /// </remarks>
        public string ContentEncoding { get; set; }

        /// <summary>
        /// Gets or sets a value, that in case the instance is a string, that indicates the media type
        /// of the contents of the string.  If "contentEncoding" is present, this
        /// property describes the decoded string.
        /// </summary>
        /// <remarks>
        /// https://datatracker.ietf.org/doc/html/draft-bhutton-json-schema-validation-00#section-8.4
        /// </remarks>
        public string ContentMediaType { get; set; }

        /// <summary>
        /// Gets or sets a value, that in case the instance is a string, and if <see cref="ContentMediaType"/>
        /// is present, this property contains a schema which describes the structure of the string.
        /// </summary>
        /// <remarks>
        /// https://datatracker.ietf.org/doc/html/draft-bhutton-json-schema-validation-00#section-8.5
        /// </remarks>
        public OpenApi.Model.Schema ContentSchema { get; set; }
    }
}
