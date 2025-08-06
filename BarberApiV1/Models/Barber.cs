namespace BarberApiV1.Models;

public class Barber
{
    public int BarberId { get; set; }
    
    public string BarberFirstName { get; set; }
    
    public string BarberLastName { get; set; }
    
    public string BarberEmail { get; set; }
    
    public string BarberPhoneNumber { get; set; }
    
    public DateTime BarberHireDate { get; set; }
    
    public bool IsActive { get; set; }

}
