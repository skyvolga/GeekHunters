using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GRS.UnitTests.Common;
using GRS.Web.Controllers;
using GRS.Web.Data;
using GRS.Web.Data.Models;
using GRS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using Moq;
using NUnit.Framework;

namespace GRS.UnitTests.Controller
{
    [TestFixture]
    public class CandidateControllerTest
    {
        #region Actions

        [Test]
        public async Task Index_SingleCandidate_IsShown()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            using (var context = contextCreator())
            {
                await context.AddAsync(new Candidate 
                { 
                    FirstName = "TestFirstName",
                    LastName = "TestLastName"
                });

                await context.SaveChangesAsync();
            }

            var controller = new CandidateController(contextCreator());

            var result = (await controller.Index()) as ViewResult;
            var model = (SearchViewModel) result.Model;

            Assert.AreEqual("TestFirstName", model.Candidates.Single().FirstName);
            Assert.AreEqual("TestLastName", model.Candidates.Single().LastName);
        }

        [Test]
        public async Task Search_View_IsIndex()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            var controller = new CandidateController(contextCreator());

            var result = (await controller.Search(new SearchViewModel())) as ViewResult;
            var model = (SearchViewModel)result.Model;

            Assert.AreEqual(nameof(CandidateController.Index), result.ViewName);
        }

        [Test]
        public async Task Search_CandidateWithSelectedSkill_IsShown()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            using (var context = contextCreator())
            {
                await context.AddAsync(new Candidate
                {
                    Id = 1,
                });

                await context.AddAsync(new CandidateSkill
                {
                    CandidateId = 1,
                    SkillId = 1,
                });

                await context.AddAsync(new Skill
                {
                    Id = 1,
                });

                await context.SaveChangesAsync();
            }

            var controller = new CandidateController(contextCreator());

            var result = (await controller.Search(new SearchViewModel { SkillId = 1})) as ViewResult;
            var model = (SearchViewModel)result.Model;

            Assert.AreEqual(1, model.Candidates.Count());
        }

        [Test]
        public async Task Search_CandidateWithOutSkill_IsNotShown()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            using (var context = contextCreator())
            {
                await context.AddAsync(new Candidate
                {
                    Id = 1,
                });

                await context.SaveChangesAsync();
            }

            var controller = new CandidateController(contextCreator());

            var result = (await controller.Search(new SearchViewModel { SkillId = 1 })) as ViewResult;
            var model = (SearchViewModel)result.Model;

            Assert.AreEqual(0, model.Candidates.Count());
        }
        [Test]
        public async Task Search_SkillIsNotSelected_CandidateIsShown()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            using (var context = contextCreator())
            {
                await context.AddAsync(new Candidate
                {
                    Id = 1,
                    FirstName = "TestFirstName",
                    LastName = "TestLastName"
                });

                await context.SaveChangesAsync();
            }

            var controller = new CandidateController(contextCreator());

            var result = (await controller.Search(new SearchViewModel ())) as ViewResult;
            var model = (SearchViewModel)result.Model;

            Assert.AreEqual(1, model.Candidates.Count());
        }


        [Test]
        public async Task Details_SingleCandidate_IsShown()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            using (var context = contextCreator())
            {
                await context.AddAsync(new Candidate
                {
                    Id = 1,
                    FirstName = "TestFirstName",
                    LastName = "TestLastName"
                });

                await context.SaveChangesAsync();
            }

            var controller = new CandidateController(contextCreator());

            var result = (await controller.Details(1)) as ViewResult;
            var model = (CandidateViewModel)result.Model;

            Assert.AreEqual("TestFirstName", model.FirstName);
            Assert.AreEqual("TestLastName", model.LastName);
        }

        [Test]
        public async Task Details_IdIsNull_RederedtToIndex()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            var controller = new CandidateController(contextCreator());

            var result = (await controller.Details(null)) as NotFoundResult;

            Assert.NotNull(result);
        }

        [Test]
        public async Task Details_InvalidId_RederedtToIndex()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            var controller = new CandidateController(contextCreator());

            var result = (await controller.Details(1)) as NotFoundResult;

            Assert.NotNull(result);
        }

        [Test]
        public async Task Create_Candidate_IsNull()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            var controller = new CandidateController(contextCreator());

            var model = new CandidateViewModel();
            var result = (await controller.Create()) as ViewResult;
            var resultModel = (CandidateViewModel)result.Model;

            Assert.IsNull(resultModel);
        }

        [Test]
        public async Task CreatePost_ModelStateInvalid_RederedtToIndex()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            var controller = new CandidateController(contextCreator());
            controller.ModelState.AddModelError("TetsError", "TetsErrorMessage");

            var model = new CandidateViewModel();
            var result = (await controller.Create(model)) as ViewResult;
            var resultModel = (CandidateViewModel)result.Model;

            Assert.AreSame(model, resultModel);
        }

        [Test]
        public async Task CreatePost_SingleCandidate_RedirectedToIndex()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            var controller = new CandidateController(contextCreator());

            var result = (await controller.Create(new CandidateViewModel ())) as RedirectToActionResult;

            Assert.AreEqual(nameof(CandidateController.Index), result.ActionName);
        }

        [Test]
        public async Task Edit_SingleCandidate_IsShown()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            using (var context = contextCreator())
            {
                await context.AddAsync(new Candidate
                {
                    Id = 1,
                    FirstName = "TestFirstName",
                    LastName = "TestLastName"
                });

                await context.SaveChangesAsync();
            }

            var controller = new CandidateController(contextCreator());

            var result = (await controller.Edit(1)) as ViewResult;
            var model = (CandidateViewModel)result.Model;

            Assert.AreEqual("TestFirstName", model.FirstName);
            Assert.AreEqual("TestLastName", model.LastName);
        }

        [Test]
        public async Task Edit_IdIsNull_RederedtToIndex()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            var controller = new CandidateController(contextCreator());

            var result = (await controller.Edit(null)) as NotFoundResult;

            Assert.NotNull(result);
        }

        [Test]
        public async Task Edit_InvalidId_RederedtToIndex()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            var controller = new CandidateController(contextCreator());

            var result = (await controller.Edit(1)) as NotFoundResult;

            Assert.NotNull(result);
        }


        [Test]
        public async Task EditPost_SingleCandidate_IsRedirectedToIndex()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            using (var context = contextCreator())
            {
                await context.AddAsync(new Candidate
                {
                    Id = 1,
                    FirstName = "TestFirstName",
                    LastName = "TestLastName"
                });

                await context.SaveChangesAsync();
            }

            var controller = new CandidateController(contextCreator());

            var result = (await controller.Edit(1, new CandidateViewModel { Id = 1 })) as RedirectToActionResult;

            Assert.AreEqual(nameof(CandidateController.Index), result.ActionName);
        }

        [Test]
        public async Task EditPost_IdIsNotConsistent_RederedtToIndex()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            var controller = new CandidateController(contextCreator());

            var result = (await controller.Edit(1, new CandidateViewModel { Id = 2 })) as NotFoundResult;

            Assert.NotNull(result);
        }

        [Test]
        public async Task EditPost_ModelStateInvalid_RederedtToIndex()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            var controller = new CandidateController(contextCreator());
            controller.ModelState.AddModelError("TetsError", "TetsErrorMessage");

            var model = new CandidateViewModel { Id = 1 };
            var result = (await controller.Edit(1, model)) as ViewResult;
            var resultModel = (CandidateViewModel) result.Model;

            Assert.AreSame(model, resultModel);
        }

        [Test]
        public void EditPost_DbUpdateConcurrencyExceptionAndCandidateExists_ThrowException()
        {
            var contextOptions = new DbContextOptions<ApplicationDbContext>();
            var contextMock = new Mock<ApplicationDbContext>(contextOptions);
            contextMock.Setup(x => x.Candidates).Returns(Helper.GetQueryableMockDbSet(new Candidate { Id = 1 }));
            contextMock.Setup(x => x.Entry(It.IsAny<object>()))
            .Throws(new DbUpdateConcurrencyException("exception",
                 new List<IUpdateEntry>() { new Mock<IUpdateEntry>().Object }));

            var controller = new CandidateController(contextMock.Object);

            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            {
                var result = await controller.Edit(1, new CandidateViewModel { Id = 1 });
            });
        }

        [Test]
        public async Task EditPost_DbUpdateConcurrencyExceptionAnd_NOT_CandidateExists_NotFoundIsShown()
        {
            var contextOptions = new DbContextOptions<ApplicationDbContext>();
            var contextMock = new Mock<ApplicationDbContext>(contextOptions);
            contextMock.Setup(x => x.Candidates).Returns(Helper.GetQueryableMockDbSet(new Candidate { Id = 2}));
            contextMock.Setup(x => x.Entry(It.IsAny<object>()))
            .Throws(new DbUpdateConcurrencyException("exception",
                 new List<IUpdateEntry>() { new Mock<IUpdateEntry>().Object }));

            var controller = new CandidateController(contextMock.Object);

            var result = (await controller.Edit(1, new CandidateViewModel { Id = 1 })) as NotFoundResult;

            Assert.NotNull(result);
        }


        [Test]
        public async Task Delete_SingleCandidate_Deleted()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            using (var context = contextCreator())
            {
                await context.AddAsync(new Candidate
                {
                    Id = 1,
                    FirstName = "TestFirstName",
                    LastName = "TestLastName"
                });

                await context.SaveChangesAsync();
            }

            var controller = new CandidateController(contextCreator());

            var result = (await controller.Delete(1)) as RedirectToActionResult;

            Assert.AreEqual(nameof(CandidateController.Index), result.ActionName);
        }

        [Test]
        public async Task Delete_IdIsNull_RederedtToIndex()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            var controller = new CandidateController(contextCreator());

            var result = (await controller.Details(null)) as NotFoundResult;

            Assert.NotNull(result);
        }

        [Test]
        public async Task Delete_InvalidId_RederedtToIndex()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            var controller = new CandidateController(contextCreator());

            var result = (await controller.Details(1)) as NotFoundResult;

            Assert.NotNull(result);
        }

        #endregion

        #region Public Methods

        [Test]
        public async Task UpdateCandidateAsync_AddNewCandidateWithSkill_CandidateWasAdded()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            using (var context = contextCreator())
            {
                context.Add(new Skill { Id = 1, Name = "TestSkill" });
                context.SaveChanges();
            }

            using (var context = contextCreator())
            {
                var controller = new CandidateController(context);

                var result = await controller.UpdateCandidateAsync(
                    new CandidateViewModel
                    {
                        FirstName = "NewName",
                        LastName = "NewLastName",
                        Skills = new List<int> { 1 },
                    });

                context.SaveChanges();
            }

            using (var context = contextCreator())
            {
                var candidate = context.Candidates
                                    .Include(x => x.CandidateSkills)
                                    .ThenInclude(x => x.Skill)
                                    .Single();

                Assert.AreEqual("TestSkill", candidate.CandidateSkills.Single().Skill.Name);
            }
        }

        [Test]
        public async Task UpdateCandidateAsync_RemoveSkill_SkillWasRemoved()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            using (var context = contextCreator())
            {
                context.Add(new Candidate { Id = 1, FirstName = "TestFisrtName" });
                context.Add(new Skill { Id = 1, Name = "TestSkill" });
                context.Add(new CandidateSkill { CandidateId = 1, SkillId = 1});
                context.SaveChanges();
            }

            using (var context = contextCreator())
            {
                var controller = new CandidateController(context);

                var result = await controller.UpdateCandidateAsync(
                    new CandidateViewModel
                    {
                        Id = 1,
                        FirstName = "NewName",
                        LastName = "NewLastName",
                        Skills = new List<int> { },
                    });

                context.SaveChanges();
            }

            using (var context = contextCreator())
            {
                var candidate = context.Candidates
                                    .Include(x => x.CandidateSkills)
                                    .ThenInclude(x => x.Skill)
                                    .Single();

                Assert.AreEqual(0, candidate.CandidateSkills.Count());
            }
        }

        #endregion
    }
}
