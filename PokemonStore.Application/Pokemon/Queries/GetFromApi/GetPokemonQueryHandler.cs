using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PokemonStore.Application.Configurations;
using PokemonStore.Application.Models;
using PokemonStore.Domain.Common;
using PokemonStore.Domain.Interfaces;
using System.Text.Json;

namespace PokemonStore.Application.Pokemon.Queries.GetFromApi;
public class GetPokemonQueryHandler(IHttpClientFactory httpClientFactory, IPokemonRepository pokemonRepository, IOptions<PokemonApiSettings> pokemonApiSettings, ILogger<GetPokemonQueryHandler> logger) : IRequestHandler<GetPokemonQuery, Result<PokemonDto>>
{
    private readonly IPokemonRepository _pokemonRepository = pokemonRepository;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly PokemonApiSettings _pokemonApiSettings = pokemonApiSettings.Value;
    private readonly ILogger<GetPokemonQueryHandler> _logger = logger;
    public async Task<Result<PokemonDto>> Handle(GetPokemonQuery request, CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient("PokemonStore");

        var response = await client.GetAsync($"{_pokemonApiSettings.BaseUrl}{_pokemonApiSettings.Endpoint}/{request.Name.ToLower()}", cancellationToken);

        if (!response.IsSuccessStatusCode)
            return Result<PokemonDto>.Failure("Pokemon not found");

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var pokemonData = JsonSerializer.Deserialize<JsonElement>(content);

        if (!pokemonData.TryGetProperty("id", out JsonElement idElement) || idElement.ValueKind != JsonValueKind.Number ||
            !pokemonData.TryGetProperty("name", out JsonElement nameElement) || nameElement.ValueKind != JsonValueKind.String)
        {
            _logger.LogError("Pokemon data is missing required properties.");
            return Result<PokemonDto>.Failure("Invalid Pokemon data");
        }

        int baseExperience = 0;
        if (pokemonData.TryGetProperty("base_experience", out JsonElement baseExpElement) && baseExpElement.ValueKind == JsonValueKind.Number)
        {
            baseExperience = baseExpElement.GetInt32();
        }

        var pokemon = new PokemonDto(idElement.GetInt32(), nameElement.GetString(), baseExperience);

        return Result<PokemonDto>.Success(pokemon);
    }
}
