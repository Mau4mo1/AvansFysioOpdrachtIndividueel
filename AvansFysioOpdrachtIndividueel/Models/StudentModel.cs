using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Models
{
    public class StudentModel : PersonModel
    {
        public int StudentNumber { get; set; }
        public StudentModel()
        {

        }
        public StudentModel(int studentNumber, int id, string name, string email) : base(id, name, email)  
        {
            Email = email;
            StudentNumber = studentNumber;
            Id = id;
            Name = name;
        }
        
    }
}
