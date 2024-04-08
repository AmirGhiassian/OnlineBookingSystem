namespace OnlineBookingSystem.Models
{
    public class Wrapper
    {
        // public Customer Customer { get; set; }
        public Feedback Feedback { get; set; }
        public Reservation Reservation { get; set; }
        public Restaurant Restaurant { get; set; }
        public RestaurantContext _dbContext { get; set; }

        public Customer Customer { get; set; }

        // public List<Customer> Customers;
        public List<Feedback> Feedbacks;
        public List<Reservation> Reservations;
        public List<Restaurant> Restaurants;


        public Wrapper(List<Reservation> reservations)
        {
            Reservations = reservations;

        }

        public Wrapper(Reservation reservation, Restaurant restaurant, Customer customer)
        {
            Reservation = reservation;
            Restaurant = restaurant;
            Customer = customer;
        }

        public Wrapper(Reservation reservation, Restaurant restaurant)
        {
            Reservation = reservation;
            Restaurant = restaurant;
        }

        public Wrapper(List<Reservation> reservations, Restaurant restaurant)
        {
            Reservations = reservations;
            Restaurant = restaurant;
        }



        // public Wrapper(List<Customer> customers)
        // {
        //     Customers = customers;
        // }

        public Wrapper(List<Feedback> feedbacks)
        {
            Feedbacks = feedbacks;
        }



        public Wrapper(List<Restaurant> restaurants)
        {
            Restaurants = restaurants;
        }

        public Wrapper(RestaurantContext dbContext)
        {
            _dbContext = dbContext;
        }


    }
}