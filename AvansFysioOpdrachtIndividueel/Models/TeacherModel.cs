using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Models
{
    public class TeacherModel : PersonModel
    {
        public int BIGNumber { get; set; }
        public int PersonnelNumber {  get; set; }
        public TeacherModel()
        {

        }
        public TeacherModel(int id, string name, string email, int BIGNumber, int personnelNumber) : base(id, name, email)
        {
            this.BIGNumber = BIGNumber;
            this.PersonnelNumber = personnelNumber;
        }

    }
}
