namespace BarberApiV1.Models;

public class Customer
{
    public int CustomerId { get; set; }

    public string CustomerFirstName { get; set; }
    
    public string CustomerLastName { get; set; }

    public string CustomerEmail { get; set; }

    public string CustomerPhoneNumber { get; set; }

    public string CustomerPassword { get; set; }

    public DateTime CustomerRegisteredDate { get; set; }
}
