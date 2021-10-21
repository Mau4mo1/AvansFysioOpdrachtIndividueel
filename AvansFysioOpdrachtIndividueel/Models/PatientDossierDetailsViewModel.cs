using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Models
{
    public class PatientDossierDetailsViewModel : IValidatableObject
    {
        // Data to fill the form
        public List<SelectListItem> Therapists { get; set; }  
        public List<SelectListItem> DiagnosisCodes { get; set; }
        public int Id { get; set; }
        public string Name {  get; set; }
        public PersonModel IntakeDoneBy { get; set; }
        public PersonModel IntakeSupervisedBy { get; set; }
        public PersonModel Therapist { get; set; }

        // Data to recieve information from the form
        [Required]
        public string IssueDescription { get; set; }
        [Required]
        public string DiagnosisCode { get; set; }
        [Required]
        public DateTime PlannedDate { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        [Required]
        public int SupervisedById { get; set; }
        [Required]
        public int IntakeDoneById { get; set; }
        [Required]
        public int TherapistId { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            if (PlannedDate.Subtract(DueDate).TotalDays > 1)
            {
                errors.Add(new ValidationResult("De geplande datum mag niet later zijn dan de eind datum!"));
            }
            return errors;
        }
    }
}
