using Calculator;
using Greet;
using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace client
{
    class Program
    {
        const string target = "127.0.0.1:50051";
        static void Main(string[] args)
        {
            Channel channel = new Channel(target, ChannelCredentials.Insecure);

            channel.ConnectAsync().ContinueWith((task) =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    Console.WriteLine("The client connected successfully");
                }
            });

            CallGreetingService(channel);
            CallCalculatorService(channel);

            channel.ShutdownAsync().Wait();
            Console.ReadKey();

        }

        private static void CallGreetingService(Channel channel)
        {
            // var client = new DummyService.DummyServiceClient(channel);
            var client = new GreetingService.GreetingServiceClient(channel);
            var greeting = new Greeting
            {
                FirstName = "Olonyl",
                LastName = "Landeros"
            };

            var request = new GreetingRequest() { Greeting = greeting };

            var response = client.Greet(request);

            Console.WriteLine(response.Result);
        }

        private static void CallCalculatorService(Channel channel)
        {
            var client = new CalculatorService.CalculatorServiceClient(channel);

            var values = new Values
            {
                X = 100,
                Y = 256
            };

            var request = new CalculatorRequest { Values = values };
            var response = client.Sum(request);

            Console.WriteLine($"Sum for X: {values.X}, Y: {values.Y} is equal to {response.Result}");
        }
    }
}
