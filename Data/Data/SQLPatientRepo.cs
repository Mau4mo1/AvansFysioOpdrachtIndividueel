using AvansFysioOpdrachtIndividueel.Data;
using AvansFysioOpdrachtIndividueel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Data.Data
{
    public class SQLPatientRepo : IRepo<PatientModel>
    {
        FysioDBContext _context = new();
        public SQLPatientRepo(FysioDBContext context)
        {
            _context = context;
        }
        public SQLPatientRepo()
        {

        }
        public void Create(PatientModel entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }
        public List<PatientModel> Get()
        {
            return _context.patients
                .Include(patient => patient.PatientDossier)
                    .ThenInclude(patient => patient.Treatments)
                        .ThenInclude(patient => patient.TreatmentDoneBy)
                .Include(patient => patient.PatientDossier.Therapist)
                .Include(patient => patient.PatientDossier.IntakeSupervisedBy)
                .ToList();
        }
        public PatientModel Get(int id)
        {
            var patient = _context.patients.Where(x => x.Id == id)
                .Include(patient => patient.PatientDossier)
                    .ThenInclude(patient => patient.Treatments)
                        .ThenInclude(patient => patient.TreatmentDoneBy)
                .Include(patient => patient.PatientDossier.Therapist)
                .Include(patient => patient.PatientDossier.IntakeSupervisedBy)
                .First();

            if (patient.PatientDossier == null)
            {
                patient.PatientDossier = new PatientDossierModel();
            }
            if (patient.PatientDossier.Treatments == null)
            {
                patient.PatientDossier.Treatments = new List<TreatmentModel>();
            }


            return patient;
        }
        public PatientModel Get(PatientModel entity)
        {
            return _context.patients.Find(entity.Id);
        }
        public void Remove(int id)
        {
            _context.Remove(_context.patients.Find(id));
            _context.SaveChanges();
        }
        public void Update(PatientModel entity, int id)
        {
            PatientModel model = Get(id);
            model.PatientDossier = entity.PatientDossier;

            _context.Update(model);
            _context.SaveChanges();
        }

    }
}
