using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

namespace AggregateGDPPopulation
{   
        
        public class Class1
        {
            public static void aggregate()
            {
                Dictionary<string, GdpPop> result = new Dictionary<string, GdpPop>();
                try
                {
                    StreamReader sr = new StreamReader(@"../../../../AggregateGDPPopulation/data/datafile.csv");
                    string data = sr.ReadToEnd(); //read everything in one go
                    sr.Close();
                    string[] array = data.Split('\n'); //split by lines
                    array[0] = array[0].Replace("\"", ""); // remove (") with empty string
                    string[] headers = array[0].Split(',');
                    int countryname = Array.IndexOf(headers, "Country Name");
                    int gdpindex = Array.IndexOf(headers, "GDP Billions (USD) 2012");
                    int popindex = Array.IndexOf(headers, "Population (Millions) 2012");
                         //Deserialize part
                    var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(@"../../../../AggregateGDPPopulation/continent.json"));

                    foreach (string line in array)
                    {
                        string[] str = line.Replace("\"", "").Split(','); //str stores the row info as an array of strings
                        if (values.ContainsKey(str[countryname]) == true)
                        {
                            if (!result.ContainsKey(values[str[countryname]]))
                            {
                                GdpPop continent = new GdpPop();
                                result.Add(values[str[countryname]], continent);
                                result[values[str[countryname]]].GDP_2012 = float.Parse(str[gdpindex]);
                                result[values[str[countryname]]].POP_2012 = float.Parse(str[popindex]);
                            }
                            else
                            {
                                result[values[str[countryname]]].GDP_2012 += float.Parse(str[gdpindex]);
                                result[values[str[countryname]]].POP_2012 += float.Parse(str[popindex]);
                            }
                        }
                    }
                    //serialize part
                    string json = JsonConvert.SerializeObject(result, Formatting.Indented);
                    File.WriteAllText(@"../../../../AggregateGDPPopulation/output/output.json", json);
               
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
        public float GDP_2012=0;
        public float POP_2012 =0;
    }
}
                 
    


