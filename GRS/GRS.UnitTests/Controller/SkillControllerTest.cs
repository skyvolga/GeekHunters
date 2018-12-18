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
    public class SkillControllerTest
    {
        [Test]
        public async Task Index_SingleSkill_IsShown()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            using (var context = contextCreator())
            {
                await context.AddAsync(new Skill
                {
                    Name = "TestName",
                });

                await context.SaveChangesAsync();
            }

            var controller = new SkillController(contextCreator());

            var result = (await controller.Index()) as ViewResult;
            var model = (List<SkillViewModel>)result.Model;

            Assert.AreEqual("TestName", model.Single().Name);
        }

        [Test]
        public async Task Details_SingleSkill_IsShown()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            using (var context = contextCreator())
            {
                await context.AddAsync(new Skill
                {
                    Id = 1,
                    Name = "TestName",
                });

                await context.SaveChangesAsync();
            }

            var controller = new SkillController(contextCreator());

            var result = (await controller.Details(1)) as ViewResult;
            var model = (SkillViewModel)result.Model;

            Assert.AreEqual("TestName", model.Name);
        }

        [Test]
        public async Task Details_IdIsNull_RederedtToIndex()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            var controller = new SkillController(contextCreator());

            var result = (await controller.Details(null)) as NotFoundResult;

            Assert.NotNull(result);
        }

        [Test]
        public async Task Details_InvalidId_RederedtToIndex()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            var controller = new SkillController(contextCreator());

            var result = (await controller.Details(1)) as NotFoundResult;

            Assert.NotNull(result);
        }

        [Test]
        public async Task Edit_SingleSkill_IsShown()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            using (var context = contextCreator())
            {
                await context.AddAsync(new Skill
                {
                    Id = 1,
                    Name = "TestName",
                });

                await context.SaveChangesAsync();
            }

            var controller = new SkillController(contextCreator());

            var result = (await controller.Edit(1)) as ViewResult;
            var model = (SkillViewModel)result.Model;

            Assert.AreEqual("TestName", model.Name);
        }

        [Test]
        public async Task Edit_IdIsNull_RederedtToIndex()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            var controller = new SkillController(contextCreator());

            var result = (await controller.Edit(null)) as NotFoundResult;

            Assert.NotNull(result);
        }

        [Test]
        public async Task Edit_InvalidId_RederedtToIndex()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            var controller = new SkillController(contextCreator());

            var result = (await controller.Edit(1)) as NotFoundResult;

            Assert.NotNull(result);
        }


        [Test]
        public async Task EditPost_SingleSkill_IsShown()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            using (var context = contextCreator())
            {
                await context.AddAsync(new Skill
                {
                    Id = 1,
                    Name = "TestName",
                });

                await context.SaveChangesAsync();
            }

            var controller = new SkillController(contextCreator());

            var result = (await controller.Edit(1, new SkillViewModel { Id = 1 })) as RedirectToActionResult;

            Assert.AreEqual(nameof(SkillController.Index), result.ActionName);
        }

        [Test]
        public async Task EditPost_IdIsNotConsistent_RederedtToIndex()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            var controller = new SkillController(contextCreator());

            var result = (await controller.Edit(1, new SkillViewModel { Id = 2 })) as NotFoundResult;

            Assert.NotNull(result);
        }

        [Test]
        public async Task EditPost_ModelStateInvalid_RederedtToIndex()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            var controller = new SkillController(contextCreator());
            controller.ModelState.AddModelError("TetsError", "TetsErrorMessage");

            var model = new SkillViewModel { Id = 1 };
            var result = (await controller.Edit(1, model)) as ViewResult;
            var resultModel = (SkillViewModel)result.Model;

            Assert.AreSame(model, resultModel);
        }

        [Test]
        public void EditPost_DbUpdateConcurrencyExceptionAndSkillExists_ThrowException()
        {
            var contextOptions = new DbContextOptions<ApplicationDbContext>();
            var contextMock = new Mock<ApplicationDbContext>(contextOptions);
            contextMock.Setup(x => x.Skills).Returns(Helper.GetQueryableMockDbSet(new Skill { Id = 1 }));
            contextMock.Setup(x => x.Entry(It.IsAny<object>()))
            .Throws(new DbUpdateConcurrencyException("exception",
                 new List<IUpdateEntry>() { new Mock<IUpdateEntry>().Object }));

            var controller = new SkillController(contextMock.Object);

            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            {
                var result = await controller.Edit(1, new SkillViewModel { Id = 1 });
            });
        }

        [Test]
        public async Task EditPost_DbUpdateConcurrencyExceptionAnd_NOT_SkillExists_NotFoundIsShown()
        {
            var contextOptions = new DbContextOptions<ApplicationDbContext>();
            var contextMock = new Mock<ApplicationDbContext>(contextOptions);
            contextMock.Setup(x => x.Skills).Returns(Helper.GetQueryableMockDbSet(new Skill { Id = 2 }));
            contextMock.Setup(x => x.Entry(It.IsAny<object>()))
            .Throws(new DbUpdateConcurrencyException("exception",
                 new List<IUpdateEntry>() { new Mock<IUpdateEntry>().Object }));

            var controller = new SkillController(contextMock.Object);

            var result = (await controller.Edit(1, new SkillViewModel { Id = 1 })) as NotFoundResult;

            Assert.NotNull(result);
        }


        [Test]
        public async Task Delete_SingleSkill_Deleted()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            using (var context = contextCreator())
            {
                await context.AddAsync(new Skill
                {
                    Id = 1,
                    Name = "TestName",
                });

                await context.SaveChangesAsync();
            }

            var controller = new SkillController(contextCreator());

            var result = (await controller.Delete(1)) as RedirectToActionResult;

            Assert.AreEqual(nameof(SkillController.Index), result.ActionName);
        }

        [Test]
        public async Task Delete_IdIsNull_RederedtToIndex()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            var controller = new SkillController(contextCreator());

            var result = (await controller.Details(null)) as NotFoundResult;

            Assert.NotNull(result);
        }

        [Test]
        public async Task Delete_InvalidId_RederedtToIndex()
        {
            var contextCreator = Helper.InMemoryContextCreator();

            var controller = new SkillController(contextCreator());

            var result = (await controller.Details(1)) as NotFoundResult;

            Assert.NotNull(result);
        }
    }
}
