using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using PageTypeComparer.Core.Common;

namespace PageTypeComparer.Core.Entities
{
    public class PageTypeExtractor
    {
        private XDocument ReadDocument { get; set; }
        
        private List<PageType> _foundPageTypes;
        public List<PageType> FoundPageTypes
        {
            get
            {
                if (_foundPageTypes == null)
                {
                    _foundPageTypes = new List<PageType>();
                }
                return _foundPageTypes;
            }
        }

        private Constants.FileOrigin FileOrigin { get; set; }

        public PageTypeExtractor(XDocument readDocument, Constants.FileOrigin fileOrigin)
        {
            ReadDocument = readDocument;
            FileOrigin = fileOrigin;
            GetPageTypes();
        }
        
        private void GetPageTypes()
        {
            if (ReadDocument.Descendants("pagetypes").Any())
            {
                var pageTypeQuery = from pt in ReadDocument.Descendants("pagetypes") select pt;
                foreach (var pageType in pageTypeQuery)
                {
                    foreach (var pageTypeItem in pageType.Descendants("PageType"))
                    {
                        var convertedPageTypeItem = new PageType();
                        convertedPageTypeItem.Origin = FileOrigin;

                        foreach (var pageTypeElement in pageTypeItem.Elements())
                        {
                            if (pageTypeElement.Name.ToString().ToLower() == "name")
                            {
                                convertedPageTypeItem.Name = pageTypeElement.Value;
                            }
                            else if (pageTypeElement.Name.ToString().ToLower() == "guid")
                            {
                                convertedPageTypeItem.GUID = pageTypeElement.Value;
                            }
                            else if (pageTypeElement.Name.ToString().ToLower() == "filename")
                            {
                                convertedPageTypeItem.FileName = pageTypeElement.Value;
                            }
                            else if (pageTypeElement.Name.ToString().ToLower() == "definitions")
                            {
                                var pageDefinitions = GetPageDefinitions(pageTypeElement);
                                if (pageDefinitions.Any())
                                {
                                    foreach (var pageDefinition in pageDefinitions)
                                    {
                                        convertedPageTypeItem.PageDefinitions.Add(pageDefinition);
                                    }
                                }
                            }
                        }
                        FoundPageTypes.Add(convertedPageTypeItem);
                    }
                }
            }
        }
        
        private List<PageDefinition> GetPageDefinitions(XElement pageDefinitionsElement)
        {
            var returnPageDefinitions = new List<PageDefinition>();
            if (pageDefinitionsElement.Descendants().Any())
            {
                var pageDefinitionQuery = from pd in pageDefinitionsElement.Descendants("PageDefinition") select pd;
                foreach (var pageDefinitionElement in pageDefinitionQuery)
                {
                    var pageDefinition = new PageDefinition();
                    foreach (var pageDefinitionAttributeElement in pageDefinitionElement.Descendants())
                    {
                        if (pageDefinitionAttributeElement.Name.ToString().ToLower() == "name")
                        {
                            pageDefinition.Name = pageDefinitionAttributeElement.Value;
                        }
                        else if (pageDefinitionAttributeElement.Name.ToString().ToLower() == "required")
                        {
                            pageDefinition.Required = pageDefinitionAttributeElement.Value;
                        }
                        else if (pageDefinitionAttributeElement.Name.ToString().ToLower() == "searchable")
                        {
                            pageDefinition.Searchable = pageDefinitionAttributeElement.Value;
                        }
                        else if (pageDefinitionAttributeElement.Name.ToString().ToLower() == "defaultvaluetype")
                        {
                            pageDefinition.DefaultValueType = pageDefinitionAttributeElement.Value;
                        }
                        else if (pageDefinitionAttributeElement.Name.ToString().ToLower() == "editcaption")
                        {
                            pageDefinition.EditCaption = pageDefinitionAttributeElement.Value;
                        }
                        else if (pageDefinitionAttributeElement.Name.ToString().ToLower() == "helptext")
                        {
                            pageDefinition.HelpText = pageDefinitionAttributeElement.Value;
                        }
                        else if (pageDefinitionAttributeElement.Name.ToString().ToLower() == "displayeditui")
                        {
                            pageDefinition.DisplayEditUI = pageDefinitionAttributeElement.Value;
                        }
                        else if (pageDefinitionAttributeElement.Name.ToString().ToLower() == "type")
                        {
                            pageDefinition.Type = GetPageDefinitonType(pageDefinitionAttributeElement);
                        }
                    }
                    returnPageDefinitions.Add(pageDefinition);
                }
            }
            return returnPageDefinitions;
        }

        private PageDefinitionType GetPageDefinitonType(XElement pageDefinitionTypeElement)
        {
            var pageDefinitionType = new PageDefinitionType();
            foreach (var pageDefinitionTypeAttributeElement in pageDefinitionTypeElement.Descendants())
            {
                if (pageDefinitionTypeAttributeElement.Name.ToString().ToLower() == "id")
                {
                    pageDefinitionType.Id = pageDefinitionTypeAttributeElement.Value;
                }
                else if (pageDefinitionTypeAttributeElement.Name.ToString().ToLower() == "datatype")
                {
                    pageDefinitionType.DataType = pageDefinitionTypeAttributeElement.Value;
                }
                else if (pageDefinitionTypeAttributeElement.Name.ToString().ToLower() == "name")
                {
                    pageDefinitionType.Name = pageDefinitionTypeAttributeElement.Value;
                }
                else if (pageDefinitionTypeAttributeElement.Name.ToString().ToLower() == "typename")
                {
                    pageDefinitionType.TypeName = pageDefinitionTypeAttributeElement.Value;
                }
                else if (pageDefinitionTypeAttributeElement.Name.ToString().ToLower() == "assemblyname")
                {
                    pageDefinitionType.AssemblyName = pageDefinitionTypeAttributeElement.Value;
                }

            }
            return pageDefinitionType;
        }

    }
}
