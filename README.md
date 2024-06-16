# Pokemon Store API

This is a minimal API for managing a Pokemon collection using .NET 8. It supports searching for Pokemon, adding/removing Pokemon from a collection, and listing the collection. The API uses Clean Architecture principles, Mediatr, CQRS, and EF Core with an in-memory database. Additionally, the project includes Unit tests and Swagger for API documentation.

## Features

- Search for Pokemon by name
- Add or remove Pokemon from your collection
- List your Pokemon collection (max 6 Pokemon, no duplicates)
- Unit tests with xUnit, Moq, and Shouldly
- Swagger for API documentation
- Frontend implemented using Razor pages and Bootstrap for UI

## Getting Started

### Prerequisites

- .NET 8 SDK
- An IDE or text editor (e.g., Visual Studio, Visual Studio Code)

### Setup

1. Clone the repository:

   ```bash
   git clone https://github.com/bryanrd87/PokemonStore.git
   cd PokemonStore
   ```

2. Ensure you have the necessary dependencies installed:

   ```bash
   dotnet restore
   ```

### Project Structure

- **PokemonStore.API**: Main project with the API implementation
- **PokemonStore.Application**: Application layer with CQRS, Mediatr handlers, and business logic
- **PokemonStore.Infrastructure**: Infrastructure layer with EF Core and database context
- **PokemonStore.Domaine**: Domain layer
- **PokemonStore.Web**: Frontend project with Razor pages and Bootstrap
- **PokemonStore.Tests**: Unit tests project using xUnit, Moq, and Shouldly
- **PokemonStore.ArchitectureTests**: Architecture tests project

### Running the API

1. Navigate to the main project directory:

   ```bash
   cd PokemonStore.API
   ```

2. Run the API:

   ```bash
   dotnet run
   ```

3. The API will be available at `https://localhost:7128`. You can access the Swagger UI at `https://localhost:7128/index.html`.

### API Endpoints

- `GET /api/pokemon/search/{name}`: Search for a Pokemon by name
- `GET /api/pokemon/collection`: Get the list of Pokemon in the collection
- `POST /api/pokemon/collection`: Add a Pokemon to the collection
- `DELETE /api/pokemon/collection/{id}`: Remove a Pokemon from the collection

### Sample Code

#### PokemonStore.Application.Pokemon.Queries.GetFromApi GetPokemonQueryHandler.cs

```csharp
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
```
