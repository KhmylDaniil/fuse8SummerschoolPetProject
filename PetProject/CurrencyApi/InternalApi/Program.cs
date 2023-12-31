using Fuse8_ByteMinds.SummerSchool.InternalApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;

public class Program
{
    static void Main(string[] args)
    {
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(
                webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseKestrel((builderContext, options) =>
                    {
                        var grpcPort = builderContext.Configuration.GetValue<int>("GrpcPort");

                        options.ConfigureEndpointDefaults(p =>
                        {
                            p.Protocols = p.IPEndPoint!.Port == grpcPort
                            ? HttpProtocols.Http2
                            : HttpProtocols.Http1;
                        });
                    });
                })
            .UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration))
            .Build().Run();
    }

    /// <summary>
    /// ����� ��� ������� ��������
    /// </summary>
    /// <returns></returns>
    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
      WebHost.CreateDefaultBuilder()
        .UseStartup<Startup>();
}