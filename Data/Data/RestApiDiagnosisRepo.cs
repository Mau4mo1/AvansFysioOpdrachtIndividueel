using AvansFysioOpdrachtIndividueel.Models;
using Core.Domain.Domain;
using Core.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.Data.Data
{
    public class RestApiDiagnosisRepo : IAsyncRepo<DiagnosisModel>
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string baseUrl = "https://localhost:44384/api/DiagnosisModels";

        public async Task<List<DiagnosisModel>> Get()
        {
            var streamTask = client.GetStreamAsync(baseUrl);
            var repositories = await JsonSerializer.DeserializeAsync<List<DiagnosisModel>>(await streamTask);
            return repositories;
        }
    }
}
