using PokemonStore.Web.Models;
using PokemonStore.Web.Service.IService;
using PokemonStore.Web.Utility;

namespace PokemonStore.Web.Service;
public class PokemonService(IBaseService baseService) : IPokemonService
{
    private readonly IBaseService _baseService = baseService;
    public async Task<ResponseDto?> CreatePokemonsAsync(PokemonDto pokemonDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = StaticDetails.ApiType.POST,
            Data = pokemonDto,
            Url = StaticDetails.PokemonAPI + "/api/pokemon/collection"
        });
    }
    public async Task<ResponseDto?> DeletePokemonAsync(int id)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = StaticDetails.ApiType.DELETE,
            Url = StaticDetails.PokemonAPI + "/api/pokemon/collection/" + id
        });
    }
    public async Task<ResponseDto?> GetAllPokemonsAsync()
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = StaticDetails.PokemonAPI + "/api/pokemon/collection"
        });
    }
    public async Task<ResponseDto?> GetPokemonByNameAsync(string name)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = StaticDetails.PokemonAPI + "/api/pokemon/search/" + name
        });
    }
}