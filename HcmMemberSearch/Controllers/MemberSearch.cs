using HcmMemberSearch.Modals;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;

namespace HcmMemberSearch.Controllers
{
    [ApiController]
    [Route("HealthCare")]
    public class MemberSearch:ControllerBase
    {
        MemberHelper _memberHelper = new MemberHelper();
        ClaimHelper _claimHelper = new ClaimHelper();

        [HttpGet]
        [Route("ByMemberId/{id}")]
        public async Task<ActionResult<Member>> GetMemberByMemberId(int id)
        {
            List<Member> members = new List<Member>();
            HttpClient client = _memberHelper.Initial();
            HttpResponseMessage res = await client.GetAsync("HealthCare/GetMembers");

            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                members = JsonConvert.DeserializeObject<List<Member>>(results);
                var member = members.FirstOrDefault(m => m.MemberId == id);
                if(member == null)
                    return NotFound("No member found by this  id " + id);
                return Ok(members);
            }
            return NotFound("No members in database");
        }

        [HttpGet]
        [Route("ByFirstNameAndLastName")]
        public async Task<ActionResult<IList<Member>>> GetMembersFirstNameAndLastName(string? firstName = null, string? lastName = null)
        {
            List<Member> members = new List<Member>();
            HttpClient client = _memberHelper.Initial();
            HttpResponseMessage res = await client.GetAsync("HealthCare/GetMembers");

            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                members = JsonConvert.DeserializeObject<List<Member>>(results);
                var Members = members.Where(m => m.FirstName == firstName && m.LastName == lastName).ToList();
                if (Members.Count == 0)
                    return NotFound("No member found by this name " + firstName + " " +lastName);
                return Ok(Members);
            }
            return NotFound("No members in database");
        }

        [HttpGet]
        [Route("GetMemberByClaim/{id}")]
        public async Task<ActionResult<Member>> GetMemberByClaimId(int id)
        {
            List<Claim> claims = new List<Claim>();
            HttpClient client = _claimHelper.Initial();
            HttpResponseMessage res = await client.GetAsync("HealthCare/GetClaims");

            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                claims = JsonConvert.DeserializeObject<List<Claim>>(results);
                var claim = claims.SingleOrDefault(c => c.ClaimId == id);
                if (claim == null)
                    return NotFound("No member with claim id " + id);

                List<Member> members = new List<Member>();
                HttpClient Client = _memberHelper.Initial();
                HttpResponseMessage Res = await client.GetAsync("HealthCare/GetMembers");

                var Results = Res.Content.ReadAsStringAsync().Result;
                members = JsonConvert.DeserializeObject<List<Member>>(Results);
                var member = members.Single(m => m.MemberId == claim.MemberId);
                Ok(member);

            }
            return NotFound("No claims in database");
        }

        [HttpGet]
        [Route("GetMembersByPhysicianName")]

        public async Task<ActionResult<Member>> GetMembersByPhysicianName(string? name = null)
        {
            List<Member> members = new List<Member>();
            HttpClient client = _memberHelper.Initial();
            HttpResponseMessage res = await client.GetAsync("HealthCare/GetMembers");

            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                members = JsonConvert.DeserializeObject<List<Member>>(results);
                List<Physician> physicians = new List<Physician>();
                HttpResponseMessage Res = await client.GetAsync("HealthCare/Physicians");
                if(Res.IsSuccessStatusCode)
                {
                    var Results = Res.Content.ReadAsStringAsync().Result;
                    physicians = JsonConvert.DeserializeObject<List<Physician>>(Results);
                    Physician physician = new Physician();
                    physician = physicians.Single(p => p.PhysicianName == name);
                    var Members = members.Where( m => m.PhysicianId == physician.PhysicianId).ToList();
                    if(Members.Count == 0)
                        return NotFound("No Physician with physician name " + name);
                    return Ok(Members);
                }
                return NotFound("No physicians in database");
            }
            return NotFound("No members in database");
        }
    }
}
