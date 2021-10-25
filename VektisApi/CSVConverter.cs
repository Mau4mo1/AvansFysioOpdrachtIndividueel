using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VektisApi.DomainDTO;
using Core.ApiInfrastructure;
using Core.Domain.Domain;
using AvansFysioOpdrachtIndividueel.Models;

namespace VektisApi
{

    // Dit is een zelfgeschreven CSV converter die het bestand ophaald en het in lokale storage zet.
    public class CSVConverter
    {
        private IRepo<DiagnosisModel> _diagnosisRepo;
        public CSVConverter(IRepo<DiagnosisModel> diagnosisRepo)
        {
            _diagnosisRepo = diagnosisRepo;
            loadFile();
        }
        public void loadFile()
        {
            List<DiagnosisDTO> categories = new List<DiagnosisDTO>();
            List<List<String>> results = new List<List<String>>();

            using (var reader = new StreamReader(@"DCSPH_lijst.csv"))
            {
                // Read through the whole file
                while (!reader.EndOfStream)
                {
                    results.Add(trimmedResults(reader.ReadLine()));
                }
            }
            // when the reading is done, loop through the results list and each time a new product is found, search the list for all the nutrients.
            convertFile(results);
        }

        private List<String> trimmedResults(String line)
        {
            List<String> newColumn = new List<String>();
            var values = line.Split(';');

            foreach (string value in values)
            {

                String replacementString = "";
                replacementString = value.Replace("\"", "");
                // replacementString = value.
                newColumn.Add(replacementString.Replace("\\", ""));
            }
            return newColumn;
        }

        private void convertFile(List<List<String>> results)
        {
            // break up the CSV file based on comma's

            // loop through the string array and fill each object accordingly
            List<DiagnosisDTO> products = new List<DiagnosisDTO>();
            int i = 1;
            foreach (List<String> result in results)
            {
                if(i > 3)
                {
                    DiagnosisModel diagnosis = new DiagnosisModel();
                    try
                    {
                        diagnosis.CodeAndDescription = result[2];
                    }
                    catch (Exception)
                    {
                        diagnosis.CodeAndDescription = "N/A";
                    }

                    try
                    {
                        diagnosis.CodeAndDescription += " " + result[4];
                    }
                    catch (Exception)
                    {
                        diagnosis.CodeAndDescription = " N/A";
                    }
                    _diagnosisRepo.Create(diagnosis);
                }
                i++;
            }
        }

    }
}
