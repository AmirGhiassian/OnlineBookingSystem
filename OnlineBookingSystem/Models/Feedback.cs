using System.ComponentModel.DataAnnotations;

/// <summary>
/// Author: Amir Ghiassian
/// This class is used to create a feedback object that will be used to store feedback information.
/// primary key of FeedbackId which is used to uniquely identify a feedback in the database.
/// </summary>

namespace OnlineBookingSystem.Models
{
    /// <summary>
    /// Model class for a single feedback, used for storing feedback information within the database of feedback
    /// </summary>
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }
        public int ReservationId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Message { get; set; }
    }
}