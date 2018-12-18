using System;
using System.Linq;
using GRS.UnitTests.Common;
using GRS.Web.Data.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace GRS.UnitTests.Data
{
    [TestFixture]
    public class ApplicationDbContextTest
    {
        [Test]
        public void Candidates_AddNewCandidate_CandidateWasCreated()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            using (var context = contextCreator())
            {
                context.Candidates.Add(new Candidate
                {
                    FirstName = "TestFirstName",
                    LastName ="TestLastName",
                });

                context.SaveChanges();
            }

            using (var context = contextCreator())
            {
                var savedModel = context.Candidates.Single();

                Assert.AreEqual("TestFirstName", savedModel.FirstName);
                Assert.AreEqual("TestLastName", savedModel.LastName);
            }
        }

        [Test]
        public void Candidates_AddNewSkill_CandidateWasCreated()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            using (var context = contextCreator())
            {
                context.Skills.Add(new Skill
                {
                    Name = "TestSkill",
                });

                context.SaveChanges();
            }

            using (var context = contextCreator())
            {
                var savedModel = context.Skills.Single();

                Assert.AreEqual("TestSkill", savedModel.Name);
            }
        }


        [Test]
        public void Candidates_AddCandidateWithNewSkill_CandidateWithSkillWasCreated()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            using (var context = contextCreator())
            {
                context.Candidates.Add(new Candidate
                {
                    Id = 1,
                    FirstName = "TestFirstName",
                    LastName = "TestLastName",
                });


                context.Skills.Add(new Skill
                {
                    Id = 2,
                    Name = "TestSkill",
                });

                context.CandidateSkills.Add(new CandidateSkill
                {
                    CandidateId = 1,
                    SkillId = 2
                });

                context.SaveChanges();
            }

            using (var context = contextCreator())
            {
                var savedModel = context.CandidateSkills
                    .Include(x => x.Candidate)
                    .Include(x => x.Skill)
                    .Single();

                Assert.AreEqual("TestFirstName", savedModel.Candidate.FirstName);
                Assert.AreEqual("TestSkill", savedModel.Skill.Name);
            }
        }
    }
}
