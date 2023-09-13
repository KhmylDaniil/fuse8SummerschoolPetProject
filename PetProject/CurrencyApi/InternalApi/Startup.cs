using Audit.Core;
using Audit.Http;
using InternalApi;
using InternalApi.gRPC;
using InternalApi.Interfaces;
using InternalApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.InternalApi;

/// <summary>
/// Стартап
/// </summary>
public class Startup
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Конструктор для <see cref="Startup"/>
    /// </summary>
    /// <param name="configuration">Конфигурация</param>
    public Startup(IConfiguration configuration) => _configuration = configuration;

    /// <summary>
    /// Конфигурация сервисов
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<CurrencySettings>(_configuration.GetRequiredSection("CurrencySettings"));

        services.AddControllers(opt => opt.Filters.Add(typeof(ExceptionFilter)))
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
            {
                Title = "Api",
                Version = "v1",
                Description = "InternalApi"
            });

            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{typeof(Program).Assembly.GetName().Name}.xml"), true);
        });

        services.AddHttpClient<CurrencyHttpClient>(x =>
            x.BaseAddress = new Uri(_configuration.GetRequiredSection("CurrencySettings").Get<CurrencySettings>().BaseAddress))
            .AddAuditHandler(audit => audit
                .IncludeRequestHeaders()
                .IncludeRequestBody()
                .IncludeResponseHeaders()
                .IncludeResponseBody()
                .IncludeContentHeaders());

        services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
        services.AddTransient<ISettingsService, SettingsService>();
        services.AddTransient<ICurrencyApi, CurrencyHttpClient>();
        services.AddTransient<ICachedCurrencyService, CachedCurrencyService>();
        services.AddTransient<IChangeCacheService, ChangeCacheService>();

        services.AddHostedService<ChangeCacheBackgroundService>();

        services.AddDbContext<AppDbContext>(o =>
        {
            o.UseNpgsql(
                connectionString: _configuration.GetConnectionString("Default"),
                npgsqlOptionsAction: sqlOptionsBuilder =>
                {
                    sqlOptionsBuilder.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "cur");
                })
            .UseSnakeCaseNamingConvention();
        });

        Configuration.Setup().UseSerilog(config => config.Message(
            auditEvent =>
            {
                if (auditEvent is AuditEventHttpClient httpClientEvent)
                {
                    var contentBody = httpClientEvent.Action?.Response?.Content?.Body;
                    if (contentBody is string { Length: > 1000 } stringBody)
                    {
                        httpClientEvent.Action.Response.Content.Body = stringBody[..1000] + "<...>";
                    }
                }
                return auditEvent.ToJson();
            }));

        services.AddGrpc();

        services.AddMemoryCache(options => options.ExpirationScanFrequency = TimeSpan.FromHours(2));

        services.AddHealthChecks()
    .AddCheck("LogHealthy", () =>
    {
        Console.WriteLine("Log is healthy");
        return HealthCheckResult.Healthy();
    })
    .AddNpgSql(_configuration.GetConnectionString("Default"));
    }

    /// <summary>
    /// Конфигурация билдера
    /// </summary>
    /// <param name="app">Билдер</param>
    /// <param name="env">Окружение</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }

        app.UseMiddleware<LoggingMiddleware>();

        app.UseWhen(predicate: context => context.Connection.LocalPort == _configuration.GetValue<int>("GrpcPort"),
            configuration: grpcBuilder =>
            {
                grpcBuilder.UseRouting();
                grpcBuilder.UseEndpoints(endpoints => endpoints.MapGrpcService<GrpcService>());
            });

        app.UseWhen(predicate: context => context.Connection.LocalPort == _configuration.GetValue<int>("ApiPort"),
            configuration: apiBuilder =>
            {
                apiBuilder.UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHealthChecks("/health");
                });
            });
    }
}
