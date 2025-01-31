namespace EntitiesLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Match")]
    public partial class Match
    {
        public int MatchID { get; set; }

        public int TeamID { get; set; }

        
        [Column(TypeName = "date")]
        public DateTime MatchDate { get; set; }

        [Required]
        [StringLength(50)]
        public string OpponentTeam { get; set; }

        [Required]
        [StringLength(100)]
        public string Location { get; set; }

        [StringLength(50)]
        public string Result { get; set; }

        public TimeSpan StartTime { get; set; }

        [StringLength(2000)]
        public string Summary { get; set; }

        public virtual Team Team { get; set; }

        [StringLength(50)]
        public string Status { get; set; }
    }
}
