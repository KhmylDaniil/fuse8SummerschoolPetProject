using Fuse8_ByteMinds.SummerSchool.PublicApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="options">Опции</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// Метод для подтягивания конфигураций
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            modelBuilder.HasDefaultSchema("user");
        }
    }
}
