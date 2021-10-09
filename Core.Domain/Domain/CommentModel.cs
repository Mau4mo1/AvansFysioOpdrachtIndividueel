using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        [Required,MaxLength(255)]
        public string Comment { get; set; }
        [Required]
        public DateTime TimeOfCreation { get; set; }
        public PersonModel CommentMadeBy { get; set; }
        [Required]
        public bool CommentVisibleForPatient { get; set; }
    }
}
