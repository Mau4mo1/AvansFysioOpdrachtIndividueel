using Core.Domain.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Models
{
    public class TeacherModel : TherapistModel
    {
        [Required]
        public int BIGNumber { get; set; }
        [Required]
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
