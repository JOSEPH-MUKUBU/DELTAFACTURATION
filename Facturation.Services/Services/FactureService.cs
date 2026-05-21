using Facturation.Core.Entities;
using Facturation.Core.Interfaces;

namespace Facturation.Services.Services;

public interface IFactureService
{
    Task<IEnumerable<Facture>> GetAllFacturesAsync();
    Task<Facture?> GetFactureByIdAsync(int id);
    Task<Facture> CreateFactureAsync(Facture facture);
    Task AnnulerFactureAsync(int id);
    Task<Facture> CalculerTotauxAsync(Facture facture);
}

public class FactureService : IFactureService
{
    private readonly IFactureRepository _factureRepository;
    private readonly IParametreService _parametreService;
    private readonly IProduitService _produitService;

    public FactureService(
        IFactureRepository factureRepository,
        IParametreService parametreService,
        IProduitService produitService)
    {
        _factureRepository = factureRepository;
        _parametreService = parametreService;
        _produitService = produitService;
    }

    public async Task<IEnumerable<Facture>> GetAllFacturesAsync()
    {
        return await _factureRepository.GetAllAsync();
    }

    public async Task<Facture?> GetFactureByIdAsync(int id)
    {
        return await _factureRepository.GetFactureWithDetailsAsync(id);
    }

    public async Task<Facture> CalculerTotauxAsync(Facture facture)
    {
        facture.TotalHT = 0;
        facture.TotalTVA = 0;

        foreach (var ligne in facture.Lignes)
        {
            if (ligne.ProduitId > 0 && ligne.PrixUnitaireHT == 0)
            {
                var produit = await _produitService.GetProduitByIdAsync(ligne.ProduitId);
                if (produit != null)
                {
                    ligne.PrixUnitaireHT = produit.PrixUnitaireHT;
                    ligne.TauxTVA = produit.TauxTVA;
                }
            }

            ligne.MontantHT = ligne.PrixUnitaireHT * ligne.Quantite;
            ligne.MontantTVA = ligne.MontantHT * (ligne.TauxTVA / 100);
            ligne.MontantTTC = ligne.MontantHT + ligne.MontantTVA;

            facture.TotalHT += ligne.MontantHT;
            facture.TotalTVA += ligne.MontantTVA;
        }

        facture.TotalTTC = facture.TotalHT + facture.TotalTVA;

        // Timbre fiscal en Tunisie s'applique généralement sur les factures TTC
        facture.TimbreFiscal = await _parametreService.GetTimbreFiscalAsync();
        
        facture.MontantTotal = facture.TotalTTC + facture.TimbreFiscal;

        return facture;
    }

    public async Task<Facture> CreateFactureAsync(Facture facture)
    {
        if (!facture.Lignes.Any())
            throw new InvalidOperationException("Une facture doit contenir au moins une ligne.");

        facture.NumeroFacture = await _factureRepository.GenerateNumeroFactureAsync(facture.DateFacture);
        facture.Statut = "Validée";
        
        // Recalcul de sécurité
        facture = await CalculerTotauxAsync(facture);

        var created = await _factureRepository.AddAsync(facture);
        await _factureRepository.SaveChangesAsync();
        
        return created;
    }

    public async Task AnnulerFactureAsync(int id)
    {
        var facture = await _factureRepository.GetByIdAsync(id);
        if (facture != null && facture.Statut == "Validée")
        {
            facture.Statut = "Annulée";
            await _factureRepository.UpdateAsync(facture);
            await _factureRepository.SaveChangesAsync();
        }
    }
}
