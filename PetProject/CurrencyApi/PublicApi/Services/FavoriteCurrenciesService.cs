using Fuse8_ByteMinds.SummerSchool.PublicApi.Interfaces;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Entities;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Services
{
    /// <summary>
    /// Методы работы с избранными курсами валют
    /// </summary>
    public class FavoriteCurrenciesService: IFavoriteCurrenciesService
    {
        private readonly AppDbContext _appDbContext;

        /// <summary>
        /// Конструктор для <see cref="FavoriteCurrenciesService"/>
        /// </summary>
        /// <param name="appDbContext">Контекст базы данных</param>
        public FavoriteCurrenciesService(AppDbContext appDbContext) => _appDbContext = appDbContext;

        /// <summary>
        /// Получить все избранные курсы валют
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>избранные курсы валют</returns>
        public async Task<GetFavoredCurrencyResponse[]> GetFavoriteCurrenciesAsync(CancellationToken cancellationToken)
        {
            var result = await _appDbContext.FavoriteCurrencies.AsNoTracking().ToArrayAsync(cancellationToken);

            return result.Select(fc => new GetFavoredCurrencyResponse(fc)).ToArray();
        }

        /// <summary>
        /// Получить избранный курс валюты по названию
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Избранный курс валюты</returns>
        public async Task<GetFavoredCurrencyResponse> GetFavoriteCurrencyAsync(string name, CancellationToken cancellationToken)
        {
            var result = await _appDbContext.FavoriteCurrencies.AsNoTracking().FirstOrDefaultAsync(fc => fc.Name == name, cancellationToken)
                ?? throw new ArgumentException(Exceptions.ExceptionMessages.FavCurNotFound);

            return new GetFavoredCurrencyResponse(result);
        }
            
        /// <summary>
        /// Создать избранный курс валюты
        /// </summary>
        /// <param name="name">Навзание</param>
        /// <param name="currency">Код валюты</param>
        /// <param name="baseCurrency">Код базовой валюты</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public async Task CreateFavoriteCurrencyAsync(string name, CurrencyCode currency, CurrencyCode baseCurrency, CancellationToken cancellationToken)
        {
            await CheckRequestAsync(name, currency, baseCurrency, cancellationToken);

            FavoriteCurrency newFavCur = new(name, currency, baseCurrency);

            _appDbContext.FavoriteCurrencies.Add(newFavCur);
            await _appDbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Изменить избранный курс валюты
        /// </summary>
        /// <param name="searchName">Назание для поиска</param>
        /// <param name="newName">Новое название</param>
        /// <param name="currency">Код валюты</param>
        /// <param name="baseCurrency">Код базовой валюты</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public async Task EditFavoriteCurrencyAsync(string searchName, string newName, CurrencyCode currency, CurrencyCode baseCurrency, CancellationToken cancellationToken)
        {
            var existingFavCur = await _appDbContext.FavoriteCurrencies.FirstOrDefaultAsync(fc => fc.Name == searchName, cancellationToken)
                ?? throw new ArgumentException(Exceptions.ExceptionMessages.FavCurNotFound);

            await CheckRequestAsync(newName, currency, baseCurrency, cancellationToken);

            existingFavCur.ChangeFavCur(newName, currency, baseCurrency);

            await _appDbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Удалить избранный курс валюты по названию
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public async Task DeleteFavoriteCurrencyAsync(string name, CancellationToken cancellationToken)
        {
            var favCurToDelete = await _appDbContext.FavoriteCurrencies.FirstOrDefaultAsync(fc => fc.Name == name, cancellationToken)
                ?? throw new ArgumentException(Exceptions.ExceptionMessages.FavCurNotFound);

            _appDbContext.FavoriteCurrencies.Remove(favCurToDelete);
            await _appDbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Проверка запроса
        /// </summary>
        /// <param name="name">Навзание</param>
        /// <param name="currency">Код валюты</param>
        /// <param name="baseCurrency">Код базовой валюты</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        private async Task CheckRequestAsync(string name, CurrencyCode currency, CurrencyCode baseCurrency, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException(Exceptions.ExceptionMessages.NameCantBeNull);
            
            if (await _appDbContext.FavoriteCurrencies.AnyAsync(fc => fc.Name == name
                || (fc.Currency == currency && fc.BaseCurrency == baseCurrency), cancellationToken))
                throw new ArgumentException(Exceptions.ExceptionMessages.NotUniqueFavCur);
        }
    }
}
