using RestSharp;

namespace WebDriver.API.Core
{
    public class BaseApiClient
    {
        protected readonly RestClient Client;
        protected readonly string BaseUrl = "https://jsonplaceholder.typicode.com";

        public BaseApiClient()
        {
            var options = new RestClientOptions(BaseUrl)
            {
                MaxTimeout = 10000
            };
            Client = new RestClient(options);
        }

        protected RestRequest CreateRequest(string resource, Method method)
        {
            var request = new RestRequest(resource, method);
            request.AddHeader("Accept", "application/json");
            return request;
        }
    }
}
