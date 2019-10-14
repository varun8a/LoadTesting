using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ParallelTestScript
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fff"));
            //Console.WriteLine(DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.ffffff"));
            //Console.WriteLine(DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffzzzz"));
            //Console.WriteLine(DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fff+00:00"));
            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff"));
            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.ffffffzzzz"));
            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffzzzz"));
            //TimeZone localZone = TimeZone.CurrentTimeZone;
            //TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time")).ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzzz");
            //string str = System.IO.File.ReadAllText(@"D:\\ParallelTestScript\\ParallelTestScript\\Request.txt");
            Task.Run(async () => await Measure());
            System.Console.ReadKey();
        }

        private static async Task Measure()
        {
            var services = new MessageClient();
            var stopper = new Stopwatch();
            var msgs = GetRequests(10);

            

            stopper.Start();
            await services.GetUsersInParallel(msgs);
            System.Console.WriteLine("In paralell: " + stopper.Elapsed);

           
        }

        private static IEnumerable<string> GetRequests(int numberOfIds)
        {
            int num = 24000;
            for (int i = 1; i <= numberOfIds; i++)
            {
                string str= System.IO.File.ReadAllText(@"D:\\ParallelTestScript\\ParallelTestScript\\RequestDamage.txt");
                str = str.Replace("50000", num++.ToString());
                str = str.Replace("01698914-9cca-42c1-9822-0fcd1a5c7054", Guid.NewGuid().ToString());
                yield return str  ;
            }
        }
    }
}
