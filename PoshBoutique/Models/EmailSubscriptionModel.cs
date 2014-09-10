using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PoshBoutique.Models
{
    public class EmailSubscriptionModel
    {
        [Required]
        [MaxLength(100)]
        [DataType(DataType.EmailAddress, ErrorMessage = "The {1} is invalid")]
        public string Email { get; set; }
    }
}