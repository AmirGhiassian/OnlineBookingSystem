namespace OnlineBookingSystem.Models
{
    public class Wrapper
    {
        // public Customer Customer { get; set; }
        public Feedback Feedback { get; set; }
        public Reservation Reservation { get; set; }
        public Restaurant Restaurant { get; set; }
        public ResturantContext _dbContext { get; set; }

        // public List<Customer> Customers;
        public List<Feedback> Feedbacks;
        public List<Reservation> Reservations;
        public List<Restaurant> Restaurants;


        public Wrapper(List<Reservation> reservations)
        {
            Reservations = reservations;

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

        public Wrapper(ResturantContext dbContext)
        {
            _dbContext = dbContext;
        }


    }
}