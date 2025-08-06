using BarberApiV1.Models;

namespace BarberApiV1.Interfaces;

public interface IBarber
{
    Task<IEnumerable<Barber>> GetAllActiveBarbersAsync();
    
    Task<Barber?> GetBarberByIdAsync(int id);
    
    Task<Barber> CreateBarberAsync(BarberRequest request);
    
    Task<Barber?> UpdateBarberAsync(int id, BarberRequest request);
    
    Task<bool> DeactivateBarberAsync(int id);
    
    Task<IEnumerable<ServiceModel>> GetBarberServicesAsync(int id);
    
    Task<IEnumerable<TimeSlot>> GetBarberAvailabilityAsync(int id, DateTime date);
    
    Task<IEnumerable<Appointment>> GetBarberAppointmentsAsync(int id, DateTime startDate, DateTime endDate);
    

}