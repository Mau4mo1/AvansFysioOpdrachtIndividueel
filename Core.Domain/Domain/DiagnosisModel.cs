using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Domain.Domain
{
    public class DiagnosisModel
    {
        public int Id {  get; set; }
        [JsonPropertyName("codeAndDescription")]
        public string CodeAndDescription { get; set; }
    }
}
