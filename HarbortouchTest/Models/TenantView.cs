using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HarbortouchTest.Models
{
    public class TenantView
    {   
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }


        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Cost { get; set; }
        public ICollection<RentalView> Rentals { get; set; }

    }
}