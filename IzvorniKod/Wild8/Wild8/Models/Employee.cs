using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wild8.Models
{
    public class Employee
    {
        [Key]
        public string EmployeeID { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Title { get; set; }
        public bool isEmployed { get; set; }
        public bool AdminRights { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

    }
}