using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;

namespace client
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new HttpClient();
            var disco = client.GetDiscoveryDocumentAsync("http://localhost:5727").GetAwaiter().GetResult();
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }
            // request token
            var tokenResponse = client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "passwordclient",
                ClientSecret = "secret",
                Scope = "api1",
                UserName = "tony",
                Password = "123456",
            }).GetAwaiter().GetResult();

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = client.GetAsync("http://localhost:5728/api/values").GetAwaiter().GetResult();
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                Console.WriteLine(JArray.Parse(content));
            }
            // return new JsonResult(json);
            Console.WriteLine("Hello World!");
        }
    }
}
