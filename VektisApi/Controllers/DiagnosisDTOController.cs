using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VektisApi.DomainDTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VektisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosisDTOController : ControllerBase
    {
        public CSVConverter _diagnosis;
        public DiagnosisDTOController()
        {
            _diagnosis = new CSVConverter();
        }
        // GET: api/<DiagnosisDTOController>
        [HttpGet]
        public List<DiagnosisDTO> Get()
        {
            return _diagnosis.DiagnosisContainerDTO.Diagnosis;
        }

        // GET api/<DiagnosisDTOController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<DiagnosisDTOController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<DiagnosisDTOController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DiagnosisDTOController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
