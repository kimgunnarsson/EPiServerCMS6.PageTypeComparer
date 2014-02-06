using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace PageTypeComparer.Core.Entities
{
    public class ImportFile
    {
        public string ExtractionPath { get; set; }
        public string FilePath { get; set; }
        public string OriginalFileName { get; set; }
        public Common.Constants.FileOrigin Origin { get; set; }

        private List<PageType> _pageTypes;
        public List<PageType> PageTypes
        {
            get
            {
                if (_pageTypes == null)
                {
                    _pageTypes = new List<PageType>();
                }
                return _pageTypes;
            }
        }
        
        public void BeginImport()
        {
            var readDocument = GetXmlDocument();
            var extractor = new PageTypeExtractor(readDocument, Origin);

            if (extractor.FoundPageTypes.Any())
            {
                foreach (var pageType in extractor.FoundPageTypes)
                {
                    PageTypes.Add(pageType);
                }
            }
        }

        private XDocument GetXmlDocument()
        {
            var importFileInfo = new System.IO.FileInfo(FilePath);
            if (importFileInfo.Exists)
            {
                var fileAttributes = importFileInfo.Attributes;
                if (fileAttributes == FileAttributes.Compressed || importFileInfo.FullName.EndsWith(".episerverdata"))
                {
                    return ExtractZIP(importFileInfo);
                }
                else
                {
                    return ExtractXML(importFileInfo);
                }
            }
            else
            {
                return null;
            }
        }

        private XDocument ExtractZIP(FileInfo importFileInfo)
        {
            if (!string.IsNullOrEmpty(ExtractionPath))
            {
                if (!Directory.Exists(ExtractionPath)) {Directory.CreateDirectory(ExtractionPath); }
                using (ZipArchive archive = ZipFile.OpenRead(importFileInfo.FullName))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.FullName.EndsWith("epix.xml", StringComparison.OrdinalIgnoreCase))
                        {
                            entry.ExtractToFile(Path.Combine(ExtractionPath, entry.FullName));
                        }
                    }
                }
                var extractedFileInfo = new FileInfo(Path.Combine(ExtractionPath, "epix.xml"));
                return ExtractXML(extractedFileInfo);
            }
            else
            {
                throw new DirectoryNotFoundException("ExtractionPath not found");
            }
        }

        private XDocument ExtractXML(FileInfo importFileInfo)
        {
            if (importFileInfo.Exists)
            {
                try
                {
                    return XDocument.Load(importFileInfo.FullName);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            else
            {
               throw new FileNotFoundException("ImportFile not found.");
            }
        }

    }
    
}
