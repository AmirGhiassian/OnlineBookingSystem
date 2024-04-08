using OnlineBookingSystem.Models;

public class Login2AuthWrapper
{
    public Customer customerRemote { get; set; }
    public LoginViewModel CustomerLocal { get; set; }

    public Login2AuthWrapper()
    {
        // Add any necessary initialization logic here
    }

    public Login2AuthWrapper(Customer customerRemote, LoginViewModel CustomerLocal)
    {
        this.customerRemote = customerRemote;
        this.CustomerLocal = CustomerLocal;
    }
}