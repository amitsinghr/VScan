using AvScanLibrary.Models;
using System;

namespace AvScanLibrary.Services
{
    public class ScannerBase : IScannerService
    {
        protected string _message, _filepath, _serviceName = "UnKnown";
        protected ScanFileLog fileLog;

        public ScannerBase()
        {
            fileLog = new ScanFileLog();
            fileLog.Scanner = _serviceName;
        }

        public string Message
        {
            get { return _message; }
        }

        public virtual ScanOutput Scan(string FilePath)
        {
            throw new NotImplementedException();
        }
    }
}
