using MediatR;
using PokemonStore.Domain.Common;
using PokemonStore.Domain.Interfaces;

namespace PokemonStore.Application.Pokemon.Commands.Remove;
public class RemovePokemonCommandHandler(IPokemonRepository pokemonRepository) : IRequestHandler<RemovePokemonCommand, Result>
{
    private readonly IPokemonRepository _pokemonRepository = pokemonRepository;
    public async Task<Result> Handle(RemovePokemonCommand request, CancellationToken cancellationToken)
    {
        var pokemon = await _pokemonRepository.GetByIdAsync(request.Id, cancellationToken);

        if (pokemon is null)
            return Result.Failure("Pokémon not found");

        await _pokemonRepository.RemoveAsync(pokemon, cancellationToken);
        return Result.Success();
    }
}
