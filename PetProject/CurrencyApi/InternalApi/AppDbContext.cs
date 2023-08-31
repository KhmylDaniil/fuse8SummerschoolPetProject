using InternalApi.Interfaces;
using InternalApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternalApi
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public class AppDbContext : DbContext, IAppDbContext
    {
        /// <summary>
        /// Конструктор для <see cref="AppDbContext"/>
        /// </summary>
        /// <param name="options">Опции</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// Данные о курсах валюты на дату
        /// </summary>
        public DbSet<CurrenciesOnDate> CurrenciesOnDates { get; set; }

        /// <summary>
        /// Задачи изменения базовой валюты кэша
        /// </summary>
        public DbSet<ChangeCacheTask> ChangeCacheTasks { get; set; }

        /// <summary>
        /// Настройки приложения в базе данных
        /// </summary>
        public DbSet<Settings> Settings { get; set; }

        /// <summary>
        /// Метод для подтягивания конфигураций
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            modelBuilder.HasDefaultSchema("cur");
        }
    }
}
