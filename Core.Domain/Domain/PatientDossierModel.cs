using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Models
{

    // Validation of this model is done in the ViewModel
    public class PatientDossierModel
    {
        public int Id { get; set; }
        // This is the global description of the issue at hand
        public string IssueDescription { get; set; }
        // Diagnosis code and description
        public string DiagnosisCode { get; set; }
        // This is the person that has done the intake for the patient
        public PersonModel IntakeDoneBy { get; set; }
        // This is who supervised the intake
        public PersonModel IntakeSupervisedBy { get; set; }
        // Therapist is the person that helps this patient
        public PersonModel Therapist { get; set; }
        // When will the patient get helped?
        public DateTime PlannedDate { get; set; }
        // Until when will the patient get helped?
        public DateTime DueDate { get; set; }
        // Extra comments to add by the therapist
        public List<CommentModel> ExtraComments { get; set; }
        // Treatment plan
        public TreatmentPlanModel TreatmentPlan { get; set; }
        // Treatments
        public List<TreatmentModel> Treatments { get; set; }
        public PatientDossierModel()
        {
            
        }
    }
}
