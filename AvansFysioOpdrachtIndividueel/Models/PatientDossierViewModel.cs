using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Models
{
    public class PatientDossierViewModel
    {
        public PatientModel PatientModel { get; set; }
        public List<PatientModel> SelectItems {  get; set; }
        public int SupervisedById { get; set; }
        public int IntakeDoneById { get; set; }
        public int TherapistId { get; set; }
        public int TreatmentDoneById { get;set; } 
        public List<TreatmentModel> TreatmentModels {  get; set; }
        public TreatmentModel TreatmentModel { get; set; }
        public PatientDossierViewModel()
        {

        }
    }
}
