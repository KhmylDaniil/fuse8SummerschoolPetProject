using Fuse8_ByteMinds.SummerSchool.PublicApi.Interfaces;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public class AppDbContext : DbContext, IAppDbContext
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="options">Опции</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// Настройки приложения в базе данных
        /// </summary>
        public DbSet<Settings> Settings { get; set; }

        /// <summary>
        /// Избранные валюты
        /// </summary>
        public DbSet<FavoriteCurrency> FavoriteCurrencies { get; set; }

        /// <summary>
        /// Метод для подтягивания конфигураций
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            modelBuilder.HasDefaultSchema("user");

            modelBuilder.HasPostgresExtension("citext");
        }
    }
}
