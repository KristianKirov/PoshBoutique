using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PoshBoutique.Identity
{
    public class Profile
    {
        [Key, ForeignKey("User")]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        [Column(TypeName="Money")]
        public decimal TotalExpenses { get; set; }
    }
}