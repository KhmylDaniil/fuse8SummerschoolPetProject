using Fuse8_ByteMinds.SummerSchool.InternalApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

var webHost = WebHost
    .CreateDefaultBuilder(args)
    .UseStartup<Startup>()
    .UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration))
    .Build();

await webHost.RunAsync();