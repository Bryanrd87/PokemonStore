using MediatR;
using PokemonStore.Application.Models;
using PokemonStore.Domain.Common;

namespace PokemonStore.Application.Pokemon.Queries.GetFromApi;
public record GetPokemonQuery(string Name) : IRequest<Result<PokemonDto>>;
