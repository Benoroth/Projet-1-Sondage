namespace Sondage.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TChoix")]
    public partial class TChoix
    {
        [Key]
        public int idChoix { get; set; }

        [StringLength(50)]
        public string nomChoix { get; set; }

        public int? idSondage { get; set; }

        public int? nbVoteChoix { get; set; }
    }
}
