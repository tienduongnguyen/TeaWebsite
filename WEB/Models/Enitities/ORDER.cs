namespace taka.Models.Enitities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ORDER")]
    public partial class ORDER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ORDER()
        {
            ORDER_DETAIL = new HashSet<ORDER_DETAIL>();
        }

        public int ID { get; set; }

        public int? ID_USER { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CREATE_DATE { get; set; }

        public decimal? TOTAL_PRICE { get; set; }

        public int? ORDER_STATUS { get; set; }

        [StringLength(50)]
        public string CLIENT_NAME { get; set; }

        [StringLength(50)]
        public string SHIPPER { get; set; }

        public int? ID_ADDRESS { get; set; }

        [StringLength(500)]
        public string NOTES { get; set; }

        public virtual ADDRESS ADDRESS { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ORDER_DETAIL> ORDER_DETAIL { get; set; }

        public virtual USER USER { get; set; }
    }
}
