using HcmMemberSearch.Modals;
using HcmMemberSearch.Provider.IProvider;
using HcmMemberSearch.Helper.IHelper;
using Newtonsoft.Json;
using System.Net;
using System.Web.Http;

namespace HcmMemberSearch.Provider
{
    public class Members : IMembers
    {
        private readonly IMemberHelper _memberHelper;
        public Members(IMemberHelper memberHelper)
        {
            this._memberHelper= memberHelper;
        }
    
        public async Task<List<Member>> GetMembersAsync()
        {
            List<Member> members = new List<Member>();
            HttpClient client = _memberHelper.Initial();
            HttpResponseMessage res = await client.GetAsync("HealthCare/GetMembers");

            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                members = JsonConvert.DeserializeObject<List<Member>>(results);
                return members;
            }
            var msg = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(string.Format("No members in database"))
            };
            throw new HttpResponseException(msg);
        }
    }
}
