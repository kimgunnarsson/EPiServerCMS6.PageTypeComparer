using System.Collections.Generic;
using PageTypeComparer.Core.Common;
using PageTypeComparer.Core.Entities;
using PageTypeComparer.Core.Entities.Comparer;

namespace PageTypeComparer.Core
{
    public class Import
    {
        public ImportFile FileA { get; set; }
        public ImportFile FileB { get; set; }

        public string FilePathA { get; set; }
        public string FilePathB { get; set; }
        public string ExtractionPath { get; set; }

        public string OriginalFileNameA { get; set; }
        public string OriginalFileNameB { get; set; }

        public List<Result> Result { get; set; } 
        
        public void Invoke()
        {
            var fileAGuid = System.Guid.NewGuid().ToString();
            var fileBGuid = System.Guid.NewGuid().ToString();

            FileA = new ImportFile();
            FileA.OriginalFileName = OriginalFileNameA;
            FileA.FilePath = FilePathA;
            FileA.Origin = Constants.FileOrigin.A;
            FileA.ExtractionPath = ExtractionPath + "/" + fileAGuid;
            FileA.BeginImport();

            FileB = new ImportFile();
            FileB.OriginalFileName = OriginalFileNameB;
            FileB.FilePath = FilePathB;
            FileB.Origin = Constants.FileOrigin.B;
            FileB.ExtractionPath = ExtractionPath + "/" + fileBGuid;
            FileB.BeginImport();

            var comparer = new Comparer(FileA, FileB);
            Result = comparer.Result;
        }

    }
}
