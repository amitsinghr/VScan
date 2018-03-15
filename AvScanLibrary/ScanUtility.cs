using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvScanLibrary
{
    public class ScanUtility
    {
        public static string Output { get; private set; }
        public static string Error { get; private set; }
        public static int Result { get; private set; }

        public static void ExecuteCommand(string utility, string command)
        {
            int exitCode;
            ProcessStartInfo processInfo;
            Process process = null;
            //string output = null, error = null;

            try
            {
                processInfo = new ProcessStartInfo(utility, command);
                processInfo.CreateNoWindow = true;
                processInfo.UseShellExecute = false;
                // *** Redirect the output ***
                processInfo.RedirectStandardError = true;
                processInfo.RedirectStandardOutput = true;

                process = Process.Start(processInfo);
                process.WaitForExit();
                // *** Read the streams ***
                // Warning: This approach can lead to deadlocks, see Edit #2
                Output = process.StandardOutput.ReadToEnd();
                Error = process.StandardError.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new AvscanException(ex.Message);
            }
            finally
            {
                exitCode = process.ExitCode;
                Result = exitCode;
                //Console.WriteLine("output>>" + (String.IsNullOrEmpty(Output) ? "(none)" : Output));
                //Console.WriteLine("error>>" + (String.IsNullOrEmpty(Error) ? "(none)" : Error));
                //Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
                process.Close();
            }
        }
    }
}
