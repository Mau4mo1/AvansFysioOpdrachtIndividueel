using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Models
{
    public class TreatmentModel : IValidatableObject
    {
        public int Id { get; set; }
        public int VektisType { get; set; }
        [Required, MaxLength(255)]
        public string Description { get; set; }
        // true is treatment, false is training.
        public bool TreatmentRoomOrTrainingRoom { get; set; }
        [Required]
        public string Complications { get; set; }
        public PersonModel TreatmentDoneBy { get; set; }
        [Required]
        public DateTime TreatmentTime { get; set; }
        public DateTime TreatmentUntil {  get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            // als het een dag later ingeplanned wordt dan returned subtract -{uur}. 
            if (DateTime.Now.Subtract(TreatmentTime).TotalHours > -24)
            {
                errors.Add(new ValidationResult("Een afspraak moet meer dan 1 dag van te voren ingeplanned worden.."));
            }
            return errors;
        }
    }
}
