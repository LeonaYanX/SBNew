using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SB;

public partial class SwapBookDbContext : DbContext
{
    public SwapBookDbContext()
    {
    }

    public SwapBookDbContext(DbContextOptions<SwapBookDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Catalog> Catalogs { get; set; }

    public virtual DbSet<Favorit> Favorits { get; set; }

    public virtual DbSet<Galary> Galaries { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //C:\\Users\\User\\OneDrive\\Desktop\\SwapBook\\Db\\
        => optionsBuilder.UseSqlite("Data Source=SwapBookDB.db");
// 30aya izmenit putx zakinutx w sb
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("Book");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Author).HasColumnName("author");
            entity.Property(e => e.IdCatalog).HasColumnName("idCatalog");
            entity.Property(e => e.IdUser).HasColumnName("idUser");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Swap).HasColumnName("swap");
            entity.Property(e => e.Title).HasColumnName("title");

            entity.HasOne(d => d.IdCatalogNavigation).WithMany(p => p.Books).HasForeignKey(d => d.IdCatalog);

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Books).HasForeignKey(d => d.IdUser);
        });

        modelBuilder.Entity<Catalog>(entity =>
        {
            entity.ToTable("Catalog");

            entity.HasIndex(e => e.Value, "IX_Catalog_Value").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
        });

        modelBuilder.Entity<Favorit>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Favorit");

            entity.Property(e => e.IdBook).HasColumnName("idBook");
            entity.Property(e => e.IdUser).HasColumnName("idUser");

            entity.HasOne(d => d.IdBookNavigation).WithMany().HasForeignKey(d => d.IdBook);

            entity.HasOne(d => d.IdUserNavigation).WithMany().HasForeignKey(d => d.IdUser);
        });

        modelBuilder.Entity<Galary>(entity =>
        {
            entity.ToTable("Galary");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdBook).HasColumnName("idBook");

            entity.HasOne(d => d.IdBookNavigation).WithMany(p => p.Galaries).HasForeignKey(d => d.IdBook);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.City).HasColumnName("city");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.Photo).HasColumnName("photo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
