
using AvansFysioOpdrachtIndividueel.Controllers;
using AvansFysioOpdrachtIndividueel.Models;
using Core.Domain.Domain;
using Core.DomainServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FysioXUnitTestProject
{    
    public class PatientUnitTest
    {
        // these tests test if the controller returns the right view and if the validation validates the right way. 
        // So every test tests if the Modelstate validates correctly (Assert.True(Modelstate.Isvalid) or not.
        // And every test checks if the controller returns the right action result.
        // To test that a model error gets passed to the controller.

        // Every BR gets tested in the happy and unhappy flow, and then the edge cases. (4 tests each)

        Mock<IPatientRepo> patientRepoMoq;
        Mock<ITherapistRepo> therapistRepoMoq;
        Mock<UserManager<IdentityUser>> mgr;
        Mock<IRepo<PersonModel>> personRepoMoq;
        PatientModelsController sut;
        public PatientUnitTest()
        {
            patientRepoMoq = new Mock<IPatientRepo>();
            therapistRepoMoq = new Mock<ITherapistRepo>();
            personRepoMoq = new Mock<IRepo<PersonModel>>();
            var store = new Mock<IUserStore<IdentityUser>>();
            mgr = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
            sut = new PatientModelsController(patientRepoMoq.Object, therapistRepoMoq.Object,mgr.Object, personRepoMoq.Object);

            personRepoMoq.Setup(m => m.Get()).Returns(new List<PersonModel> { });
        }
        [Fact]
        public void Patient_Controller_Add_Patient_Works()
        {
            // Arrange
            var patient = new PatientModel
            {
                Id = 1,
                Name = "Leo",
                // Old enough
                DateOfBirth = new DateTime(1999, 8, 24),
                Email = "Leo@outlook.com",
                Gender = "Man",
                PatientNumber = 132,
                TeacherOrStudentNumber = 33
            };

            // Act
            IActionResult result = sut.Create(patient);
            // Assert
            // if the controller redirected that means the person got created succesfully.
            // if it is a viewresult, the create failed
            Assert.Equal(typeof(RedirectToActionResult), result.GetType());
            Assert.True(sut.ModelState.IsValid);
        }
        [Fact]
        public void Patient_Controller_Add_Patient_Too_Young_Fails()
        {
            var patient = new PatientModel
            {
                Id = 1,
                Name = "Leo",
                // Way too young
                DateOfBirth = new DateTime(2011, 8, 24),
                Email = "Leo@outlook.com",
                Gender = "Man",
                PatientNumber = 132,
                TeacherOrStudentNumber = 33,
                PatientDossier = new PatientDossierModel()
            };
            // add the error we need to test the controller
            sut.ModelState.AddModelError("Age Error", "Patient too young");
            // check if the validation works.
            var validationResultList = new List<ValidationResult>();
            bool validateResult = Validator.TryValidateObject(patient, new ValidationContext(patient), validationResultList);
            // Act
            IActionResult result = sut.Create(patient);

            // Assert
            // if the controller returned a viewresult that means the person did not get created
            Assert.Equal(typeof(ViewResult), result.GetType());
            Assert.False(validateResult);
        }
        [Fact]
        public void Patient_Controller_Add_Patient_Too_Young_Fails_EdgeCase()
        {
            var patient = new PatientModel
            {
                Id = 1,
                Name = "Leo",
                // One day too young
                DateOfBirth = new DateTime(DateTime.Now.Year-16,DateTime.Now.Month , DateTime.Now.Day+1),
                Email = "Leo@outlook.com",
                Gender = "Man",
                PatientNumber = 132,
                TeacherOrStudentNumber = 33,
                PatientDossier = new PatientDossierModel()
            };

            sut.ModelState.AddModelError("Age Error", "Patient too young");

            var validationResultList = new List<ValidationResult>();
            bool validateResult = Validator.TryValidateObject(patient, new ValidationContext(patient), validationResultList);

            // Act
            IActionResult result = sut.Create(patient);
            // Assert
            Assert.Equal(typeof(ViewResult), result.GetType());
            Assert.False(validateResult);
        }

        [Fact]
        public void Patient_Controller_Add_Patient_Too_Young_Succeeds_EdgeCase()
        {
            var patient = new PatientModel
            {
                Id = 1,
                Name = "Leo",
                // One day old enough
                DateOfBirth = new DateTime(DateTime.Now.Year - 16, DateTime.Now.Month, DateTime.Now.Day),
                Email = "Leo@outlook.com",
                Gender = "Man",
                PatientNumber = 132,
                TeacherOrStudentNumber = 33,
                PatientDossier = new PatientDossierModel()
            };
            
            var validationResultList = new List<ValidationResult>();
            bool validateResult = Validator.TryValidateObject(patient, new ValidationContext(patient), validationResultList);

            // Act
            IActionResult result = sut.Create(patient);
            // Assert
            Assert.Equal(typeof(RedirectToActionResult), result.GetType());
            Assert.True(validateResult);
        }
    }
}
