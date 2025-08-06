using BarberApiV1.Infraestructure.Rest.Interfaces;
using Newtonsoft.Json;

namespace BarberApiV1.Infraestructure.Rest.Services;

public class RestService : IRest
{
    public async Task<TOutput> Execute<TInput, TOutput>(TInput body, string baseUrl, HttpMethod method, string apikey = null,
        Dictionary<string, string> parameters = null, Dictionary<string, string> headers = null) where TOutput : new()
    {
        try
        {
            using var httpClient = new HttpClient();
        
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            if (!string.IsNullOrEmpty(apikey))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apikey}");
            }
        
            AddHeaders<TInput, TOutput>(headers, httpClient);
        
            var uriBuilder = UriBuilder<TInput, TOutput>(baseUrl, parameters);

            HttpResponseMessage response;
        
            if (method == HttpMethod.Get || method == HttpMethod.Delete)
            {
                response = await httpClient.SendAsync(new HttpRequestMessage(method, uriBuilder.Uri));
            }
            else
            {
                string jsonBody = JsonConvert.SerializeObject(body);
                var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");
            
                if (headers != null)
                {
                    foreach (var header in headers.Where(h => IsContentHeader(h.Key)))
                    {
                        content.Headers.Add(header.Key, header.Value);
                    }
                }

                response = await httpClient.SendAsync(new HttpRequestMessage(method, uriBuilder.Uri)
                    { Content = content });
            }

            await ValidateResponseAsync(response);

            string responseContent = await response.Content.ReadAsStringAsync();

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            Console.WriteLine($"execute: {baseUrl} content: {responseContent}");

            var deserializeObject = JsonConvert.DeserializeObject<TOutput>(responseContent, settings);

            return deserializeObject;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Execute: {e.Message}");
            throw new Exception(e.Message);
        }
    }

    private static UriBuilder UriBuilder<TInput, TOutput>(string baseUrl, Dictionary<string, string> parameters) where TOutput : new()
    {
        var uriBuilder = new UriBuilder(baseUrl);
        if (parameters != null && parameters.Any())
        {
            var query = string.Join("&",
                parameters.Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));
            uriBuilder.Query = query;
        }

        return uriBuilder;
    }

    private void AddHeaders<TInput, TOutput>(Dictionary<string, string> headers, HttpClient httpClient) where TOutput : new()
    {
        if (headers != null && headers.Any())
        {
            foreach (var header in headers)
            {
                if (IsContentHeader(header.Key))
                {
                    continue;
                }

                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }
    }

    private async Task ValidateResponseAsync(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Request failed with status {response.StatusCode}: {errorContent}");
        }
    }

    private bool IsContentHeader(string headerName)
    {
        var contentHeaders = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Content-Type",
            "Content-Length",
            "Content-Encoding",
            "Content-Language",
            "Content-Location",
            "Content-MD5",
            "Content-Range",
            "Content-Disposition",
            "Expires",
            "Last-Modified"
        };

        return contentHeaders.Contains(headerName);
    }
}