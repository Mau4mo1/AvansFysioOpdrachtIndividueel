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
        Mock<IVektisRepo> vektisRepoMoq;

        public PatientTreatementUnitTest()
        {
            patientRepoMoq = new Mock<IPatientRepo>();
            therapistRepoMoq = new Mock<ITherapistRepo>();
            personRepoMoq = new Mock<IRepo<PersonModel>>();
            treatmentRepoMoq = new Mock<IRepo<TreatmentModel>>();
            vektisRepoMoq = new Mock<IVektisRepo>();


            diagnosisRepoMoq = new Mock<IAsyncRepo<DiagnosisModel>>();
            treatmentManagerMoq = new SQLTreatmentManager(patientRepoMoq.Object, therapistRepoMoq.Object, treatmentRepoMoq.Object);

            var store = new Mock<IUserStore<IdentityUser>>();
            mgr = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
            // by default the usermanager should return a registered user. only in the last couple of unit tests is where this shouldn't be the case.
            
            patientController = new PatientModelsController(patientRepoMoq.Object, therapistRepoMoq.Object, mgr.Object, personRepoMoq.Object);
            patientDossierController = new PatientDossierModelsController(patientRepoMoq.Object, therapistRepoMoq.Object, diagnosisRepoMoq.Object);

            sut = new PatientTreatmentModelsController(patientRepoMoq.Object, therapistRepoMoq.Object, treatmentManagerMoq, vektisRepoMoq.Object,mgr.Object);

        }
        // Cancel treatment BR_6
        [Fact]
        public void Patient_Can_Cancel_Treatment()
        {
            // Arrange
            personRepoMoq.Setup(m => m.Get()).Returns(new List<PersonModel> { });

            IdentityUser user = new IdentityUser
            {
                UserName = "Leo@outlook.com",
                Email = "Leo@outlook.com",
                NormalizedEmail = "Leo@outlook.com"
            };

            mgr.Setup(m => m.FindByEmailAsync("Leo@outlook.com")).Returns(Task.FromResult(user));

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
                            TreatmentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 2, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                            // Hard coded because the auto generation is done by the controller wich is not tested.
                            TreatmentUntil = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 2, DateTime.Now.Hour + 2, DateTime.Now.Minute, DateTime.Now.Second),
                            VektisType = new VektisModel
                            {
                                Id = 1,
                                NeedsDescription = true,
                                Value = "1"
                            }
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

            var result = sut.Delete(patient.Id, patient.PatientDossier.Treatments.First().Id);
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

            IdentityUser user = new IdentityUser
            {
                UserName = "Leo@outlook.com",
                Email = "Leo@outlook.com",
                NormalizedEmail = "Leo@outlook.com"
            };

            mgr.Setup(m => m.FindByEmailAsync("Leo@outlook.com")).Returns(Task.FromResult(user));

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
                            VektisType = new VektisModel
                            {
                                Id = 1,
                                NeedsDescription = true,
                                Value = "1"
                            }
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

            var result = sut.Delete(patient.Id, patient.PatientDossier.Treatments.First().Id);

            // Assert
            Assert.Equal(typeof(RedirectToActionResult), result.GetType());
            Assert.False(sut.ModelState.IsValid);
        }
        [Fact]
        public void Patient_Can_Not_Cancel_Treatment_Edge_Case()
        {
            // Arrange
            personRepoMoq.Setup(m => m.Get()).Returns(new List<PersonModel> { });

            IdentityUser user = new IdentityUser
            {
                UserName = "Leo@outlook.com",
                Email = "Leo@outlook.com",
                NormalizedEmail = "Leo@outlook.com"
            };

            mgr.Setup(m => m.FindByEmailAsync("Leo@outlook.com")).Returns(Task.FromResult(user));

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
                            TreatmentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                            // Hard coded because the auto generation is done by the controller wich is not tested.
                            TreatmentUntil = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour + 2, DateTime.Now.Minute, DateTime.Now.Second),
                            VektisType = new VektisModel
                            {
                                Id = 1,
                                NeedsDescription = true,
                                Value = "1"
                            }
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

            var result = sut.Delete(patient.Id, patient.PatientDossier.Treatments.First().Id);

            // Assert
            Assert.Equal(typeof(RedirectToActionResult), result.GetType());
            Assert.False(sut.ModelState.IsValid);
        }
        [Fact]
        public void Patient_Can_Cancel_Treatment_Edge_Case()
        {
            // Arrange
            personRepoMoq.Setup(m => m.Get()).Returns(new List<PersonModel> { });

            IdentityUser user = new IdentityUser
            {
                UserName = "Leo@outlook.com",
                Email = "Leo@outlook.com",
                NormalizedEmail = "Leo@outlook.com"
            };

            mgr.Setup(m => m.FindByEmailAsync("Leo@outlook.com")).Returns(Task.FromResult(user));
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
                            TreatmentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+1, DateTime.Now.Hour, DateTime.Now.Minute+5, DateTime.Now.Second),
                            // Hard coded because the auto generation is done by the controller wich is not tested.
                            TreatmentUntil = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+1, DateTime.Now.Hour + 2, DateTime.Now.Minute, DateTime.Now.Second),
                            VektisType = new VektisModel
                            {
                                Id = 1,
                                NeedsDescription = true,
                                Value = "1"
                            }
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

            var result = sut.Delete(patient.Id, patient.PatientDossier.Treatments.First().Id);

            // Assert
            Assert.Equal(typeof(RedirectToActionResult), result.GetType());
            Assert.True(sut.ModelState.IsValid);
        }
        //  Add treatment BR_1
        [Fact]
        public async void Patient_Can_Add_Treatment_Edge_Case()
        {
            // Arrange

            IdentityUser user = new IdentityUser
            {
                UserName = "Leo@outlook.com",
                Email = "Leo@outlook.com",
                NormalizedEmail = "Leo@outlook.com"
            };

            mgr.Setup(m => m.FindByEmailAsync("Leo@outlook.com")).Returns(Task.FromResult(user));

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
                        AmountOfTreaments = 2,
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
                            TreatmentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+1, DateTime.Now.Hour, DateTime.Now.Minute+5, DateTime.Now.Second),
                            // Hard coded because the auto generation is done by the controller wich is not tested.
                            TreatmentUntil = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+1, DateTime.Now.Hour + 2, DateTime.Now.Minute, DateTime.Now.Second),
                            VektisType = new VektisModel
                            {
                                Id = 1,
                                NeedsDescription = true,
                                Value = "1"
                            }
                        }
                    }
                }
            };
            var treatmentViewModel = new TreatmentFormViewModel
            {
                Id = 1,
                Treatment = new TreatmentModel
                {
                    Complications = "None",
                    Description = "Test treatment 1",
                    Id = 2,
                    TreatmentDoneBy = therapist,
                    TreatmentRoomOrTrainingRoom = false,
                    TreatmentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 3, DateTime.Now.Hour, DateTime.Now.Minute + 5, DateTime.Now.Second),
                    // Hard coded because the auto generation is done by the controller wich is not tested.
                    TreatmentUntil = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 3, DateTime.Now.Hour + 2, DateTime.Now.Minute, DateTime.Now.Second),
                    VektisType = new VektisModel
                    {
                        Id = 1,
                        NeedsDescription = true,
                        Value = "1"
                    }
                },
                TherapyDoneById = 1
            };

            patientRepoMoq.Setup(m => m.Get(1)).Returns(patient);
            therapistRepoMoq.Setup(m => m.Get(1)).Returns(therapist);

            List<TherapistModel> list = new List<TherapistModel>();
            list.Add(therapist);

            therapistRepoMoq.Setup(m => m.Get()).Returns(list);

            List<TreatmentModel> treatmentList = new List<TreatmentModel>();

            treatmentRepoMoq.Setup(m => m.Get()).Returns(treatmentList);
            var vektis = new VektisModel
            {
                Id = 1,
                NeedsDescription = treatmentViewModel.Treatment.VektisType.NeedsDescription,
                Value = treatmentViewModel.Treatment.VektisType.Value
            };

            var responseTask = Task.FromResult(vektis);

            vektisRepoMoq.Setup(m => m.Get(1)).Returns(responseTask);
            //sut.Create(tvm, patient.Id);

            // Act

            var result = await sut.Create(treatmentViewModel, patient.Id);

            // Assert
            Assert.Equal(typeof(RedirectToActionResult), result.GetType());
            Assert.True(sut.ModelState.IsValid);
        }
        [Fact]
        public async void Patient_Can_Not_Add_Treatment_Edge_Case()
        {
            // Arrange

            IdentityUser user = new IdentityUser
            {
                UserName = "Leo@outlook.com",
                Email = "Leo@outlook.com",
                NormalizedEmail = "Leo@outlook.com"
            };

            mgr.Setup(m => m.FindByEmailAsync("Leo@outlook.com")).Returns(Task.FromResult(user));

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
                        AmountOfTreaments = 1,
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
                            TreatmentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+1, DateTime.Now.Hour, DateTime.Now.Minute+5, DateTime.Now.Second),
                            // Hard coded because the auto generation is done by the controller wich is not tested.
                            TreatmentUntil = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+1, DateTime.Now.Hour + 1, DateTime.Now.Minute, DateTime.Now.Second),
                            VektisType = new VektisModel
                            {
                                Id = 1,
                                NeedsDescription = true,
                                Value = "1"
                            }
                        }
                    }
                }
            };
            var treatmentViewModel = new TreatmentFormViewModel
            {
                Id = 1,
                Treatment = new TreatmentModel
                {
                    Complications = "None",
                    Description = "Test treatment 1",
                    Id = 2,
                    TreatmentDoneBy = therapist,
                    TreatmentRoomOrTrainingRoom = false,
                    TreatmentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 3, DateTime.Now.Hour, DateTime.Now.Minute + 5, DateTime.Now.Second),
                    // Hard coded because the auto generation is done by the controller wich is not tested.
                    TreatmentUntil = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 3, DateTime.Now.Hour + 1, DateTime.Now.Minute, DateTime.Now.Second),
                    VektisType = new VektisModel
                    {
                        Id = 1,
                        NeedsDescription = true,
                        Value = "1"
                    }
                },
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

            var result = await sut.Create(treatmentViewModel, patient.Id);

            // Assert
            Assert.Equal(typeof(ViewResult), result.GetType());
            Assert.False(sut.ModelState.IsValid);
        }
        // Add Treatment Therapist not available BR_3
        [Fact]
        public async void Patient_Can_Add_Treatment_Therapist_Not_Available_Edge_Case()
        {
            // Arrange

            IdentityUser user = new IdentityUser
            {
                UserName = "Leo@outlook.com",
                Email = "Leo@outlook.com",
                NormalizedEmail = "Leo@outlook.com"
            };

            mgr.Setup(m => m.FindByEmailAsync("Leo@outlook.com")).Returns(Task.FromResult(user));

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
                        AmountOfTreaments = 2,
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
                            TreatmentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+1, DateTime.Now.Hour, DateTime.Now.Minute+5, DateTime.Now.Second),
                            // Hard coded because the auto generation is done by the controller wich is not tested.
                            TreatmentUntil = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+1, DateTime.Now.Hour + 2, DateTime.Now.Minute, DateTime.Now.Second),
                            VektisType = new VektisModel
                            {
                                Id = 1,
                                NeedsDescription = true,
                                Value = "1"
                            }
                        }
                    }
                }
            };
            var treatmentViewModel = new TreatmentFormViewModel
            {
                Id = 1,
                Treatment = new TreatmentModel
                {
                    Complications = "None",
                    Description = "Test treatment 1",
                    Id = 2,
                    TreatmentDoneBy = therapist,
                    TreatmentRoomOrTrainingRoom = false,
                    TreatmentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, DateTime.Now.Hour, DateTime.Now.Minute + 5, DateTime.Now.Second),
                    // Hard coded because the auto generation is done by the controller wich is not tested.
                    TreatmentUntil = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, DateTime.Now.Hour + 2, DateTime.Now.Minute, DateTime.Now.Second),
                    VektisType = new VektisModel
                    {
                        Id = 1,
                        NeedsDescription = true,
                        Value = "1"
                    }
                },
                TherapyDoneById = 1
            };

            patientRepoMoq.Setup(m => m.Get(1)).Returns(patient);
            therapistRepoMoq.Setup(m => m.Get(1)).Returns(therapist);

            List<TherapistModel> list = new List<TherapistModel>();
            list.Add(therapist);

            therapistRepoMoq.Setup(m => m.Get()).Returns(list);

            List<TreatmentModel> treatmentList = new List<TreatmentModel>();
            treatmentList.AddRange(patient.PatientDossier.Treatments);

            treatmentRepoMoq.Setup(m => m.Get()).Returns(treatmentList);
            var vektis = new VektisModel
            {
                Id = 1,
                NeedsDescription = treatmentViewModel.Treatment.VektisType.NeedsDescription,
                Value = treatmentViewModel.Treatment.VektisType.Value
            };

            var responseTask = Task.FromResult(vektis);

            vektisRepoMoq.Setup(m => m.Get(1)).Returns(responseTask);
            //sut.Create(tvm, patient.Id);

            // Act

            var result = await sut.Create(treatmentViewModel, patient.Id);

            // Assert
            Assert.Equal(typeof(ViewResult), result.GetType());
            Assert.False(sut.ModelState.IsValid);
        }
        [Fact]
        public async void Patient_Can_Add_Treatment_Therapist_Available_Edge_Case()
        {
            // Arrange

            IdentityUser user = new IdentityUser
            {
                UserName = "Leo@outlook.com",
                Email = "Leo@outlook.com",
                NormalizedEmail = "Leo@outlook.com"
            };

            mgr.Setup(m => m.FindByEmailAsync("Leo@outlook.com")).Returns(Task.FromResult(user));

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
                        AmountOfTreaments = 2,
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
                            TreatmentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+1, DateTime.Now.Hour, DateTime.Now.Minute+5, DateTime.Now.Second),
                            // Hard coded because the auto generation is done by the controller wich is not tested.
                            TreatmentUntil = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+1, DateTime.Now.Hour + 2, DateTime.Now.Minute, DateTime.Now.Second),
                            VektisType = new VektisModel
                            {
                                Id = 1,
                                NeedsDescription = true,
                                Value = "1"
                            }
                        }
                    }
                }
            };
            var treatmentViewModel = new TreatmentFormViewModel
            {
                Id = 1,
                Treatment = new TreatmentModel
                {
                    Complications = "None",
                    Description = "Test treatment 1",
                    Id = 2,
                    TreatmentDoneBy = therapist,
                    TreatmentRoomOrTrainingRoom = false,
                    TreatmentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, DateTime.Now.Hour, DateTime.Now.Minute + 5, DateTime.Now.Second),
                    // Hard coded because the auto generation is done by the controller wich is not tested.
                    TreatmentUntil = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, DateTime.Now.Hour + 2, DateTime.Now.Minute, DateTime.Now.Second),
                    VektisType = new VektisModel
                    {
                        Id = 1,
                        NeedsDescription = true,
                        Value = "1"
                    }
                },
                TherapyDoneById = 1
            };

            patientRepoMoq.Setup(m => m.Get(1)).Returns(patient);
            therapistRepoMoq.Setup(m => m.Get(1)).Returns(therapist);

            List<TherapistModel> list = new List<TherapistModel>();
            list.Add(therapist);

            therapistRepoMoq.Setup(m => m.Get()).Returns(list);

            List<TreatmentModel> treatmentList = new List<TreatmentModel>();

            treatmentRepoMoq.Setup(m => m.Get()).Returns(treatmentList);

            var vektis = new VektisModel
            {
                Id = 1,
                NeedsDescription = treatmentViewModel.Treatment.VektisType.NeedsDescription,
                Value = treatmentViewModel.Treatment.VektisType.Value
            };

            var responseTask = Task.FromResult(vektis);

            vektisRepoMoq.Setup(m => m.Get(1)).Returns(responseTask);
            // Act

            var result = await sut.Create(treatmentViewModel, patient.Id);

            // Assert
            Assert.Equal(typeof(RedirectToActionResult), result.GetType());
            Assert.True(sut.ModelState.IsValid);
        }

        // Should not fail
        [Fact]
        public async void Patient_Can_Add_Treatment_Description_Is_Not_Mandatory()
        {
            // Arrange

            IdentityUser user = new IdentityUser
            {
                UserName = "Leo@outlook.com",
                Email = "Leo@outlook.com",
                NormalizedEmail = "Leo@outlook.com"
            };

            mgr.Setup(m => m.FindByEmailAsync("Leo@outlook.com")).Returns(Task.FromResult(user));

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
                    //PlannedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                    //DueDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, DateTime.Now.Day),
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
                            //TreatmentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+1, DateTime.Now.Hour, DateTime.Now.Minute+5, DateTime.Now.Second),
                            // Hard coded because the auto generation is done by the controller wich is not tested.
                            //TreatmentUntil = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+1, DateTime.Now.Hour + 1, DateTime.Now.Minute, DateTime.Now.Second),
                            VektisType = new VektisModel
                            {
                                Id = 1,
                                NeedsDescription = false,
                                Value = "1"
                            }
                        }
                    }
                }
            };
            var treatmentViewModel = new TreatmentFormViewModel
            {
                Id = 1,
                Treatment = new TreatmentModel
                {
                    Complications = "None",
                    Description = "Test treatment 1",
                    Id = 2,
                    TreatmentDoneBy = therapist,
                    TreatmentRoomOrTrainingRoom = false,
                    TreatmentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, DateTime.Now.Hour, DateTime.Now.Minute + 5, DateTime.Now.Second),
                    // Hard coded because the auto generation is done by the controller wich is not tested.
                    TreatmentUntil = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, DateTime.Now.Hour + 1, DateTime.Now.Minute, DateTime.Now.Second),
                    VektisType = new VektisModel
                    {
                        Id = 1,
                        NeedsDescription = false,
                        Value = "1"
                    }
                },
                TherapyDoneById = 1
            };

            patientRepoMoq.Setup(m => m.Get(1)).Returns(patient);
            therapistRepoMoq.Setup(m => m.Get(1)).Returns(therapist);

            List<TherapistModel> list = new List<TherapistModel>();
            list.Add(therapist);

            therapistRepoMoq.Setup(m => m.Get()).Returns(list);

            List<TreatmentModel> treatmentList = new List<TreatmentModel>();

            treatmentRepoMoq.Setup(m => m.Get()).Returns(treatmentList);
            var vektis = new VektisModel
            {
                Id = 1,
                NeedsDescription = treatmentViewModel.Treatment.VektisType.NeedsDescription,
                Value = treatmentViewModel.Treatment.VektisType.Value
            };

            var responseTask = Task.FromResult(vektis);

            vektisRepoMoq.Setup(m => m.Get(1)).Returns(responseTask);
            // Act

            var result = await sut.Create(treatmentViewModel, patient.Id);

            // Assert
            Assert.Equal(typeof(RedirectToActionResult), result.GetType());
            Assert.True(sut.ModelState.IsValid);
        }

        // Should fail
        [Fact]
        public async void Patient_Can_Not_Add_Treatment_Description_Is_Mandatory()
        {
            // Arrange

            IdentityUser user = new IdentityUser
            {
                UserName = "Leo@outlook.com",
                Email = "Leo@outlook.com",
                NormalizedEmail = "Leo@outlook.com"
            };

            mgr.Setup(m => m.FindByEmailAsync("Leo@outlook.com")).Returns(Task.FromResult(user));

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
                        AmountOfTreaments = 2,
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
                            TreatmentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+1, DateTime.Now.Hour, DateTime.Now.Minute+5, DateTime.Now.Second),
                            // Hard coded because the auto generation is done by the controller wich is not tested.
                            TreatmentUntil = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+1, DateTime.Now.Hour + 2, DateTime.Now.Minute, DateTime.Now.Second),
                            VektisType = new VektisModel
                            {
                                Id = 1,
                                NeedsDescription = true,
                                Value = "1"
                            }
                        }
                    }
                }
            };
            var treatmentViewModel = new TreatmentFormViewModel
            {
                Id = 1,
                Treatment = new TreatmentModel
                {
                    Complications = "None",
                    Id = 2,
                    TreatmentDoneBy = therapist,
                    TreatmentRoomOrTrainingRoom = false,
                    TreatmentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, DateTime.Now.Hour, DateTime.Now.Minute + 5, DateTime.Now.Second),
                    // Hard coded because the auto generation is done by the controller wich is not tested.
                    TreatmentUntil = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, DateTime.Now.Hour + 2, DateTime.Now.Minute, DateTime.Now.Second),
                    VektisType = new VektisModel
                    {
                        Id = 1,
                        NeedsDescription = true
                    }
                },
                TherapyDoneById = 1
            };

            patientRepoMoq.Setup(m => m.Get(1)).Returns(patient);
            therapistRepoMoq.Setup(m => m.Get(1)).Returns(therapist);

            List<TherapistModel> list = new List<TherapistModel>();
            list.Add(therapist);

            var vektis = new VektisModel
            {
                Id = 1,
                NeedsDescription = treatmentViewModel.Treatment.VektisType.NeedsDescription,
                Value = treatmentViewModel.Treatment.VektisType.Value
            };

            var responseTask = Task.FromResult(vektis);

            vektisRepoMoq.Setup(m => m.Get(1)).Returns(responseTask);

            therapistRepoMoq.Setup(m => m.Get()).Returns(list);

            List<TreatmentModel> treatmentList = new List<TreatmentModel>();

            treatmentRepoMoq.Setup(m => m.Get()).Returns(treatmentList);

            // Act

            var result = await sut.Create(treatmentViewModel, patient.Id);

            // Assert
            Assert.Equal(typeof(ViewResult), result.GetType());
            Assert.False(sut.ModelState.IsValid);
        }

        // Should not fail
        // Description not mandatory and not filled in
        [Fact]
        public async void Patient_Can_Add_Treatment_Description_Is_Not_Mandatory_Description_Not_Filled()
        {
            // Arrange

            IdentityUser user = new IdentityUser
            {
                UserName = "Leo@outlook.com",
                Email = "Leo@outlook.com",
                NormalizedEmail = "Leo@outlook.com"
            };

            mgr.Setup(m => m.FindByEmailAsync("Leo@outlook.com")).Returns(Task.FromResult(user));

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
                        AmountOfTreaments = 2,
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
                            TreatmentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+1, DateTime.Now.Hour, DateTime.Now.Minute+5, DateTime.Now.Second),
                            // Hard coded because the auto generation is done by the controller wich is not tested.
                            TreatmentUntil = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+1, DateTime.Now.Hour + 2, DateTime.Now.Minute, DateTime.Now.Second),
                            VektisType = new VektisModel
                            {
                                Id = 1,
                                NeedsDescription = true,
                                Value = "1"
                            }
                        }
                    }
                }
            };
            var treatmentViewModel = new TreatmentFormViewModel
            {
                Id = 1,
                Treatment = new TreatmentModel
                {
                    Complications = "None",
                    Id = 2,
                    TreatmentDoneBy = therapist,
                    TreatmentRoomOrTrainingRoom = false,
                    TreatmentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, DateTime.Now.Hour, DateTime.Now.Minute + 5, DateTime.Now.Second),
                    // Hard coded because the auto generation is done by the controller wich is not tested.
                    TreatmentUntil = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, DateTime.Now.Hour + 2, DateTime.Now.Minute, DateTime.Now.Second),
                    VektisType = new VektisModel
                    {
                        Id = 1,
                        NeedsDescription = false,
                        Value = "1"
                    }
                },
                TherapyDoneById = 1
            };

            patientRepoMoq.Setup(m => m.Get(1)).Returns(patient);
            therapistRepoMoq.Setup(m => m.Get(1)).Returns(therapist);

            List<TherapistModel> list = new List<TherapistModel>();
            list.Add(therapist);

            therapistRepoMoq.Setup(m => m.Get()).Returns(list);

            List<TreatmentModel> treatmentList = new List<TreatmentModel>();

            treatmentRepoMoq.Setup(m => m.Get()).Returns(treatmentList);

            var vektis = new VektisModel
            {
                Id = 1,
                NeedsDescription = treatmentViewModel.Treatment.VektisType.NeedsDescription,
                Value = treatmentViewModel.Treatment.VektisType.Value
            };

            var responseTask = Task.FromResult(vektis);

            vektisRepoMoq.Setup(m => m.Get(1)).Returns(responseTask);
            // Act

            var result = await sut.Create(treatmentViewModel, patient.Id);

            // Assert
            Assert.Equal(typeof(RedirectToActionResult), result.GetType());
            Assert.True(sut.ModelState.IsValid);
        }


        // The following tests will test BR 3
        // A treatment may only be added to a patient if the patient has a useridentity (if he has registered)
        // How to test:
        // -Make a user identity
        // -Make a patient
        // -Add a patient dossier
        // -Add a treatment to this dossier with the controller
        // This is the happy flow
        // Unhappy flow is to do this without a user identity.

        // should succeed
        [Fact]
        public async void Treatment_Can_Be_Added_To_Registered_Patient()
        {
            // Arrange

            IdentityUser user = new IdentityUser
            {
                UserName = "Leo@outlook.com",
                Email = "Leo@outlook.com",
                NormalizedEmail = "Leo@outlook.com"
            };

            mgr.Setup(m => m.FindByEmailAsync("Leo@outlook.com")).Returns(Task.FromResult(user));

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
                        AmountOfTreaments = 2,
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
                            TreatmentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+1, DateTime.Now.Hour, DateTime.Now.Minute+5, DateTime.Now.Second),
                            // Hard coded because the auto generation is done by the controller wich is not tested.
                            TreatmentUntil = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+1, DateTime.Now.Hour + 2, DateTime.Now.Minute, DateTime.Now.Second),
                            VektisType = new VektisModel
                            {
                                Id = 1,
                                NeedsDescription = true,
                                Value = "1"
                            }
                        }
                    }
                }
            };
            var treatmentViewModel = new TreatmentFormViewModel
            {
                Id = 1,
                Treatment = new TreatmentModel
                {
                    Complications = "None",
                    Id = 2,
                    TreatmentDoneBy = therapist,
                    TreatmentRoomOrTrainingRoom = false,
                    TreatmentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, DateTime.Now.Hour, DateTime.Now.Minute + 5, DateTime.Now.Second),
                    // Hard coded because the auto generation is done by the controller wich is not tested.
                    TreatmentUntil = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, DateTime.Now.Hour + 2, DateTime.Now.Minute, DateTime.Now.Second),
                    VektisType = new VektisModel
                    {
                        Id = 1,
                        NeedsDescription = false,
                        Value = "1"
                    }
                },
                TherapyDoneById = 1
            };

            patientRepoMoq.Setup(m => m.Get(1)).Returns(patient);
            therapistRepoMoq.Setup(m => m.Get(1)).Returns(therapist);

            List<TherapistModel> list = new List<TherapistModel>();
            list.Add(therapist);

            therapistRepoMoq.Setup(m => m.Get()).Returns(list);

            List<TreatmentModel> treatmentList = new List<TreatmentModel>();

            treatmentRepoMoq.Setup(m => m.Get()).Returns(treatmentList);

            var vektis = new VektisModel
            {
                Id = 1,
                NeedsDescription = treatmentViewModel.Treatment.VektisType.NeedsDescription,
                Value = treatmentViewModel.Treatment.VektisType.Value
            };

            var responseTask = Task.FromResult(vektis);

            vektisRepoMoq.Setup(m => m.Get(1)).Returns(responseTask);

            // Act

            var result = await sut.Create(treatmentViewModel, patient.Id);

            // Assert
            Assert.Equal(typeof(RedirectToActionResult), result.GetType());
            Assert.True(sut.ModelState.IsValid);
        }
        [Fact]
        public async void Treatment_Can_Not_Be_Added_To_Not_Registered_Patient()
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
                        AmountOfTreaments = 2,
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
                            TreatmentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+1, DateTime.Now.Hour, DateTime.Now.Minute+10, DateTime.Now.Second),
                            // Hard coded because the auto generation is done by the controller wich is not tested.
                            TreatmentUntil = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+1, DateTime.Now.Hour + 2, DateTime.Now.Minute, DateTime.Now.Second),
                            VektisType = new VektisModel
                            {
                                Id = 1,
                                NeedsDescription = true,
                                Value = "1"
                            }
                        }
                    }
                }
            };
            var treatmentViewModel = new TreatmentFormViewModel
            {
                Id = 1,
                Treatment = new TreatmentModel
                {
                    Complications = "None",
                    Id = 2,
                    TreatmentDoneBy = therapist,
                    TreatmentRoomOrTrainingRoom = false,
                    TreatmentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, DateTime.Now.Hour, DateTime.Now.Minute + 5, DateTime.Now.Second),
                    // Hard coded because the auto generation is done by the controller wich is not tested.
                    TreatmentUntil = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, DateTime.Now.Hour + 2, DateTime.Now.Minute, DateTime.Now.Second),
                    VektisType = new VektisModel
                    {
                        Id = 1,
                        NeedsDescription = false,
                        Value = "1"
                    }
                },
                TherapyDoneById = 1
            };

            patientRepoMoq.Setup(m => m.Get(1)).Returns(patient);
            therapistRepoMoq.Setup(m => m.Get(1)).Returns(therapist);

            mgr.Setup(m => m.FindByEmailAsync("email")).Returns(Task.FromResult((IdentityUser)null));

            List<TherapistModel> list = new List<TherapistModel>();
            list.Add(therapist);

            therapistRepoMoq.Setup(m => m.Get()).Returns(list);

            List<TreatmentModel> treatmentList = new List<TreatmentModel>();

            treatmentRepoMoq.Setup(m => m.Get()).Returns(treatmentList);

            var vektis = new VektisModel
            {
                Id = 1,
                NeedsDescription = treatmentViewModel.Treatment.VektisType.NeedsDescription,
                Value = treatmentViewModel.Treatment.VektisType.Value
            };

            var responseTask = Task.FromResult(vektis);

            vektisRepoMoq.Setup(m => m.Get(1)).Returns(responseTask);
            // Act

            var result = await sut.Create(treatmentViewModel, patient.Id);

            // Assert
            Assert.Equal(typeof(ViewResult), result.GetType());
            Assert.False(sut.ModelState.IsValid);
        }
    }
}
