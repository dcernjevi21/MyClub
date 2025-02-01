namespace DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Membership")]
    public partial class Membership
    {
        public int MembershipID { get; set; }

        public int UserID { get; set; }

        public decimal Amount { get; set; }

        [Column(TypeName = "date")]
        public DateTime Month { get; set; }

        public bool Paid { get; set; }

        public virtual User User { get; set; }

        public string FirstName => User?.FirstName ?? "N/A";
        public string LastName => User?.LastName ?? "N/A";
    }
}
