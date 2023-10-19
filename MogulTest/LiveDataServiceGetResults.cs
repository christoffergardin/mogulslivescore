using System.Reflection;
using FluentAssertions;
using Moq;
using Microsoft.Extensions.Logging;
using Moguls.Live.Web.v2.Data;
using Moguls.Live.Web.v2.Repository;
using Newtonsoft.Json;

namespace MogulTest
{
    [TestFixture]
    public class LiveDataServiceGetResultsTests
    {
        private Mock<ILogger<LiveDataService>> _mockLogger;
        private LiveDataService _liveDataService;
        private Mock<ILiveDataRepository> _mockRepository;
        
        [SetUp]
        public void SetUp()
        {
            string outputDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string projectDirectory = Directory.GetParent(outputDirectory).Parent.Parent.FullName;
            
            string statusFilePath = Path.Combine(projectDirectory, "Mockdata", "status1825.json");
            string resultsFilePath = Path.Combine(projectDirectory, "Mockdata", "result1825.json");

            string statusData = File.ReadAllText(statusFilePath);
            LiveDataServiceClasses.StatusResult status = 
                Newtonsoft.Json.JsonConvert.DeserializeObject<LiveDataServiceClasses.StatusResult>(statusData);
           
            string resultsData = File.ReadAllText(resultsFilePath);
            
            var mockResults = new List<LiveDataServiceClasses.ResultDualCompetition>
            {
                new LiveDataServiceClasses.ResultDualCompetition
                {
                    Name = "GRAVENFORS Filip",
                    Nationality = "SWE",
                    NationalityFlag = "SE",
                    Score = "22.00",
                    Place = "1",
                    Bib = "1",
                    highlightColor = new LiveDataServiceClasses.ParaResult.Highlightcolor 
                    { 
                        r = 184, 
                        g = 62, 
                        b = 77, 
                        a = 0.5f 
                    }
                },
                new LiveDataServiceClasses.ResultDualCompetition
                {
                    Name = "HOLMGREN Emil",
                    Nationality = "SWE",
                    NationalityFlag = "SE",
                    Score = "13.00",
                    Place = "2",
                    Bib = "2",
                    highlightColor = new LiveDataServiceClasses.ParaResult.Highlightcolor 
                    { 
                        r = 102, 
                        g = 153, 
                        b = 204, 
                        a = 0.5f 
                    }
      
                }
            };
            
            _mockLogger = new Mock<ILogger<LiveDataService>>();
            _mockRepository = new Mock<ILiveDataRepository>();
            _mockRepository.Setup(r => r.FetchStatusResultAsync(It.IsAny<string>()))
                .ReturnsAsync(status.activeResultTables[0].ToString());
            _mockRepository.Setup(r => r.FetchResultsAsync(It.IsAny<string>()))
                .ReturnsAsync(JsonConvert.DeserializeObject<LiveDataServiceClasses.ParaResult>(resultsData));
            _mockRepository.Setup(r => r.ReturnDualResults(It.IsAny<List<LiveDataServiceClasses.ResultDualCompetition>>(), 
                    It.IsAny<List<LiveDataServiceClasses.ParaResult.Update>>()))
                .Returns(mockResults);

            _liveDataService = new LiveDataService(_mockLogger.Object, _mockRepository.Object);
        }
        
        [Test]
        public async Task GetDualMogulResult_WhenCalledWithValidId_ShouldReturnExpectedResults()
        {
            
            var result = await _liveDataService.GetDualMogulResult("1825");
            
            result.Should().NotBeNull();
            result.Should().HaveCountGreaterThan(0);

            var firstResult = result[0];
            firstResult.Name.Should().Be("GRAVENFORS Filip");
            firstResult.Nationality.Should().Be("SWE");
            firstResult.NationalityFlag.Should().Be("SE");
            firstResult.Score.Should().Be("22.00");
            firstResult.Place.Should().Be("1");
            firstResult.Bib.Should().Be("1");
            firstResult.highlightColor.Should().BeEquivalentTo(new { r = 184, g = 62, b = 77, a = 0.5 });
            
            var secondResult = result[1];
            secondResult.Name.Should().Be("HOLMGREN Emil");
            secondResult.Nationality.Should().Be("SWE");
            secondResult.NationalityFlag.Should().Be("SE");
            secondResult.Score.Should().Be("13.00");
            secondResult.Place.Should().Be("2");
            secondResult.Bib.Should().Be("2");
            secondResult.highlightColor.Should().BeEquivalentTo(new { r = 102, g = 153, b = 204, a = 0.5 });
        }
    }
}