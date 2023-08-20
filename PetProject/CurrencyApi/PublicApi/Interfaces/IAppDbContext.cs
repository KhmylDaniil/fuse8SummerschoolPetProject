using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Interfaces
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public interface IAppDbContext
    {
        /// <summary>
        /// Настройки приложения в базе данных
        /// </summary>
        DbSet<Settings> Settings { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
