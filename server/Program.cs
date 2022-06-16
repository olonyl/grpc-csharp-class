using Blog;
using Grpc.Core;
using Grpc.Reflection;
using Grpc.Reflection.V1Alpha;
using System;
using System.IO;

namespace server
{
    class Program
    {
        const int Port = 50051;
        static void Main(string[] args)
        {
            Server server = null;

            try
            {
                var reflectionServiceImpl = new ReflectionServiceImpl(BlogService.Descriptor, ServerReflection.Descriptor);
                server = new Server
                {
                    Services = {
                        BlogService.BindService(new BlogServiceImpl()),
                      //  GreetingService.BindService(new GreetingServiceImpl()) ,
                       ServerReflection.BindService(reflectionServiceImpl)
                        //CalculatorService.BindService(new CalculatorServiceImpl()),
                        //SqrtService.BindService(new SqrtServiceImpl()),
                        //WavingService.BindService(new WavingServiceImpl())
                    },
                    Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
                };

                server.Start();
                Console.WriteLine($"The server is listening on the port : {Port}");
                Console.ReadKey();
            }
            catch (IOException e)
            {
                Console.WriteLine("The server failed to start:  " + e.Message);
                throw;
            }
            finally
            {
                if (server != null)
                {
                    server.ShutdownAsync().Wait();
                }
            }
        }
    }
}
