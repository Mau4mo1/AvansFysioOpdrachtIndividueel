using AvansFysioOpdrachtIndividueel.Data;
using AvansFysioOpdrachtIndividueel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Data
{
    public class SQLTeacherRepo : IRepo<TeacherModel>
    {
        FysioDBContext _context = new();
        public SQLTeacherRepo(FysioDBContext context)
        {
            _context = context;
        }

        public void Create(TeacherModel entity)
        {
            throw new NotImplementedException();
        }

        public List<TeacherModel> Get()
        {
            return _context.teachers.ToList();   
        }

        public TeacherModel Get(int id)
        {
            return _context.teachers.Find(id);
        }

        public TeacherModel Get(TeacherModel entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(TeacherModel entity, int id)
        {
            throw new NotImplementedException();
        }
    }
}
