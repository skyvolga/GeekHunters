using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GRS.Web.Models;
using NUnit.Framework;

namespace GRS.UnitTests.Models
{
    [TestFixture]
    public class CandidateViewModelTest
    {
        [Test]
        public void Validate_ValidName_Success()
        {
            var model = new CandidateViewModel
            {
                FirstName = "TestFIrstName",
                LastName = "TestLastName"
            };

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(model, new ValidationContext(model), validationResults, true);

            Assert.IsTrue(actual);
            Assert.AreEqual(0, validationResults.Count);
        }

        [Test]
        public void Validate_EmptyFirstName_Error()
        {
            var model = new CandidateViewModel
            {
                FirstName = "",
                LastName = "TestLastNanme"
            };

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(model, new ValidationContext(model), validationResults, true);

            Assert.IsFalse(actual);
            Assert.AreEqual(1, validationResults.Count);
        }

        [Test]
        public void Validate_EmptyLastName_Error()
        {
            var model = new CandidateViewModel
            {
                FirstName = "TestFirstName",
                LastName = ""
            };

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(model, new ValidationContext(model), validationResults, true);

            Assert.IsFalse(actual);
            Assert.AreEqual(1, validationResults.Count);
        }
    }
}
