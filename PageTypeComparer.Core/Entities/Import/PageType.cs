using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageTypeComparer.Core.Entities
{
    public class PageType
    {
        public string Name { get; set; }
        public string GUID { get; set; }
        public string FileName { get; set; }
        public Common.Constants.FileOrigin Origin { get; set; }

        private List<PageDefinition> _pageDefinitions;
        public List<PageDefinition> PageDefinitions
        {
            get
            {
                if (_pageDefinitions == null)
                {
                    _pageDefinitions = new List<PageDefinition>();
                }
                return _pageDefinitions;
            }
        }

    }
}
