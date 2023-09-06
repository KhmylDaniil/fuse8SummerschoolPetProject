using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Responses;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Interfaces
{
    /// <summary>
    /// Интерфейс работы с избранными курсами валют
    /// </summary>
    public interface IFavoriteCurrenciesService
    {
        /// <summary>
        /// Получить все избранные курсы валют
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>избранные курсы валют</returns>
        Task<GetFavoredCurrencyResponse[]> GetFavoriteCurrenciesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получить избранный курс валюты по названию
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Избранный курс валюты</returns>
        Task<GetFavoredCurrencyResponse> GetFavoriteCurrencyAsync(string name, CancellationToken cancellationToken);
    
        /// <summary>
        /// Создать избранный курс валюты
        /// </summary>
        /// <param name="name">Навзание</param>
        /// <param name="currency">Код валюты</param>
        /// <param name="baseCurrency">Код базовой валюты</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        Task CreateFavoriteCurrencyAsync(string name, CurrencyCode currency, CurrencyCode baseCurrency, CancellationToken cancellationToken);

        /// <summary>
        /// Изменить избранный курс валюты
        /// </summary>
        /// <param name="searchName">Назание для поиска</param>
        /// <param name="newName">Новое название</param>
        /// <param name="currency">Код валюты</param>
        /// <param name="baseCurrency">Код базовой валюты</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        Task EditFavoriteCurrencyAsync(string searchName, string newName, CurrencyCode currency, CurrencyCode baseCurrency, CancellationToken cancellationToken);

        /// <summary>
        /// Удалить избранный курс валюты по названию
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        Task DeleteFavoriteCurrencyAsync(string name, CancellationToken cancellationToken);
    }
}
