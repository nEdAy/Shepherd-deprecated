using MvcApplication1.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sheep.Controllers
{
    public class FileController : Controller
    {
        public ActionResult Index() {
            return Content("File");
        }
        //
        // GET: /File/

        public string multFileUpload()
        {
            int num = Request.Files.Count;
            List<string> list = new List<string>();
            for (int i = 0; i < num; i++)
            {
                HttpPostedFileBase file = Request.Files[i];
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string fileExt = Path.GetExtension(fileName);

                    string newfileName = Guid.NewGuid().ToString();
                    string fullDir = "/FileUploadImage/" + newfileName + fileExt;
                    file.SaveAs(Request.MapPath(fullDir));
                    list.Add(newfileName);
                }
                else
                {
                    return JsonHelper.Serialize(new { result = "失败", code = 0 });

                }
            }
            return JsonHelper.Serialize(new { result = list, code = 1 });
        }
        public string fileUpload()
        {
            string newfileName = "";
            HttpPostedFileBase file = Request.Files[0];
            if (file != null && file.ContentLength > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                string fileExt = Path.GetExtension(fileName);

                newfileName = Guid.NewGuid().ToString();
                string fullDir = "/FileUploadImage/" + newfileName + fileExt;
                file.SaveAs(Request.MapPath(fullDir));
            }
            else
            {
                return JsonHelper.Serialize(new { result = "失败", code = 0 });

            }
            return JsonHelper.Serialize(new { result = newfileName, code = 1 });
        }
        public ActionResult Image(String id)
        {
            //return Json(new { code = "0", result = "身份验证失败" }, JsonRequestBehavior.AllowGet);
            return File(Server.MapPath(@"/FileUploadImage/" + id), @"image/gif");
        }

    }
}
