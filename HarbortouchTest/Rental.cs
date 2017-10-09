namespace HarbortouchTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Rental")]
    public partial class Rental
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Rental()
        {
            Tenants = new HashSet<Tenant>();
        }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Required]
        [StringLength(150)]
        public string Address { get; set; }

        [Required]
        [StringLength(150)]
        public string City { get; set; }

        [Required]
        [StringLength(150)]
        public string State { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        public int ZipCode { get; set; }

        [Required]
        [StringLength(150)]        
        public string OwnerName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tenant> Tenants { get; set; }
    }
}
