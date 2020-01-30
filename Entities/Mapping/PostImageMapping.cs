using Entities.Core;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Mapping
{
   
    public class PostImageMapping  : CoreMapping<PostImage>
    {
        public override void Configure(EntityTypeBuilder<PostImage> builder)
        {
            builder.ToTable("Resimler");

            builder.Property(map => map.ImageUrl).IsRequired(true).HasMaxLength(100).HasColumnName("ResimYolu");
            builder.Property(map => map.IsMain).IsRequired(true);
            builder.Property(map => map.PostId).HasColumnType("Guid").IsRequired(true);

            builder.HasOne(map => map.Post)        // bir resmin, bir post'u olur
                .WithMany(map => map.Images)       // bir post'un birden fazla resmi olur
                .HasForeignKey(map => map.PostId)  // ikincil anahtar tanımlaması
                .OnDelete(DeleteBehavior.Cascade); // db üzerinden bir post silinirse, o posta ait tüm resimlerde silinir.

            base.Configure(builder);
        }
    }
}
