using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using EntityFramework.Triggers;
using Wild8.DAL;

namespace Wild8.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string UserNote { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime AcceptanceDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string EmpolyeeID { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set;}
    }
}