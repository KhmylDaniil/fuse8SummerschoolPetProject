﻿using Audit.Core;
using Audit.Http;
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

        services.AddHttpClient<CurrencyHttpClient>(x => 
			x.BaseAddress = new Uri(_configuration.GetRequiredSection("CurrencySettings").Get<CurrencySettings>().BaseAddress))
			.AddAuditHandler(audit => audit
			.IncludeRequestHeaders()
			.IncludeRequestBody()
			.IncludeResponseHeaders()
			.IncludeResponseBody()
			.IncludeContentHeaders());

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
