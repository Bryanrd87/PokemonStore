using PokemonStore.Domain.Entities;

namespace PokemonStore.Domain.Interfaces;
public interface IPokemonRepository
{
    Task<List<Pokemon>> GetAllAsync(CancellationToken cancellationToken);
    Task<Pokemon> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(Pokemon pokemon, CancellationToken cancellationToken);
    Task RemoveAsync(Pokemon pokemon, CancellationToken cancellationToken);
}
