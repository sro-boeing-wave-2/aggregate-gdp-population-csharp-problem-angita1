using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AggregateGDPPopulation
{

    public class AggregateClass
    {

        public static async Task AggregateAsync()
        {
            Dictionary<string, GdpPop> result = new Dictionary<string, GdpPop>();
            string filepath = @"../../../../AggregateGDPPopulation/data/datafile.csv";
            string mapperfile = @"../../../../AggregateGDPPopulation/continent.json";
            string outputpath = @"../../../../AggregateGDPPopulation/output/output.json";
           
            
            Task<string> dataTask = ReadWrite.ReadDataAsync(filepath); // since ReadDataAsync is a static method ,no obj creation is required
            Task<string> maptask = ReadWrite.ReadDataAsync(mapperfile);
            await Task.WhenAll(dataTask, maptask);
            string data = dataTask.Result;
            string mapdata = maptask.Result;
            string[] csvLines = data.Split('\n');
            string[] headers = csvLines[0].Replace("\"", "").Split(',');
            var values = De_Se_rialize.deserialize(mapdata);
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
                        string continent = values[str[countryindex]];
                        if (result.ContainsKey(continent)) //Since result will have the continent as keys
                        {
                            result[continent].GDP_2012 += float.Parse(str[gdpindex]);
                            result[continent].POP_2012 += float.Parse(str[popindex]);

                        }
                        else
                        {
                            float gdp = float.Parse(str[gdpindex]);
                            float pop = float.Parse(str[popindex]);
                            GdpPop gdp_pop_obj = new GdpPop(gdp, pop); // add an element with continent as key and obj as value 
                            result.Add(continent, gdp_pop_obj);       //str[gdpindex] is a string, so convert to float                           
                        }
                    }
                }
                string json = De_Se_rialize.serialize(result);
                ReadWrite.WriteDataAsync(outputpath, json);

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
        public float GDP_2012;
        public float POP_2012;

        //Constructor
        public GdpPop(float gdp, float pop)
        {
            GDP_2012 = gdp;
            POP_2012 = pop;

        }


    }
    public class ReadWrite
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
    }

    public class De_Se_rialize
    {
       public static string serialize( Dictionary<string,GdpPop> result)
        {
            string json = JsonConvert.SerializeObject(result, Formatting.Indented);
            return json;
        }
        public static Dictionary<string, string> deserialize(string mapdata)
        {
            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(mapdata);
            return values;
        }
        
    }
}






