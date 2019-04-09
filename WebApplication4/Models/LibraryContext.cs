using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApplication4.Models
{
    public partial class LibraryContext : DbContext
    {
        public LibraryContext()
        {
        }

        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Books> Books { get; set; }
        public virtual DbSet<UserBooks> UserBooks { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\TEW_SQLEXPRESS;Database=Library;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Books>(entity =>
            {
                entity.Property(e => e.Author)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<UserBooks>(entity =>
            {
                entity.Property(e => e.Date).HasColumnName("date");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Pwd)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(10);
            });
        }
    }
}
