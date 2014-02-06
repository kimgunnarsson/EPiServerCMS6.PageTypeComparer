using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using PageTypeComparer.Web.Models;

namespace PageTypeComparer.Web.Controllers
{
    public class HomeController : Controller
    {


        //// GET: /Home/
        //public ActionResult Index()
        //{
        //    var result = new List<Models.ResultItem>();

        //    return View(result);
        //}

        // GET: /Home/
        public ActionResult Index(ResultModel model)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase fileA, HttpPostedFileBase fileB)
        {
            var resultModel = new ResultModel();
            var result = new List<Core.Entities.Comparer.Result>();
            var guid = System.Guid.NewGuid();
            var targetPath = Server.MapPath("~/App_Data/uploads/" + guid);
            var pathA = "";
            var pathB = "";
            var fileNameA = "";
            var fileNameB = "";

            if (!Directory.Exists(targetPath)) { Directory.CreateDirectory(targetPath);}

            if (fileA != null && fileA.ContentLength > 0)
            {
                var fileExtension = Path.GetExtension(fileA.FileName);
                pathA = Path.Combine(targetPath, "fileA" + fileExtension);
                fileNameA = fileA.FileName;
                fileA.SaveAs(pathA);
            }

            if (fileB != null && fileB.ContentLength > 0)
            {
                var fileExtension = Path.GetExtension(fileB.FileName);
                pathB  = Path.Combine(targetPath, "fileB" + fileExtension);
                fileNameB = fileB.FileName;
                fileB.SaveAs(pathB);
            }

            if (pathA.Length > 0 && pathB.Length > 0)
            {
                var import = new Core.Import();
                import.ExtractionPath = targetPath; 

                import.FilePathA = pathA;
                import.OriginalFileNameA = fileNameA;

                import.FilePathB = pathB;
                import.OriginalFileNameB = fileNameB;

                import.Invoke();

                foreach (var resultItem in import.Result)
                {
                    result.Add(resultItem);
                }

                resultModel.Result = result;
                resultModel.FileA = import.FileA;
                resultModel.FileB = import.FileB;
            }
            
            return View(resultModel);
        }
	}
}