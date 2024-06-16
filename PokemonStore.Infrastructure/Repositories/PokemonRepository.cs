using Microsoft.EntityFrameworkCore;
using PokemonStore.Domain.Entities;
using PokemonStore.Domain.Interfaces;
using PokemonStore.Infrastructure.Data;

namespace PokemonStore.Infrastructure.Repositories;
internal class PokemonRepository : IPokemonRepository
{
    private readonly PokemonDbContext _context;

    public PokemonRepository(PokemonDbContext context)
    {
        _context = context;
    }

    public async Task<List<Pokemon>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Pokemons.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Pokemon> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Pokemons.FindAsync(id, cancellationToken);
    }

    public async Task AddAsync(Pokemon pokemon, CancellationToken cancellationToken = default)
    {
        _context.Pokemons.Add(pokemon);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(Pokemon pokemon, CancellationToken cancellationToken = default)
    {
        _context.Pokemons.Remove(pokemon);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
