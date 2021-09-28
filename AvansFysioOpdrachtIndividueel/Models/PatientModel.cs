using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Models
{
    public class PatientModel : PersonModel
    {
        public int PatientNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
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
