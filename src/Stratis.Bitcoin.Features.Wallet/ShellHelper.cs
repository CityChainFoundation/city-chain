using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Stratis.Bitcoin.Features.Wallet.Shell
{
    public static class ShellHelper
    {
        public static string RunCommand(string command, string args, bool windows)
        {
            try
            {
                var escapedArgs = args.Replace("\"", "\\\"");
                var flag = windows ? "/c" : "-c";

                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = command,
                        Arguments = $"{flag} \"{escapedArgs}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    }
                };

                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (string.IsNullOrEmpty(error))
                {
                    return output;
                }
                else
                {
                    return error;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
