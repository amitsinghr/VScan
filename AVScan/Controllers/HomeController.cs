using AvScanLibrary;
using AvScanLibrary.Models;
using AvScanLibrary.Services;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AVScan.Controllers
{
    public class HomeController : Controller
    {
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
    }
}