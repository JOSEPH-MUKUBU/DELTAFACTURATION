using Facturation.Core.Interfaces;

namespace Facturation.Services.Services;

public interface IAnalyseService
{
    Task<FiscalDashboardDto> GetFiscalDashboardDataAsync(DateTime debut, DateTime fin);
    Task<VentesDashboardDto> GetVentesDashboardDataAsync(DateTime debut, DateTime fin);
}

public class FiscalDashboardDto
{
    public decimal TvaTotaleCollectee { get; set; }
    public decimal MontantTotalTimbre { get; set; }
    public Dictionary<decimal, decimal> TvaParTaux { get; set; } = new Dictionary<decimal, decimal>();
    public Dictionary<string, decimal> EvolutionTvaMensuelle { get; set; } = new Dictionary<string, decimal>();
}

public class VentesDashboardDto
{
    public decimal ChiffreAffairesTotalHT { get; set; }
    public decimal ChiffreAffairesTotalTTC { get; set; }
    public Dictionary<string, decimal> CA_ParPeriode { get; set; } = new Dictionary<string, decimal>();
    public List<TopClientDto> TopClients { get; set; } = new List<TopClientDto>();
    public List<TopProduitDto> TopProduits { get; set; } = new List<TopProduitDto>();
    public Dictionary<string, decimal> EvolutionCA { get; set; } = new Dictionary<string, decimal>();
}

public class TopClientDto
{
    public string Nom { get; set; } = string.Empty;
    public decimal TotalAchatHT { get; set; }
}

public class TopProduitDto
{
    public string Libelle { get; set; } = string.Empty;
    public decimal QuantiteVendue { get; set; }
    public decimal ValeurTotaleHT { get; set; }
}

public class AnalyseService : IAnalyseService
{
    private readonly IFactureRepository _factureRepository;

    public AnalyseService(IFactureRepository factureRepository)
    {
        _factureRepository = factureRepository;
    }

    public async Task<FiscalDashboardDto> GetFiscalDashboardDataAsync(DateTime debut, DateTime fin)
    {
        var factures = await _factureRepository.GetFacturesByPeriodeAsync(debut, fin);
        
        var dto = new FiscalDashboardDto
        {
            TvaTotaleCollectee = factures.Sum(f => f.TotalTVA),
            MontantTotalTimbre = factures.Sum(f => f.TimbreFiscal)
        };

        // TVA par taux
        var lignes = factures.SelectMany(f => f.Lignes);
        dto.TvaParTaux = lignes
            .GroupBy(l => l.TauxTVA)
            .ToDictionary(g => g.Key, g => g.Sum(l => l.MontantTVA));

        // Evolution Mensuelle
        dto.EvolutionTvaMensuelle = factures
            .GroupBy(f => f.DateFacture.ToString("yyyy-MM"))
            .OrderBy(g => g.Key)
            .ToDictionary(g => g.Key, g => g.Sum(f => f.TotalTVA));

        return dto;
    }

    public async Task<VentesDashboardDto> GetVentesDashboardDataAsync(DateTime debut, DateTime fin)
    {
        var factures = await _factureRepository.GetFacturesByPeriodeAsync(debut, fin);
        
        var dto = new VentesDashboardDto
        {
            ChiffreAffairesTotalHT = factures.Sum(f => f.TotalHT),
            ChiffreAffairesTotalTTC = factures.Sum(f => f.TotalTTC)
        };

        // CA par période (groupé par jour ou mois selon la plage)
        bool groupDaily = (fin - debut).TotalDays <= 31;
        
        dto.CA_ParPeriode = factures
            .GroupBy(f => f.DateFacture.ToString(groupDaily ? "yyyy-MM-dd" : "yyyy-MM"))
            .OrderBy(g => g.Key)
            .ToDictionary(g => g.Key, g => g.Sum(f => f.TotalHT));

        // Top Clients
        dto.TopClients = factures
            .GroupBy(f => f.Client != null ? f.Client.Nom : "Inconnu")
            .Select(g => new TopClientDto
            {
                Nom = g.Key,
                TotalAchatHT = g.Sum(f => f.TotalHT)
            })
            .OrderByDescending(c => c.TotalAchatHT)
            .Take(10)
            .ToList();

        // Top Produits
        var lignes = factures.SelectMany(f => f.Lignes);
        dto.TopProduits = lignes
            .GroupBy(l => l.Produit != null ? l.Produit.Libelle : "Inconnu")
            .Select(g => new TopProduitDto
            {
                Libelle = g.Key,
                QuantiteVendue = g.Sum(l => l.Quantite),
                ValeurTotaleHT = g.Sum(l => l.MontantHT)
            })
            .OrderByDescending(p => p.ValeurTotaleHT)
            .Take(10)
            .ToList();

        // Evolution CA (tendances sur les factures sélectionnées)
        dto.EvolutionCA = factures
            .GroupBy(f => f.DateFacture.ToString("yyyy-MM"))
            .OrderBy(g => g.Key)
            .ToDictionary(g => g.Key, g => g.Sum(f => f.TotalHT));

        return dto;
    }
}
