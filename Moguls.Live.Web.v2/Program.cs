using EventAggregator.Blazor;

using Moguls.Live.Web.v2.Data;
using Moguls.Live.Web.v2.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddHttpClient("LiveDataClient", client =>
{
	client.BaseAddress = new Uri("https://api.live-scoring.com/");
});

builder.Services.AddSingleton<ILiveDataRepository, LiveDataRepository>();
builder.Services.AddScoped<LiveDataService>();
builder.Services.AddHostedService<AutoUpdateService>();

builder.Services.AddEventAggregator(options => options.AutoRefresh = true);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
