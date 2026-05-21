using Facturation.Core.Entities;
using Facturation.Core.Interfaces;
using System.Text;

namespace Facturation.Services.Services;

public interface IExportService
{
    Task<byte[]> ExportComptableCsvAsync(DateTime debut, DateTime fin);
    Task<List<Produit>> GetProduitsEnAlerteStockAsync();
    Task<List<PrevisionVenteDto>> GetPrevisionVentesAsync(int moisAPrevoir = 3);
}

public class PrevisionVenteDto
{
    public string Mois { get; set; } = string.Empty;
    public decimal MontantPrevu { get; set; }
    public bool EstPrevision { get; set; }
}

public class ExportService : IExportService
{
    private readonly IFactureRepository _factureRepository;
    private readonly IRepository<Produit> _produitRepository;

    public ExportService(IFactureRepository factureRepository, IRepository<Produit> produitRepository)
    {
        _factureRepository = factureRepository;
        _produitRepository = produitRepository;
    }

    /// <summary>
    /// Exporter les écritures comptables au format CSV
    /// Format: Date;N°Facture;Compte;Libellé;Débit;Crédit
    /// </summary>
    public async Task<byte[]> ExportComptableCsvAsync(DateTime debut, DateTime fin)
    {
        var factures = await _factureRepository.GetFacturesByPeriodeAsync(debut, fin);

        var sb = new StringBuilder();
        sb.AppendLine("Date;N°Facture;Compte;Libellé;Débit;Crédit");

        foreach (var facture in factures.OrderBy(f => f.DateFacture))
        {
            string date = facture.DateFacture.ToString("dd/MM/yyyy");
            string num = facture.NumeroFacture;
            string client = facture.Client?.Nom ?? "Client";

            // Écriture Débit Client (411)
            sb.AppendLine($"{date};{num};411000;{client} - Facture {num};{facture.MontantTotal:F3};");

            // Écriture Crédit Vente HT (701)
            sb.AppendLine($"{date};{num};701000;Ventes de marchandises;; {facture.TotalHT:F3}");

            // Écriture Crédit TVA collectée par taux
            var tvaGroupes = facture.Lignes.GroupBy(l => l.TauxTVA);
            foreach (var groupe in tvaGroupes)
            {
                decimal tvaGroupe = groupe.Sum(l => l.MontantTVA);
                string compteTva = groupe.Key switch
                {
                    7m => "436710",
                    13m => "436713",
                    19m => "436719",
                    _ => $"4367{groupe.Key:00}"
                };
                sb.AppendLine($"{date};{num};{compteTva};TVA collectée {groupe.Key}%;;{tvaGroupe:F3}");
            }

            // Écriture Crédit Timbre fiscal (447)
            if (facture.TimbreFiscal > 0)
            {
                sb.AppendLine($"{date};{num};447000;Timbre fiscal;;{facture.TimbreFiscal:F3}");
            }
        }

        return Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray();
    }

    /// <summary>
    /// Retourner la liste des produits dont le stock est sous le seuil minimal
    /// </summary>
    public async Task<List<Produit>> GetProduitsEnAlerteStockAsync()
    {
        // Récupérer tous les produits actifs puis filtrer en mémoire
        // (évite un bug EF Core quand StockActuel <= SeuilMinimal est évalué côté serveur)
        var tousLesProduits = await _produitRepository.FindAsync(p => p.EstActif);
        return tousLesProduits
            .Where(p => p.StockActuel <= p.SeuilMinimal)
            .OrderBy(p => p.StockActuel)
            .ToList();
    }

    /// <summary>
    /// Prévisions de ventes basées sur une régression linéaire simple
    /// </summary>
    public async Task<List<PrevisionVenteDto>> GetPrevisionVentesAsync(int moisAPrevoir = 3)
    {
        // Récupérer les CA des 12 derniers mois
        var debut = new DateTime(DateTime.Today.Year - 1, DateTime.Today.Month, 1);
        var fin = DateTime.Today;
        var factures = await _factureRepository.GetFacturesByPeriodeAsync(debut, fin);

        var caMensuel = factures
            .GroupBy(f => new { f.DateFacture.Year, f.DateFacture.Month })
            .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
            .Select((g, index) => new
            {
                Index = index,
                Mois = $"{g.Key.Year}-{g.Key.Month:D2}",
                Total = g.Sum(f => f.TotalHT)
            })
            .ToList();

        var result = new List<PrevisionVenteDto>();

        // Ajouter l'historique
        foreach (var mois in caMensuel)
        {
            result.Add(new PrevisionVenteDto
            {
                Mois = mois.Mois,
                MontantPrevu = mois.Total,
                EstPrevision = false
            });
        }

        // Régression linéaire simple: y = a*x + b
        if (caMensuel.Count >= 2)
        {
            int n = caMensuel.Count;
            double sumX = caMensuel.Sum(c => (double)c.Index);
            double sumY = caMensuel.Sum(c => (double)c.Total);
            double sumXY = caMensuel.Sum(c => (double)c.Index * (double)c.Total);
            double sumX2 = caMensuel.Sum(c => (double)c.Index * (double)c.Index);

            double a = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
            double b = (sumY - a * sumX) / n;

            for (int i = 1; i <= moisAPrevoir; i++)
            {
                double prediction = a * (n - 1 + i) + b;
                if (prediction < 0) prediction = 0;

                var futureDate = DateTime.Today.AddMonths(i);
                result.Add(new PrevisionVenteDto
                {
                    Mois = $"{futureDate.Year}-{futureDate.Month:D2}",
                    MontantPrevu = (decimal)prediction,
                    EstPrevision = true
                });
            }
        }

        return result;
    }
}
