using GeoWorker.Services;

namespace GeoWorker
{
	public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;
		private readonly IServiceScopeFactory _scopeFactory;

		public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
		{
			_logger = logger;
			_scopeFactory = scopeFactory;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("Worker Started...");
			while (!stoppingToken.IsCancellationRequested)
			{
				using (var scope = _scopeFactory.CreateScope())
				{
					var qProcessor = scope.ServiceProvider.GetRequiredService<QProcessorService>();

					Task task2 = Task.Run(() => qProcessor.ProcessorQ2Async());
					Task task1 = Task.Run(() => qProcessor.ProcessorQ1Async());
					await Task.WhenAll(task2, task1);
				}

				await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
			}

			_logger.LogInformation("Worker stopped.");
		}
	}
}
