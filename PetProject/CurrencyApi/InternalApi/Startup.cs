﻿using Audit.Core;
using Audit.Http;
using InternalApi;
using InternalApi.gRPC;
using InternalApi.Interfaces;
using InternalApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Serilog;
using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.InternalApi;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<CurrencySettings>(_configuration.GetRequiredSection("CurrencySettings"));

        services.AddControllers(opt => opt.Filters.Add(typeof(ExceptionFilter)))

            // Добавляем глобальные настройки для преобразования Json
            .AddJsonOptions(
                options =>
                {
                    // Добавляем конвертер для енама
                    // По умолчанию енам преобразуется в цифровое значение
                    // Этим конвертером задаем перевод в строковое занчение
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

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

        services.AddTransient<ISettingsService, SettingsService>();
        services.AddTransient<ICurrencyApi, CurrencyHttpClient>();
        services.AddTransient<ICachedCurrencyService, CachedCurrencyService>();
        services.AddTransient<IChangeCacheService, ChangeCacheService>();


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

        services.AddScoped<IAppDbContext, AppDbContext>();

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
    }

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
                .UseEndpoints(endpoints => endpoints.MapControllers());
            });
    }
}
