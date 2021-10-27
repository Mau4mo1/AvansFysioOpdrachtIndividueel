using AvansFysioOpdrachtIndividueel.Models;
using Core.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ApiInfrastructure
{
    public class DiagnosisRepo : IRepo<DiagnosisModel>
    {

        private readonly VektisDBContext _context;
        
        public DiagnosisRepo(VektisDBContext context)
        {
            _context = context;
        }
        public void Create(DiagnosisModel entity)
        {
            _context.diagnosis.Add(entity);
            _context.SaveChanges();
        }

        public List<DiagnosisModel> Get()
        {
            return _context.diagnosis.Take(50).ToList();
        }

        public DiagnosisModel Get(int id)
        {
            return _context.diagnosis.Find(id);
        }

        public DiagnosisModel Get(DiagnosisModel entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            _context.diagnosis.Remove(Get(id));
            _context.SaveChanges();
        }

        public void Update(DiagnosisModel entity, int id)
        {
            throw new NotImplementedException();
        }
    }
}
