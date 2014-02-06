using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageTypeComparer.Core.Entities.Comparer
{
    public class Result
    {
        public PageType PageType { get; set; }
        public PageDefinition PageDefinition { get; set; }
        public PageDefinitionType PageDefinitionType { get; set; }
        public Common.Constants.ResultType ResultType { get; set; }
        public string Message { get; set; }
        public Common.Constants.FileOrigin Origin { get; set; }
    }
}
