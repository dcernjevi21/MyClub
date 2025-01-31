namespace EntitiesLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("AthleteEvaluation")]
    public partial class AthleteEvaluation
    {
        [Key]
        public int EvaluationID { get; set; }

        [StringLength(255)]
        public string Comment { get; set; }

        public decimal Mark { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public int UserID { get; set; }

        public virtual User User { get; set; }
    }
}
