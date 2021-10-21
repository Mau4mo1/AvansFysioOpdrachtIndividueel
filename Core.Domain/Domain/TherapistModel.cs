using AvansFysioOpdrachtIndividueel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Domain
{
    abstract public class TherapistModel : PersonModel
    {
        public TherapistModel(int id, string name, string email ) : base(id,name,email)
        {

        }
        public TherapistModel()
        {

        }
    }
}
