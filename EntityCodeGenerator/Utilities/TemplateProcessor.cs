// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
namespace Microsoft.DbContextPackage.Utilities
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using Microsoft.VisualStudio.TextTemplating;
    using Microsoft.VisualStudio.TextTemplating.VSHost;

    internal class TemplateProcessor
    {
        private readonly IDictionary<string, string> _templateCache;

        public TemplateProcessor()
        {
            _templateCache = new Dictionary<string, string>();
        }

        public string Process(string templatePath, EfTextTemplateHost host)
        {

            host.TemplateFile = templatePath;
            Engine engine = new Engine();
            var output = engine.ProcessTemplate(
                GetTemplate(templatePath),
                host);

           
            return output;
        }



        private string GetTemplate(string templatePath)
        {

            if (_templateCache.ContainsKey(templatePath))
            {
                return _templateCache[templatePath];
            }

            var items = templatePath.Split('\\');
            Debug.Assert(items.Length > 1);

            //var childProjectItem
            //    = _project.ProjectItems
            //        .GetItem(items[0]);

            //for (int i = 1; childProjectItem != null && i < items.Length; i++)
            //{
            //    var item = items[i];

            //    childProjectItem = childProjectItem.ProjectItems.GetItem(item);
            //}

            string contents = null;

            //if (childProjectItem != null)
            //{
            //    var path = (string)childProjectItem.Properties.Item("FullPath").Value;

            //    if (!string.IsNullOrWhiteSpace(path))
            //    {
            //        contents = File.ReadAllText(path);
            //    }
            //}

            //if (contents == null)
            //{
            //    contents = Templates.GetDefaultTemplate(templatePath);
            //}

            //_templateCache.Add(templatePath, contents);


            if(File.Exists(templatePath))
            {
                if (!string.IsNullOrWhiteSpace(templatePath))
                {
                    contents = File.ReadAllText(templatePath);
                }
            }

            _templateCache.Add(templatePath, contents);

            return contents;
        }

        //private static ITextTemplatingEngine GetEngine()
        //{
        //    var textTemplating = (ITextTemplatingComponents)Package.GetGlobalService(typeof(STextTemplating));

        //    return textTemplating.Engine;
        //}
    }
}
