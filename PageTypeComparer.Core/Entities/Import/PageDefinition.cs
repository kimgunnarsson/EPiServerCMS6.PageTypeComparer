using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageTypeComparer.Core.Entities
{
    public class PageDefinition
    {
        public string Name { get; set; }
        public PageDefinitionType Type { get; set; }
        public string Required { get; set; }
        public string Searchable { get; set; }
        public string DefaultValueType { get; set; }
        public string EditCaption { get; set; }
        public string HelpText { get; set; }
        public string DisplayEditUI { get; set; }
    }
}
