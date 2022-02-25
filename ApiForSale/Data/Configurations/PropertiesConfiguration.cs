using Microsoft.EntityFrameworkCore;
using ApiForSale.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;





namespace ApiForSale.Data.Configurations
{
    public class PropertiesConfiguration : IEntityTypeConfiguration<Properties>
    {
        public void Configure(EntityTypeBuilder<Properties> builder)
        {
            builder.Property(g => g.Id).UseIdentityColumn<int>();
            builder.Property(g => g.Price).IsRequired();
            builder.Property(g => g.City).IsRequired();
            builder.Property(g => g.Parkings).IsRequired();
            builder.Property(g => g.KitchenAndLiving).HasDefaultValue(false);
            builder.Property(g => g.HasPool).HasDefaultValue(false);
            builder.Property(g => g.Details).HasMaxLength(400);
            builder.Property(g => g.Title).IsRequired().HasMaxLength(50);
        }
    }
}



