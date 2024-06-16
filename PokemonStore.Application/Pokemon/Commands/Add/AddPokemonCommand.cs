using MediatR;
using PokemonStore.Application.Models;
using PokemonStore.Domain.Common;

namespace PokemonStore.Application.Pokemon.Commands.Add;
public record AddPokemonCommand(PokemonDto Pokemon) : IRequest<Result>;
