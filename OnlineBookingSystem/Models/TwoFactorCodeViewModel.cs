using System.ComponentModel.DataAnnotations;

/// <summary>
/// Author: Amir Ghiassian
/// This class is used to create a TwoFactorCodeViewModel object that will be used to store two factor code information.
/// It has a required code field that will be used to store the user's two factor code.
/// </summary>

namespace OnlineBookingSystem.Models
{
    /// <summary>
    /// Model class for a single two factor code, used for two factor authentication within the database of two factor codes
    /// </summary>
    public class TwoFactorCodeViewModel
    {
        /// <summary>
        /// Required code field that will be used to store the user's two factor code.
        /// </summary>
        [Required]
        public required string code { get; set; }
    }
}
