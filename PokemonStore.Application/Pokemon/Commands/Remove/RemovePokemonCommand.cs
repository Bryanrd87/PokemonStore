using MediatR;
using PokemonStore.Domain.Common;

namespace PokemonStore.Application.Pokemon.Commands.Remove;
public record RemovePokemonCommand(int Id) : IRequest<Result>;
