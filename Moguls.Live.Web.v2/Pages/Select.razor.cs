using EventAggregator.Blazor;
using Microsoft.AspNetCore.Components;
using Moguls.Live.Web.v2.Data;
using System;

namespace Moguls.Live.Web.v2.Pages;

public class SelectComponent : ComponentBase, IHandle<Selected>, IDisposable
{

    [Inject]
    public ILogger<SelectComponent> Logger { get; set; }

    [Inject] public IEventAggregator Aggregator { get; set; }

    public bool Singel { get; set; }
    public bool Dual { get; set; }

    protected override void OnInitialized()
    {
        Aggregator.Subscribe(this);
    }

    protected async Task onClickSingel()
    {
        Singel = true;
        Dual = false;
        await InvokeAsync(StateHasChanged);
    }
    
    protected async Task onClickDual()
    {
        Singel = false;
        Dual = true;
        await InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        Aggregator.Unsubscribe(this);
    }

    public Task HandleAsync(Selected message)
    {
        throw new NotImplementedException();
    }
}