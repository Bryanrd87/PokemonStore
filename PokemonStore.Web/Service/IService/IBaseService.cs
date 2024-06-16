using PokemonStore.Web.Models;

namespace PokemonStore.Web.Service.IService;
public interface IBaseService
{
    Task<ResponseDto?> SendAsync(RequestDto requestDto);
}
