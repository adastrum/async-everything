using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace AsyncEverything.Core
{
    public static class AsyncConsole
    {
        const int STD_INPUT_HANDLE = -10;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CancelIoEx(IntPtr handle, IntPtr lpOverlapped);

        public static Task<string> ReadLineAsync(TimeSpan timeout)
        {
            string result = null;

            return Task.WhenAny(
                Task.Run(async () =>
                {
                    await Task.Delay(timeout);

                    if (string.IsNullOrEmpty(result))
                    {
                        var handle = GetStdHandle(STD_INPUT_HANDLE);
                        CancelIoEx(handle, IntPtr.Zero);

                        return "Timeout";
                    }

                    return result;
                }),
                Task.Run(Console.ReadLine)).Unwrap();
        }

        public static Task WriteLineAsync(string value)
        {
            return Task.Run(() => Console.WriteLine(value));
        }
    }
}
