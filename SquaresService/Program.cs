using System;
using System.Collections.Generic;
using ServiceStack;
using SquaresServiceInterface;
using SquaresSolver;

namespace SquaresService
{
    class Program
    {
        //Define the Web Services AppHost
        public class AppHost : AppSelfHostBase
        {
            public AppHost()
                : base("HttpListener Self-Host", typeof(SquareSolverService).Assembly) { }

            public override void Configure(Funq.Container container) { }
        }

        //Run it!
        static void Main(string[] args)
        {
            SquareTilingCombinatorics.Init();

            var listeningOn = args.Length == 0 ? "http://localhost:1330/" : "http://localhost:890" + args[0] + "/";
            var appHost = new AppHost().Init();

            try { appHost.Start(listeningOn);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Service was unable to start.");
                Console.WriteLine(ex.Message);
                return;
            }

            Console.WriteLine("AppHost Created at {0}, listening on {1}",
                DateTime.Now, listeningOn);

            var map = new List<bool[]>();

            map.Add(new bool[1]);

            map[0][0] = true;

            var client = new JsonServiceClient(listeningOn);

            SquareSolverResponse response = client.Post<SquareSolverResponse>(new SquareSolver { Map = map, CostMargin = 1 });

            Console.WriteLine(response.Solution.Count);

            Console.ReadLine();
        }
    }
}
