using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Configurations
{
    /// <summary>
    /// Конфигурация для <see cref="FavoriteCurrency"/>
    /// </summary>
    public class FavoriteCurrencyConfiguration : IEntityTypeConfiguration<FavoriteCurrency>
    {
        public void Configure(EntityTypeBuilder<FavoriteCurrency> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Name).IsUnique().UseCollation("ignore_case_collation");

            builder.Property(x => x.Currency).IsRequired();
            builder.Property(x => x.BaseCurrency).IsRequired();

            builder.HasIndex(x => new { x.Currency, x.BaseCurrency }).IsUnique();
        }
    }
}
