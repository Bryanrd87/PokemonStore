namespace PokemonStore.Web.Utility;
public class StaticDetails
{
    public static string PokemonAPI { get; set; }
    public static string ApiKey { get; set; }
    public enum ApiType
    {
        GET,
        POST,
        PUT,
        DELETE
    }
    public enum ContentType
    {
        Json,
        MultipartFormData,
    }

    public static string MediaType = "application/json";
}
