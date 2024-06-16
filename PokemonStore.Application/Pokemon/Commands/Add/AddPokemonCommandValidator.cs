using FluentValidation;

namespace PokemonStore.Application.Pokemon.Commands.Add;
public class AddPokemonCommandValidator : AbstractValidator<AddPokemonCommand>
{
    public AddPokemonCommandValidator()
    {
        RuleFor(x => x.Pokemon.Id).GreaterThan(0);
        RuleFor(x => x.Pokemon.Name).NotEmpty();
    }
}
