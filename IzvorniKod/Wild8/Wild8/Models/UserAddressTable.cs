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
    
    public partial class UserAddressTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserAddressTable()
        {
            this.OrderUsers = new HashSet<OrderUser>();
        }
    
        public int Id { get; set; }
        public int userId { get; set; }
        public int adressId { get; set; }
    
        public virtual Address Address { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderUser> OrderUsers { get; set; }
        public virtual User User { get; set; }
    }
}