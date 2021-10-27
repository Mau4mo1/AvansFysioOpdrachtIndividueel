using Core.Domain.Domain;
using Core.DomainServices;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.Data.Data
{
    public class RestApiVektisRepo : IVektisRepo
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string baseUrl = "https://localhost:44384/api/VektisModels";

        public async Task<List<VektisModel>> Get()
        {
            var streamTask = client.GetStreamAsync(baseUrl);
            var repositories = await JsonSerializer.DeserializeAsync<List<VektisModel>>(await streamTask);
            return repositories;
        }

        public async Task<List<SelectListItem>> GetVekti()
        {
            // Add therapists to the selectlist.
            List<VektisModel> vektisModels = await Get();

            List<SelectListItem> therapists = new List<SelectListItem>();

            foreach (var vektisModel in vektisModels)
            {
                therapists.Add(new SelectListItem { Text = vektisModel.Value, Value = vektisModel.Id.ToString() });
            }
            return therapists;
        }

        public async Task<VektisModel> Get(int id)
        {
            var streamTask = client.GetStreamAsync(baseUrl + "/" + id);
            var repositories = await JsonSerializer.DeserializeAsync<VektisModel>(await streamTask);
            return repositories;
        }
    }
}
