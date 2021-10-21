using AvansFysioOpdrachtIndividueel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices
{
    public interface ITreatmentManager
    {
        // We need the patientId to get their treatments and treatmentplan. 
        // We need the treatment that we want to add to validate it.
        public bool IsWithinAllowedTreatmentAmount(int patientId, TreatmentModel treatment);

        public bool IsTherapistAvailable(int therapistId, TreatmentModel treatmentModel);
    }
}
