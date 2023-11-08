using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AppCajero.Models
{
    public partial class DbcajeroContext : DbContext
    {
        public DbcajeroContext()
        {
        }

        public DbcajeroContext(DbContextOptions<DbcajeroContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Operacion> Operaciones { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Configure the database connection here
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC6179E9EF");

                entity.Property(e => e.UserId).HasColumnName("UserID");
                entity.Property(e => e.Password).HasMaxLength(50);
                entity.Property(e => e.NroTarjeta).HasMaxLength(16);
                entity.Property(e => e.Username).HasMaxLength(10).IsRequired();
                entity.Property(e => e.Saldo);
                entity.Property(e => e.Bloqueado);
                entity.Property(e => e.Cant);
                entity.Property(e => e.FechaVenc);
            });



            modelBuilder.Entity<Operacion>(entity =>
            {
                entity.HasKey(e => e.OperacionID).HasName("PK__Operaciones__1788CCAC6179E9EF");

                entity.Property(e => e.OperacionID).HasColumnName("OperacionID");
                entity.Property(e => e.TipoOperacion).HasMaxLength(50).IsRequired();
                entity.Property(e => e.FechaOperacion).HasColumnType("datetime").IsRequired();
                entity.Property(e => e.CantidadDinero).IsRequired();
                entity.Property(e => e.UserID).IsRequired(); // Asegúrate de ajustar esto según tu base de datos

                // Propiedad de navegación
                entity.HasOne(d => d.User)
                      .WithMany(p => p.Operaciones)
                      .HasForeignKey(d => d.UserID)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_Operaciones_Users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
