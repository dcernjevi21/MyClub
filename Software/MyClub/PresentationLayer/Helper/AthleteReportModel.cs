using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Helper
{
    public class AthleteReportModel
    {
        public string FullName { get; set; }
        public int TotalEvents { get; set; }
        public int Attendances { get; set; }
        public int Absences { get; set; }
        public double AttendancePercentage { get; set; }
    }
}
