namespace BarberApiV1.Infraestructure.Rest.Interfaces;

public interface IRest
{
    Task<TOutput> Execute<TInput, TOutput>(TInput body, string baseUrl, HttpMethod method, string apikey = null,
        Dictionary<string, string> parameters = null, Dictionary<string, string> headers = null) where TOutput : new();
}