// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
namespace EntityCodeGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Common;
    using System.Data.Entity.Design;
    using System.Data.Entity.Design.PluralizationServices;
    using System.Data.Metadata.Edm;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using Microsoft.DbContextPackage.Utilities;
    internal class EngineerCodeFirstHandler
    {
        private static readonly IEnumerable<EntityStoreSchemaFilterEntry> _storeMetadataFilters = new[]
            {
                new EntityStoreSchemaFilterEntry(null, null, "EdmMetadata", EntityStoreSchemaFilterObjectTypes.Table, EntityStoreSchemaFilterEffect.Exclude),
                new EntityStoreSchemaFilterEntry(null, null, "__MigrationHistory", EntityStoreSchemaFilterObjectTypes.Table, EntityStoreSchemaFilterEffect.Exclude)
            };
   

        public void ReverseEngineerCodeFirst()
        {


            try
            {
                // Find connection string and provider
                string connectionString = "data source=172.21.4.31,10086;initial catalog=Eme4;user id=eme;password=io77@68;MultipleActiveResultSets=True;App=EntityFramework&quot;";
                var providerInvariant = "System.Data.SqlClient";
                string projectNamespace ="TEST";
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();

                DbConnection connection = new SqlConnection(connectionString);

                // Load store schema
                var storeGenerator = new EntityStoreSchemaGenerator(providerInvariant, connectionString, "dbo");
                storeGenerator.GenerateForeignKeyProperties = true;
                var errors = storeGenerator.GenerateStoreMetadata(_storeMetadataFilters).Where(e => e.Severity == EdmSchemaErrorSeverity.Error);
               

                // Generate default mapping
              
                var contextName = connection.Database.Replace(" ", string.Empty).Replace(".", string.Empty) + "Context";
                var modelGenerator = new EntityModelSchemaGenerator(storeGenerator.EntityContainer, "DefaultNamespace", contextName);
                modelGenerator.PluralizationService = PluralizationService.CreateService(new CultureInfo("en"));
                modelGenerator.GenerateForeignKeyProperties = true;
                modelGenerator.GenerateMetadata();

                // Pull out info about types to be generated
                var entityTypes = modelGenerator.EdmItemCollection.OfType<EntityType>().ToArray();
                var mappings = new EdmMapping(modelGenerator, storeGenerator.StoreItemCollection);
               

                // Generate Entity Classes and Mappings
                var templateProcessor = new TemplateProcessor();
                var modelsNamespace = projectNamespace + ".Models";
                var modelsDirectory = Path.Combine(currentDirectory, "Models");
                var mappingNamespace = modelsNamespace + ".Mapping";
                var mappingDirectory = Path.Combine(modelsDirectory, "Mapping");
                var entityFrameworkVersion = GetEntityFrameworkVersion();

                foreach (var entityType in entityTypes)
                {
                    // Generate the code file
                    var entityHost = new EfTextTemplateHost
                    {
                        EntityType = entityType,
                        EntityContainer = modelGenerator.EntityContainer,
                        Namespace = modelsNamespace,
                        ModelsNamespace = modelsNamespace,
                        MappingNamespace = mappingNamespace,
                        EntityFrameworkVersion = entityFrameworkVersion,
                        TableSet = mappings.EntityMappings[entityType].Item1,
                        PropertyToColumnMappings = mappings.EntityMappings[entityType].Item2,
                        ManyToManyMappings = mappings.ManyToManyMappings
                    };
                    var entityContents = templateProcessor.Process(Templates.EntityTemplate, entityHost);

                    var filePath = Path.Combine(modelsDirectory, entityType.Name + entityHost.FileExtension);
                    FileGenerator.AddNewFile(filePath, entityContents);

                    var mappingHost = new EfTextTemplateHost
                    {
                        EntityType = entityType,
                        EntityContainer = modelGenerator.EntityContainer,
                        Namespace = mappingNamespace,
                        ModelsNamespace = modelsNamespace,
                        MappingNamespace = mappingNamespace,
                        EntityFrameworkVersion = entityFrameworkVersion,
                        TableSet = mappings.EntityMappings[entityType].Item1,
                        PropertyToColumnMappings = mappings.EntityMappings[entityType].Item2,
                        ManyToManyMappings = mappings.ManyToManyMappings
                    };
                    var mappingContents = templateProcessor.Process(Templates.MappingTemplate, mappingHost);

                    var mappingFilePath = Path.Combine(mappingDirectory, entityType.Name + "Map" + mappingHost.FileExtension);
                    FileGenerator.AddNewFile(filePath, entityContents);
                }

                // Generate Context

                var contextHost = new EfTextTemplateHost
                {
                    EntityContainer = modelGenerator.EntityContainer,
                    Namespace = modelsNamespace,
                    ModelsNamespace = modelsNamespace,
                    MappingNamespace = mappingNamespace,
                    EntityFrameworkVersion = entityFrameworkVersion
                };
                var contextContents = templateProcessor.Process(Templates.ContextTemplate, contextHost);

                var contextFilePath = Path.Combine(modelsDirectory, modelGenerator.EntityContainer.Name + contextHost.FileExtension);
                FileGenerator.AddNewFile(contextFilePath, contextContents);

              
            }
            catch (Exception exception)
            {
                
            }
        }


        public void CodeGenerator(string connectionString, string projectNamespace,Action<string> action)
        {


            try
            {
                // Find connection string and provider
                var providerInvariant = "System.Data.SqlClient";
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();

                using (DbConnection connection = new SqlConnection(connectionString))
                {

                    // Load store schema
                    var storeGenerator = new EntityStoreSchemaGenerator(providerInvariant, connectionString, "dbo");
                    storeGenerator.GenerateForeignKeyProperties = true;
                    var errors = storeGenerator.GenerateStoreMetadata(_storeMetadataFilters).Where(e => e.Severity == EdmSchemaErrorSeverity.Error);


                    // Generate default mapping

                    var contextName = connection.Database.Replace(" ", string.Empty).Replace(".", string.Empty) + "Context";
                    var modelGenerator = new EntityModelSchemaGenerator(storeGenerator.EntityContainer, "DefaultNamespace", contextName);
                    modelGenerator.PluralizationService = PluralizationService.CreateService(new CultureInfo("en"));
                    modelGenerator.GenerateForeignKeyProperties = true;
                    modelGenerator.GenerateMetadata();

                    // Pull out info about types to be generated
                    var entityTypes = modelGenerator.EdmItemCollection.OfType<EntityType>().ToArray();
                    var mappings = new EdmMapping(modelGenerator, storeGenerator.StoreItemCollection);


                    // Generate Entity Classes and Mappings
                    var templateProcessor = new TemplateProcessor();
                    var modelsNamespace = projectNamespace + ".Models";
                    var modelsDirectory = Path.Combine(currentDirectory, "Models");
                    var mappingNamespace = modelsNamespace + ".Mapping";
                    var mappingDirectory = Path.Combine(modelsDirectory, "Mapping");
                    var entityFrameworkVersion = GetEntityFrameworkVersion();

                    foreach (var entityType in entityTypes)
                    {
                        // Generate the code file
                        var entityHost = new EfTextTemplateHost
                        {
                            EntityType = entityType,
                            EntityContainer = modelGenerator.EntityContainer,
                            Namespace = modelsNamespace,
                            ModelsNamespace = modelsNamespace,
                            MappingNamespace = mappingNamespace,
                            EntityFrameworkVersion = entityFrameworkVersion,
                            TableSet = mappings.EntityMappings[entityType].Item1,
                            PropertyToColumnMappings = mappings.EntityMappings[entityType].Item2,
                            ManyToManyMappings = mappings.ManyToManyMappings
                        };
                        var entityContents = templateProcessor.Process(Templates.EntityTemplate, entityHost);

                        var filePath = Path.Combine(modelsDirectory, entityType.Name + entityHost.FileExtension);
                        FileGenerator.AddNewFile(filePath, entityContents);

                        var mappingHost = new EfTextTemplateHost
                      {
                          EntityType = entityType,
                          EntityContainer = modelGenerator.EntityContainer,
                          Namespace = mappingNamespace,
                          ModelsNamespace = modelsNamespace,
                          MappingNamespace = mappingNamespace,
                          EntityFrameworkVersion = entityFrameworkVersion,
                          TableSet = mappings.EntityMappings[entityType].Item1,
                          PropertyToColumnMappings = mappings.EntityMappings[entityType].Item2,
                          ManyToManyMappings = mappings.ManyToManyMappings
                      };
                        var mappingContents = templateProcessor.Process(Templates.MappingTemplate, mappingHost);

                        var mappingFilePath = Path.Combine(mappingDirectory, entityType.Name + "Map" + mappingHost.FileExtension);
                        FileGenerator.AddNewFile(mappingFilePath, mappingContents);
                    }

                    // Generate Context

                    var contextHost = new EfTextTemplateHost
                    {
                        EntityContainer = modelGenerator.EntityContainer,
                        Namespace = modelsNamespace,
                        ModelsNamespace = modelsNamespace,
                        MappingNamespace = mappingNamespace,
                        EntityFrameworkVersion = entityFrameworkVersion
                    };
                    var contextContents = templateProcessor.Process(Templates.ContextTemplate, contextHost);

                    var contextFilePath = Path.Combine(modelsDirectory, modelGenerator.EntityContainer.Name + contextHost.FileExtension);
                    FileGenerator.AddNewFile(contextFilePath, contextContents);
                    if (action != null)
                    {
                        action("代码生成成功");
                    }

                }
            }
            catch (Exception exception)
            {
                if (action != null)
                {
                    action(exception.Message);
                }
            }
        }
        
        private static Version GetEntityFrameworkVersion()
        {
            return new Version("6.0.0.0");
        }
       

        private static string FixUpConnectionString(string connectionString, string providerName)
        {
     

            if (providerName != "System.Data.SqlClient")
            {
                return connectionString;
            }

            var builder = new SqlConnectionStringBuilder(connectionString)
                {
                    MultipleActiveResultSets = true
                };
            builder.Remove("Pooling");

            return builder.ToString();
        }

        private class EdmMapping
        {
            public EdmMapping(EntityModelSchemaGenerator mcGenerator, StoreItemCollection store)
            {

                // Pull mapping xml out
                var mappingDoc = new XmlDocument();
                var mappingXml = new StringBuilder();

                using (var textWriter = new StringWriter(mappingXml))
                {
                    mcGenerator.WriteStorageMapping(new XmlTextWriter(textWriter));
                }

                mappingDoc.LoadXml(mappingXml.ToString());

                var entitySets = mcGenerator.EntityContainer.BaseEntitySets.OfType<EntitySet>();
                var associationSets = mcGenerator.EntityContainer.BaseEntitySets.OfType<AssociationSet>();
                var tableSets = store.GetItems<EntityContainer>().Single().BaseEntitySets.OfType<EntitySet>();

                this.EntityMappings = BuildEntityMappings(mappingDoc, entitySets, tableSets);
                this.ManyToManyMappings = BuildManyToManyMappings(mappingDoc, associationSets, tableSets);
            }

            public Dictionary<EntityType, Tuple<EntitySet, Dictionary<EdmProperty, EdmProperty>>> EntityMappings { get; set; }

            public Dictionary<AssociationType, Tuple<EntitySet, Dictionary<RelationshipEndMember, Dictionary<EdmMember, string>>>> ManyToManyMappings { get; set; }

            private static Dictionary<AssociationType, Tuple<EntitySet, Dictionary<RelationshipEndMember, Dictionary<EdmMember, string>>>> BuildManyToManyMappings(XmlDocument mappingDoc, IEnumerable<AssociationSet> associationSets, IEnumerable<EntitySet> tableSets)
            {

                // Build mapping for each association
                var mappings = new Dictionary<AssociationType, Tuple<EntitySet, Dictionary<RelationshipEndMember, Dictionary<EdmMember, string>>>>();
                var namespaceManager = new XmlNamespaceManager(mappingDoc.NameTable);
                namespaceManager.AddNamespace("ef", mappingDoc.ChildNodes[0].NamespaceURI);
                foreach (var associationSet in associationSets.Where(a => !a.ElementType.AssociationEndMembers.Where(e => e.RelationshipMultiplicity != RelationshipMultiplicity.Many).Any()))
                {
                    var setMapping = mappingDoc.SelectSingleNode(string.Format("//ef:AssociationSetMapping[@Name=\"{0}\"]", associationSet.Name), namespaceManager);
                    var tableName = setMapping.Attributes["StoreEntitySet"].Value;
                    var tableSet = tableSets.Single(s => s.Name == tableName);

                    var endMappings = new Dictionary<RelationshipEndMember, Dictionary<EdmMember, string>>();
                    foreach (var end in associationSet.AssociationSetEnds)
                    {
                        var propertyToColumnMappings = new Dictionary<EdmMember, string>();
                        var endMapping = setMapping.SelectSingleNode(string.Format("./ef:EndProperty[@Name=\"{0}\"]", end.Name), namespaceManager);
                        foreach (XmlNode fk in endMapping.ChildNodes)
                        {
                            var propertyName = fk.Attributes["Name"].Value;
                            var property = end.EntitySet.ElementType.Properties[propertyName];
                            var columnName = fk.Attributes["ColumnName"].Value;
                            propertyToColumnMappings.Add(property, columnName);
                        }

                        endMappings.Add(end.CorrespondingAssociationEndMember, propertyToColumnMappings);
                    }

                    mappings.Add(associationSet.ElementType, Tuple.Create(tableSet, endMappings));
                }

                return mappings;
            }

            private static Dictionary<EntityType, Tuple<EntitySet, Dictionary<EdmProperty, EdmProperty>>> BuildEntityMappings(XmlDocument mappingDoc, IEnumerable<EntitySet> entitySets, IEnumerable<EntitySet> tableSets)
            {


                // Build mapping for each type
                var mappings = new Dictionary<EntityType, Tuple<EntitySet, Dictionary<EdmProperty, EdmProperty>>>();
                var namespaceManager = new XmlNamespaceManager(mappingDoc.NameTable);
                namespaceManager.AddNamespace("ef", mappingDoc.ChildNodes[0].NamespaceURI);
                foreach (var entitySet in entitySets)
                {
                    // Post VS2010 builds use a different structure for mapping
                    var setMapping = mappingDoc.ChildNodes[0].NamespaceURI == "http://schemas.microsoft.com/ado/2009/11/mapping/cs"
                        ? mappingDoc.SelectSingleNode(string.Format("//ef:EntitySetMapping[@Name=\"{0}\"]/ef:EntityTypeMapping/ef:MappingFragment", entitySet.Name), namespaceManager)
                        : mappingDoc.SelectSingleNode(string.Format("//ef:EntitySetMapping[@Name=\"{0}\"]", entitySet.Name), namespaceManager);

                    var tableName = setMapping.Attributes["StoreEntitySet"].Value;
                    var tableSet = tableSets.Single(s => s.Name == tableName);

                    var propertyMappings = new Dictionary<EdmProperty, EdmProperty>();
                    foreach (var prop in entitySet.ElementType.Properties)
                    {
                        var propMapping = setMapping.SelectSingleNode(string.Format("./ef:ScalarProperty[@Name=\"{0}\"]", prop.Name), namespaceManager);
                        var columnName = propMapping.Attributes["ColumnName"].Value;
                        var columnProp = tableSet.ElementType.Properties[columnName];

                        propertyMappings.Add(prop, columnProp);
                    }

                    mappings.Add(entitySet.ElementType, Tuple.Create(tableSet, propertyMappings));
                }

                return mappings;
            }
        }
    }
}
