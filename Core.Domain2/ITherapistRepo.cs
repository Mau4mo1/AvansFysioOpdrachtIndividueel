using AvansFysioOpdrachtIndividueel.Models;
using Core.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering.SelectListItem;

namespace Core.DomainServices
{
    public interface ITherapistRepo : IRepo<TherapistModel>
    {
        public List<SelectListItem> GetTherapists();
    }
}
