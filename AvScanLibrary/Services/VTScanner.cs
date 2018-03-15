using AvScanLibrary.Models;
using AVScanLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using VirusTotalNET;
using VirusTotalNET.Objects;
using VirusTotalNET.ResponseCodes;
using VirusTotalNET.Results;

namespace AvScanLibrary.Services
{
    public class VTScanner : ScannerBase, IScannerService
    {
        private const string ScanUrl = "http://www.google.com/";
        public VTScanner()
        {
            _serviceName = "Virus Total";
        }
        public override ScanOutput Scan(string FilePath)
        {
            try
            {
                ScanOutput output = new ScanOutput();
                fileLog.Status = ScanStatus.Queued;
                fileLog.FilePath = FilePath;
                fileLog.Scanner = _serviceName;

                Task<List<VTScanResult>> task = Task.Run<List<VTScanResult>>(async () => await RunExample(FilePath,fileLog));

                _message = "File is queued! Please check back after some time";
                output.Result = false;
                output.Message = "Please check back in a while";
                output.ServiceName = _serviceName;

                return output;
            }
            catch (Exception ex)
            {
                fileLog.Status = ScanStatus.Error;
                fileLog.Message = ex.Message;
                throw new AvscanException(ex);
            }
            finally
            {
                ScanFileLogRepo.Record(fileLog);
            }
        }

        public void GetReport(ScanFileLog fileLog)
        {
            try
            {
                Task<List<VTScanResult>> task = Task.Run<List<VTScanResult>>(async () => await RunExample(fileLog.FilePath, fileLog));
            }
            catch (Exception ex)
            {
                fileLog.Status = ScanStatus.Error;
                fileLog.Message = ex.Message;
                throw new AvscanException(ex);
            }
            finally
            {
                //ScanFileLogRepo.Record(fileLog);
            }
        }

        #region private virustotal methods
        //private static async Task RunExample(string test)
        private async Task<List<VTScanResult>> RunExample(string strfile, ScanFileLog scanFileinfo)
        {
            List<VTScanResult> log = new List<VTScanResult>();
            Debug.WriteLine(DateTime.Now + ": RunExample Task Started ");
            ScanFileLog scanFileLog = ScanFileLogRepo.GetbyId(scanFileinfo.ScanId);
            scanFileLog.StartTime = DateTime.Now.ToString();
            scanFileLog.Status = ScanStatus.Started;

            try
            {
                VirusTotal virusTotal = new VirusTotal("86c775d4d351a9a180f798b3957d51cf9bd838b80f88cd226f28dbaa6a4d3102");

                //Use HTTPS instead of HTTP
                virusTotal.UseTLS = true;

                //Create the EICAR test virus. See http://www.eicar.org/86-0-Intended-use.html
                //byte[] eicar = Encoding.ASCII.GetBytes(@"X5O!P%@AP[4\PZX54(P^)7CC)7}$EICAR-STANDARD-ANTIVIRUS-TEST-FILE!$H+H*");

                //Check if the file has been scanned before.
                //FileReport fileReport = await virusTotal.GetFileReportAsync(eicar);
                FileReport fileReport = await virusTotal.GetFileReportAsync(new System.IO.FileInfo(strfile));

                bool hasFileBeenScannedBefore = fileReport.ResponseCode == FileReportResponseCode.Present;

                Debug.WriteLine("File has been scanned before: " + (hasFileBeenScannedBefore ? "Yes" : "No"));

                //If the file has been scanned before, the results are embedded inside the report.
                if (hasFileBeenScannedBefore)
                {
                    scanFileLog.Status = ScanStatus.Completed;
                    scanFileLog.Message = (fileReport.Positives > 0) ? string.Format("Virus Found {0}", fileReport.Positives) : fileReport.VerboseMsg;

                    if (fileReport.ResponseCode == FileReportResponseCode.Present)
                    {
                        foreach (KeyValuePair<string, ScanEngine> scan in fileReport.Scans)
                        {
                            Debug.WriteLine("{0,-25} Detected: {1}", scan.Key, scan.Value.Detected);
                            log.Add(new VTScanResult
                            {
                                ScanId = scan.Key,
                                VirusFound = scan.Value.Detected,
                                Result = scan.Value.Result

                            });
                        }
                    }
                }
                else
                {
                    //ScanResult fileResult = await virusTotal.ScanFileAsync(eicar, "EICAR.txt");
                    scanFileLog.Status = ScanStatus.Queued;
                    ScanResult fileResult = await virusTotal.ScanFileAsync(new System.IO.FileInfo(strfile));
                    scanFileLog.Message = fileResult.VerboseMsg;

                    log.Add(new VTScanResult
                    {
                        ScanId = fileResult.ScanId,
                        Result = fileResult.VerboseMsg
                         
                    });                
                }
            }
            catch (Exception ex)
            {
                scanFileLog.Status = ScanStatus.Error;
                scanFileLog.Message = ex.Message;
                //throw new;
            }
            finally
            {
                scanFileLog.FinishTime = DateTime.Now.ToString();
                scanFileLog.ScanResults = log;
               // ScanFileLogRepo.Record(scanFileLog);
            }

            return log as List<VTScanResult>; 
        }

        
        #endregion
    }
}
