using Moq.Protected;
using Moq;
using PokemonStore.Application.Configurations;
using PokemonStore.Application.Pokemon.Queries.GetFromApi;
using System.Net;
using Shouldly;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PokemonStore.Domain.Interfaces;

namespace PokemonStore.Tests;
public class GetPokemonQueryHandlerTests
{
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly Mock<IPokemonRepository> _mockPokemonRepository;
    private readonly Mock<ILogger<GetPokemonQueryHandler>> _mockLogger;
    private readonly IOptions<PokemonApiSettings> _pokemonApiSettings;
    private readonly GetPokemonQueryHandler _handler;

    public GetPokemonQueryHandlerTests()
    {
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _mockPokemonRepository = new Mock<IPokemonRepository>();
        _mockLogger = new Mock<ILogger<GetPokemonQueryHandler>>();

        var client = new HttpClient(_mockHttpMessageHandler.Object);
        _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

        _pokemonApiSettings = Options.Create(new PokemonApiSettings
        {
            BaseUrl = "https://pokeapi.co/api/v2/",
            Endpoint = "pokemon"
        });

        _handler = new GetPokemonQueryHandler(
            _mockHttpClientFactory.Object,
            _mockPokemonRepository.Object,
            _pokemonApiSettings,
            _mockLogger.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnPokemonDto_WhenPokemonExists()
    {
        // Arrange
        var query = new GetPokemonQuery("ditto");
        const string jsonResponse = "{\"id\": 132, \"name\": \"ditto\", \"base_experience\": 101}";

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            });

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Id.ShouldBe(132);
        result.Value.Name.ShouldBe("ditto");
        result.Value.BaseExperience.ShouldBe(101);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenPokemonDoesNotExist()
    {
        // Arrange
        var query = new GetPokemonQuery("unknown");

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            });

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe("Pokemon not found");
    }
}
