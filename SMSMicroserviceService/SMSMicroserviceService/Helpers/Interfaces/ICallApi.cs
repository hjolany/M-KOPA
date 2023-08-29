namespace SMSMicroService.Helpers.Interfaces
{
    public interface ICallApi<in TRequest>
    {
        public Task<HttpResponseMessage> Get(string url, TRequest data);
        public Task<HttpResponseMessage> Post(string url, TRequest data);
        public Task<HttpResponseMessage> Post(string url, TRequest data, string token);
    } 
}
