using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Models
{
    public class PersonViewModel
    {
        public PatientModel PatientModel { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public int Id { get; set; }
        public PersonViewModel()
        {

        }
    }
}
