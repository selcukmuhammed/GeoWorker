using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoWorker.Models
{
	public class QCompleted
	{
		public long Id { get; set; }
		public string Latitude { get; set; }
		public string Longitude { get; set; }
		public string FullAddress { get; set; }
		public string? Q1 { get; set; }
		public string? Q2 { get; set; }
	}
}
