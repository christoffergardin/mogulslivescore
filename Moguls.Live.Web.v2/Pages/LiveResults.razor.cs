using EventAggregator.Blazor;
using Microsoft.AspNetCore.Components;
using Moguls.Live.Web.v2.Data;

namespace Moguls.Live.Web.v2.Pages;

public class LiveResultComponent : ComponentBase, IHandle<UpdatedScore>, IDisposable
{
	[Inject] public LiveDataService LiveDataService { get; set; }
		
	[Inject]
	public ILogger<LiveResultComponent> Logger { get; set; }

	[Inject] public IEventAggregator Aggregator { get; set; }

	public List<ResultModel> Results { get; set; }
	public ResultModel LastRun
    {
        get
        {
            if (Results.All(x => x.Score == " "))
            {
                return Results.OrderBy(x => Convert.ToInt32(x.StartNumber)).First();
            }
            return Results.Where(x => x.Score != "DNS" && x.Score != " ").OrderBy(x => Convert.ToInt32(x.StartNumber)).Last();
        }
    }

    public string Id { get; set; }
	public string ResultTable { get; set; }
	public DateTime LastUpdate { get; set; }

	public bool Started { get; set; }

    protected override void OnInitialized()
    {
        Aggregator.Subscribe(this);
        Started = false;
    }

    protected async Task Load()
	{
		Started = true;
		Results = await LiveDataService.GetSingleMogulResult(Id, ResultTable);
		await InvokeAsync(StateHasChanged);
	}

	public async Task HandleAsync(UpdatedScore message)
	{
		if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(ResultTable) && Started)
		{
			Logger.LogInformation("Result was updated.");
			Results = await LiveDataService.GetSingleMogulResult(Id, ResultTable);
			LastUpdate = message.UpdatedOn;
		}
	}


	public void Dispose()
	{
		Aggregator.Unsubscribe(this);
	}
}