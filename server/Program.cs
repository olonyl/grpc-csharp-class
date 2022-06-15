﻿using Greet;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.IO;

namespace server
{
    class Program
    {
        const int Port = 50051;
        static void Main(string[] args)
        {
            Server server = null;
            var serverCert = File.ReadAllText("ssl/server.crt");
            var serverKey = File.ReadAllText("ssl/server.key");
            var keypair = new KeyCertificatePair(serverCert, serverKey);

            var cacert = File.ReadAllText("ssl/ca.crt");

            var credentials = new SslServerCredentials(new List<KeyCertificatePair>() { keypair }, cacert, true);
            try
            {
                server = new Server
                {
                    Services = {
                        GreetingService.BindService(new GreetingServiceImpl()) ,
                        //CalculatorService.BindService(new CalculatorServiceImpl()),
                        //SqrtService.BindService(new SqrtServiceImpl()),
                        //WavingService.BindService(new WavingServiceImpl())
                    },
                    Ports = { new ServerPort("localhost", Port, credentials) }
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
