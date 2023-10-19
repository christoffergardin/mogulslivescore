using Moguls.Live.Web.v2.Repository;
using static Moguls.Live.Web.v2.Data.LiveDataServiceClasses;

namespace Moguls.Live.Web.v2.Data;

public class LiveDataService
{
    private readonly ILogger<LiveDataService> _logger;
    private readonly ILiveDataRepository _liveDataRepository;

    public LiveDataService(ILogger<LiveDataService> logger, ILiveDataRepository liveDataRepository)
    {
        _logger = logger;
        _liveDataRepository = liveDataRepository;
    }

    public async Task<List<ResultDualCompetition>> GetDualMogulResult(string competition)
    {
        var resultStatus = await _liveDataRepository.FetchStatusResultAsync(competition);
        var fetchResult = await _liveDataRepository.FetchResultsAsync(competition);

        var enumerable = fetchResult.updates.Where(x => x.resultTableId == Convert.ToInt32(resultStatus)).ToList();
        var results = new List<ResultDualCompetition>();
  
        return _liveDataRepository.ReturnDualResults(results, enumerable);
    }
 
    public async Task<List<ResultModel>> GetSingleMogulResult(string competition, string resultTable)
    {
        var fetchResult = await _liveDataRepository.FetchResultsAsync(competition);

        return _liveDataRepository.ReturnSingleMogulResult(fetchResult, resultTable);
    }
}