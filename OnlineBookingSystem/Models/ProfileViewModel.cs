using System;

/// <summary>
/// Author: Eric Hanoun
///  This class is used to create a profile view model object that will be used to store profile information.
///  This class has a Username, Email, and PhotoPath field that will be used to store the user's profile information.
/// </summary>

namespace OnlineBookingSystem.Models
{
    public class ProfileViewModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhotoPath { get; set; }
    }
}