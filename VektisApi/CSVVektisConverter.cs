using AvansFysioOpdrachtIndividueel.Models;
using Core.Domain.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VektisApi
{
    public class CSVVektisConverter
    {
        private IRepo<VektisModel> _vektisRepo;
        public CSVVektisConverter(IRepo<VektisModel> vektisRepo)
        {
            _vektisRepo = vektisRepo;
            loadFile();
        }
        public void loadFile()
        {
            List<VektisModel> categories = new List<VektisModel>();
            List<List<String>> results = new List<List<String>>();

            using (var reader = new StreamReader(@"VektisLijst.csv"))
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
            var values = line.Split(',');

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
            List<VektisModel> products = new List<VektisModel>();
            int i = 0;
            foreach (List<String> result in results)
            {
                if (i > 0)
                {
                    VektisModel diagnosis = new VektisModel();
                    try
                    {
                        diagnosis.Value = result[0]+ " " + result[1];
                    }
                    catch (Exception)
                    {
                        diagnosis.Value = "N/A";
                    }

                    try
                    {
                        if (result[2].Contains("Ja"))
                        {
                            diagnosis.NeedsDescription = true;
                        }
                        else
                        {
                            diagnosis.NeedsDescription = false;
                        }
                        
                    }
                    catch (Exception)
                    {

                    }
                    _vektisRepo.Create(diagnosis);
                }
                i++;
            }
        }
    }
}
