using Facturation.Core.Entities;
using Facturation.Core.Interfaces;

namespace Facturation.Services.Services;

public interface IParametreService
{
    Task<decimal> GetTimbreFiscalAsync();
    Task SetTimbreFiscalAsync(decimal montant);
    Task<IEnumerable<decimal>> GetTvaRatesAsync();
    Task EnsureDefaultParametersAsync();
}

public class ParametreService : IParametreService
{
    private readonly IRepository<Parametre> _parametreRepository;
    private const string TIMBRE_FISCAL_KEY = "TIMBRE_FISCAL";

    public ParametreService(IRepository<Parametre> parametreRepository)
    {
        _parametreRepository = parametreRepository;
    }

    public async Task<decimal> GetTimbreFiscalAsync()
    {
        var param = (await _parametreRepository.FindAsync(p => p.Cle == TIMBRE_FISCAL_KEY)).FirstOrDefault();
        if (param != null && decimal.TryParse(param.Valeur, out decimal result))
        {
            return result;
        }
        return 1.000m; // Par défaut: 1 DT
    }

    public async Task SetTimbreFiscalAsync(decimal montant)
    {
        var param = (await _parametreRepository.FindAsync(p => p.Cle == TIMBRE_FISCAL_KEY)).FirstOrDefault();
        if (param != null)
        {
            param.Valeur = montant.ToString("0.000");
            await _parametreRepository.UpdateAsync(param);
        }
        else
        {
            await _parametreRepository.AddAsync(new Parametre 
            { 
                Cle = TIMBRE_FISCAL_KEY, 
                Valeur = montant.ToString("0.000"),
                Description = "Montant du timbre fiscal applicable"
            });
        }
        await _parametreRepository.SaveChangesAsync();
    }

    public Task<IEnumerable<decimal>> GetTvaRatesAsync()
    {
        // En Tunisie, les taux standards sont généralement 7%, 13% et 19%
        return Task.FromResult<IEnumerable<decimal>>(new List<decimal> { 0m, 7m, 13m, 19m });
    }

    public async Task EnsureDefaultParametersAsync()
    {
        if (!await _parametreRepository.ExistsAsync(p => p.Cle == TIMBRE_FISCAL_KEY))
        {
            await _parametreRepository.AddAsync(new Parametre 
            { 
                Cle = TIMBRE_FISCAL_KEY, 
                Valeur = "1.000",
                Description = "Montant du timbre fiscal applicable"
            });
            await _parametreRepository.SaveChangesAsync();
        }
    }
}
