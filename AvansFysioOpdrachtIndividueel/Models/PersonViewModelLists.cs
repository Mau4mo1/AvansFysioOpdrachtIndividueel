using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Models
{
    public class PersonViewModelLists
    {
        public List<PersonModel> PersonModels { get; set; }
        public List<StudentModel> StudentModels {  get; set; }
        public List<PatientModel> PatientModels { get; set; }
        public List<TeacherModel> TeacherModels {  get; set; }

        public PersonViewModelLists()
        {

        }
    }
}
