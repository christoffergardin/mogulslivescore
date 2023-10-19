using EventAggregator.Blazor;
using Microsoft.AspNetCore.Components;
using Moguls.Live.Web.v2.Data;

namespace Moguls.Live.Web.v2.Pages;

public class StartLiveResultComponent : ComponentBase
{
	[Inject] public LiveDataService LiveDataService { get; set; }
		
	[Inject]
	public ILogger<StartLiveResultComponent> Logger { get; set; }

	[Inject] public IEventAggregator Aggregator { get; set; }


	public string Id { get; set; }
	public string ResultTable { get; set; }

	public bool Show { get; set; }

    protected override void OnInitialized()
    {
        Show = true;
        Aggregator.Subscribe(this);

    }

    protected async Task Load()
	{
		await Aggregator.PublishAsync(new Start { Id = Id, ResultTable = ResultTable });
		Show = false;
		await InvokeAsync(StateHasChanged);
	}
}