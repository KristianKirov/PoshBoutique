using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PoshBoutique.Identity
{
    public class Address
    {
        [Key, ForeignKey("User")]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string District { get; set; }

        [Required]
        public string Commune { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string PostCode { get; set; }

        [Required]
        public string AddresDetails { get; set; }
    }
}