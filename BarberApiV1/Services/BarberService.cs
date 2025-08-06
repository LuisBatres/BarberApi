using BarberApiV1.Interfaces;
using BarberApiV1.Models;

namespace BarberApiV1.Services;

public class BarberService : IBarber
{
    public Task<IEnumerable<Barber>> GetAllActiveBarbersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Barber?> GetBarberByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Barber> CreateBarberAsync(BarberRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Barber?> UpdateBarberAsync(int id, BarberRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeactivateBarberAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ServiceModel>> GetBarberServicesAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TimeSlot>> GetBarberAvailabilityAsync(int id, DateTime date)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Appointment>> GetBarberAppointmentsAsync(int id, DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }
}