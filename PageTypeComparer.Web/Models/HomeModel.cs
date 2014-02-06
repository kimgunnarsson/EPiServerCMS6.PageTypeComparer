using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PageTypeComparer.Web.Models
{
    public class HomeModel
    {
        
    }

    public class ResultModel
    {
        public IEnumerable<Core.Entities.Comparer.Result> Result { get; set; }
        public Core.Entities.ImportFile FileA { get; set; }
        public Core.Entities.ImportFile FileB { get; set; }
    }

    public class ResultItem : PageTypeComparer.Core.Entities.Comparer.Result
    {
        
    }

}