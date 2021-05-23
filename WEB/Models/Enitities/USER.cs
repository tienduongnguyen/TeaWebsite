namespace taka.Models.Enitities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("USER")]
    public partial class USER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public USER()
        {
            ADDRESSes = new HashSet<ADDRESS>();
            CARTs = new HashSet<CART>();
            ORDERs = new HashSet<ORDER>();
            RATEs = new HashSet<RATE>();
        }

        public int ID { get; set; }

        [StringLength(20)]
        public string PHONE { get; set; }

        [StringLength(200)]
        public string PASSWORD { get; set; }

        [StringLength(50)]
        public string EMAIL { get; set; }

        [StringLength(50)]
        public string FULLNAME { get; set; }

        public int? isBAN { get; set; }

        [StringLength(50)]
        public string KEYSEARCH { get; set; }

        [StringLength(250)]
        public string GOOGLE_ID { get; set; }

        [StringLength(10)]
        public string GENDER { get; set; }

        [StringLength(50)]
        public string BIRTHDAY { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ADDRESS> ADDRESSes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CART> CARTs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ORDER> ORDERs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RATE> RATEs { get; set; }
    }
}
