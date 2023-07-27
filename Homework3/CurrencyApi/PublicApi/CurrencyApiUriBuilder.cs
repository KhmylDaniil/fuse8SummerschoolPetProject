using Microsoft.AspNetCore.Http.Extensions;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi
{
    /// <summary>
    /// Кастомный сборщик uri
    /// </summary>
    public class CurrencyApiUriBuilder
    {
        private readonly UriBuilder _uriBuilder;
        private readonly QueryBuilder _queryBuilder;

        /// <summary>
        /// Конструктор для включения базового адреса внешнего API и билдера запроса
        /// </summary>
        /// <param name="settings">Настойки приложения по валюте</param>
        public CurrencyApiUriBuilder(CurrencySettings settings)
        {
            _uriBuilder = new UriBuilder(settings.BaseAddress);
            _queryBuilder = new QueryBuilder();
        }

        /// <summary>
        /// Добавление пути в uri
        /// </summary>
        /// <param name="path">Путь</param>
        public void AddPath(string path) => _uriBuilder.Path += path;

        /// <summary>
        /// Добавление запроса к uri
        /// </summary>
        /// <param name="name">Название параметра</param>
        /// <param name="value">Значение параметра</param>
        public void AddQuery(string name, string value) => _queryBuilder.Add(name, value);

        /// <summary>
        /// Создание uri для обращения к внешнему API
        /// </summary>
        /// <returns>Готовая строка, содержащая uri</returns>
        public override string ToString()
        {
            _uriBuilder.Query = _queryBuilder.ToString();
            return _uriBuilder.Uri.ToString();
        }
    }
}
