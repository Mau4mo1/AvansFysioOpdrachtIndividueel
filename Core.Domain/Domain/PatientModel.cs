using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Models
{
    public class PatientModel : PersonModel
    {
        public int PatientNumber { get; set; }
        [Required]
        public int TeacherOrStudentNumber { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Gender { get; set; }
        public PatientDossierModel PatientDossier {  get; set;}
        // the patient can be either a student or a teacher
        public PatientModel()
        {

        }
        public PatientModel(int patientNumber, DateTime dateOfBirth, string gender, string name, string email) : base( name, email)
        {
            PatientNumber = patientNumber;
            DateOfBirth = dateOfBirth;
            Gender = gender;
        }

        //public PatientModel(int patientNumber, DateTime dateOfBirth, string gender, string name, string email) : base(name, email)
        //{
        //    PatientNumber = patientNumber;
        //    DateOfBirth = dateOfBirth;
        //    Gender = gender;
        //}

    }
}
