using AvansFysioOpdrachtIndividueel.Data;
using AvansFysioOpdrachtIndividueel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Data
{
    public class SQLPersonRepo : IRepo<PersonModel>
    {
        FysioDBContext _context = new();
        public SQLPersonRepo(FysioDBContext context)
        {
            _context = context;
        }
        public void Create(PersonModel entity)
        {
            throw new NotImplementedException();
        }

        public List<PersonModel> Get()
        {
            return _context.persons.ToList();
        }

        public PersonModel Get(int id)
        {
            throw new NotImplementedException();
        }

        public PersonModel Get(PersonModel entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(PersonModel entity, int id)
        {
            throw new NotImplementedException();
        }
    }
}
