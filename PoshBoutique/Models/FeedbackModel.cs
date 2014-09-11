using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PoshBoutique.Models
{
    public class FeedbackModel
    {
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "The {1} is invalid")]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string Message { get; set; }
    }
}