using Facturation.Core.Entities;

namespace Facturation.Core.Interfaces;

public interface IFactureRepository : IRepository<Facture>
{
    Task<Facture?> GetFactureWithDetailsAsync(int id);
    Task<IEnumerable<Facture>> GetFacturesByClientAsync(int clientId);
    Task<IEnumerable<Facture>> GetFacturesByPeriodeAsync(DateTime debut, DateTime fin);
    Task<string> GenerateNumeroFactureAsync(DateTime date);
}
