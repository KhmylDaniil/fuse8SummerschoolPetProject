using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Configurations
{
    /// <summary>
    /// Конфигурация для <see cref="Settings"/>
    /// </summary>
    public class SettingsConfiguration : IEntityTypeConfiguration<Settings>
    {
        public void Configure(EntityTypeBuilder<Settings> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.DefaultCurrency).IsRequired();
            builder.Property(x => x.CurrencyRoundCount).IsRequired();

            builder.HasData(new Settings
            {
                Id = -1,
                CurrencyRoundCount = 2,
                DefaultCurrency = CurrencyCode.Usd
            });
        }
    }
}
