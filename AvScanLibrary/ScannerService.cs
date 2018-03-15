using AvScanLibrary;
using AvScanLibrary.Models;
using AvScanLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AVScanLibrary
{
    public static class Vault
    {
        public static string FilePath { get; set; }
        public static string FileName { get; set; }
    }
    /// <summary>
    /// This Service is Used for Scanning Files
    /// </summary>
    public class ScannerService
    {
        string _serviceName = string.Empty;
        Dictionary<string, IScannerService> _dict;         

        public ScannerService()
        {
            _dict = new Dictionary<string, IScannerService>();
            _dict.Add("wd", new WDScannner());
            _dict.Add("vt", new VTScanner());            
        }

        public IEnumerable<ScanOutput> Run(string serviceName, string filePath)
        {
            IEnumerable<ScanOutput> result = null;
            try
            {
                if (serviceName.ToLower() == "all")
                {
                    var service = _dict.ToList();
                    result = service.Select(q => q.Value).Select(q => q.Scan(filePath)).Cast<ScanOutput>().ToList();                    
                }
                else
                {
                    var sservice = _dict.Where(q => q.Key == serviceName).Select(q => q.Value);
                    result = sservice.Select(q =>  q.Scan(filePath)).Cast<ScanOutput>();
                }
            }
            catch(Exception ex)
            {
                throw new AvscanException(ex.Message);
            }
            return result;
        }
    }
}