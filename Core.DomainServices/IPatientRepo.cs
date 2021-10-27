using AvansFysioOpdrachtIndividueel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices
{
    public interface IPatientRepo : IRepo<PatientModel>
    {
        public void UpdatePatientDossier(PatientModel entity, int id);
    }
}
