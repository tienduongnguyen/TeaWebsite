namespace taka.Models.Enitities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RATE")]
    public partial class RATE
    {
        public int ID { get; set; }

        [Column("RATE")]
        public int? RATE1 { get; set; }

        [StringLength(4000)]
        public string COMMENT { get; set; }

        public int? ID_USER { get; set; }

        public int? ID_TEA { get; set; }

        public virtual TEA TEA { get; set; }

        public virtual USER USER { get; set; }
    }
}
