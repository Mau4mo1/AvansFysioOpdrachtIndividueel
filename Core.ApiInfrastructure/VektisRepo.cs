using AvansFysioOpdrachtIndividueel.Models;
using Core.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ApiInfrastructure
{
    public class VektisRepo : IRepo<VektisModel>
    {
        private readonly VektisDBContext _dbContext;
        public VektisRepo(VektisDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(VektisModel entity)
        {
            _dbContext.vekti.Add(entity);
            _dbContext.SaveChanges();
        }

        public List<VektisModel> Get()
        {
            return _dbContext.vekti.Take(200).ToList();
        }

        public VektisModel Get(int id)
        {
            return _dbContext.vekti.Find(id);
        }

        public VektisModel Get(VektisModel entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(VektisModel entity, int id)
        {
            throw new NotImplementedException();
        }
    }
}
