using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PoshBoutique.Areas.Admin.Models
{
    public class DiscountModel
    {
        [Required]
        [Display(Name="Стойност")]
        public int Value { get; set; }

        [Required]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Вид")]
        public int Type { get; set; }
    }
}