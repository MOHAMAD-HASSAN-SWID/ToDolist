using Microsoft.EntityFrameworkCore;
using to_do_list.Domain.Entities;

namespace to_do_list.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Dolist> Dolists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.UserName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Permission).IsRequired();
                entity.Property(u => u.DateRegistered).IsRequired();
            });

            modelBuilder.Entity<Dolist>(entity =>
            {
                entity.ToTable("Dolists");
                entity.HasKey(d => d.ListID);

                entity.Property(d => d.ListTitle).IsRequired().HasMaxLength(200);
                entity.Property(d => d.ListBody).HasMaxLength(1000);
                entity.Property(d => d.Completed).IsRequired();
                entity.Property(d => d.Category).HasMaxLength(100);
                entity.Property(d => d.Priority).IsRequired();

                entity.HasOne(d => d.user)
                      .WithMany(u => u.Dolists)
                      .HasForeignKey(d => d.UserID)
                      .OnDelete(DeleteBehavior.Cascade); 
            });

        }
    }



}
