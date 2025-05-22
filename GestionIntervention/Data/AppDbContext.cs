using Microsoft.EntityFrameworkCore;

using GestionIntervention.Models;

namespace GestionIntervention.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Produit> Produits { get; set; }
        public DbSet<Technicien> Techniciens { get; set; }
        public DbSet<Intervention> Interventions { get; set; }
        public DbSet<Compte> Comptes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent API configurations
            modelBuilder.Entity<Intervention>()
                .HasOne(i => i.Client)
                .WithMany(c => c.Interventions)
                .HasForeignKey(i => i.CodeClt)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Intervention>()
                .HasOne(i => i.Produit)
                .WithMany(p => p.Interventions)
                .HasForeignKey(i => i.ReferencePd)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Intervention>()
                .HasOne(i => i.Technicien)
                .WithMany(t => t.Interventions)
                .HasForeignKey(i => i.CodeTech)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed initial data
            modelBuilder.Entity<Compte>().HasData(
                new Compte { Id = 1, Login = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), Role = "Admin" },
                new Compte { Id = 2, Login = "user", PasswordHash = BCrypt.Net.BCrypt.HashPassword("user123"), Role = "User" }
            );

            modelBuilder.Entity<Client>().HasData(
                new Client { CodeClt = "CLT1", Nom = "Dupont", Prenom = "Jean", Adresse = "123 Rue de Paris", CP = "75001" },
                new Client { CodeClt = "CLT2", Nom = "Martin", Prenom = "Sophie", Adresse = "456 Avenue de Lyon", CP = "69001" }
            );

            modelBuilder.Entity<Produit>().HasData(
                new Produit { ReferencePd = "PRD1", Designation = "Lave-linge", Prix = 499.99m },
                new Produit { ReferencePd = "PRD2", Designation = "Réfrigérateur", Prix = 799.99m }
            );

            modelBuilder.Entity<Technicien>().HasData(
                new Technicien { CodeTech = "TCH1", Nom = "Durand", Prenom = "Paul" },
                new Technicien { CodeTech = "TCH2", Nom = "Lefevre", Prenom = "Marie" }
            );

            modelBuilder.Entity<Intervention>().HasData(
                new Intervention { NumInterv = 1, DateInterv = DateTime.Now.AddDays(-5), CodeClt = "CLT1", ReferencePd = "PRD1", CodeTech = "TCH1", Statut = "Terminée" },
                new Intervention { NumInterv = 2, DateInterv = DateTime.Now.AddDays(-2), CodeClt = "CLT2", ReferencePd = "PRD2", CodeTech = "TCH2", Statut = "Planifiée" }
            );
        }
    }
}