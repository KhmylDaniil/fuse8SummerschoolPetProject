using InternalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InternalApi.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<CurrenciesOnDate> CurrenciesOnDates { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
