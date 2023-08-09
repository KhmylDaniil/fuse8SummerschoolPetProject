using Fuse8_ByteMinds.SummerSchool.InternalApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;

var webHost = WebHost
    .CreateDefaultBuilder(args)
    .UseStartup<Startup>()
    .UseKestrel((builderContext, options) =>
    {
        var grpcPort = builderContext.Configuration.GetValue<int>("GrpcPort");

        options.ConfigureEndpointDefaults(p =>
        {
            p.Protocols = p.IPEndPoint!.Port == grpcPort
            ? HttpProtocols.Http2
            : HttpProtocols.Http1;
        });
    })
    .UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration))
    .Build();

await webHost.RunAsync();
