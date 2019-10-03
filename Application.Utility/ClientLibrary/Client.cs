using System.Net.Http;

namespace Application.Utility.ClientLibrary
{
    public interface IClient
    {
        IHttpClientFactory HttpClientFactory { get; }
    }

    public class Client : IClient
    {
        public IHttpClientFactory HttpClientFactory { get; }

        public Client(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
        }
    }
}