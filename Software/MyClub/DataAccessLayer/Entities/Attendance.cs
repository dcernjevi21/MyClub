namespace DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Attendance")]
    public partial class Attendance
    {
        public int AttendanceID { get; set; }

        public int TrainingID { get; set; }

        public int UserID { get; set; }

        public int StatusID { get; set; }

        public virtual Status Status { get; set; }

        public virtual Training Training { get; set; }

        public virtual User User { get; set; }
    }
}
