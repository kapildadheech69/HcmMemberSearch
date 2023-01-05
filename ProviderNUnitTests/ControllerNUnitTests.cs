using Castle.Core.Resource;
using HcmMemberSearch.Controllers;
using HcmMemberSearch.Helper;
using HcmMemberSearch.Modals;
using HcmMemberSearch.Provider;
using HcmMemberSearch.Provider.IProvider;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web.Http;

namespace NUnitTests
{
    [TestFixture]
    public class ControllerNUnitTests
    {
        List<Member> members;
        List<Physician> physicians;
        List<Claim> claims;
        [SetUp]
        public void Setup()
        {
            members = new()
            {
                new Member()
                {
                    MemberId = 2,
                    FirstName ="Kapil",
                    LastName ="Dadheech",
                    UserName = "Kamil.monu@gmail.com",
                    Address ="Ward No 15",
                    State = "Rajasthan",
                    City = "Jhunjhunu",
                    Email = "kamil.monu@gmail.com",
                    DateOfBirth = DateTime.Now, 
                    PhysicianId = 4
                },
                new Member()
                {
                    MemberId = 3,
                    FirstName ="Rohit",
                    LastName ="Dadheech",
                    UserName = "rohit.sonu@gmail.com",
                    Address ="Ward No 15",
                    State = "Rajasthan",
                    City = "Jhunjhunu",
                    Email = "rohit.sonu@gmail.com",
                    DateOfBirth = DateTime.Now,
                    PhysicianId = 6
                }
            };
            physicians = new()
            {
                new Physician(){PhysicianId=1,PhysicianName="John",PhysicianState="UT"},
                new Physician(){PhysicianId=2,PhysicianName="Hari",PhysicianState="UA"},
                new Physician(){PhysicianId=3,PhysicianName="Mittal",PhysicianState="TX"},
                new Physician(){PhysicianId=4,PhysicianName="Patrick",PhysicianState="OH"},
                new Physician(){PhysicianId=5,PhysicianName="Mark",PhysicianState="CA"},
                new Physician(){PhysicianId=6,PhysicianName="Jessica",PhysicianState="NY"},
                new Physician(){PhysicianId=7,PhysicianName="Mary",PhysicianState="IL"},
                new Physician(){PhysicianId=8,PhysicianName="Stacy",PhysicianState="FL"}
            };
            claims = new()
            {
                new Claim(){ClaimId=1,ClaimAmount=45699,ClaimDate=DateTime.Now,ClaimType="Accident",Remarks="",MemberId=2},
                new Claim(){ClaimId=2,ClaimAmount=8900,ClaimDate=DateTime.Now,ClaimType="Other",Remarks="",MemberId=3}
            };
        }

        [Test]
        [TestCase(2)]
        [TestCase(3)]
        public async Task GetMemberById_InputMemberId_ReturnMember(int id)
        {
            var logMock = new Mock<ILogger<MemberSearch>>();
            var claimsMock = new Mock<IClaims>();
            var membersMock = new Mock<IMembers>();
            var physiciansMock = new Mock<IPhysicians>();
            membersMock.Setup(u => u.GetMembersAsync()).ReturnsAsync(members);
            physiciansMock.Setup(u => u.GetPhysicians()).ReturnsAsync(physicians);

            MemberSearch memberSearch = new MemberSearch(claimsMock.Object, membersMock.Object, 
                physiciansMock.Object, logMock.Object);
            var response = await memberSearch.GetMemberByMemberId(id);
            OkObjectResult result = response.Result as OkObjectResult;
            Assert.AreEqual(200, result.StatusCode);
        }
        [Test]
        [TestCase(5)]
        [TestCase(6)]
        public void GetMemberById_InputInvalidId_ReturnsNotFound(int id)
        {
            var logMock = new Mock<ILogger<MemberSearch>>();
            var claimsMock = new Mock<IClaims>();
            var membersMock = new Mock<IMembers>();
            var physiciansMock = new Mock<IPhysicians>();
            membersMock.Setup(u => u.GetMembersAsync()).ReturnsAsync(members);
            physiciansMock.Setup(u => u.GetPhysicians()).ReturnsAsync(physicians);

            MemberSearch memberSearch = new MemberSearch(claimsMock.Object, membersMock.Object,
                physiciansMock.Object, logMock.Object);
            var exceptionDetails = Assert
                .ThrowsAsync<HttpResponseException>(() => memberSearch.GetMemberByMemberId(id));
            Assert.AreEqual(HttpStatusCode.NotFound, exceptionDetails.Response.StatusCode);
        }
        [Test]
        [TestCase("Kapil","Dadheech")]
        [TestCase("Rohit","Dadheech")]
        public async Task GetMember_InputFirstNameAndLastName_OutputMembers(string firstName,string lastName)
        {
            var logMock = new Mock<ILogger<MemberSearch>>();
            var claimsMock = new Mock<IClaims>();
            var membersMock = new Mock<IMembers>();
            var physiciansMock = new Mock<IPhysicians>();
            membersMock.Setup(u => u.GetMembersAsync()).ReturnsAsync(members);
            physiciansMock.Setup(u => u.GetPhysicians()).ReturnsAsync(physicians);

            MemberSearch memberSearch = new MemberSearch(claimsMock.Object, membersMock.Object,
                physiciansMock.Object, logMock.Object);
            var response = await memberSearch.GetMembersFirstNameAndLastName(firstName,lastName);
            OkObjectResult result = response.Result as OkObjectResult;
            Assert.AreEqual(200, result.StatusCode);
        }
        [Test]
        [TestCase("Ranjhana", "Dadheech")]
        [TestCase("Prahlad Rai", "Dadheech")]
        public void GetMember_InputFirstNameAndLastName_OutputNotFound(string fisrtName, string lastName)
        {
            var logMock = new Mock<ILogger<MemberSearch>>();
            var claimsMock = new Mock<IClaims>();
            var membersMock = new Mock<IMembers>();
            var physiciansMock = new Mock<IPhysicians>();
            membersMock.Setup(u => u.GetMembersAsync()).ReturnsAsync(members);
            physiciansMock.Setup(u => u.GetPhysicians()).ReturnsAsync(physicians);

            MemberSearch memberSearch = new MemberSearch(claimsMock.Object, membersMock.Object,
                physiciansMock.Object, logMock.Object);
            var exceptionDetails = Assert
                .ThrowsAsync<HttpResponseException>(() => memberSearch.GetMembersFirstNameAndLastName(fisrtName, lastName));
            Assert.AreEqual(HttpStatusCode.NotFound, exceptionDetails.Response.StatusCode);
        }
        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public async Task GetMember_InputClaimId_OutPutMembers(int id)
        {
            var logMock = new Mock<ILogger<MemberSearch>>();
            var claimsMock = new Mock<IClaims>();
            var membersMock = new Mock<IMembers>();
            var physiciansMock = new Mock<IPhysicians>();
            membersMock.Setup(u => u.GetMembersAsync()).ReturnsAsync(members);
            claimsMock.Setup(u => u.GetClaims()).ReturnsAsync(claims);
            physiciansMock.Setup(u => u.GetPhysicians()).ReturnsAsync(physicians);

            MemberSearch memberSearch = new MemberSearch(claimsMock.Object, membersMock.Object,
                physiciansMock.Object, logMock.Object);
            var response = await memberSearch.GetMemberByClaimId(id);
            OkObjectResult result = response.Result as OkObjectResult;
            Assert.AreEqual(200, result.StatusCode);
        }
        [Test]
        [TestCase(3)]
        [TestCase(4)]
        public void GetMember_InputClaimId_OutputNotFound(int id)
        {
            var logMock = new Mock<ILogger<MemberSearch>>();
            var claimsMock = new Mock<IClaims>();
            var membersMock = new Mock<IMembers>();
            var physiciansMock = new Mock<IPhysicians>();
            membersMock.Setup(u => u.GetMembersAsync()).ReturnsAsync(members);
            claimsMock.Setup(u => u.GetClaims()).ReturnsAsync(claims);
            physiciansMock.Setup(u => u.GetPhysicians()).ReturnsAsync(physicians);

            MemberSearch memberSearch = new MemberSearch(claimsMock.Object, membersMock.Object,
                physiciansMock.Object, logMock.Object);
            var exceptionDetails = Assert
                .ThrowsAsync<HttpResponseException>(() => memberSearch.GetMemberByClaimId(id));
            Assert.AreEqual(HttpStatusCode.NotFound, exceptionDetails.Response.StatusCode);
        }
        [Test]
        [TestCase("Patrick")]
        [TestCase("Jessica")]
        public async Task GetMember_InputPhysicianName_OutputMembers(string name)
        {
            var logMock = new Mock<ILogger<MemberSearch>>();
            var claimsMock = new Mock<IClaims>();
            var membersMock = new Mock<IMembers>();
            var physiciansMock = new Mock<IPhysicians>();
            membersMock.Setup(u => u.GetMembersAsync()).ReturnsAsync(members);
            physiciansMock.Setup(u => u.GetPhysicians()).ReturnsAsync(physicians);

            MemberSearch memberSearch = new MemberSearch(claimsMock.Object, membersMock.Object,
                physiciansMock.Object, logMock.Object);
            var response = await memberSearch.GetMembersByPhysicianName(name);
            OkObjectResult result = response.Result as OkObjectResult;
            Assert.AreEqual(200, result.StatusCode);
        }
        [Test]
        [TestCase("Patric")]
        [TestCase("Jessic")]
        public void GetMembers_InputPhysicianName_OutputNotFound(string name)
        {
            var logMock = new Mock<ILogger<MemberSearch>>();
            var claimsMock = new Mock<IClaims>();
            var membersMock = new Mock<IMembers>();
            var physiciansMock = new Mock<IPhysicians>();
            membersMock.Setup(u => u.GetMembersAsync()).ReturnsAsync(members);
            physiciansMock.Setup(u => u.GetPhysicians()).ReturnsAsync(physicians);

            MemberSearch memberSearch = new MemberSearch(claimsMock.Object, membersMock.Object,
                physiciansMock.Object, logMock.Object);
            var exceptionDetails = Assert
                .ThrowsAsync<InvalidOperationException>(() => memberSearch.GetMembersByPhysicianName(name));
            Assert.AreEqual("Sequence contains no matching element", exceptionDetails.Message);

        }
    }
}
