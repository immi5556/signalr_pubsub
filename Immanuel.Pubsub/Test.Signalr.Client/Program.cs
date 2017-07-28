using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Test.Signalr.Client
{
    class Program
    {
        delegate string ConvertMethod(string inString);
        static void Main(string[] args)
        {
            //MainAsync().Wait();

            List<int> myintList = new List<int> { 1, 4, 5, 6 };
            Func<List<int>, List<int>> SquareList = (m) =>
            {
                return m.ConvertAll(x => x * x);
            };
            var tt = SquareList(myintList);

            var sqlst = myintList.ConvertAll<int>(t => t * t);

            Console.ReadLine();
        }

        private static string UppercaseString(string inputString)
        {
            return inputString.ToUpper();
        }

        static async Task MainAsync()
        {
            try
            {

                var hubConnection = new HubConnection("http://localhost:53684/");
                //hubConnection.TraceLevel = TraceLevels.All;
                //hubConnection.TraceWriter = Console.Out;
                IHubProxy hubProxy = hubConnection.CreateHubProxy("GroupHub");
                //hubProxy.On<string>("SendToAll", data =>
                //{
                //    Console.WriteLine("Incoming data: {0}", data);
                //});
                //ServicePointManager.DefaultConnectionLimit = 10;
                //await hubConnection.Start();

                hubConnection.Start().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Console.WriteLine("There was an error opening the connection:{0}",
                                          task.Exception.GetBaseException());
                    }
                    else
                    {
                        Console.WriteLine("Connected");
                    }

                }).Wait();

                hubProxy.Invoke<string>("SendToSender", "HELLO World ").ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Console.WriteLine("There was an error calling send: {0}",
                                          task.Exception.GetBaseException());
                    }
                    else
                    {
                        Console.WriteLine(task.Result);
                    }
                });

                hubProxy.On<string>("Send", param =>
                {
                    Console.WriteLine(param);
                });

                hubProxy.Invoke<string>("SendToAll", "I'm doing something!!!").Wait();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
            }
        }
    }
}
