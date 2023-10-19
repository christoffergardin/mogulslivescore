using FluentAssertions;
using Moguls.Live.Web.v2.Data;
using System.Text.Json;
using static Moguls.Live.Web.v2.Data.LiveDataServiceClasses;

namespace MogulTest
{
    public class ResultTests
    {
        [Test]
        public void Dual_Mogul_Result_Should_Be_Of_Correct_Type()
        {
            
            string relativePath = "MogulTest" + Path.DirectorySeparatorChar + "Mockdata" + Path.DirectorySeparatorChar + "status1825.json";
            string mockJson = File.ReadAllText(Path.Combine("..", "..", "..", "..", relativePath));

            var listOfResults = JsonSerializer.Deserialize<StatusResult>(mockJson);
            listOfResults.Should().BeOfType<StatusResult>();
        }
    }
}