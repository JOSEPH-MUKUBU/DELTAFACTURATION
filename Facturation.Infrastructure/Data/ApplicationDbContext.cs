using Facturation.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Facturation.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Client> Clients { get; set; }
    public DbSet<Produit> Produits { get; set; }
    public DbSet<Facture> Factures { get; set; }
    public DbSet<LigneFacture> LigneFactures { get; set; }
    public DbSet<Parametre> Parametres { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relations et contraintes Facture -> Client
        modelBuilder.Entity<Facture>()
            .HasOne(f => f.Client)
            .WithMany(c => c.Factures)
            .HasForeignKey(f => f.ClientId)
            .OnDelete(DeleteBehavior.Restrict); // Ne pas supprimer un client s'il a des factures

        // Relations Facture -> LigneFacture
        modelBuilder.Entity<Facture>()
            .HasMany(f => f.Lignes)
            .WithOne(l => l.Facture)
            .HasForeignKey(l => l.FactureId)
            .OnDelete(DeleteBehavior.Cascade); // Suppression en cascade des lignes si facture supprimée

        // Relations LigneFacture -> Produit
        modelBuilder.Entity<LigneFacture>()
            .HasOne(l => l.Produit)
            .WithMany(p => p.LigneFactures)
            .HasForeignKey(l => l.ProduitId)
            .OnDelete(DeleteBehavior.Restrict); // Ne pas supprimer un produit s'il est dans une facture

        // Index uniques
        modelBuilder.Entity<Client>()
            .HasIndex(c => c.MatriculeFiscal)
            .IsUnique();

        modelBuilder.Entity<Produit>()
            .HasIndex(p => p.Code)
            .IsUnique();

        modelBuilder.Entity<Facture>()
            .HasIndex(f => f.NumeroFacture)
            .IsUnique();

        modelBuilder.Entity<Parametre>()
            .HasIndex(p => p.Cle)
            .IsUnique();
            
        // Precisions décimales globales
        foreach (var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            // Les taux de TVA sont en 5,2, les autres en 10,3 ou 12,3 définis dans les annotations
            // Si aucune annotation n'est présente, la valeur par défaut pourrait être appliquée ici.
            // Mais nous avons utilisé [Column(TypeName=...)] dans nos entités.
        }
    }
}
