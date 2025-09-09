using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WeatherApp.Models;

public partial class MydatabaseContext : IdentityDbContext<IdentityUser>
{
    public MydatabaseContext()
    {
    }

    public MydatabaseContext(DbContextOptions<MydatabaseContext> options)
        : base(options)
    {
    }

   

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Locate> Locates { get; set; }

   
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity
                .HasKey(e => e.Id);
            entity
                .ToTable("City");

            entity.Property(e => e.CityName)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

       

        modelBuilder.Entity<Locate>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("Locate");
        });
        base.OnModelCreating(modelBuilder); // これが必須

        modelBuilder.Entity<IdentityUserLogin<string>>()
            .HasKey(l => new { l.LoginProvider, l.ProviderKey });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
