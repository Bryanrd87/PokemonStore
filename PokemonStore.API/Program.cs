using MediatR;
using PokemonStore.Infrastructure;
using PokemonStore.Application;
using PokemonStore.Application.Pokemon.Commands.Add;
using PokemonStore.Application.Pokemon.Commands.Remove;
using PokemonStore.Application.Pokemon.Queries.GetFromApi;
using PokemonStore.Application.Pokemon.Queries.Get;
using PokemonStore.Application.Models;
using PokemonStore.API.Models;
using PokemonStore.API.Middleware;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Pokemon API",
        Version = "v1"
    });

    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "API Key needed to access the endpoints. API Key must be in the request header. Example: \"ApiKey: {your_api_key}\"",
        In = ParameterLocation.Header,
        Name = "ApiKey",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference{
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddInfrastructure();
builder.Services.AddApplication(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pokemon API V1");
    c.RoutePrefix = string.Empty;
    c.DefaultModelsExpandDepth(-1);
});

app.UseMiddleware<ApiKeyMiddleware>();

// Configure the HTTP request pipeline.
app.MapGet("/api/pokemon/search/{name}", async (string name, IMediator mediator) =>
{
    ResponseDto _apiResponse = new();

    var query = new GetPokemonQuery(name);
    var result = await mediator.Send(query);

    if (!result.IsSuccess)
    {
        _apiResponse.IsSuccess = false;
        _apiResponse.Message = result.Error;
    }
    else
    {
        _apiResponse.Result = result.Value;
    }

    return _apiResponse;
});

app.MapGet("/api/pokemon/collection", async (HttpContext context, IMediator mediator) =>
{
    ResponseDto _apiResponse = new();

    var query = new GetAllPokemonsQuery();

    var result = await mediator.Send(query);

    _apiResponse.Result = result.Value;

    return _apiResponse;
});

app.MapPost("/api/pokemon/collection", async (PokemonDto pokemonDto, IMediator mediator) =>
{
    ResponseDto _apiResponse = new();

    var pokemon = new AddPokemonCommand(pokemonDto);
    var result = await mediator.Send(pokemon);

    if (!result.IsSuccess)
    {
        _apiResponse.IsSuccess = false;
        _apiResponse.Message = result.Error;
    }
    else
    {
        _apiResponse.Result = result.Value;
    }

    return _apiResponse;
});

app.MapDelete("/api/pokemon/collection/{id}", async (int id, IMediator mediator) =>
{
    ResponseDto _apiResponse = new();

    var command = new RemovePokemonCommand(id);
    var result = await mediator.Send(command);

    if (!result.IsSuccess)
    {
        _apiResponse.Message = result.Error;
    }

    return _apiResponse;
});

app.Run();
