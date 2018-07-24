using System;
using System.IO;
using Xunit;
using AggregateGDPPopulation;
using System.Threading.Tasks;

namespace AggregateGDPPopulation.Tests
{
    public class UnitTest1
    {
        [Fact]
      async  public void Test1()
        {
            await Class1.AggregateAsync();   
            string expected = @"../../../../AggregateGDPPopulation.Tests/expected-output.json";
            string output = @"../../../../AggregateGDPPopulation/output/output.json";
            //string actualOutput = streamReader1.ReadToEnd();
            //string expectedOutput = streamReader2.ReadToEnd();
            //Assert.Equal(actualOutput, expectedOutput);
            Task<string> task1Actual = Class1.ReadDataAsync(expected);
            string task1Data = await task1Actual;
            Task<string> actualOutput = Class1.ReadDataAsync(output);
            string task2Data = await actualOutput;
            Assert.Equal(task2Data, task1Data);


        }
    }
}
