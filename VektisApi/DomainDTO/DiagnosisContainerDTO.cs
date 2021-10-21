using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VektisApi.DomainDTO
{
    public class DiagnosisContainerDTO
    {
        public List<DiagnosisDTO> Diagnosis {  get; set; }
        public DiagnosisContainerDTO()
        {
            Diagnosis = new List<DiagnosisDTO>();
        }
    }
}
