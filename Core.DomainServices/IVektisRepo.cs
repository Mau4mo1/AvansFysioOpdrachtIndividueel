using Core.Domain.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices
{
    public interface IVektisRepo : IAsyncRepo<VektisModel>
    {
        public Task<List<SelectListItem>> GetVekti();
        public Task<VektisModel> Get(int id);
    }
}
