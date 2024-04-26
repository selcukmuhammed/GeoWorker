using GeoWorker.Data;
using GeoWorker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoWorker.Services
{
	public class QProcessorService
	{
		private readonly ILogger<QProcessorService> _logger;
		private readonly IConfiguration _config;
		private readonly IServiceScopeFactory _scopeFactory;
		private readonly HttpClient _client;
		public QProcessorService(ILogger<QProcessorService> logger, IConfiguration config, IServiceScopeFactory scopeFactory)
		{
			_logger = logger;
			_config = config;
			_scopeFactory = scopeFactory;
			_client = new HttpClient();
		}

		public async Task ProcessorQ1Async()
		{
			_logger.LogInformation("Processing Q1...");

			using (var scope = _scopeFactory.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<DataContext>();
				var q1Entiries = context.Q1s.ToList();

				foreach (var q1Entiry in q1Entiries)
				{
					string fullAddress = await GetFullAddress(q1Entiry.Latitude, q1Entiry.Longitude);

					var qCompleted = new QCompleted
					{
						Latitude = q1Entiry.Latitude,
						Longitude = q1Entiry.Longitude,
						FullAddress = fullAddress,
						Q1 = "Processed"
					};
					context.QCompleteds.Add(qCompleted);

					context.Q1s.Remove(q1Entiry);
				}
				await context.SaveChangesAsync();
			}
		}

		public async Task ProcessorQ2Async()
		{
			_logger.LogInformation("Processing Q2...");

			using (var scope = _scopeFactory.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<DataContext>();
				var q2Entiries = context.Q2s.ToList();

				foreach (var q2Entiry in q2Entiries)
				{
					string fullAddress = await GetFullAddress(q2Entiry.Latitude, q2Entiry.Longitude);

					var qCompleted = new QCompleted
					{
						Latitude = q2Entiry.Latitude,
						Longitude = q2Entiry.Longitude,
						FullAddress = fullAddress,
						Q2 = "Processed"
					};
					context.QCompleteds.Add(qCompleted);

					context.Q2s.Remove(q2Entiry);
				}
				await context.SaveChangesAsync();
			}
		}

		private async Task<string> GetFullAddress(string latitude, string longitude)
		{
			try
			{
				string apiKey = _config["Coordinates:Api"];
				string apiUrl = _config["Coordinates:URL"];

				apiUrl = apiUrl.Replace("{latitude}", latitude.ToString()).Replace("{longitude}", longitude.ToString()).Replace("{Api}", apiKey.ToString());
				string a = apiUrl;
				var response = await _client.GetAsync(apiUrl);
				if (response.IsSuccessStatusCode)
				{
					var responseContent = response.Content.ReadAsStringAsync().Result;
					var responseObject = JsonConvert.DeserializeObject<ResponseModel>(responseContent);

					if (responseObject.Results != null && responseObject.Results.Count > 0)
					{
						return responseObject.Results[0].Formatted;
					}
				}
				else
				{
					_logger.LogError("API isteği durum koduyla başarısız oldu: {StatusCode}", response.StatusCode);
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e, "API isteği işlenirken bir hata oluştu.");
			}
			return null;
		}

		public class ResponseModel
		{
			public List<Result> Results { get; set; }
		}

		public class Result
		{
			public string Formatted { get; set; }
		}
	}
}
