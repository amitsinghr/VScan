using AvScanLibrary.Models;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace AvScanLibrary.Services
{
    public class WDScannner : ScannerBase, IScannerService
    {
        public WDScannner() 
        {
            this._serviceName = "Windows Defender";            
        }

        private string RunUtil
        {
            get
            {
                if (ConfigurationManager.AppSettings["WDKey"] == null)
                    throw new AvscanException("Windows Defender utility not configured or found");

                return ConfigurationManager.AppSettings["WDKey"].ToString();
            }
        }

        private string RunCommand
        {
            get
            {
                if (ConfigurationManager.AppSettings["WDCommand"] == null)
                    throw new AvscanException("Windows Defender Command not configured or found");

                return string.Format(" {0} {1}", ConfigurationManager.AppSettings["WDCommand"].ToString(), _filepath);
            }
        }

        public override ScanOutput Scan(string FilePath)
        {
            ScanOutput output = new ScanOutput();
            VTScanResult scanResult = new VTScanResult();
            List<VTScanResult> logresult = new List<VTScanResult>();
            bool result = false;

            try
            {
                _filepath = FilePath;
                fileLog.StartTime = DateTime.Now.ToString();
                fileLog.Status = ScanStatus.Started;
                fileLog.FilePath = FilePath;
                fileLog.Scanner = _serviceName;

                ScanUtility.ExecuteCommand(RunUtil, RunCommand);
                _message = (String.IsNullOrEmpty(ScanUtility.Output) ? "(none)" : ScanUtility.Output);
                result = (ScanUtility.Result == 0) ? false : true;

                fileLog.FinishTime = DateTime.Now.ToString();
                fileLog.Status = (ScanUtility.Result == 0) ? ScanStatus.Passed : ScanStatus.Failed;
                fileLog.Message = _message;

                output.Result = result;
                output.Message = _message;
                output.ServiceName = _serviceName;
            }
            catch (Exception ex)
            {
                _message = ex.Message;
                fileLog.Status = ScanStatus.Error;
                result = false;
                //throw new AvscanException(ex);
            }
            finally
            {
                scanResult.ScanId = _serviceName;
                scanResult.VirusFound = result;
                scanResult.Result = _message;
                logresult.Add(scanResult);

                fileLog.ScanResults = logresult;
                ScanFileLogRepo.Record(fileLog);
            }
            return output;
        }
    }
}
