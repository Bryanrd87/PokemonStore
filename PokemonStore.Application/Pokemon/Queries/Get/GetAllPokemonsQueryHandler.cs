using MediatR;
using PokemonStore.Application.Models;
using PokemonStore.Domain.Common;
using PokemonStore.Domain.Interfaces;

namespace PokemonStore.Application.Pokemon.Queries.Get;
public class GetAllPokemonsQueryHandler(IPokemonRepository pokemonRepository) : IRequestHandler<GetAllPokemonsQuery, Result<List<PokemonDto>>>
{
    private readonly IPokemonRepository _pokemonRepository = pokemonRepository;
    public async Task<Result<List<PokemonDto>>> Handle(GetAllPokemonsQuery request, CancellationToken cancellationToken)
    {
        var result = await _pokemonRepository.GetAllAsync(cancellationToken);

        List<PokemonDto> pokemonList = result.ConvertAll(p => new PokemonDto(p.Id, p.Name, p.BaseExperience));

        return Result<List<PokemonDto>>.Success(pokemonList);
    }
}
