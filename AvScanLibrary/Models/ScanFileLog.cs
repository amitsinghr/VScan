using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvScanLibrary.Models
{
    public enum ScanStatus
    {
        Queued,
        Started,
        InProgress,
        Completed,
        Passed,
        Failed,
        Error
    }

    public class ScanFileLog
    {
        private string _filepath;
        private FileInfo fileinfo;
        private static int _scanId;

        

        public ScanFileLog()
        {
            _scanId = _scanId + 1;
            ScanId = _scanId;
        }

        public int ScanId { get; private set; }
        public string FileName { get; private set; }
        public string Scanner { get; set; }        
        public string FileType { get; private set; }
        public long Filesize { get; private set; }

        public string FilePath
        {
            get { return _filepath; }
            set
            {

                _filepath = value;
                fileinfo = new FileInfo(_filepath);
                this.FileName = fileinfo.Name;
                this.FileType = fileinfo.Extension;
                this.Filesize = fileinfo.Length;

            }
        }

        public string StartTime { get; set; }
        public string FinishTime { get; set; }
        public ScanStatus Status { get; set; }
        public List<VTScanResult> ScanResults { get; set; }
        public string Message { get; internal set; }
    }
}
