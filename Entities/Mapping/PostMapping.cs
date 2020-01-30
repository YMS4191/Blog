using Entities.Core;
using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Entities.Mapping
{
    public class PostMapping : CoreMapping<Post>
    {
        public override void Configure(EntityTypeBuilder<Post> builder)
        {
            // NOT : Dependency içerisinde yer alan paketleri önceden indiriniz.,

            builder.ToTable("Icerikler");

            // HasMaxLength(50) => tabloda kaç karakterlik yer alacak => nvarchar(50)
            // HasField("")     => property'nin sql üzerinde kolon adını belirler, eğer eklemezseniz default olarak property'nin adını yazar
            // IsRequired       => kolonun boş geçilip geçilemeyeceği, true / false değeri alır.

            builder.Property(map => map.Title).HasMaxLength(100).IsRequired(true).HasColumnName("Baslik");
            builder.Property(map => map.User).HasMaxLength(50).IsRequired(false);
            builder.Property(map => map.Content).HasMaxLength(300).IsRequired(true);
            builder.Property(map => map.ShowType).IsRequired(true);

            builder.HasMany(map => map.Images)  // bir paylaşımın birden fazla resmi vardır.
                .WithOne(map => map.Post)       // ilgili paylaşıma ait her bir resmin, bir paylaşımı olur.
                .HasForeignKey(map => map.PostId) // resim içerisinde post nesnesi ile haberleştiği foreign key property'si
                .IsRequired(true);

                
            base.Configure(builder);
        }
    }
}
