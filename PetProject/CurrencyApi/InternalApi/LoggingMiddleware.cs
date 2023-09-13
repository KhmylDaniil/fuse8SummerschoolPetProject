namespace Fuse8_ByteMinds.SummerSchool.InternalApi;

    /// <summary>
    /// Миддлвар логирования входящих запросов
    /// </summary>
    public class LoggingMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        /// <summary>
        /// Конструктор для <see cref="LoggingMiddleware"/>
        /// </summary>
        /// <param name="next">Делегат</param>
        /// <param name="loggerFactory">Фабрика создания логгера</param>
        public LoggingMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<LoggingMiddleware>();
            _next = next;
        }

        /// <summary>
        /// Вызов запроса
        /// </summary>
        /// <param name="context">Контест http</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            _logger.LogInformation("Поступил запрос {method} {path}", request.Method, request.Path);
            await _next(context);
            _logger.LogInformation("Запрос обработан {method} {path}", request.Method, request.Path);
        }
    }

