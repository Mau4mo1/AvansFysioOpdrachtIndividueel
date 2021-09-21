using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Models
{
    public class LocalPatientRepo : IRepo<PatientModel>
    {
        public static List<PatientModel> Patients = new List<PatientModel>();  
        public void Create(PatientModel entity) => Patients.Add(entity);
        public List<PatientModel> Get() => Patients.ToList();
        public PatientModel Get(int id) => (from patient in Patients
                                            where patient.Id == id
                                            select patient).First();
        public void Remove(int id) => Patients.Remove(Get(id));
        public void Update(PatientModel entity, int id)
        {
            //Patients.Find(id) = entity;
        }
    }
}
