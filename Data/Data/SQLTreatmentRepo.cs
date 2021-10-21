using AvansFysioOpdrachtIndividueel.Data;
using AvansFysioOpdrachtIndividueel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Data
{
    public class SQLTreatmentRepo : IRepo<TreatmentModel>
    {
        FysioDBContext _context = new();
        public SQLTreatmentRepo(FysioDBContext context)
        {
            _context = context;
        }

        public void Create(TreatmentModel entity)
        {
            throw new NotImplementedException();
        }

        public List<TreatmentModel> Get()
        {
            return _context.treatments.ToList();
        }

        public TreatmentModel Get(int id)
        {
            return _context.treatments.Find(id);
        }

        public TreatmentModel Get(TreatmentModel entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(TreatmentModel entity, int id)
        {
            throw new NotImplementedException();
        }
    }
}
