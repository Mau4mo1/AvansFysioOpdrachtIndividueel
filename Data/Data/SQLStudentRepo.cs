using AvansFysioOpdrachtIndividueel.Data;
using AvansFysioOpdrachtIndividueel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Data
{
    public class SQLStudentRepo : IRepo<StudentModel>
    {
        FysioDBContext _context = new();
        public SQLStudentRepo(FysioDBContext context)
        {
            _context = context;
        }
        public void Create(StudentModel entity)
        {
            throw new NotImplementedException();
        }
        public List<StudentModel> Get()
        {
            return _context.students.ToList();
        }

        public StudentModel Get(int id)
        {
            return _context.students.Find(id);
        }

        public StudentModel Get(StudentModel entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(StudentModel entity, int id)
        {
            throw new NotImplementedException();
        }
    }
}
