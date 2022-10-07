using System;
using System.Net.Http;
namespace HcmMemberSearch.Modals
{
    public class ClaimHelper
    {
        public HttpClient Initial()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/");
            return client;
        }
    }
}
