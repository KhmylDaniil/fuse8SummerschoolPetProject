using Fuse8_ByteMinds.SummerSchool.PublicApi.Interfaces;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Controllers
{
    /// <summary>
    /// Методы для работы с избранными курсами валют
    /// </summary>
    [Route("api/[controller]/")]
    [ApiController]
    public class FavoriteCurrencyController : ControllerBase
    {
        private readonly IFavoriteCurrenciesService _service;

        /// <summary>
        /// Конструктор для <see cref="FavoriteCurrencyController"/>
        /// </summary>
        /// <param name="service">Сервис работы с избранными курсами валют</param>
        public FavoriteCurrencyController(IFavoriteCurrenciesService service) => _service = service;

        /// <summary>
        /// Получить все избранные курсы валют
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <response code="200">
        /// Возвращает при успехе
        /// </response>
        /// <response code="500">
        /// Возвращает при ошибке
        /// </response>
        /// <returns>Все избранные курсы валют</returns>
        [HttpGet]
        public Task<IEnumerable<GetFavoredCurrencyResponse>> GetAllFavoriveCurrenciesAsync(CancellationToken cancellationToken)
            => _service.GetFavoriteCurrenciesAsync(cancellationToken);

        /// <summary>
        /// Получить избранный курс валюты по названию
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <param name="name">Название</param>
        /// <response code="200">
        /// Возвращает при успехе
        /// </response>
        /// <response code="500">
        /// Возвращает при ошибке
        /// </response>
        /// <returns>Избранный курс валюты</returns>
        [HttpGet("{name}")]
        public Task<GetFavoredCurrencyResponse> GetFavoriveCurrencyByNameAsync(string name, CancellationToken cancellationToken)
            => _service.GetFavoriteCurrencyAsync(name, cancellationToken);

        /// <summary>
        /// Создать избранный курс валюты
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="currency">Код валюты</param>
        /// <param name="baseCurrency">Код базовой валюты</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <response code="200">
        /// Возвращает при успехе
        /// </response>
        /// <response code="500">
        /// Возвращает при ошибке
        /// </response>
        /// <returns></returns>
        [HttpPost]
        public async Task CreateFavoriveCurrencyAsync(
            string name,
            CurrencyCode currency,
            CurrencyCode baseCurrency,
            CancellationToken cancellationToken)
                => await _service.CreateFavoriteCurrencyAsync(name, currency, baseCurrency, cancellationToken);

        /// <summary>
        /// Изменить избранный курс валюты
        /// </summary>
        /// <param name="searchName">Название для поиска</param>
        /// <param name="newName">Новое название</param>
        /// <param name="currency">Код валюты</param>
        /// <param name="baseCurrency">Код базовой валюты</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <response code="200">
        /// Возвращает при успехе
        /// </response>
        /// <response code="500">
        /// Возвращает при ошибке
        /// </response>
        /// <returns></returns>
        [HttpPut]
        public Task EditFavoriveCurrencyByNameAsync(
            string searchName,
            string newName,
            CurrencyCode currency,
            CurrencyCode baseCurrency,
            CancellationToken cancellationToken)
                => _service.EditFavoriteCurrencyAsync(searchName, newName, currency, baseCurrency, cancellationToken);

        /// <summary>
        /// Удалить избранный курс валюты по названию
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <param name="name">Название</param>
        /// <response code="200">
        /// Возвращает при успехе
        /// </response>
        /// <response code="500">
        /// Возвращает при ошибке
        /// </response>
        /// <returns>Избранный курс валюты</returns>
        [HttpDelete]
        public Task DeleteFavoriveCurrencyByNameAsync(string name, CancellationToken cancellationToken)
            => _service.DeleteFavoriteCurrencyAsync(name, cancellationToken);
    }
}
