// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
namespace Microsoft.DbContextPackage.Utilities
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Data.Metadata.Edm;
    using System.Data.Objects;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Microsoft.VisualStudio.TextTemplating;

    public class EfTextTemplateHost : ITextTemplatingEngineHost
    {
        public EntityType EntityType { get; set; }
        public EntityContainer EntityContainer { get; set; }
        public string Namespace { get; set; }
        public string ModelsNamespace { get; set; }
        public string MappingNamespace { get; set; }
        public Version EntityFrameworkVersion { get; set; }
        public EntitySet TableSet { get; set; }
        public Dictionary<EdmProperty, EdmProperty> PropertyToColumnMappings { get; set; }
        public Dictionary<AssociationType, Tuple<EntitySet, Dictionary<RelationshipEndMember, Dictionary<EdmMember, string>>>> ManyToManyMappings { get; set; }

        #region T4 plumbing

        public CompilerErrorCollection Errors { get; set; }
        public string FileExtension { get; set; }
        public Encoding OutputEncoding { get; set; }
        public string TemplateFile { get; set; }

        public virtual string ResolveAssemblyReference(string assemblyReference)
        {
            if (File.Exists(assemblyReference))
            {
                return assemblyReference;
            }

            try
            {
                // TODO: This is failing to resolve partial assembly names (e.g. "System.Xml")
                var assembly = Assembly.Load(assemblyReference);

                if (assembly != null)
                {
                    return assembly.Location;
                }
            }
            catch (FileNotFoundException)
            {
            }
            catch (FileLoadException)
            {
            }
            catch (BadImageFormatException)
            {
            }

            return string.Empty;
        }

        IList<string> ITextTemplatingEngineHost.StandardAssemblyReferences
        {
            get
            {
                return new[]
                    {
                        Assembly.GetExecutingAssembly().Location,
                        typeof(Uri).Assembly.Location,
                        typeof(Enumerable).Assembly.Location,
                        typeof(ObjectContext).Assembly.Location,

                        //       Because of the issue in ResolveAssemblyReference, these are not being
                        //       loaded but are required by the default templates
                        typeof(System.Data.AcceptRejectRule).Assembly.Location,
                        typeof(System.Data.Entity.Design.EdmToObjectNamespaceMap).Assembly.Location,
                        typeof(System.Xml.ConformanceLevel).Assembly.Location,
                        typeof(System.Xml.Linq.Extensions).Assembly.Location

                       

                    };                   
            }
        }

        IList<string> ITextTemplatingEngineHost.StandardImports
        {
            get
            {
                return new[]
                    {
                        "System",
                        "Microsoft.DbContextPackage.Utilities",
                        "System.Linq",
                        "System.IO",
                        "System.Collections.Generic",
                        "System.Data.Objects",
                        "System.Data.Objects.DataClasses",
                        "System.Xml",
                        "System.Xml.Linq",
                        "System.Globalization",
                        "System.Reflection",
                        "System.Data.Metadata.Edm",
                        "System.Data.Mapping",
                        "System.Data.Entity.Design",
                        "System.CodeDom",
                        "System.CodeDom.Compiler",
                         "Microsoft.CSharp",
                         "System.Text"
                    };
            }
        }

        object ITextTemplatingEngineHost.GetHostOption(string optionName)
        {
            if (optionName == "CacheAssemblies")
            {
                return 1;
            }

            return null;
        }

        bool ITextTemplatingEngineHost.LoadIncludeText(string requestFileName, out string content, out string location)
        {
            location = ((ITextTemplatingEngineHost)this).ResolvePath(requestFileName);

            //if (File.Exists(location))
            //{
            //    content = File.ReadAllText(location);

            //    return true;
            //}

            //string stmp = Assembly.GetExecutingAssembly().Location;

            //stmp = stmp.Substring(0, stmp.LastIndexOf('\\'));//删除文件名
            //location = Path.Combine(stmp, requestFileName);

            if (File.Exists(location))
            {
                content = File.ReadAllText(location);

                // Our implementation doesn't require respecting the CleanupBehavior custom directive, and since
                // implementing a fallback custom directive processor would essencially force us to have two
                // different versions of the EF Power Tools (one for VS 2010, another one for VS 2012) the simplest
                // solution is to remove the custom directive from the in-memory copy of the ttinclude
                content = content.Replace(@"<#@ CleanupBehavior Processor=""T4VSHost"" CleanupAfterProcessingTemplate=""true"" #>", "");

                return true;
            }

            location = string.Empty;
            content = string.Empty;

            return false;
        }

        void ITextTemplatingEngineHost.LogErrors(CompilerErrorCollection errors)
        {
            Errors = errors;
        }

        AppDomain ITextTemplatingEngineHost.ProvideTemplatingAppDomain(string content)
        {
            return AppDomain.CurrentDomain;
        }

        Type ITextTemplatingEngineHost.ResolveDirectiveProcessor(string processorName)
        {
            throw new Exception(processorName);
        }

        string ITextTemplatingEngineHost.ResolveParameterValue(string directiveId, string processorName, string parameterName)
        {
            return string.Empty;
        }

        string ITextTemplatingEngineHost.ResolvePath(string path)
        {
            if (!Path.IsPathRooted(path) && Path.IsPathRooted(TemplateFile))
            {
                return Path.Combine(Path.GetDirectoryName(TemplateFile), path);
            }

            return path;
        }


        public string ResolvePath(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("the file name cannot be null");
            }

            //If the argument is the fully qualified path of an existing file,
            //then we are done
            //----------------------------------------------------------------
            if (File.Exists(fileName))
            {
                return fileName;
            }

            //Maybe the file is in the same folder as the text template that 
            //called the directive.
            //----------------------------------------------------------------
            string candidate = Path.Combine(Path.GetDirectoryName(this.TemplateFile), fileName);
            if (File.Exists(candidate))
            {
                return candidate;
            }

            //Look more places.
            //----------------------------------------------------------------
            //More code can go here...

            //If we cannot do better, return the original file name.
            return fileName;
        }

        public string ResolveParameterValue(string directiveId, string processorName, string parameterName)
        {
            if (directiveId == null)
            {
                throw new ArgumentNullException("the directiveId cannot be null");
            }
            if (processorName == null)
            {
                throw new ArgumentNullException("the processorName cannot be null");
            }
            if (parameterName == null)
            {
                throw new ArgumentNullException("the parameterName cannot be null");
            }

            //Code to provide "hard-coded" parameter values goes here.
            //This code depends on the directive processors this host will interact with.

            //If we cannot do better, return the empty string.
            return String.Empty;
        }

        void ITextTemplatingEngineHost.SetFileExtension(string extension)
        {
            FileExtension = extension;
        }

        void ITextTemplatingEngineHost.SetOutputEncoding(Encoding encoding, bool fromOutputDirective)
        {
            OutputEncoding = encoding;
        }

        #endregion
    }
}
