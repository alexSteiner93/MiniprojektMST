using System;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AutoReservation.Service.Grpc.Testing.Common
{
    public class ServiceTestFixture
        : IDisposable
    {
        private readonly IHost _host;
        public GrpcChannel Channel { get; }

        public ServiceTestFixture()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseUrls("http://localhost:50001")
                        .UseStartup<Startup>();
                })
                .Build();

            _host.Start();

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            Channel = GrpcChannel.ForAddress("http://localhost:50001");
        }

        public void Dispose()
        {
            _host.Dispose();
        }
    }
}