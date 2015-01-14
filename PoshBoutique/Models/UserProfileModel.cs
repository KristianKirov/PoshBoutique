using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoshBoutique.Models
{
    public class UserProfileModel
    {
        public Guid UserId { get; set; }

        public decimal TotalExpenses { get; set; }
    }
}