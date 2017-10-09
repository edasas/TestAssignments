using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HarbortouchTest.Models
{
    public class TenantRentingData
    {
        public int Id { get; set; }
        public string TenantName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Cost { get; set; }
        public bool IsRenting { get; set; }

        public string DateString
        {
            get
            {
                return GetDateString();
            }
        }

        private string GetDateString()
        {
            var startDateStr = StartDate != null ? StartDate.ToShortDateString() : "";
            var endDateStr = EndDate != null ? EndDate.ToShortDateString() : "";
            return string.Format("{0} = > {1}", startDateStr, endDateStr);
        }
    }
}