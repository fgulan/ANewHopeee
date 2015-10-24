using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Wild8.Models
{

    public enum UserType { UserTypeOwner, UserTypeWorker, UserTypeClient };

    public class User
    {
        [Key][MinLength(4), MaxLength(18)]
        public String username { get; set; }
        [Required]
        public String hashPaswword { get; set; }
        [Required]
        public String firstName { get; set; }
        [Required]
        public String lastName { get; set; }
        [Required]
        public String email { get; set; }
        public String phoneNumber { get; set; }
        [Required]
        public Address address { get; set; }
        public UserType userType { get; set; }
    }
    public class Wild8DBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
    }
}