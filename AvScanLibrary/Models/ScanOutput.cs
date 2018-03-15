namespace AvScanLibrary.Models
{
    public class ScanOutput
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public string File { get; internal set; }
        public string ServiceName { get; set; }
    }
}
