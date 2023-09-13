using InternalApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternalApi.Configurations
{
    /// <summary>
    /// Конфигурация для <see cref="CurrenciesOnDate"/>
    /// </summary>
    public class CurrenciesOnDateConfiguration : IEntityTypeConfiguration<CurrenciesOnDate>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<CurrenciesOnDate> builder)
        {
            builder.HasKey(x => x.Date);

            builder.Property(x => x.Currencies).HasColumnType("jsonb");
        }
    }
}
