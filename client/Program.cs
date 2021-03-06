using Blog;
using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace client
{
    class Program
    {
        const string target = "127.0.0.1:50051";
        static async Task Main(string[] args)
        {

            Channel channel = new Channel(target, ChannelCredentials.Insecure);

            await channel.ConnectAsync().ContinueWith((task) =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    Console.WriteLine("The client connected successfully");
                }
            });

            //var client = new GreetingService.GreetingServiceClient(channel);

            //CallGreetingService(client);
            //CallCalculatorService(channel);
            //await CallServerStreamRequest(client);
            //await CallCientStreamRequest(client);
            //await CallBidirectionalRequest(client);
            //CallSqrtService(channel);

            //CallWaveService(channel);

            var client = new BlogService.BlogServiceClient(channel);

            var newBlog = CreateBlog(client);
            //UpdateBlog(client, newBlog);
            //DeleteBlog(client, newBlog);

            await ListBlog(client);

            //ReadBlog(client);

            channel.ShutdownAsync().Wait();
            Console.ReadKey();

        }

        private static void ReadBlog(BlogService.BlogServiceClient client)
        {
            try
            {
                var resp = client.ReadBlog(new ReadBlogRequest { BlogId = "62ab4cdf244a04dbd15e892e" });
                Console.WriteLine(resp.Blog.ToString());
            }
            catch (RpcException ex)
            {
                Console.WriteLine(ex.Status.Detail);
            }
        }

        private static Blog.Blog CreateBlog(BlogService.BlogServiceClient client)
        {
            var response = client.CreateBlog(
                new CreateBlogRequest
                {
                    Blog = new Blog.Blog
                    {
                        AuthorId = "Kraken",
                        Title = "New Blog!",
                        Content = "Hellow World! This is a new blog"
                    }
                });

            Console.WriteLine($"The blog {response.Blog.Id} was created!");

            return response.Blog;
        }

        private static void UpdateBlog(BlogService.BlogServiceClient client, Blog.Blog blog)
        {
            try
            {
                blog.AuthorId = "Kraken2.0";
                blog.Title = "Mil y una noche";
                blog.Content = "new awesome content";

                var response = client.UpdateBlog(new UpdateBlogRequest
                {
                    Blog = blog
                });

                Console.WriteLine(response.Blog.ToString());
            }
            catch (RpcException ex)
            {
                Console.WriteLine(ex.Status.Detail);
            }
        }


        private static void DeleteBlog(BlogService.BlogServiceClient client, Blog.Blog blog)
        {
            try
            {
                var response = client.DeleteBlog(new DeleteBlogRequest { BlogId = blog.Id });

                Console.WriteLine($"The bog with id {response.BlogId} was deleted");
            }
            catch (RpcException ex)
            {
                Console.WriteLine(ex.Status.Detail);
            }
        }

        private static async Task ListBlog(BlogService.BlogServiceClient client)
        {
            var response = client.ListBlog(new ListBlogRequest());

            while (await response.ResponseStream.MoveNext())
            {
                Console.WriteLine(response.ResponseStream.Current.Blog.ToString());
            }
        }
        //private static async Task CallBidirectionalRequest(GreetingService.GreetingServiceClient client)
        //{
        //    var stream = client.GreetEveryone();

        //    var reponseRenderTask = Task.Run(async () =>
        //    {
        //        while (await stream.ResponseStream.MoveNext())
        //        {
        //            Console.WriteLine("Received: " + stream.ResponseStream.Current.Result);
        //        }
        //    });

        //    Greeting[] greetings =
        //         {
        //        new Greeting {FirstName="John", LastName="Doe"},
        //        new Greeting {FirstName="Clement", LastName="Jean"},
        //        new Greeting {FirstName="Olonyl", LastName="Landeros"}
        //    };

        //    foreach (var greeting in greetings)
        //    {
        //        Console.WriteLine("Sending: " + greeting.ToString());
        //        await stream.RequestStream.WriteAsync(new GreetEveryoneRequest { Greeting = greeting });
        //    }

        //    await stream.RequestStream.CompleteAsync();
        //    await reponseRenderTask;

        //}

        //private static async Task CallCientStreamRequest(GreetingService.GreetingServiceClient client)
        //{
        //    var greeting = new Greeting
        //    {
        //        FirstName = "Client",
        //        LastName = "Stream"
        //    };

        //    var request = new LongGreetRequest { Greeting = greeting };
        //    var stream = client.LongGreet();

        //    foreach (int i in Enumerable.Range(1, 10))
        //    {
        //        await stream.RequestStream.WriteAsync(request);
        //    }
        //    await stream.RequestStream.CompleteAsync();

        //    var response = await stream.ResponseAsync;

        //    Console.WriteLine(response.Result);
        //}

        //private static async Task CallServerStreamRequest(GreetingService.GreetingServiceClient client)
        //{
        //    var greeting = new Greeting
        //    {
        //        FirstName = "Server",
        //        LastName = "Stream"
        //    };

        //    var request = new GreetManyTimesRequest { Greeting = greeting };
        //    var response = client.GreetManyTimes(request);

        //    while (await response.ResponseStream.MoveNext())
        //    {
        //        Console.WriteLine(response.ResponseStream.Current.Result);
        //        await Task.Delay(200);
        //    }

        //}
        //private static void CallGreetingService(GreetingService.GreetingServiceClient client)
        //{
        //    var greeting = new Greeting
        //    {
        //        FirstName = "Olonyl",
        //        LastName = "Landeros"
        //    };

        //    var request = new GreetingRequest() { Greeting = greeting };

        //    var response = client.Greet(request);

        //    Console.WriteLine(response.Result);
        //}

        //private static void CallCalculatorService(Channel channel)
        //{
        //    var client = new CalculatorService.CalculatorServiceClient(channel);

        //    var values = new Values
        //    {
        //        X = 100,
        //        Y = 256
        //    };

        //    var request = new CalculatorRequest { Values = values };
        //    var response = client.Sum(request);

        //    Console.WriteLine($"Sum for X: {values.X}, Y: {values.Y} is equal to {response.Result}");
        //}

        //private static void CallWaveService(Channel channel)
        //{
        //    var client = new WavingService.WavingServiceClient(channel);

        //    try
        //    {
        //        var response = client.wage_with_deadline(new WavingRequest { Name = "Olonyl" },
        //            deadline: DateTime.UtcNow.AddMilliseconds(100));

        //        Console.WriteLine(response.Result);

        //    }
        //    catch (RpcException ex) when (ex.StatusCode == StatusCode.DeadlineExceeded)
        //    {
        //        Console.WriteLine($"Error: {ex.Status.Detail}");
        //    }
        //}


        //private static void CallSqrtService(Channel channel)
        //{
        //    var client = new SqrtService.SqrtServiceClient(channel);

        //    Calculate(client, 16);
        //    Calculate(client, -1);

        //}

        //private static void Calculate(SqrtService.SqrtServiceClient client, int number)
        //{
        //    try
        //    {
        //        var response = client.Sqrt(new SqrtRequest { Number = number });

        //        Console.WriteLine(response.SquareRoot);
        //    }
        //    catch (RpcException ex)
        //    {
        //        Console.WriteLine("Error: " + ex.Status.Detail);

        //    }
        //}


    }
}
