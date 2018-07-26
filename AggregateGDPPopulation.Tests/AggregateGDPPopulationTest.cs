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
            await AggregateClass.AggregateAsync();   
            string expected = @"../../../../AggregateGDPPopulation.Tests/expected-output.json";
            string output = @"../../../../AggregateGDPPopulation/output/output.json";
            //string actualOutput = streamReader1.ReadToEnd();
            //string expectedOutput = streamReader2.ReadToEnd();
            //Assert.Equal(actualOutput, expectedOutput);
            Task<string> task1Actual = ReadWrite.ReadDataAsync(expected);
            string task1Data = await task1Actual;
            Task<string> actualOutput = ReadWrite.ReadDataAsync(output);
            string task2Data = await actualOutput;
            Assert.Equal(task1Data, task2Data);


        }
    }
}
