using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectralAnalysis.Model
{
    class Calculation
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ImageFilePath  { get; set; }

        public string? PointsFilePath {get; set; }

        public DateTime date { get; set; }
    }
}
