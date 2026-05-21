using Facturation.Core.Entities;
using Facturation.Core.Interfaces;

namespace Facturation.Services.Services;

public interface IClientService
{
    Task<IEnumerable<Client>> GetAllClientsAsync();
    Task<Client?> GetClientByIdAsync(int id);
    Task<Client> CreateClientAsync(Client client);
    Task UpdateClientAsync(Client client);
    Task DeleteClientAsync(int id);
    Task<bool> CanDeleteClientAsync(int id);
}

public class ClientService : IClientService
{
    private readonly IRepository<Client> _clientRepository;
    private readonly IFactureRepository _factureRepository;

    public ClientService(IRepository<Client> clientRepository, IFactureRepository factureRepository)
    {
        _clientRepository = clientRepository;
        _factureRepository = factureRepository;
    }

    public async Task<IEnumerable<Client>> GetAllClientsAsync()
    {
        return await _clientRepository.GetAllAsync();
    }

    public async Task<Client?> GetClientByIdAsync(int id)
    {
        return await _clientRepository.GetByIdAsync(id);
    }

    public async Task<Client> CreateClientAsync(Client client)
    {
        client.DateCreation = DateTime.Now;
        var created = await _clientRepository.AddAsync(client);
        await _clientRepository.SaveChangesAsync();
        return created;
    }

    public async Task UpdateClientAsync(Client client)
    {
        await _clientRepository.UpdateAsync(client);
        await _clientRepository.SaveChangesAsync();
    }

    public async Task<bool> CanDeleteClientAsync(int id)
    {
        var factures = await _factureRepository.FindAsync(f => f.ClientId == id);
        return !factures.Any();
    }

    public async Task DeleteClientAsync(int id)
    {
        if (!await CanDeleteClientAsync(id))
            throw new InvalidOperationException("Impossible de supprimer un client ayant des factures.");

        var client = await _clientRepository.GetByIdAsync(id);
        if (client != null)
        {
            await _clientRepository.DeleteAsync(client);
            await _clientRepository.SaveChangesAsync();
        }
    }
}
