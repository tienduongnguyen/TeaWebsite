namespace taka.Models.Enitities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TEA")]
    public partial class TEA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TEA()
        {
            CARTs = new HashSet<CART>();
            IMAGEs = new HashSet<IMAGE>();
            ORDER_DETAIL = new HashSet<ORDER_DETAIL>();
            RATEs = new HashSet<RATE>();
        }

        public int ID { get; set; }

        [StringLength(500)]
        public string TITLE { get; set; }

        public decimal? PRICE { get; set; }

        public int? AMOUNT { get; set; }

        [StringLength(4000)]
        public string DESCRIPTION { get; set; }

        public int? ID_CATEGORY { get; set; }

        [StringLength(500)]
        public string KEYSEARCH { get; set; }

        public int? RATEPOINT { get; set; }

        public int? RATECOUNT { get; set; }

        public int? isHIDDEN { get; set; }

        [StringLength(4000)]
        public string STORY { get; set; }

        [StringLength(4000)]
        public string INGREDIENT { get; set; }

        [StringLength(4000)]
        public string FUNCTION { get; set; }

        [StringLength(100)]
        public string CAFFEIN { get; set; }

        [StringLength(100)]
        public string WEIGHT { get; set; }

        [StringLength(4000)]
        public string USE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CART> CARTs { get; set; }

        public virtual CATEGORY CATEGORY { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IMAGE> IMAGEs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ORDER_DETAIL> ORDER_DETAIL { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RATE> RATEs { get; set; }
    }
}
