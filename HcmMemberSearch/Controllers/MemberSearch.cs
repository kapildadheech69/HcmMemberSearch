using HcmMemberSearch.Modals;
using HcmMemberSearch.Provider;
using HcmMemberSearch.Provider.IProvider;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Web.Http;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace HcmMemberSearch.Controllers
{
    [ApiController]
    [Route("HealthCare")]
    public class MemberSearch:ControllerBase
    {
        private readonly IClaims _claims;
        private readonly IMembers _members;
        private readonly IPhysicians _physicians;
        private readonly ILogger<MemberSearch> log;
        public MemberSearch(IClaims claims, IMembers members, IPhysicians physicians, ILogger<MemberSearch> log)
        {
            _claims = claims;
            _members = members;
            _physicians = physicians;
            this.log = log; 
        }

        [HttpGet]
        [Route("ByMemberId/{id}")]
        public async Task<ActionResult<Member>> GetMemberByMemberId(int id)
        {
           
            log.LogInformation("Getting members from member microservice");

            List<Member> members = await _members.GetMembersAsync();

            log.LogInformation("filtering member based on id provided");

            var member = members.SingleOrDefault(m => m.MemberId ==id);
            if (member == null)
            {
                var msg = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("No members found by this id " + id))
                };
                throw new HttpResponseException(msg);
            }
            IEnumerable<Physician> physicians = new List<Physician>();
            log.LogInformation("Getting physician from physician microservice");
            physicians = await _physicians.GetPhysicians();
            Physician physician = new Physician();
            physician = physicians.FirstOrDefault(p => p.PhysicianId == member.PhysicianId);
            member.PhysicianName = physician.PhysicianName;
            return Ok(member);
        }

        [HttpGet]
        [Route("ByFirstNameAndLastName")]
        public async Task<ActionResult<IList<Member>>> GetMembersFirstNameAndLastName(string firstName = null, string lastName = null)
        {
            log.LogInformation("Getting members from member microservice");

            List<Member> members = await _members.GetMembersAsync();

            log.LogInformation("Filtering member based on first and last name");

            var Members = members.Where(m => m.FirstName == firstName && m.LastName == lastName).ToList();
            if (Members.Count == 0)
            {
                var msg = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string
                    .Format("No members found by this name {0} {1}", firstName, lastName))
                };
                throw new HttpResponseException(msg);
            }
            IEnumerable<Physician> physicians = new List<Physician>();
            log.LogInformation("Getting physician from physician microservice");
            physicians = await _physicians.GetPhysicians();
            Physician physician = new Physician();
            foreach (var member in Members)
            {
                physician = physicians.FirstOrDefault(p => p.PhysicianId == member.PhysicianId);
                member.PhysicianName = physician.PhysicianName;
            }
            return Ok(Members);
        }

        [HttpGet]
        [Route("GetMemberByUserName")]
        public async Task<ActionResult<Member>> GetMemberByUserName(string username = null)
        {

            log.LogInformation("Getting members from member microservice");

            List<Member> members = await _members.GetMembersAsync();

            log.LogInformation("filtering member based on id provided");

            var member = members.SingleOrDefault(m => m.UserName == username);
            if (member == null)
            {
                var msg = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("No members found by this username " + username))
                };
                throw new HttpResponseException(msg);
            }
            IEnumerable<Physician> physicians = new List<Physician>();
            log.LogInformation("Getting physician from physician microservice");
            physicians = await _physicians.GetPhysicians();
            Physician physician = new Physician();
            physician = physicians.FirstOrDefault(p => p.PhysicianId == member.PhysicianId);
            member.PhysicianName = physician.PhysicianName;
            return Ok(member);
        }

        [HttpGet]
        [Route("GetMemberByClaim/{id}")]
        public async Task<ActionResult<Member>> GetMemberByClaimId(int id)
        {
            log.LogInformation("Getting claims from claims microservice");

            List<Claim> claims = await _claims.GetClaims();

            log.LogInformation("Filtering claims based on id provided");

            var claim = claims.SingleOrDefault(c => c.ClaimId == id);
            if (claim == null)
            {
                var msg = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("No member with claim id " + id))
                };
                throw new HttpResponseException(msg);
            }
            log.LogInformation("Getting members from member microservice");

            List<Member> members = await _members.GetMembersAsync();

            log.LogInformation("Filtering member");

            var member = members.Single(m => m.MemberId == claim.MemberId);

            IEnumerable<Physician> physicians = new List<Physician>();
            log.LogInformation("Getting physician from physician microservice");
            physicians = await _physicians.GetPhysicians();
            Physician physician = new Physician();
            physician = physicians.SingleOrDefault(p => p.PhysicianId == member.PhysicianId);
            member.PhysicianName = physician.PhysicianName; 
            return Ok(member);
        }

        [HttpGet]
        [Route("GetMembersByPhysicianName")]

        public async Task<ActionResult<Member>> GetMembersByPhysicianName(string name = null)
        {
            log.LogInformation("Getting members from member microservice");

            List<Member> members = await _members.GetMembersAsync();
            IEnumerable<Physician> physicians = new List<Physician>();

            log.LogInformation("Getting physician from physician microservice");

            physicians = await _physicians.GetPhysicians();
            Physician physician = new Physician();

            log.LogInformation("Filtering physician based on physician name");

            physician = physicians.Single(p => p.PhysicianName == name);

            log.LogInformation("Filtering member based on physician name");

            var Members = members.Where(m => m.PhysicianId == physician.PhysicianId).ToList();
            if (Members.Count == 0)
            {
                return NotFound("No members with this physician name" + name);
            }
            foreach (var member in Members)
            {
                member.PhysicianName = physician.PhysicianName; 
            }
            return Ok(Members);
        }
    }
}
