using AvScanLibrary.Models;
using AVScanLibrary;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace AVScan.Controllers
{
    public class UploadController : Controller
    {
        static string _filePath = null;
        ScanOutputs output;
        
        
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                string _FileName = string.Empty;
                if (file.ContentLength > 0)
                {
                    _FileName = Path.GetFileName(file.FileName).Replace(" ", "");
                    string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                    file.SaveAs(_path);
                    Vault.FilePath = _path;
                    Vault.FileName = file.FileName;
                }
                ViewBag.Message = string.Format("{0} File Uploaded Successfully!!", _FileName );
                return View("choosescan", _filePath);
            }
            catch
            {
                ViewBag.Message = "File upload failed!!";
                return View();
            }
             
            
        }

        [HttpGet]
        public ActionResult ChooseScan()
        {
            if (string.IsNullOrEmpty(Vault.FileName))
                return View("uploadfile");
            else
                return View();
        }

        [HttpGet]
        public ActionResult Scan(string servicename)
        {
            if (string.IsNullOrEmpty(servicename))
            {
                return View("uploadfile");
            }
            else
            {
                ScannerService service = new ScannerService();
                output = new ScanOutputs();
                output.ScanResults = service.Run(servicename, Vault.FilePath);
                Vault.FileName = null;
                Vault.FilePath = null;
            }

            return View(output);
        }

    }
}