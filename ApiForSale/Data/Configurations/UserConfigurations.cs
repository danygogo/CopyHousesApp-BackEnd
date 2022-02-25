using Microsoft.EntityFrameworkCore;
using ApiForSale.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ApiForSale.Data.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(g => g.Id).UseIdentityColumn<int>();
            builder.Property(g => g.Name).IsRequired().HasMaxLength(100);
            builder.Property(g => g.Mail).IsRequired().HasMaxLength(80);
            builder.Property(g => g.Phone).IsRequired().HasMaxLength(9);
            builder.Property(g => g.Password).IsRequired().HasMaxLength(15);
        }
    }
}


