//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wild8.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.OrderMeals = new HashSet<OrderMeal>();
        }
    
        public int Id { get; set; }
        public int orderUserId { get; set; }
        public int staffId { get; set; }
        public System.DateTime orderDate { get; set; }
        public System.DateTime acceptanceDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderMeal> OrderMeals { get; set; }
        public virtual OrderUser OrderUser { get; set; }
        public virtual User User { get; set; }
    }
}
