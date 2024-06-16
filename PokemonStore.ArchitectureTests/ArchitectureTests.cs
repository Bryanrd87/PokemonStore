using NetArchTest.Rules;
using System.Reflection;

namespace PokemonStore.ArchitectureTests;
public class ArchitectureTests
{
    private const string ApplicationNamespace = "PokemonStore.Application";
    private const string DomainNamespace = "PokemonStore.Domain";
    private const string InfrastructureNamespace = "PokemonStore.Infrastructure";
    private const string ApiNamespace = "PokemonStore.API";

    [Fact]
    public void Application_ShouldNotDependOnInfrastructureOrApi()
    {
        var assembly = Assembly.Load(ApplicationNamespace);

        var result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOn(InfrastructureNamespace)
            .GetResult();
        Assert.True(result.IsSuccessful, "Application layer should not depend on Infrastructure layer.");

        result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOn(ApiNamespace)
            .GetResult();
        Assert.True(result.IsSuccessful, "Application layer should not depend on API layer.");
    }

    [Fact]
    public void Domain_ShouldNotDependOnApplicationInfrastructureOrApi()
    {
        var assembly = Assembly.Load(DomainNamespace);

        var result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOn(ApplicationNamespace)
            .GetResult();
        Assert.True(result.IsSuccessful, "Domain layer should not depend on Application layer.");

        result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOn(InfrastructureNamespace)
            .GetResult();
        Assert.True(result.IsSuccessful, "Domain layer should not depend on Infrastructure layer.");

        result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOn(ApiNamespace)
            .GetResult();
        Assert.True(result.IsSuccessful, "Domain layer should not depend on API layer.");
    }

    [Fact]
    public void Infrastructure_ShouldNotDependOnApi()
    {
        var assembly = Assembly.Load(InfrastructureNamespace);

        var result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOn(ApiNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful, "Infrastructure layer should not depend on API layer.");
    }
}