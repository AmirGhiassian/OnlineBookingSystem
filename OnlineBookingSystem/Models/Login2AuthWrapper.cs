using OnlineBookingSystem.Models;

/// <summary>
/// Author: Amir Ghiassian
/// This class is designed to be used in assistance with the wrapper class to complete
/// 2FA authentication. This class is used to wrap the customerRemote object and the CustomerLocal
/// </summary>
public class Login2AuthWrapper
{
    public Customer customerRemote { get; set; }
    public LoginViewModel CustomerLocal { get; set; }

    public Login2AuthWrapper()
    {
        // Add any necessary initialization logic here
    }

    /// <summary>
    /// This constructor is used to create a Login2AuthWrapper object with the customerRemote and CustomerLocal
    /// </summary>
    /// <param name="customerRemote"></param>
    /// <param name="CustomerLocal"></param>
    public Login2AuthWrapper(Customer customerRemote, LoginViewModel CustomerLocal)
    {
        this.customerRemote = customerRemote;
        this.CustomerLocal = CustomerLocal;
    }
}