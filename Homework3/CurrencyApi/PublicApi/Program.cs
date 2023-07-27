using Fuse8_ByteMinds.SummerSchool.PublicApi;
using Microsoft.AspNetCore;
using Serilog;

var webHost = WebHost
	.CreateDefaultBuilder(args)
	.UseStartup<Startup>()
    .UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration))
	.Build();

await webHost.RunAsync();