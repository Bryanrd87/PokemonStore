using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PokemonStore.Application.Models;
using PokemonStore.Domain.Common;
using PokemonStore.Domain.Interfaces;

namespace PokemonStore.Application.Pokemon.Commands.Add;
public class AddPokemonCommandHandler(IValidator<AddPokemonCommand> validator, IPokemonRepository repository, ILogger<AddPokemonCommandHandler> logger) : IRequestHandler<AddPokemonCommand, Result>
{
    private readonly ILogger<AddPokemonCommandHandler> _logger = logger;
    public async Task<Result> Handle(AddPokemonCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequestAsync(request, cancellationToken);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        var checkExistenceResult = await CheckIfPokemonExistsAsync(request.Pokemon.Id, cancellationToken);
        if (!checkExistenceResult.IsSuccess)
        {
            return checkExistenceResult;
        }

        var checkCollectionSizeResult = await CheckCollectionSizeAsync(cancellationToken);
        if (!checkCollectionSizeResult.IsSuccess)
        {
            return checkCollectionSizeResult;
        }

        var addPokemonResult = await AddPokemonAsync(request.Pokemon, cancellationToken);
        if (!addPokemonResult.IsSuccess)
        {
            return addPokemonResult;
        }

        return Result.Success();
    }

    private async Task<Result> ValidateRequestAsync(AddPokemonCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errorMessages = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            _logger.LogWarning("Validation failed for AddPokemonCommand: {Errors}", errorMessages);
            return Result.Failure(errorMessages);
        }

        return Result.Success();
    }

    private async Task<Result> CheckIfPokemonExistsAsync(int pokemonId, CancellationToken cancellationToken)
    {
        var existingPokemons = await repository.GetAllAsync(cancellationToken);
        if (existingPokemons.Any(p => p.Id == pokemonId))
        {
            _logger.LogWarning("Pokemon with Id {PokemonId} already exists in the collection.", pokemonId);
            return Result.Failure("Pokemon already in collection");
        }

        return Result.Success();
    }

    private async Task<Result> CheckCollectionSizeAsync(CancellationToken cancellationToken)
    {
        var existingPokemons = await repository.GetAllAsync(cancellationToken);
        if (existingPokemons.Count >= 6)
        {
            _logger.LogWarning("The collection is full. Cannot add more Pokemons.");
            return Result.Failure("Collection is full");
        }

        return Result.Success();
    }

    private async Task<Result> AddPokemonAsync(PokemonDto pokemonDto, CancellationToken cancellationToken)
    {
        var pokemon = new Domain.Entities.Pokemon
        {
            Id = pokemonDto.Id,
            Name = pokemonDto.Name,
            BaseExperience = pokemonDto.BaseExperience
        };

        try
        {
            await repository.AddAsync(pokemon, cancellationToken);
            _logger.LogInformation("Pokemon with Id {Pokemon.Id} added to the collection.", pokemon.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding Pokemon with Id {PokemonId} to the collection.", pokemon.Id);
            return Result.Failure("An error occurred while adding the Pokemon to the collection.");
        }
    }
}
