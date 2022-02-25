using Microsoft.EntityFrameworkCore;
using ApiForSale.Data.Configurations;
using ApiForSale.Models;



namespace ApiForSale.Data
{
    public class Database: DbContext
    {
        public Database(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }
        public DbSet<Properties> Properties { get; set; }
        public DbSet<Seen> Seen { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Properties>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(p => p.UserId);
            modelBuilder.ApplyConfiguration(new PropertiesConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfigurations());
        }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.ApplyConfiguration(new UserConfigurations());
        //}

    }
}


