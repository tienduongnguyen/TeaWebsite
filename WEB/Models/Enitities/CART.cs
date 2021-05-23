namespace taka.Models.Enitities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CART")]
    public partial class CART
    {
        public int ID { get; set; }

        public int? ID_TEA { get; set; }

        public int? ID_USER { get; set; }

        public int? AMOUNT { get; set; }

        public virtual TEA TEA { get; set; }

        public virtual USER USER { get; set; }
    }
}
