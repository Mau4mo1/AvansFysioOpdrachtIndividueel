using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Models
{
    abstract public class PersonModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public PersonModel()
        {

        }
        public PersonModel(int id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
        

    }
}
