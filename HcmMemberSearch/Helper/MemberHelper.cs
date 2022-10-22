using HcmMemberSearch.Helper.IHelper;
using System;
using System.Net.Http;

namespace HcmMemberSearch.Helper
{
    public class MemberHelper:IMemberHelper
    {
        public HttpClient Initial()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44335/");
            return client;
        }
    }
}
