using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Core
{
    public class CoreMapping<T>
        : IEntityTypeConfiguration<T>  where T
        : CoreEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(map => map.Id);
            builder.Property(map => map.CreatedDate).IsRequired(true);
        }
    }
}
