using AvansFysioOpdrachtIndividueel.Data;
using AvansFysioOpdrachtIndividueel.Models;
using Core.Domain.Domain;
using Core.DomainServices;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Core.Data.Data
{
    public class SQLTherapistRepo : ITherapistRepo
    {
        FysioDBContext _context = new();
        public SQLTherapistRepo(FysioDBContext context)
        {
            _context = context;
        }

        public void Create(TherapistModel entity)
        {
            throw new NotImplementedException();
        }
        public List<SelectListItem> GetTherapists()
        {
            // Add therapists to the selectlist.
            List<TherapistModel> therapistModels = Get();

            List<SelectListItem> therapists = new List<SelectListItem>();

            foreach (var therapistModel in therapistModels)
            {
                therapists.Add(new SelectListItem { Text = therapistModel.Name, Value = therapistModel.Id.ToString() });
            }
            return therapists;
        }

        public List<TherapistModel> Get()
        {
            return _context.therapists.ToList();
        }

        public TherapistModel Get(int id)
        {
            return _context.therapists.Where(therapist => therapist.Id == id).FirstOrDefault();
        }

        public TherapistModel Get(TherapistModel entity)
        {
            throw new NotImplementedException();
        }


        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(TherapistModel entity, int id)
        {
            throw new NotImplementedException();
        }
    }
}
