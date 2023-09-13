using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InternalApi.Models.Entities;
using Fuse8_ByteMinds.SummerSchool.InternalApi;

namespace InternalApi.Configurations
{
    /// <summary>
    /// Конфигурация для <see cref="Settings"/>
    /// </summary>
    public class SettingsConfiguration : IEntityTypeConfiguration<Settings>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Settings> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.BaseCurrency).IsRequired();

            builder.HasData(new Settings
            {
                Id = -1,
                BaseCurrency = CurrencyCode.Usd
            });
        }
    }
}
