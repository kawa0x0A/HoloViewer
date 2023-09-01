using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace UpdateChecker
{
    public class Program : UpdateCheckLibrary.Program
    {
        static async Task Main (string[] args)
        {
            if ((args.Length > 0) && (args[0] == "/Update"))
            {
                Console.WriteLine("アップデート開始");

                var processID = int.Parse(args[1]);

                try
                {
                    var process = Process.GetProcessById(processID);

                    if (process != null)
                    {
                        process.Kill();

                        Console.Write("初期化中");

                        await Task.Run(() => { for (int i = 0; i < 5; i++) { Console.Write("."); System.Threading.Thread.Sleep(1000); } });

                        Console.WriteLine();
                    }
                }
                catch
                {
                    throw;
                }

                Console.WriteLine("ファイルコピー中");

                CopyExtractFiles();

                Console.WriteLine("アップデート完了");

                Process.Start(new ProcessStartInfo("HoloViewer.exe", "/Update"));
            }
        }
    }
}
