using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using PageTypeComparer.Core.Common;

namespace PageTypeComparer.Core.Entities.Comparer
{
    public class Comparer
    {

        private List<Result> _result;
        public List<Result> Result
        {
            get
            {
                if (_result == null) { _result = new List<Result>();}
                return _result;
            }
        } 
        
        public Comparer(ImportFile fileA, ImportFile fileB)
        {

            if (!fileA.PageTypes.Any())
            {
                AddResult(null, null, null, Constants.ResultType.Empty, "FileA does not contain any PageTypes.", Constants.FileOrigin.A);
            } 
            else if (!fileB.PageTypes.Any())
            {
                AddResult(null, null, null, Constants.ResultType.Empty, "FileB does not contain any PageTypes.", Constants.FileOrigin.B );   
            }
            else
            {
                ComparePageTypes(fileA.PageTypes, fileB.PageTypes, Constants.FileOrigin.A);
                ComparePageTypes(fileB.PageTypes, fileA.PageTypes, Constants.FileOrigin.B);
            }
        }

        private void ComparePageTypes(IEnumerable<PageType> sourceTypes, IEnumerable<PageType> compareTypes,
            Constants.FileOrigin origin)
        {
            foreach (var pageType in sourceTypes)
            {
                pageType.Origin = origin;

                var matchingPageType = FindPageType(pageType, compareTypes);
                if (matchingPageType != null)
                {
                    var isEqual = true;
                    ComparePageTypeDefinitions(pageType, matchingPageType, out isEqual);

                    if (pageType.FileName.ToLower() != matchingPageType.FileName.ToLower())
                    {
                        isEqual = false;
                        AddResult(pageType, null, null,
                       Constants.ResultType.MismatchOnPageType,
                       "FileName mismatch. File " + pageType.Origin.ToString() + ": " + pageType.FileName + ". File " + matchingPageType.Origin.ToString() + ": " + matchingPageType.FileName + ".", pageType.Origin);
                    }

                    if (pageType.GUID.ToLower() != matchingPageType.GUID.ToLower())
                    {
                        isEqual = false;
                        AddResult(pageType, null, null,
                       Constants.ResultType.MismatchOnPageType,
                       "GUID mismatch. File " + pageType.Origin.ToString() + ": " + pageType.GUID + ". File " + matchingPageType.Origin.ToString() + ": " + matchingPageType.GUID + ".", pageType.Origin);
                    }

                    if (pageType.Name.ToLower() != matchingPageType.Name.ToLower())
                    {
                        isEqual = false;
                        AddResult(pageType, null, null,
                       Constants.ResultType.MismatchOnPageType,
                       "Name mismatch. File " + pageType.Origin.ToString() + ": " + pageType.Name + ". File " + matchingPageType.Origin.ToString() + ": " + matchingPageType.Name + ".", pageType.Origin);
                    }

                    if (pageType.PageDefinitions.Count() != matchingPageType.PageDefinitions.Count())
                    {
                        isEqual = false;
                        AddResult(pageType, null, null,
                       Constants.ResultType.MismatchOnPageType,
                       "PageDefinition Count mismatch. File " + pageType.Origin.ToString() + ": " + pageType.PageDefinitions.Count() + ". File " + matchingPageType.Origin.ToString() + ": " + matchingPageType.PageDefinitions.Count() + ".", pageType.Origin);
                    }

                    if (isEqual)
                    {
                        AddResult(pageType, null, null, Constants.ResultType.Identical, "PageTypes are identical.", pageType.Origin);
                    }
                }
                else
                {
                    AddResult(pageType, null, null, Constants.ResultType.Single, "PageType occurs in file " + pageType.Origin.ToString() + " only.", pageType.Origin);
                }
            }
        }

        private void ComparePageTypeDefinitions(PageType pageTypeA, PageType pageTypeB, out bool isEqual)
        {
            isEqual = true;
            foreach (var pageDefinition in pageTypeA.PageDefinitions)
            {
                var matchingDefinition = pageTypeB.PageDefinitions.Find(x => x.Name == pageDefinition.Name && x.Type.Id == pageDefinition.Type.Id) ?? null;
                if (matchingDefinition == null)
                {
                    isEqual = false;
                    AddResult(pageTypeA, pageDefinition, null,
                            Constants.ResultType.MismatchOnPageDefinition,
                            "PageTypeDefinition " + pageDefinition.Name + " does only exist in file " + pageTypeA.Origin.ToString(), pageTypeA.Origin);
                }
                else if (pageDefinition.Type.DataType != matchingDefinition.Type.DataType)
                {
                    isEqual = false;
                    AddResult(pageTypeA, pageDefinition, pageDefinition.Type,
                        Constants.ResultType.MismatchOnPageDefinitionType,
                        "Mismatch on 'DataType'. In file " + pageTypeA.Origin.ToString() + " is set to '" + pageDefinition.Type.DataType +
                        "' and in file " + pageTypeB.Origin.ToString() + " is set to '" + matchingDefinition.Type.DataType + "'", pageTypeA.Origin);
                }
                else if (pageDefinition.Type.TypeName != matchingDefinition.Type.TypeName)
                {
                    isEqual = false;

                    AddResult(pageTypeA, pageDefinition, pageDefinition.Type,
                        Constants.ResultType.MismatchOnPageDefinitionType,
                        "Mismatch on 'TypeName'. In file " + pageTypeA.Origin.ToString() + " is set to '" + pageDefinition.Type.TypeName +
                        "' and in file " + pageTypeB.Origin.ToString() + " is set to '" + matchingDefinition.Type.TypeName + "'", pageTypeA.Origin);
                }
                else if (pageDefinition.Type.AssemblyName != matchingDefinition.Type.AssemblyName)
                {
                    isEqual = false;

                    AddResult(pageTypeA, pageDefinition, pageDefinition.Type,
                        Constants.ResultType.MismatchOnPageDefinitionType,
                        "Mismatch on 'AssemblyName'. In file " + pageTypeA.Origin.ToString() + " is set to '" +
                        pageDefinition.Type.AssemblyName +
                        "' and in file " + pageTypeB.Origin.ToString() + " is set to '" +
                        matchingDefinition.Type.AssemblyName + "'", pageTypeA.Origin);
                }
            }
        }

        private PageType FindPageType(PageType controlPageType, IEnumerable<PageType> pageTypes)
        {
            foreach (var pageType in pageTypes)
            {
                if (pageType.GUID == controlPageType.GUID && pageType.Name == controlPageType.Name) { return pageType; }
            }
            return null;
        }

        private void AddResult(PageType pageType, PageDefinition pageDefinition, PageDefinitionType pageDefinitionType, Constants.ResultType type, string message, Constants.FileOrigin origin)
        {
            var resultItem = new Result();
            resultItem.PageType = pageType;
            resultItem.PageDefinition = pageDefinition;
            resultItem.PageDefinitionType = pageDefinitionType;
            resultItem.ResultType = type;
            resultItem.Message = message;
            resultItem.Origin = origin;

            if (!Contains(resultItem)) { Result.Add(resultItem); }
        }

        private void AddResult(Result resultItem, Constants.ResultType type, string message)
        {
            resultItem.ResultType = type;
            resultItem.Message = message;

            if (!Contains(resultItem)) { Result.Add(resultItem); }
        }

        private bool Contains(Result item)
        {
            foreach (var itemResult in Result)
            {
                if (itemResult.PageType.GUID == item.PageType.GUID && itemResult.PageType.Name == item.PageType.Name)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
