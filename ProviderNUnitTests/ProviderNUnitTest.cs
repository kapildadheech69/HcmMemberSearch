using HcmMemberSearch.Helper;
using HcmMemberSearch.Helper.IHelper;
using HcmMemberSearch.Modals;
using HcmMemberSearch.Provider;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NUnitTests
{
    [TestFixture]
    public class ProviderNUnitTest
    {
        Physicians _physicians;
        IEnumerable<Physician> physicians;
        [SetUp]
        public void SetUp()
        {
            _physicians = new Physicians(new MemberHelper());
            physicians = new List<Physician>()
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
        }
        [Test]
        public async Task GetMembers_OuputCount()
        {
            Members _members = new Members(new MemberHelper());
            List<Member> members = await _members.GetMembersAsync();
            Assert.That(members.Count, Is.EqualTo(4));
        }
        [Test]
        public async Task GetClaims_OutputCount()
        {
            Claims _claims = new Claims(new ClaimHelper());
            List<Claim> claims = await _claims.GetClaims();
            Assert.That(claims.Count, Is.EqualTo(1));
        }
        [Test]
        public async Task GetPhysicians_OutputPhysicians()
        {
            IEnumerable<Physician> result = await _physicians.GetPhysicians();
            CollectionAssert.AreEqual(result, physicians);
        } 
    }
}
