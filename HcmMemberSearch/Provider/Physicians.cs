using HcmMemberSearch.Modals;
using HcmMemberSearch.Provider.IProvider;
using HcmMemberSearch.Helper.IHelper;
using Newtonsoft.Json;
using System.Net;
using System.Web.Http;
using System;

namespace HcmMemberSearch.Provider
{
    public class Physicians : IPhysicians
    {
        private readonly IMemberHelper _memberHelper;
        public Physicians(IMemberHelper memberHelper)
        {
            _memberHelper = memberHelper;
        }
    
        public async Task<IEnumerable<Physician>> GetPhysicians()
        {
            IEnumerable<Physician> physicians = new List<Physician>();
            HttpClient client = _memberHelper.Initial();
            HttpResponseMessage res = await client.GetAsync("HealthCare/Physicians");

            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                physicians = JsonConvert.DeserializeObject<IEnumerable<Physician>>(results);
                return physicians;
            }
            var msg = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(string.Format("No physicians in database"))
            };
            throw new HttpResponseException(msg);
        }
    }
}
