using Facturation.Core.Entities;
using Facturation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Facturation.Web;

/// <summary>
/// Peuple la base de données avec des données de démonstration réalistes
/// si la base est vide ou presque vide.
/// </summary>
public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Ne pas re-seeder si des données existent déjà
        if (await db.Clients.CountAsync() >= 5)
            return;

        // ─── CLIENTS ────────────────────────────────────────────────────────────
        var clients = new List<Client>
        {
            new() { Nom = "Alpha Tech SARL",     MatriculeFiscal = "1234567/A/M/000", Adresse = "12 Rue de la Liberté, Tunis 1001",    Telephone = "71 234 567", Email = "contact@alphatech.tn" },
            new() { Nom = "Bâtiments & Travaux", MatriculeFiscal = "2345678/B/M/000", Adresse = "45 Avenue Habib Bourguiba, Sfax 3000", Telephone = "74 345 678", Email = "info@btp-sfax.tn" },
            new() { Nom = "Delta Import Export",  MatriculeFiscal = "3456789/C/M/000", Adresse = "8 Route de Bizerte, Ariana 2080",      Telephone = "71 456 789", Email = "delta@importexport.tn" },
            new() { Nom = "Épicerie Centrale Ben Salah", MatriculeFiscal = "4567890/D/P/000", Adresse = "23 Souk El Blat, Monastir 5000", Telephone = "73 567 890", Email = "epicerie.bensalah@gmail.com" },
            new() { Nom = "Pharma Distribution",  MatriculeFiscal = "5678901/E/M/000", Adresse = "Route de La Marsa, La Marsa 2070",     Telephone = "71 678 901", Email = "pharma.dist@tn.net" },
            new() { Nom = "Horizon Consulting",   MatriculeFiscal = "6789012/F/M/000", Adresse = "Tour Montplaisir, Tunis 1073",          Telephone = "71 789 012", Email = "contact@horizon-consulting.tn" },
            new() { Nom = "Textiles du Sahel",    MatriculeFiscal = "7890123/G/M/000", Adresse = "Zone Industrielle, Sousse 4000",        Telephone = "73 890 123", Email = "textiles.sahel@gmail.com" },
            new() { Nom = "Agro-Bio SARL",        MatriculeFiscal = "8901234/H/M/000", Adresse = "Route de Nabeul, Hammamet 8050",        Telephone = "72 901 234", Email = "agrobio@tn.com" },
            new() { Nom = "Société El Amal", MatriculeFiscal = "9000001/I/M/000", Adresse = "10 Avenue de Carthage, Tunis 1000", Telephone = "71 210 001", Email = "contact@elamal.tn" },
            new() { Nom = "Groupe Carthage", MatriculeFiscal = "9000002/J/M/000", Adresse = "5 Rue de la République, Sfax 3000", Telephone = "74 210 002", Email = "contact@carthage.tn" },
            new() { Nom = "Maghreb Distribution", MatriculeFiscal = "9000003/K/M/000", Adresse = "18 Avenue Habib Bourguiba, Sousse 4000", Telephone = "73 210 003", Email = "contact@maghreb.tn" },
            new() { Nom = "Industries du Sahel", MatriculeFiscal = "9000004/L/M/000", Adresse = "Zone Industrielle, Monastir 5000", Telephone = "73 210 004", Email = "contact@indusahel.tn" },
            new() { Nom = "Zitouna Services", MatriculeFiscal = "9000005/M/M/000", Adresse = "22 Rue de Marseille, Tunis 1001", Telephone = "71 210 005", Email = "contact@zitouna.tn" },
            new() { Nom = "Atlas Technologies", MatriculeFiscal = "9000006/N/M/000", Adresse = "Technopole El Ghazala, Ariana 2088", Telephone = "71 210 006", Email = "contact@atlas-tech.tn" },
            new() { Nom = "Mediterranee Commerce", MatriculeFiscal = "9000007/O/M/000", Adresse = "Port de Bizerte, Bizerte 7000", Telephone = "72 210 007", Email = "contact@medcommerce.tn" },
            new() { Nom = "Ennajah SARL", MatriculeFiscal = "9000008/P/M/000", Adresse = "15 Rue Principale, Kairouan 3100", Telephone = "77 210 008", Email = "contact@ennajah.tn" },
            new() { Nom = "El Baraka Trading", MatriculeFiscal = "9000009/Q/M/000", Adresse = "7 Avenue Farhat Hached, Gabès 6000", Telephone = "75 210 009", Email = "contact@elbaraka.tn" },
            new() { Nom = "Compagnie du Centre", MatriculeFiscal = "9000010/R/M/000", Adresse = "12 Rue Ibn Khaldoun, Sidi Bouzid 9100", Telephone = "76 210 010", Email = "contact@centre.tn" },
        };

        db.Clients.AddRange(clients);
        await db.SaveChangesAsync();

        // ─── PRODUITS ────────────────────────────────────────────────────────────
        var produits = new List<Produit>
        {
            new() { Code = "INF-001", Libelle = "Développement Application Web",    PrixUnitaireHT = 1500.000m, TauxTVA = 19m, EstActif = true,  StockActuel = 100, SeuilMinimal = 5 },
            new() { Code = "INF-002", Libelle = "Maintenance Logicielle (mois)",     PrixUnitaireHT = 350.000m,  TauxTVA = 19m, EstActif = true,  StockActuel = 50,  SeuilMinimal = 5 },
            new() { Code = "INF-003", Libelle = "Formation Informatique (jour)",     PrixUnitaireHT = 250.000m,  TauxTVA = 19m, EstActif = true,  StockActuel = 30,  SeuilMinimal = 3 },
            new() { Code = "MAT-001", Libelle = "Ordinateur Portable Dell",          PrixUnitaireHT = 2200.000m, TauxTVA = 19m, EstActif = true,  StockActuel = 8,   SeuilMinimal = 5 },
            new() { Code = "MAT-002", Libelle = "Switch Réseau 24 ports",            PrixUnitaireHT = 580.000m,  TauxTVA = 19m, EstActif = true,  StockActuel = 3,   SeuilMinimal = 5 },
            new() { Code = "MAT-003", Libelle = "Imprimante Laser A4",               PrixUnitaireHT = 450.000m,  TauxTVA = 19m, EstActif = true,  StockActuel = 6,   SeuilMinimal = 3 },
            new() { Code = "CON-001", Libelle = "Audit Système (demi-journée)",      PrixUnitaireHT = 400.000m,  TauxTVA = 7m,  EstActif = true,  StockActuel = 99,  SeuilMinimal = 2 },
            new() { Code = "CON-002", Libelle = "Conseil Stratégique (heure)",       PrixUnitaireHT = 150.000m,  TauxTVA = 7m,  EstActif = true,  StockActuel = 99,  SeuilMinimal = 2 },
            new() { Code = "LOG-001", Libelle = "Licence Antivirus (an)",            PrixUnitaireHT = 85.000m,   TauxTVA = 19m, EstActif = true,  StockActuel = 20,  SeuilMinimal = 5 },
            new() { Code = "LOG-002", Libelle = "Licence Office 365 (an)",           PrixUnitaireHT = 120.000m,  TauxTVA = 19m, EstActif = true,  StockActuel = 15,  SeuilMinimal = 5 },
            new() { Code = "MAT-004", Libelle = "Câblage Réseau (mètre)",            PrixUnitaireHT = 2.500m,    TauxTVA = 19m, EstActif = true,  StockActuel = 2,   SeuilMinimal = 10 },
            new() { Code = "INF-004", Libelle = "Hébergement Serveur (mois)",        PrixUnitaireHT = 90.000m,   TauxTVA = 19m, EstActif = false, StockActuel = 0,   SeuilMinimal = 0 },
        };

        db.Produits.AddRange(produits);
        await db.SaveChangesAsync();

        // Récupérer les IDs après insertion
        var pById = produits.ToDictionary(p => p.Code, p => p);
        var cByIdx = clients.ToList();

        // ─── FACTURES (sur les 12 derniers mois) ────────────────────────────────
        decimal timbre = 1.000m;
        var factures = new List<Facture>();

        // Helper local pour créer une facture complète
        static Facture BuildFacture(string num, DateTime date, Client client, decimal timbre,
            List<(Produit prod, decimal qte)> lignes)
        {
            var f = new Facture
            {
                NumeroFacture = num,
                DateFacture   = date,
                ClientId      = client.Id,
                TimbreFiscal  = timbre,
                Statut        = "Validée"
            };

            foreach (var (prod, qte) in lignes)
            {
                var montantHT  = prod.PrixUnitaireHT * qte;
                var montantTVA = montantHT * prod.TauxTVA / 100m;
                f.Lignes.Add(new LigneFacture
                {
                    ProduitId      = prod.Id,
                    Quantite       = qte,
                    PrixUnitaireHT = prod.PrixUnitaireHT,
                    TauxTVA        = prod.TauxTVA,
                    MontantHT      = montantHT,
                    MontantTVA     = montantTVA
                });
            }

            f.TotalHT      = f.Lignes.Sum(l => l.MontantHT);
            f.TotalTVA     = f.Lignes.Sum(l => l.MontantTVA);
            f.TotalTTC     = f.TotalHT + f.TotalTVA;
            f.MontantTotal = f.TotalTTC + timbre;
            return f;
        }

        var today = DateTime.Today;

        // Mois -11 → aujourd'hui (données pour graphe d'évolution)
        var seedData = new[]
        {
            // Mois -11
            (num:"F-2024-001", mois:-11, cli:0, lignes: new[]{(pById["INF-001"],1m),(pById["LOG-001"],3m)}),
            (num:"F-2024-002", mois:-11, cli:2, lignes: new[]{(pById["MAT-001"],2m),(pById["MAT-002"],1m)}),
            // Mois -10
            (num:"F-2024-003", mois:-10, cli:1, lignes: new[]{(pById["CON-001"],4m),(pById["CON-002"],8m)}),
            (num:"F-2024-004", mois:-10, cli:3, lignes: new[]{(pById["INF-002"],3m)}),
            // Mois -9
            (num:"F-2024-005", mois:-9, cli:4, lignes: new[]{(pById["INF-001"],1m),(pById["INF-002"],2m),(pById["LOG-001"],5m)}),
            (num:"F-2024-006", mois:-9, cli:0, lignes: new[]{(pById["MAT-003"],2m),(pById["MAT-004"],100m)}),
            // Mois -8
            (num:"F-2024-007", mois:-8, cli:5, lignes: new[]{(pById["INF-001"],2m),(pById["CON-002"],10m)}),
            (num:"F-2024-008", mois:-8, cli:1, lignes: new[]{(pById["LOG-002"],10m),(pById["LOG-001"],5m)}),
            // Mois -7
            (num:"F-2024-009", mois:-7, cli:2, lignes: new[]{(pById["INF-003"],3m),(pById["CON-001"],2m)}),
            (num:"F-2024-010", mois:-7, cli:6, lignes: new[]{(pById["MAT-001"],1m),(pById["MAT-002"],2m)}),
            // Mois -6
            (num:"F-2025-001", mois:-6, cli:0, lignes: new[]{(pById["INF-001"],1m),(pById["INF-002"],6m)}),
            (num:"F-2025-002", mois:-6, cli:7, lignes: new[]{(pById["CON-001"],5m),(pById["CON-002"],12m)}),
            (num:"F-2025-003", mois:-6, cli:3, lignes: new[]{(pById["MAT-003"],1m),(pById["LOG-001"],3m)}),
            // Mois -5
            (num:"F-2025-004", mois:-5, cli:1, lignes: new[]{(pById["INF-001"],1m),(pById["MAT-001"],3m)}),
            (num:"F-2025-005", mois:-5, cli:4, lignes: new[]{(pById["INF-002"],4m),(pById["LOG-002"],8m)}),
            // Mois -4
            (num:"F-2025-006", mois:-4, cli:5, lignes: new[]{(pById["INF-003"],6m),(pById["CON-002"],15m)}),
            (num:"F-2025-007", mois:-4, cli:6, lignes: new[]{(pById["INF-001"],2m),(pById["MAT-002"],1m)}),
            (num:"F-2025-008", mois:-4, cli:0, lignes: new[]{(pById["LOG-001"],10m),(pById["LOG-002"],5m)}),
            // Mois -3
            (num:"F-2025-009", mois:-3, cli:2, lignes: new[]{(pById["INF-001"],3m),(pById["CON-001"],3m)}),
            (num:"F-2025-010", mois:-3, cli:7, lignes: new[]{(pById["MAT-001"],2m),(pById["MAT-003"],2m)}),
            // Mois -2
            (num:"F-2025-011", mois:-2, cli:3, lignes: new[]{(pById["INF-002"],5m),(pById["INF-003"],4m)}),
            (num:"F-2025-012", mois:-2, cli:1, lignes: new[]{(pById["INF-001"],1m),(pById["LOG-002"],12m),(pById["CON-002"],8m)}),
            (num:"F-2025-013", mois:-2, cli:5, lignes: new[]{(pById["MAT-002"],3m),(pById["MAT-004"],200m)}),
            // Mois -1
            (num:"F-2025-014", mois:-1, cli:4, lignes: new[]{(pById["INF-001"],2m),(pById["INF-002"],3m)}),
            (num:"F-2025-015", mois:-1, cli:6, lignes: new[]{(pById["CON-001"],8m),(pById["CON-002"],20m)}),
            (num:"F-2025-016", mois:-1, cli:0, lignes: new[]{(pById["MAT-001"],4m),(pById["LOG-001"],8m)}),
            // Mois courant
            (num:"F-2025-017", mois:0, cli:7, lignes: new[]{(pById["INF-001"],1m),(pById["INF-003"],2m),(pById["LOG-002"],3m)}),
            (num:"F-2025-018", mois:0, cli:2, lignes: new[]{(pById["CON-002"],10m),(pById["MAT-003"],1m)}),
            (num:"F-2025-019", mois:0, cli:1, lignes: new[]{(pById["INF-002"],2m),(pById["INF-001"],1m)}),
        };

        foreach (var (num, mois, cliIdx, lignesData) in seedData)
        {
            var date = today.AddMonths(mois);
            date = new DateTime(date.Year, date.Month, Math.Min(15, DateTime.DaysInMonth(date.Year, date.Month)));

            var lignes = lignesData.Select(l => (l.Item1, l.Item2)).ToList();
            var facture = BuildFacture(num, date, cByIdx[cliIdx], timbre, lignes);
            db.Factures.Add(facture);
        }

        await db.SaveChangesAsync();
    }
}
