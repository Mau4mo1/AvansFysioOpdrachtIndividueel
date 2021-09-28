using AvansFysioOpdrachtIndividueel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Data
{
    public class SQLPatientRepo : IRepo<PatientModel>
    {
        FysioDBContext _context = new FysioDBContext();
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
            return _context.patients.ToList();
        }
        public PatientModel Get(int id)
        {
            // TODO CLEAN?
            var patient = _context.patients
                .Include(patient => patient.PatientDossier).First();
           
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
