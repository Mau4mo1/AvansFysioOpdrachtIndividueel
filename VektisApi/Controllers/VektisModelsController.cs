using AvansFysioOpdrachtIndividueel.Models;
using Core.Domain.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VektisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VektisModelsController : ControllerBase
    {
        private readonly IRepo<VektisModel> _vektisRepo;
        public VektisModelsController(IRepo<VektisModel> vektisRepo)
        {
            _vektisRepo = vektisRepo;
        }
        // GET: api/<VektisModelsController>
        [HttpGet]
        public List<VektisModel> Get()
        {
            return _vektisRepo.Get();
        }

        // GET api/<VektisModelsController>/5
        [HttpGet("{id}")]
        public VektisModel Get(int id)
        {
            return _vektisRepo.Get(id);
        }

        // POST api/<VektisModelsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<VektisModelsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<VektisModelsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
