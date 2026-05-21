using Facturation.Core.Entities;
using Facturation.Core.Interfaces;
using Facturation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Facturation.Infrastructure.Repositories;

public class FactureRepository : Repository<Facture>, IFactureRepository
{
    public FactureRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Facture?> GetFactureWithDetailsAsync(int id)
    {
        return await _context.Factures
            .Include(f => f.Client)
            .Include(f => f.Lignes)
                .ThenInclude(l => l.Produit)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<IEnumerable<Facture>> GetFacturesByClientAsync(int clientId)
    {
        return await _context.Factures
            .Include(f => f.Client)
            .Where(f => f.ClientId == clientId)
            .OrderByDescending(f => f.DateFacture)
            .ToListAsync();
    }

    public async Task<IEnumerable<Facture>> GetFacturesByPeriodeAsync(DateTime debut, DateTime fin)
    {
        return await _context.Factures
            .Include(f => f.Client)
            .Include(f => f.Lignes) // Important pour les analyses
            .Where(f => f.DateFacture >= debut && f.DateFacture <= fin && f.Statut == "Validée")
            .OrderBy(f => f.DateFacture)
            .ToListAsync();
    }

    public async Task<string> GenerateNumeroFactureAsync(DateTime date)
    {
        string prefix = $"FACT-{date:yyyyMM}-";
        
        var lastFacture = await _context.Factures
            .Where(f => f.NumeroFacture.StartsWith(prefix))
            .OrderByDescending(f => f.NumeroFacture)
            .FirstOrDefaultAsync();

        if (lastFacture == null)
        {
            return $"{prefix}0001";
        }

        string sequenceStr = lastFacture.NumeroFacture.Substring(prefix.Length);
        if (int.TryParse(sequenceStr, out int sequence))
        {
            return $"{prefix}{(sequence + 1):D4}";
        }

        return $"{prefix}0001";
    }
}
