using EventAggregator.Blazor;
using Microsoft.AspNetCore.Components;
using Moguls.Live.Web.v2.Data;
using System;
using static Moguls.Live.Web.v2.Data.LiveDataServiceClasses;

namespace Moguls.Live.Web.v2.Pages;

public class LiveResultDualComponent : ComponentBase, IHandle<UpdatedScore>, IDisposable
{
	[Inject] public LiveDataService LiveDataService { get; set; }
		
	[Inject]
	public ILogger<LiveResultDualComponent> Logger { get; set; }

	[Inject] public IEventAggregator Aggregator { get; set; }

	public List<ResultDualCompetition> Results { get; set; }
	public string Id { get; set; }
	public string ResultTable { get; set; }
	public DateTime LastUpdate { get; set; }

	public bool Started { get; set; }

    public string StyleForNumber(int n)
    {
        if (n > 100) return "background:blue";
        if (n < 1) return "background:red";
        return "background:lightgreen";
    }

	public string CrownBearer(ResultDualCompetition athlete, bool isBlue)
	{
		if (athlete.Score != " " && decimal.Parse(athlete.Score, System.Globalization.CultureInfo.InvariantCulture) > decimal.Parse(Results.Find(x => isBlue ? !x.IsBlue : x.IsBlue).Score, System.Globalization.CultureInfo.InvariantCulture))
		{
			return "👑";
		}
		return "";
    }

    protected override void OnInitialized()
    {
        Aggregator.Subscribe(this);
        Started = false;
    }

    protected async Task Load()
	{
		Started = true;
		Results = await LiveDataService.GetDualMogulResult(Id);
		LastUpdate = DateTime.Now;
		await InvokeAsync(StateHasChanged);
	}

	public async Task HandleAsync(UpdatedScore message)
	{
		if (!string.IsNullOrEmpty(Id) && Started)
		{
			Logger.LogInformation("Result was updated.");
			Results = await LiveDataService.GetDualMogulResult(Id);
			LastUpdate = message.UpdatedOn;
		}
	}
	
    public void Dispose()
	{
		Aggregator.Unsubscribe(this);
	}
}