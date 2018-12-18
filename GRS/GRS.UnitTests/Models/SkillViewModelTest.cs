using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GRS.Web.Models;
using NUnit.Framework;

namespace GRS.UnitTests.Models
{
    [TestFixture]
    public class SkillViewModelTest
    {
        [Test]
        public void Validate_ValidName_Success()
        {
            var model = new SkillViewModel
            {
                Name = "TestName"
            };

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(model, new ValidationContext(model), validationResults, true);

            Assert.IsTrue(actual);
            Assert.AreEqual(0, validationResults.Count);
        }

        [Test]
        public void Validate_EmptyName_Error()
        {
            var model = new SkillViewModel
            {
                Name = ""
            };

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(model, new ValidationContext(model), validationResults, true);

            Assert.IsFalse(actual);
            Assert.AreEqual(1, validationResults.Count);
        }
    }
}
