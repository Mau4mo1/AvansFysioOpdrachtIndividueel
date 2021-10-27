using AvansFysioOpdrachtIndividueel.Models;
using Core.Domain.Domain;
using Core.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLApi.GraphQL
{
    public class Query
    {
        private readonly IRepo<DiagnosisModel> _diagnosisRepo;
        public Query(IRepo<DiagnosisModel> diagnosisRepo)
        {
            _diagnosisRepo = diagnosisRepo;
        }

        public List<DiagnosisModel> Diagnosis => _diagnosisRepo.Get();
    }
}
