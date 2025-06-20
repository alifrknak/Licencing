using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class MasterContext : DbContext
{

    public MasterContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Feature> Features { get; set; }

    public DbSet<UserLicense> UserLicenses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("POSTGRESQL_URI"));
        }
    }

    //override protected void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    base.OnModelCreating(modelBuilder);
    //    modelBuilder.Entity<User>().Property(u => u.FirstName).IsRequired();
    //    modelBuilder.Entity<User>().Property(u => u.LastName).IsRequired();
    //    modelBuilder.Entity<User>().Property(u => u.Email).IsRequired();
    //    modelBuilder.Entity<Product>().Property(p => p.Name).IsRequired();
    //    modelBuilder.Entity<Product>().Property(p => p.Description).IsRequired();
    //    modelBuilder.Entity<Product>().Property(p => p.Price).IsRequired();
    //    modelBuilder.Entity<Feature>().Property(f => f.Name).IsRequired();
    //    modelBuilder.Entity<Feature>().Property(f => f.Description).IsRequired();

    //    modelBuilder.Entity<Feature>()
    //        .HasOne(f => f.Product)
    //        .WithMany(p => p.Features)
    //        .HasForeignKey(f => f.Id)
    //        .OnDelete(DeleteBehavior.Cascade);

    //    modelBuilder.Entity<User>()
    //        .HasIndex(u => u.Email)
    //        .IsUnique();
    //    modelBuilder.Entity<User>()
    //        .Property(u => u.UserType)
    //        .HasConversion<string>()
    //        .IsRequired();
    //    modelBuilder.Entity<User>()
    //        .Property(u => u.PasswordModel)
    //        .HasConversion(
    //            static v => System.Text.Json.JsonSerializer.Serialize(v, new JsonSerializerOptions()),
    //            v => System.Text.Json.JsonSerializer.Deserialize<PasswordModel>(v, new JsonSerializerOptions()))
    //        .IsRequired();
    //    modelBuilder.Entity<User>()
    //        .Property(u => u.CreatedBy)
    //        .IsRequired();
    //    modelBuilder.Entity<User>()
    //        .Property(u => u.CreatedOn)
    //        .HasDefaultValueSql("getutcdate()");
    //    modelBuilder.Entity<User>()
    //        .Property(u => u.LastModifiedOn)
    //        .IsRequired(false);

    //}

}
