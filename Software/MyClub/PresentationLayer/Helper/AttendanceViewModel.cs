using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Helper
{
    public class AttendanceViewModel
    {
        public User User { get; set; }
        public int StatusID { get; set; }
        public string Notes { get; set; }
        public bool IsExistingAttendance { get; set; }
        public int? AttendanceID { get; set; }
    }
}
