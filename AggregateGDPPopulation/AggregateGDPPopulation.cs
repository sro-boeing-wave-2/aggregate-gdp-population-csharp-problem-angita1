using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AggregateGDPPopulation
{

    public class Class1
    {


        public static async Task<string> ReadDataAsync(string filepath)
        {
            string dataTask = "";
            using (StreamReader streamReader = new StreamReader(filepath))
            {
                dataTask = await streamReader.ReadToEndAsync();
            }
            return dataTask;
        }
        public static async void WriteDataAsync(string filepath, string contents)
        {
            using (StreamWriter streamwriter = new StreamWriter(filepath))
            {
                await streamwriter.WriteAsync(contents);
            };

        }

        public static async Task AggregateAsync()
        {
            Dictionary<string, GdpPop> result = new Dictionary<string, GdpPop>();
            string filepath = @"../../../../AggregateGDPPopulation/data/datafile.csv";
            string mapperfile = @"../../../../AggregateGDPPopulation/continent.json";
            string outputpath = @"../../../../AggregateGDPPopulation/output/output.json";
            Task<string> dataTask = ReadDataAsync(filepath); // since the return type is a task which contains the string  
            Task<string> maptask = ReadDataAsync(mapperfile);
            await Task.WhenAll(dataTask, maptask);
            string data = dataTask.Result;
            string mapdata =maptask.Result;
            string[] csvLines = data.Split('\n');
            string[] headers = csvLines[0].Replace("\"", "").Split(',');
            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(mapdata);
            int countryindex = Array.IndexOf(headers, "Country Name");
            int gdpindex = Array.IndexOf(headers, "GDP Billions (USD) 2012");
            int popindex = Array.IndexOf(headers, "Population (Millions) 2012");
            try
            {
                foreach (string s in csvLines)
                {
                    string[] str = s.Replace("\"", "").Split(','); //str stores the row info as an array of strings
                    if (values.ContainsKey(str[countryindex]) == true)
                    {
                        if (result.ContainsKey(values[str[countryindex]])) //Since result will have the continent as keys
                        {
                            result[values[str[countryindex]]].GDP_2012 += float.Parse(str[gdpindex]);
                            result[values[str[countryindex]]].POP_2012 += float.Parse(str[popindex]);
                        }
                        else
                        {
                            GdpPop continent = new GdpPop();
                            result.Add(values[str[countryindex]], continent); // add an element with continent as key and obj as value
                            result[values[str[countryindex]]].GDP_2012 = float.Parse(str[gdpindex]); //str[gdpindex] is a string, so convert to float
                            result[values[str[countryindex]]].POP_2012 = float.Parse(str[popindex]);
                        }
                    }
                }
                string json = JsonConvert.SerializeObject(result, Formatting.Indented);
                WriteDataAsync(outputpath, json);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
    public class GdpPop
    {
        //attributes
        public float GDP_2012 = 0;
        public float POP_2012 = 0;
    }
}




