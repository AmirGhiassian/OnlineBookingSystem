using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

/// <summary>
/// Author: Eric Hanoun
// This class is used to create a customer object that will be used to store customer information
// in the database. This class inherits from IdentityUser which is a class that is used to store
// user information in the database. This class has a primary key of CustID which is used to uniquely
// identify a customer in the database. This class also has a list of integers that are used to store
// the reservation IDs of the reservations that the customer has made. This class is used to store
// customer information in the database.
/// </summary>

namespace OnlineBookingSystem.Models
{
    /// <summary>
    /// Model class for a single customer, used for registration within the CustomerRepo
    /// Extends IdentityUser to store user information in the database for Identity
    /// </summary>
    public class Customer : IdentityUser
    {
        public List<int>? Reservations { get; set; }
    }
}
