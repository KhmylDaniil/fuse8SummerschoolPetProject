using InternalApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternalApi.Configurations
{
    /// <summary>
    /// Конфигурация для <see cref="ChangeCacheTask"/>
    /// </summary>
    public class ChangeCacheTaskConfiguration : IEntityTypeConfiguration<ChangeCacheTask>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<ChangeCacheTask> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CreationTime).IsRequired();
            builder.Property(x => x.NewBaseCurrency).IsRequired();
            builder.Property(x => x.CacheTaskStatus).IsRequired();
        }
    }
}
