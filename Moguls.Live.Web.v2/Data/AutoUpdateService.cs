using EventAggregator.Blazor;

namespace Moguls.Live.Web.v2.Data;

public class AutoUpdateService : BackgroundService
{
	private readonly ILogger<AutoUpdateService> _logger;
	private readonly IEventAggregator _eventAggregator;

	public AutoUpdateService(ILogger<AutoUpdateService> logger, IEventAggregator eventAggregator)
	{
		_logger = logger;
		_eventAggregator = eventAggregator;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			await Task.Delay(3000, stoppingToken);
			_logger.LogInformation("Send update to live result");
			await _eventAggregator.PublishAsync(new UpdatedScore { UpdatedOn = DateTime.Now });
		}
	}
}