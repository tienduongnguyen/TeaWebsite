namespace taka.Models.Enitities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ORDER_DETAIL
    {
        public int ID { get; set; }

        public int? ID_ORDER { get; set; }

        public int? ID_TEA { get; set; }

        public int? AMOUNT { get; set; }

        public virtual ORDER ORDER { get; set; }

        public virtual TEA TEA { get; set; }
    }
}
