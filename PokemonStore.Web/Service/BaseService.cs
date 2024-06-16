using Newtonsoft.Json;
using PokemonStore.Web.Models;
using PokemonStore.Web.Service.IService;
using PokemonStore.Web.Utility;
using System.Net;
using System.Text;
using static PokemonStore.Web.Utility.StaticDetails;

namespace PokemonStore.Web.Service;
public class BaseService(IHttpClientFactory httpClientFactory) : IBaseService
{
    public async Task<ResponseDto?> SendAsync(RequestDto requestDto)
    {
        try
        {
            HttpClient client = httpClientFactory.CreateClient("PokemonStore");

            HttpRequestMessage message = new();
            message.Headers.Add("Accept", StaticDetails.MediaType);

            client.DefaultRequestHeaders.Add("ApiKey", StaticDetails.ApiKey);

            message.RequestUri = new Uri(requestDto.Url);

            if (requestDto.Data != null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, StaticDetails.MediaType);
            }

            HttpResponseMessage? apiResponse = null;

            switch (requestDto.ApiType)
            {
                case ApiType.POST:
                    message.Method = HttpMethod.Post;
                    break;
                case ApiType.DELETE:
                    message.Method = HttpMethod.Delete;
                    break;
                case ApiType.PUT:
                    message.Method = HttpMethod.Put;
                    break;
                case ApiType.GET:
                    break;
                default:
                    message.Method = HttpMethod.Get;
                    break;
            }

            apiResponse = await client.SendAsync(message);

            switch (apiResponse.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    return new() { IsSuccess = false, Message = "Not Found" };
                case HttpStatusCode.BadRequest:
                    return new() { IsSuccess = false, Message = "Bad Request" };
                case HttpStatusCode.Forbidden:
                    return new() { IsSuccess = false, Message = "Access Denied" };
                case HttpStatusCode.Unauthorized:
                    return new() { IsSuccess = false, Message = "Unauthorized" };
                case HttpStatusCode.InternalServerError:
                    return new() { IsSuccess = false, Message = "Internal Server Error" };
                default:
                    var apiContent = await apiResponse.Content.ReadAsStringAsync();
                    var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                    return apiResponseDto;
            }
        }
        catch (Exception ex)
        {
            var dto = new ResponseDto
            {
                Message = ex.Message,
                IsSuccess = false
            };
            return dto;
        }
    }
}
