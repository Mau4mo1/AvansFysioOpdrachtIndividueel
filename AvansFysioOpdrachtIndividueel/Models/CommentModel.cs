using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string Comment {  get; set; }
        public DateTime TimeOfCreation { get; set; }
        public PersonModel CommentMadeBy { get; set; }
        public bool CommentVisibleForPatient { get; set; }
    }
}
