using GeoWorker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoWorker.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{

		}
		public DbSet<Q1> Q1s { get; set; }
		public DbSet<Q2> Q2s { get; set; }
		public DbSet<QCompleted> QCompleteds { get; set; }
	}
}
