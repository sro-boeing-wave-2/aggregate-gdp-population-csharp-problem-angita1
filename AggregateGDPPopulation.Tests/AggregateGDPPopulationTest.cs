using System;
using System.IO;
using Xunit;
using AggregateGDPPopulation;


namespace AggregateGDPPopulation.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Class1.aggregate();   
            StreamReader streamReader1 = new StreamReader(@"../../../../AggregateGDPPopulation.Tests/expected-output.json");
            StreamReader streamReader2 = new StreamReader(@"../../../../AggregateGDPPopulation/output/output.json");
            string actualOutput = streamReader1.ReadToEnd();
            string expectedOutput = streamReader2.ReadToEnd();
            Assert.Equal(actualOutput, expectedOutput);
        }
    }
}
