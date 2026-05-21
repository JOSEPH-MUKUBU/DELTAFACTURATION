using Facturation.Core.Entities;
using Facturation.Core.Interfaces;

namespace Facturation.Services.Services;

public interface IProduitService
{
    Task<IEnumerable<Produit>> GetAllProduitsAsync(bool includeInactive = false);
    Task<Produit?> GetProduitByIdAsync(int id);
    Task<Produit> CreateProduitAsync(Produit produit);
    Task UpdateProduitAsync(Produit produit);
    Task ToggleStatusAsync(int id);
}

public class ProduitService : IProduitService
{
    private readonly IRepository<Produit> _produitRepository;

    public ProduitService(IRepository<Produit> produitRepository)
    {
        _produitRepository = produitRepository;
    }

    public async Task<IEnumerable<Produit>> GetAllProduitsAsync(bool includeInactive = false)
    {
        if (includeInactive)
            return await _produitRepository.GetAllAsync();
            
        return await _produitRepository.FindAsync(p => p.EstActif);
    }

    public async Task<Produit?> GetProduitByIdAsync(int id)
    {
        return await _produitRepository.GetByIdAsync(id);
    }

    public async Task<Produit> CreateProduitAsync(Produit produit)
    {
        produit.EstActif = true;
        var created = await _produitRepository.AddAsync(produit);
        await _produitRepository.SaveChangesAsync();
        return created;
    }

    public async Task UpdateProduitAsync(Produit produit)
    {
        await _produitRepository.UpdateAsync(produit);
        await _produitRepository.SaveChangesAsync();
    }

    public async Task ToggleStatusAsync(int id)
    {
        var produit = await _produitRepository.GetByIdAsync(id);
        if (produit != null)
        {
            produit.EstActif = !produit.EstActif;
            await _produitRepository.UpdateAsync(produit);
            await _produitRepository.SaveChangesAsync();
        }
    }
}
