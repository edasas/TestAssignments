using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HarbortouchTest.Models
{
    public class RentalView
    {
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
        public ICollection<TenantRentingData> Tenants { get; set; }

        public RentalView()
        {
            Tenants = new Collection<TenantRentingData>();
        }

    }
}