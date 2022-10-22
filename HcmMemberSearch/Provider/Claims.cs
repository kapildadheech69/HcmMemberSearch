using HcmMemberSearch.Modals;
using HcmMemberSearch.Provider.IProvider;
using HcmMemberSearch.Helper.IHelper;
using Newtonsoft.Json;
using Serilog;
using System.Net;
using System.Web.Http;

namespace HcmMemberSearch.Provider
{
    public class Claims : IClaims
    {
        private readonly IClaimHelper _claimHelper;
        public Claims(IClaimHelper claimHelper)
        {
            _claimHelper = claimHelper;
        }

        public async Task<List<Claim>> GetClaims()
        {
            List<Claim> claims = new List<Claim>();
            HttpClient client = _claimHelper.Initial();
            HttpResponseMessage res = await client.GetAsync("HealthCare/GetClaims");

            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                claims = JsonConvert.DeserializeObject<List<Claim>>(results);
                return claims;
            }
            var msg = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(string.Format("No claims in database"))
            };
            throw new HttpResponseException(msg);
        }
    }
}
