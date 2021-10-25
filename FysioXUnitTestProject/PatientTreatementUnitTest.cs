using AvansFysioOpdrachtIndividueel;
using AvansFysioOpdrachtIndividueel.Controllers;
using AvansFysioOpdrachtIndividueel.Models;
using Core.Data.Data;
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
    public class PatientTreatementUnitTest
    {
        Mock<IPatientRepo> patientRepoMoq;
        Mock<ITherapistRepo> therapistRepoMoq;
        // Mock<ITreatmentManager> treatmentManagerMoq;
        Mock<IRepo<TreatmentModel>> treatmentRepoMoq;
        SQLTreatmentManager treatmentManagerMoq;
        Mock<IRepo<PersonModel>> personRepoMoq;
        PatientModelsController patientController;
        Mock<IAsyncRepo<DiagnosisModel>> diagnosisRepoMoq;
        Mock<UserManager<IdentityUser>> mgr;
        PatientTreatmentModelsController sut;
        PatientDossierModelsController patientDossierController;
        public PatientTreatementUnitTest()
        {
            patientRepoMoq = new Mock<IPatientRepo>();
            therapistRepoMoq = new Mock<ITherapistRepo>();
            personRepoMoq = new Mock<IRepo<PersonModel>>();
            treatmentRepoMoq = new Mock<IRepo<TreatmentModel>>();

            var store = new Mock<IUserStore<IdentityUser>>();

            diagnosisRepoMoq = new Mock<IAsyncRepo<DiagnosisModel>>();
            treatmentManagerMoq = new SQLTreatmentManager(patientRepoMoq.Object, therapistRepoMoq.Object, treatmentRepoMoq.Object);

            mgr = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);

            patientController = new PatientModelsController(patientRepoMoq.Object, therapistRepoMoq.Object, mgr.Object, personRepoMoq.Object);
            patientDossierController = new PatientDossierModelsController(patientRepoMoq.Object, therapistRepoMoq.Object, diagnosisRepoMoq.Object);

            sut = new PatientTreatmentModelsController(patientRepoMoq.Object, therapistRepoMoq.Object, treatmentManagerMoq);
        }

        [Fact]
        public void Patient_Can_Cancel_Treatment()
        {
            // Arrange
            personRepoMoq.Setup(m => m.Get()).Returns(new List<PersonModel> { });

            var therapist = new TeacherModel()
            {
                BIGNumber = 100,
                Email = "kevin@outlook.com",
                Name = "Kevin Verhoeven",
                Id = 1,
                PersonnelNumber = 1001
            };
            var patient = new PatientModel
            {
                Id = 1,
                Name = "Leo",
                // Old enough
                DateOfBirth = new DateTime(1999, 8, 24),
                Email = "Leo@outlook.com",
                Gender = "Man",
                PatientNumber = 132,
                TeacherOrStudentNumber = 33,
                PatientDossier = new PatientDossierModel
                {
                    Id = 1,
                    //DiagnosisCode = "3",
                    IssueDescription = "Test description",
                    PlannedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                    DueDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, DateTime.Now.Day),
                    Therapist = therapist,
                    IntakeDoneBy = therapist,
                    IntakeSupervisedBy = therapist,
                    DiagnosisCode = new DiagnosisModel
                    {
                        Id = 1,
                        CodeAndDescription = "22 - Test"
                    },
                    TreatmentPlan = new TreatmentPlanModel
                    {
                        Id = 1,
                        AmountOfTreaments = 3,
                        TimeOfTreatment = 120
                    },
                    Treatments = new List<TreatmentModel>
                    {
                        new TreatmentModel
                        {
                            Complications = "None",
                            Description = "Test treatment 1",
                            Id = 1,
                            TreatmentDoneBy = therapist,
                            TreatmentRoomOrTrainingRoom = false,
                            TreatmentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                            // Hard coded because the auto generation is done by the controller wich is not tested.
                            TreatmentUntil = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, DateTime.Now.Hour + 2, DateTime.Now.Minute, DateTime.Now.Second),
                            VektisType = 1
                        }
                    }
                }
            };
            var treatmentViewModel = new TreatmentFormViewModel
            {
                Id = 1,
                Treatment = patient.PatientDossier.Treatments.First(),
                TherapyDoneById = 1
            };

            patientRepoMoq.Setup(m => m.Get(1)).Returns(patient);
            therapistRepoMoq.Setup(m => m.Get(1)).Returns(therapist);
            
            List<TherapistModel> list = new List<TherapistModel>();
            list.Add(therapist);
            
            therapistRepoMoq.Setup(m => m.Get()).Returns(list);

            List<TreatmentModel> treatmentList = new List<TreatmentModel>();

            treatmentRepoMoq.Setup(m => m.Get()).Returns(treatmentList);

            //sut.Create(tvm, patient.Id);

            // Act
            var result = sut.Create(treatmentViewModel, patient.Id);
            // Assert
            // if the controller redirected that means the person got created succesfully.
            // if it is a viewresult, the create failed
            Assert.Equal(typeof(RedirectToActionResult), result.GetType());
            Assert.True(sut.ModelState.IsValid);
        }

        [Fact]
        public void Patient_Can_Not_Cancel_Treatment()
        {
            // Arrange
            personRepoMoq.Setup(m => m.Get()).Returns(new List<PersonModel> { });

            var therapist = new TeacherModel()
            {
                BIGNumber = 100,
                Email = "kevin@outlook.com",
                Name = "Kevin Verhoeven",
                Id = 1,
                PersonnelNumber = 1001
            };
            var patient = new PatientModel
            {
                Id = 1,
                Name = "Leo",
                // Old enough
                DateOfBirth = new DateTime(1999, 8, 24),
                Email = "Leo@outlook.com",
                Gender = "Man",
                PatientNumber = 132,
                TeacherOrStudentNumber = 33,
                PatientDossier = new PatientDossierModel
                {
                    Id = 1,
                    //DiagnosisCode = "3",
                    IssueDescription = "Test description",
                    PlannedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                    DueDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, DateTime.Now.Day),
                    Therapist = therapist,
                    IntakeDoneBy = therapist,
                    IntakeSupervisedBy = therapist,
                    DiagnosisCode = new DiagnosisModel
                    {
                        Id = 1,
                        CodeAndDescription = "22 - Test"
                    },
                    TreatmentPlan = new TreatmentPlanModel
                    {
                        Id = 1,
                        AmountOfTreaments = 3,
                        TimeOfTreatment = 120
                    },
                    Treatments = new List<TreatmentModel>
                    {
                        new TreatmentModel
                        {
                            Complications = "None",
                            Description = "Test treatment 1",
                            Id = 1,
                            TreatmentDoneBy = therapist,
                            TreatmentRoomOrTrainingRoom = false,
                            TreatmentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                            // Hard coded because the auto generation is done by the controller wich is not tested.
                            TreatmentUntil = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, DateTime.Now.Hour + 2, DateTime.Now.Minute, DateTime.Now.Second),
                            VektisType = 1
                        }
                    }
                }
            };
            var treatmentViewModel = new TreatmentFormViewModel
            {
                Id = 1,
                Treatment = patient.PatientDossier.Treatments.First(),
                TherapyDoneById = 1
            };

            patientRepoMoq.Setup(m => m.Get(1)).Returns(patient);
            therapistRepoMoq.Setup(m => m.Get(1)).Returns(therapist);

            List<TherapistModel> list = new List<TherapistModel>();
            list.Add(therapist);

            therapistRepoMoq.Setup(m => m.Get()).Returns(list);

            List<TreatmentModel> treatmentList = new List<TreatmentModel>();

            treatmentRepoMoq.Setup(m => m.Get()).Returns(treatmentList);

            //sut.Create(tvm, patient.Id);

            // Act
            var result = sut.Create(treatmentViewModel, patient.Id);
            // Assert
            // if the controller redirected that means the person got created succesfully.
            // if it is a viewresult, the create failed
            Assert.Equal(typeof(RedirectToActionResult), result.GetType());
            Assert.True(sut.ModelState.IsValid);
        }
    }
}
