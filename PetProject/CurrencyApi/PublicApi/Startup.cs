﻿using Audit.Core;
using Audit.Http;
using Fuse8_ByteMinds.SummerSchool.PublicApi.gRPC;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Interfaces;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Serilog;
using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi;

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
                Description = "TestDescription"
            });

            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{typeof(Program).Assembly.GetName().Name}.xml"), true);
		});

		Configuration.Setup().UseSerilog(config => config.Message(
			auditEvent =>
			{
				if (auditEvent is AuditEventHttpClient httpClientEvent)
				{
					var contentBody = httpClientEvent.Action?.Response?.Content?.Body;
					if (contentBody is string { Length: > 1000} stringBody)
					{
						httpClientEvent.Action.Response.Content.Body = stringBody[..1000] + "<...>";
					}
				}
                return auditEvent.ToJson();
            }));

		services.AddGrpcClient<GrpcDocument.GrpcDocumentClient>(c =>
		{
			c.Address = new Uri(_configuration.GetValue<string>("GrpcServiceAddress"));
		})
			.AddAuditHandler(a => a.IncludeRequestBody());

		services.AddTransient<IGrpcClient, GrpcClient>();

        services.AddDbContext<AppDbContext>(o =>
        {
            o.UseNpgsql(
                connectionString: _configuration.GetConnectionString("Default"),
                npgsqlOptionsAction: sqlOptionsBuilder =>
                {
                    sqlOptionsBuilder.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "user");
                })
            .UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IAppDbContext, AppDbContext>();
		services.AddTransient<ISettingsService, SettingsService>();
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

        app.UseRouting()
            .UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
