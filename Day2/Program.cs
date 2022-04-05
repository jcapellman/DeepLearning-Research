using static Tensorflow.Binding;
using static Tensorflow.KerasApi;
using Tensorflow;
using Tensorflow.NumPy;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Day2
{
    class Day2
    {
        public class DeviceIdentification
        {
            private static string ExecProcess(string processName, string? command = null)
            {
                var procStartInfo = new ProcessStartInfo(processName, command ?? string.Empty)
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                var proc = new Process() { StartInfo = procStartInfo };
                proc.Start();

                return proc.StandardOutput.ReadToEnd();
            }

            private static string GetLinuxID() => ExecProcess("sudo dmidecode -t system | grep \"Serial Number\"");

            private static string GetWindowsID()
            {
                var output = ExecProcess("cmd", "/c " + "wmic csproduct get UUID");

                return output.Replace("UUID", string.Empty).Trim().ToUpper();
            }

            private static string GetMacID() => ExecProcess("system_profiler | grep \"Serial Number(system)\"");

            public static string UniqueID
            {
                get
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        return GetWindowsID();
                    }

                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        return GetLinuxID();
                    }

                    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        return GetMacID();
                    }

                    throw new PlatformNotSupportedException($"Unsupported Platform ({RuntimeInformation.OSDescription})");
                }
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine(DeviceIdentification.UniqueID);

            Console.ReadKey();
        }
    }
}