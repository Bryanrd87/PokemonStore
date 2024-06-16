using PokemonStore.Web.Models;

namespace PokemonStore.Web.Service.IService;
public interface IPokemonService
{
    Task<ResponseDto?> GetAllPokemonsAsync();
    Task<ResponseDto?> GetPokemonByNameAsync(string name);
    Task<ResponseDto?> CreatePokemonsAsync(PokemonDto productDto);
    Task<ResponseDto?> DeletePokemonAsync(int id);
}
