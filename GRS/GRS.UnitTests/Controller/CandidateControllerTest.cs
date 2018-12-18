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
            var model = (List<CandidateViewModel>) result.Model;

            Assert.AreEqual("TestFirstName", model.Single().FirstName);
            Assert.AreEqual("TestLastName", model.Single().LastName);
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
        public async Task EditPost_SingleCandidate_IsShown()
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
            var contextMock = new Mock<ApplicationDbContext>();
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
            var contextMock = new Mock<ApplicationDbContext>();
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
    }
}
