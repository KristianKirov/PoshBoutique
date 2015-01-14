using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PoshBoutique.Models
{
    public class CouponeModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int? Value { get; set; }

        [Required]
        public CouponeValueType? ValueType { get; set; }

        [Required]
        public bool? FreeShipping { get; set; }
    }

    public enum CouponeValueType
    {
        Percent = 1,
        Absolute = 2
    }
}