namespace DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Method")]
    public partial class Method
    {
        public int MethodID { get; set; }

        [Required]
        [StringLength(50)]
        public string MethodName { get; set; }
    }
}
