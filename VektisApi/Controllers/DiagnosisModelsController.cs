using AvansFysioOpdrachtIndividueel.Models;
using Core.Domain.Domain;
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
    public class DiagnosisModelsController : ControllerBase
    {
        private readonly IRepo<DiagnosisModel> _diagnosisRepo;
        public DiagnosisModelsController( IRepo<DiagnosisModel> diagnosisRepo)
        {
            _diagnosisRepo = diagnosisRepo;

        }
        // GET: api/<DiagnosisDTOController>
        [HttpGet]
        public List<DiagnosisModel> Get()
        {
            return _diagnosisRepo.Get();
          //  return _diagnosis.DiagnosisContainerDTO.Diagnosis;
        }

        // get api/<diagnosisdtocontroller>/5
        [HttpGet("{id}")]
        public DiagnosisModel Get(int id)
        {
            return _diagnosisRepo.Get(id);
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
