using AvScanLibrary.Models;

namespace AvScanLibrary
{
    public interface IScannerService
    {
        ScanOutput Scan(string FilePath);
        string Message { get; }
    }
}
