/// <summary>
/// Author: Amir Ghiassian
/// The Wrapper class is a utility class that wraps various entities such as Customer, Feedback, Reservation, and Restaurant.
/// It is used to pass multiple entities as a single object.
/// </summary>

namespace OnlineBookingSystem.Models
{
    public class Wrapper
    {
        /// <summary>
        /// Represents the Feedback object to be wrapped.
        /// </summary>
        public Feedback Feedback { get; set; }

        /// <summary>
        /// Represents the Reservation object to be wrapped.
        /// </summary>
        public Reservation Reservation { get; set; }

        /// <summary>
        /// Represents the Restaurant object to be wrapped.
        /// </summary>
        public Restaurant Restaurant { get; set; }

        /// <summary>
        /// Represents the database context for restaurants
        /// </summary>
        public RestaurantContext _dbContext { get; set; }

        /// <summary>
        /// Gets or sets the customer instance
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// Represents the list of feedback instances
        /// </summary>
        public List<Feedback> Feedbacks;

        /// <summary>
        /// Represents the list of reservation instances
        /// </summary>
        public List<Reservation> Reservations;

        /// <summary>
        /// Represents the list of restaurant instances
        /// </summary>
        public List<Restaurant> Restaurants;


        /// <summary>
        /// Initializes a new instance of the Wrapper class with a list of reservations.
        /// </summary>
        /// <param name="reservations">The list of reservations.</param>
        public Wrapper(List<Reservation> reservations)
        {
            Reservations = reservations;

        }

        /// <summary>
        /// Author: Eric Hanoun
        /// Initializes a new instance of the Wrapper class with a list of reservations and a list of restaurants.
        /// </summary>
        public Wrapper(List<Reservation> reservations, List<Restaurant> restaurants)
        {
            Reservations = reservations;
            Restaurants = restaurants;
        }

        /// <summary>
        /// Initializes a new instance of the Wrapper class with a reservation, a restaurant, and a customer.
        /// </summary>
        /// <param name="reservation">The reservation instance.</param>
        /// <param name="restaurant">The restaurant instance.</param>
        /// <param name="customer">The customer instance.</param>
        public Wrapper(Reservation reservation, Restaurant restaurant, Customer customer)
        {
            Reservation = reservation;
            Restaurant = restaurant;
            Customer = customer;
        }

        /// <summary>
        /// Initializes a new instance of the Wrapper class with a reservation and a restaurant.
        /// </summary>
        /// <param name="reservation">The reservation instance.</param>
        /// <param name="restaurant">The restaurant instance.</param>
        public Wrapper(Reservation reservation, Restaurant restaurant)
        {
            Reservation = reservation;
            Restaurant = restaurant;
        }

        /// <summary>
        /// Initializes a new instance of the Wrapper class with a list of reservations and restaurant.
        /// </summary>
        /// <param name="reservations">The list of reservations.</param>
        /// <param name="restaurant">The restaurant instance.</param>
        public Wrapper(List<Reservation> reservations, Restaurant restaurant)
        {
            Reservations = reservations;
            Restaurant = restaurant;
        }

        /// <summary>
        /// Initializes a new instance of the Wrapper class with a list of feedback objects
        /// </summary>
        /// <param name="feedbacks">The list of feedback objects</param>
        public Wrapper(List<Feedback> feedbacks)
        {
            Feedbacks = feedbacks;
        }


        /// <summary>
        /// Initializes a new instance of the Wrapper class with a list of restaurant objects
        /// </summary>
        /// <param name="restaurants">The list of restaurant objects</param>
        public Wrapper(List<Restaurant> restaurants)
        {
            Restaurants = restaurants;
        }

        /// <summary>
        /// Initializes a new instance of the Wrapper class with a RestaurantContext object
        /// </summary>
        /// <param name="dbContext">The RestaurantContext object</param>
        public Wrapper(RestaurantContext dbContext)
        {
            _dbContext = dbContext;
        }


    }
}