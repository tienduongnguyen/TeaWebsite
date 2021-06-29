namespace taka.Models.Enitities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("IMAGE")]
    public partial class IMAGE
    {
        public int ID { get; set; }

        [StringLength(4000)]
        public string URL { get; set; }

        public int? ID_TEA { get; set; }

        public int? ORDER { get; set; }

        public virtual TEA TEA { get; set; }
    }
}
