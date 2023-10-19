using System.Net;
using System.Reflection;
using FluentAssertions;
using Moguls.Live.Web.v2.Repository;
using Moq;

namespace MogulTest
{
    [TestFixture]
    public class LiveDataRepositoryTests
    {
        private Mock<IHttpClientFactory> _mockHttpClientFactory;
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private LiveDataRepository _liveDataRepository;
        private string _projectDirectory;

        [SetUp]
        public void SetUp()
        {
            _mockHttpClientFactory = new Mock<IHttpClientFactory>(); 
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
    
            string outputDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _projectDirectory = Directory.GetParent(outputDirectory).Parent.Parent.FullName;
            
            _liveDataRepository = new LiveDataRepository(_mockHttpClientFactory.Object);
        }

        private string ReadMockData(string fileName)
        {
            string filePath = Path.Combine(_projectDirectory, "Mockdata", fileName);
            return File.ReadAllText(filePath);
        }
        
        public class MockHttpMessageHandler : HttpMessageHandler
        {
            private readonly string _mockResponse;
            private readonly HttpStatusCode _statusCode;

            public MockHttpMessageHandler(string mockResponse, HttpStatusCode statusCode = HttpStatusCode.OK)
            {
                _mockResponse = mockResponse;
                _statusCode = statusCode;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = _statusCode,
                    Content = new StringContent(_mockResponse)
                });
            }
        }

        [Test]
        public async Task FetchStatusResultAsync_ShouldReturnExpectedResult_OnHappyPath()
        {
            string mockData = ReadMockData("status1825.json");
            var mockHandler = new MockHttpMessageHandler(mockData);
            var mockHttpClient = new HttpClient(mockHandler)
            {
                BaseAddress = new Uri("https://api.live-scoring.com/")
            };

            _mockHttpClientFactory.Setup(factory => factory.CreateClient("LiveDataClient"))
                                  .Returns(mockHttpClient);
            
            _liveDataRepository = new LiveDataRepository(_mockHttpClientFactory.Object);
            
            var result = await _liveDataRepository.FetchStatusResultAsync("1825");
            
            result.Should().Be("200852");
        }
        
        [Test]
        public async Task FetchResultsAsync_ShouldReturnExpectedResult_OnHappyPath()
        {
            string mockData = ReadMockData("result1825.json");
            var mockHandler = new MockHttpMessageHandler(mockData);
            var mockHttpClient = new HttpClient(mockHandler)
            {
                BaseAddress = new Uri("https://api.live-scoring.com/")
            };
            _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(mockHttpClient);
            
            _liveDataRepository = new LiveDataRepository(_mockHttpClientFactory.Object);
            
            var result = await _liveDataRepository.FetchResultsAsync("1825");
            
            result.Should().NotBeNull();
            result.updates.First().competitionResultRevision.Should().Be(438);
            result.updates.First().data.entries.First().athleteUuid.Should().Be("6a97b1cb-0971-45db-8aee-6fc351b8eb6c");
        }
    }
}
