using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel.Models
{
    public class TreatmentModel
    {
        public int Id { get; set; }
        public int VektisType { get; set; }
        public string Description { get; set; }
        // true is treatment, false is training.
        public bool TreatmentRoomOrTrainingRoom { get; set; }
        public string Complications { get; set; }
        public PersonModel TreatmentDoneBy { get; set; }
        public DateTime TreatmentTime { get; set; }
    }
}
