using GeoWorker;
using GeoWorker.Data;
using Microsoft.EntityFrameworkCore;

IHost host = Host.CreateDefaultBuilder(args)
	.ConfigureServices((hostContext, services) =>
	{
		services.AddDbContext<DataContext>(options =>
		{
			options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection"));
		});

		services.AddHostedService<Worker>();
	})
	.Build();

host.Run();
