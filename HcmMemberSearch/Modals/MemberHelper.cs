using System;
using System.Net.Http;
namespace HcmMemberSearch.Modals
{
    public class MemberHelper
    {
        public HttpClient Initial()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44335/");
            return client;
        }
    }
}
