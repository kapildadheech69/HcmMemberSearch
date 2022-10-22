using HcmMemberSearch.Helper.IHelper;
using System;
using System.Net.Http;

namespace HcmMemberSearch.Helper
{
    public class ClaimHelper:IClaimHelper
    {
        public HttpClient Initial()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44316/");
            return client;
        }
    }
}
