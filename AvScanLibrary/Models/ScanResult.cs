using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvScanLibrary.Models
{
    public class VTScanResult
    {
        public string ScanId { get; set; }
        public bool VirusFound { get; set; }
        public string Result { get; set; }
    }
}
