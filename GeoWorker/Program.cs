using GeoWorker;
using GeoWorker.Data;
using GeoWorker.Services;
using Microsoft.EntityFrameworkCore;

IHost host = Host.CreateDefaultBuilder(args)
	.ConfigureServices((hostContext, services) =>
	{
		services.AddDbContext<DataContext>(options =>
		{
			options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection"));
		});

		services.AddScoped<QProcessorService>();
		services.AddHostedService<Worker>();
	})
	.Build();

host.Run();
