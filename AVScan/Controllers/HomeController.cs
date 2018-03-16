using AvScanLibrary;
using AvScanLibrary.Models;
using AvScanLibrary.Services;
using AVScanLibrary;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace AVScan.Controllers
{
    public class HomeController : Controller
    {
        static string _filePath = null;
        private ScanOutputs output;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Log()
        {
            IEnumerable<ScanFileLog> repository = ScanFileLogRepo.List;
            return View(repository);
        }

        public ActionResult Details(int id)
        {
            ScanFileLog fileLog = ScanFileLogRepo.GetbyId(id);
            return View(fileLog);
        }

        public ActionResult CheckReport(int scanid)
        {
            VTScanner vt = new VTScanner();

            ScanFileLog fileLog = ScanFileLogRepo.GetbyId(scanid);

            if (fileLog.Status == ScanStatus.Queued)
            {
                if (fileLog.Scanner.ToLower() == "virus total")
                {
                    vt.GetReport(fileLog);
                }
            }

            return RedirectToAction("Details", new { id = scanid });
        }

        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
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
                ViewBag.Message = string.Format("{0} File Uploaded Successfully!!", _FileName);
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