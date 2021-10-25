
using AvansFysioOpdrachtIndividueel.Models;
using Core.DomainServices;
using FluentDateTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Data
{
    public class SQLTreatmentManager : ITreatmentManager
    {
        IPatientRepo _patientRepo;
        ITherapistRepo _therapistRepo;
        IRepo<TreatmentModel> _treatmentRepo;
        public SQLTreatmentManager(IPatientRepo patientRepo, ITherapistRepo therapistRepo, IRepo<TreatmentModel> treatmentRepo)
        {
            _patientRepo = patientRepo;
            _therapistRepo = therapistRepo;
            _treatmentRepo = treatmentRepo;
        }
        // True means the therapist is available
        public bool IsTherapistAvailable(int therapistId, TreatmentModel treatmentModel)
        {
            var therapist = _therapistRepo.Get(therapistId);
            // if the time is inside the start and the end of the treatment, it will be found
            var treatments = _treatmentRepo.Get().Where(t =>
                t.TreatmentDoneBy.Id == therapist.Id &&
                // Check if the start time is between the start and end of the therapists treatment
                t.TreatmentTime >= treatmentModel.TreatmentTime && t.TreatmentTime <= treatmentModel.TreatmentUntil ||
                // Check if the end time is between the start and end of the therapists treatment
                t.TreatmentUntil >= treatmentModel.TreatmentTime && t.TreatmentUntil <= treatmentModel.TreatmentUntil).ToList();

            if (treatments.Count >= 1)
            {
                return false;
            }
            return true;
        }

        // True means the patient is allowed to make another treatment
        public bool IsWithinAllowedTreatmentAmount(int patientId, TreatmentModel treatment)
        {
            // Get the patient that wants to add a treatment
            var patient = _patientRepo.Get(patientId);

            var monday = treatment.TreatmentTime.Previous(DayOfWeek.Monday);
            var sunday = treatment.TreatmentTime.Next(DayOfWeek.Sunday);
            // Take the time between the start of the week and end of the week and all treatments that are between that time will be selected.
            try
            {
                var treatmentsThisWeek = patient.PatientDossier.Treatments.Where(t => t.TreatmentTime.Day >= monday.Day && t.TreatmentTime.Day <= sunday.Day).ToList();
                // Check if the patient has more than the maximum allowed treatment amount + 1 because we will add one more treatment after.
                if ((patient.PatientDossier.Treatments.Count + 1) > (patient.PatientDossier.TreatmentPlan.AmountOfTreaments))
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return true;
            }
            

            return true;
        }
    }
}
