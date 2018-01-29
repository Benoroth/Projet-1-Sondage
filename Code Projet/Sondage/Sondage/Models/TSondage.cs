namespace Sondage.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TSondage")]
    public partial class TSondage
    {
        [Key]
        public int idSondage { get; set; }

        public float? dureeSondage { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] dateSondage { get; set; }

        public int? nbVote { get; set; }

        [StringLength(100)]
        public string nomQuestion { get; set; }

        [StringLength(50)]
        public string lienResult { get; set; }

        [StringLength(50)]
        public string lienPartage { get; set; }

        [StringLength(50)]
        public string lienSuppr { get; set; }

        public bool? choixMultiple { get; set; }

        [Column("private")]
        public bool? _private { get; set; }

        public double? note { get; set; }
    }
}
