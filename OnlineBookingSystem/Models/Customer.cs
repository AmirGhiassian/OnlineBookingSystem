using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace OnlineBookingSystem.Models
{
    public class Customer : IdentityUser //Model class for a single customer, used for registration within the CustomerRepo
    {
        [Key]
        public int CustID { get; set; }
        public List<int>? Reservations { get; set; }
    }
}