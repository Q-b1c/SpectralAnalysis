using Microsoft.EntityFrameworkCore;
using SpectralAnalysis.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectralAnalysis
{
    class appContext: DbContext
    {
        public DbSet<Calculation> Calculations { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=dbapp.db");
        }
    }
}
