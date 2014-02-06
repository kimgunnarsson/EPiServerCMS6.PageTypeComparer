using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageTypeComparer.Core.Common
{
    public class Constants
    {
        
        public enum FileOrigin { A, B}
        public enum ResultType { Empty, NotFound, Identical, Single, MismatchOnPageType, MismatchOnPageDefinition, MismatchOnPageDefinitionType }

    }
}
