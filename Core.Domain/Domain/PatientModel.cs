using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Models
{
    public class PatientModel : PersonModel,IValidatableObject
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
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            var now = DateTime.Now;
            int age = now.Year - DateOfBirth.Year;

            if (now.Month < DateOfBirth.Month || (now.Month == DateOfBirth.Month && now.Day < DateOfBirth.Day))
            {
                age--;
            }

            if (age < 16)
            {
                errors.Add(new ValidationResult("Patient moet ouder dan 16 zijn."));
            }
            return errors;
        }

        //public PatientModel(int patientNumber, DateTime dateOfBirth, string gender, string name, string email) : base(name, email)
        //{
        //    PatientNumber = patientNumber;
        //    DateOfBirth = dateOfBirth;
        //    Gender = gender;
        //}

    }
}
