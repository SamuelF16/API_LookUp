using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_LookUp.Models;
using Microsoft.EntityFrameworkCore;

namespace API_LookUp.Repository
{
    public class AppDbContext : DbContext   /*MUDAR MUITA COISA PELO BANCO SER MYSQL, Talvez não*/
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Produtos> Produtos { get; set; }
        public DbSet<ComprasUsuario> ComprasUsuario { get; set; }
        public DbSet<Itens> Itens { get; set; }
        public DbSet<EnderecoUsuario> EnderecoUsuario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ComprasUsuario>(entity =>
            {
                entity.ToTable("compras_usuario");
                
                entity.HasKey(k => k.IdComprasUsuario);
                

                entity.HasOne(f => f.Usuario)
                    .WithMany(u => u.ComprasUsuarios)
                    .HasForeignKey(i => i.IdUsuario)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            
            modelBuilder.Entity<EnderecoUsuario>(entity =>
            {
                entity.ToTable("endereco_usuario");
                
                entity.HasKey(k => k.IdEnderecoUsuario);
                

                entity.HasOne(f => f.Usuario)
                    .WithMany(u => u.EnderecoUsuarios)
                    .HasForeignKey(i => i.IdUsuario)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Itens>(entity =>
            {
                entity.ToTable("itens");
                
                entity.HasKey(k => k.IdItens);
                

                entity.HasOne(f => f.Produto)
                    .WithMany(u => u.Itens)
                    .HasForeignKey(c => c.IdProduto)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(f => f.CompraUsuario)
                    .WithMany(u => u.Itens)
                    .HasForeignKey(i => i.IdComprasUsuario)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Produtos>()
                .Property(p => p.Preco)
                .HasPrecision(8, 2);

                modelBuilder.Entity<Produtos>()
                    .Property(p => p.Modelo)
                    .HasConversion<string>()  // Isso faz o EF converter o enum C# para string
                    .HasColumnType("enum('Feminino','Masculino','Unisex','Infantil')");

            modelBuilder.Entity<ComprasUsuario>()
                .Property(c => c.ValorTotal)
                .HasPrecision(10, 2);

                modelBuilder.Entity<ComprasUsuario>()
                    .Property(c => c.StatusCompra)
                    .HasConversion<string>()  // Isso faz o EF converter o enum C# para string
                    .HasColumnType("enum('Pendente','Pago','Cancelado')");

            modelBuilder.Entity<Itens>()
                .Property(i => i.PrecoUnitario)
                .HasPrecision(10, 2);
        }
    }
}