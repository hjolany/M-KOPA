using System.Net;
using System.Net.Http.Headers;
using System.Text;
using SMSMicroService.Helpers.Interfaces;
using SMSMicroService.Infrastructures.Extensions;

namespace SMSMicroService.Helpers;

public class CallApi<TRequest> : ICallApi<TRequest>
{
    private readonly ILogger<ICallApi<TRequest>> _logger;
    private readonly HttpClient _httpClient;

    public CallApi(ILogger<ICallApi<TRequest>> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }
    public async Task<HttpResponseMessage> Get(string url, TRequest data)
    {
        try
        {
            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            return await _httpClient.GetAsync(url);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex.GetFullMessage());
        }
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
    }

    public async Task<HttpResponseMessage> Post(string url, TRequest data)
    {
        try
        {
            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(url, content);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex.GetFullMessage());
        }
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
    }

    public async Task<HttpResponseMessage> Post(string url, TRequest data, string token)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(url, content);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex.GetFullMessage());
        }
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
    }
}