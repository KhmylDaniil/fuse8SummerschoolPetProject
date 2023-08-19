using InternalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InternalApi.Interfaces
{
    /// <summary>
    /// контекст базы данных
    /// </summary>
    public interface IAppDbContext
    {
        /// <summary>
        /// Данные о курсах валюты на дату
        /// </summary>
        DbSet<CurrenciesOnDate> CurrenciesOnDates { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
