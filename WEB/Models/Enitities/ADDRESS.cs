namespace taka.Models.Enitities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ADDRESS")]
    public partial class ADDRESS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ADDRESS()
        {
            ORDERs = new HashSet<ORDER>();
        }

        public int ID { get; set; }

        [StringLength(50)]
        public string NAME { get; set; }

        [StringLength(20)]
        public string PHONE { get; set; }

        [Column("ADDRESS")]
        [StringLength(200)]
        public string ADDRESS1 { get; set; }

        public int? ID_USER { get; set; }

        public virtual USER USER { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ORDER> ORDERs { get; set; }
    }
}
