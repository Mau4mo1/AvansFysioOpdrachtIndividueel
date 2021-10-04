using AvansFysioOpdrachtIndividueel.Data;
using AvansFysioOpdrachtIndividueel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Data
{
    class SQLTherapistRepo : IRepo<TherapistModel>
    {
        FysioDBContext _context = new();
        public SQLTherapistRepo(FysioDBContext context)
        {
            _context = context;
        }
        public void Create(TherapistModel entity)
        {
            throw new NotImplementedException();
        }

        public List<TherapistModel> Get()
        {
            throw new NotImplementedException();
        }

        public TherapistModel Get(int id)
        {
            throw new NotImplementedException();
        }

        public TherapistModel Get(TherapistModel entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)      
        {
            throw new NotImplementedException();
        }

        public void Update(TherapistModel entity, int id)
        {
            throw new NotImplementedException();
        }
    }
}
