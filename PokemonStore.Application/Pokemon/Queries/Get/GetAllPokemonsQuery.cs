using MediatR;
using PokemonStore.Application.Models;
using PokemonStore.Domain.Common;

namespace PokemonStore.Application.Pokemon.Queries.Get;
public record GetAllPokemonsQuery : IRequest<Result<List<PokemonDto>>>;
