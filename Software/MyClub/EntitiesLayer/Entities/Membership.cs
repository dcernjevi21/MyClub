namespace DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Membership")]
    public partial class Membership
    {
        public int MembershipID { get; set; }

        public int UserID { get; set; }

        public decimal Amount { get; set; }

        public int MethodID { get; set; }

        [Column(TypeName = "date")]
        public DateTime Month { get; set; }

        public bool Paid { get; set; }

        public virtual Method Method { get; set; }

        public virtual User User { get; set; }
    }
}
