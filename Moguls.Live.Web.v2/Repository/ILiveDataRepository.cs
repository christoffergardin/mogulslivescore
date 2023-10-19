using Moguls.Live.Web.v2.Data;

namespace Moguls.Live.Web.v2.Repository;
using static Moguls.Live.Web.v2.Data.LiveDataServiceClasses;

public interface ILiveDataRepository
{
    Task<string> FetchStatusResultAsync(string competition);
    Task<ParaResult> FetchResultsAsync(string competition);
    List<ResultDualCompetition> ReturnDualResults(List<ResultDualCompetition> results, List<ParaResult.Update> updates);
    List<ResultModel> ReturnSingleMogulResult(ParaResult result, string resultTable);
}