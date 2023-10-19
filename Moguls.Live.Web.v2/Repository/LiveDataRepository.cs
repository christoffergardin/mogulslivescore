using Moguls.Live.Web.v2.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Moguls.Live.Web.v2.Data.LiveDataServiceClasses;

namespace Moguls.Live.Web.v2.Repository
{
    public class LiveDataRepository : ILiveDataRepository
    {
        private readonly HttpClient _httpClient;

        public LiveDataRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("LiveDataClient");
        }
        public async Task<string> FetchStatusResultAsync(string competition)
        {
            var status = await _httpClient.GetFromJsonAsync<StatusResult>($"competitions/{competition}/status");
            return status.activeResultTables[0].ToString();
        }

        public async Task<ParaResult> FetchResultsAsync(string competition)
        {
            var siteResults = await _httpClient.GetStringAsync($"competitions/{competition}/results-since/-1");
            return JsonConvert.DeserializeObject<ParaResult>(siteResults);
        }

        public List<ResultDualCompetition> ReturnDualResults(List<ResultDualCompetition> results, List<ParaResult.Update> enumerable)
        {
            var flagConverter = new FlagConverter();
            
            foreach (var dataEntry in enumerable.First().data.entries)
            {
                var skier = dataEntry.values.First();

                var resultDual = new ResultDualCompetition()
                {
                    Place = skier[0].Value<string>(),
                    Bib = skier[1].Value<string>(),
                    Name = skier[2].ToObject<RowValue>()?.text,
                    Nationality = skier[3].Value<string>(),
                    NationalityFlag = flagConverter.getFlag(skier[3].Value<string>()),
                    Score = skier[4].Value<string>(),
                    highlightColor = dataEntry.highlightColor,
                };
                results.Add(resultDual);
            }
            return results;
        }

        public List<ResultModel> ReturnSingleMogulResult(ParaResult result, string resultTable)
        {
            var flagConverter = new FlagConverter();
            var resultModels = new List<ResultModel>();
            foreach (var dataEntry in result.updates.Where(x => x.resultTableId == Convert.ToInt32(resultTable)).First().data.entries)
            {
                foreach (var o in dataEntry.values)
                {
                    var list = new List<string>();
                    foreach (JToken jToken in o)
                    {
                        string value;
                        try
                        {
                            var rowValue = jToken.ToObject<RowValue>();
                            value = rowValue.text;
                        }
                        catch (Exception)
                        {
                            value = jToken.ToObject<string>();
                        }
                        list.Add(value);
                    }
                    var resultModel = new ResultModel
                    {
                        Rank = list[0],
                        StartNumber = list[1],
                        Bib = list[2],
                        Name = list[3],
                        Nationality = list[4],
                        NationalityFlag = flagConverter.getFlag(list[4]),
                        Score = list[14]
                    };
                    resultModels.Add(resultModel);
                }
            }
            return resultModels;
        }
    }
}
