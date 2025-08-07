using System.Data;
using BarberApiV1.Interfaces;
using BarberApiV1.Models;
using BarberApiV1.Repositories.Interfaces;

namespace BarberApiV1.Services;

public class BarberService : IBarber
{
    private IRepository _repository;
    
    public BarberService(IRepository repository)
    {
        _repository = repository;
    }
    public async Task<List<Barber>> GetAllActiveBarbersAsync()
    {
        var dataSet
            = await _repository.ExecuteStoredProcedureAsync<DataSet>("getActiveBarbers");
        
        var barberList = new List<Barber>();

        if (dataSet != null && dataSet.Tables.Count > 0)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                var barber = new Barber
                {
                    BarberFirstName = row["BarberFirstName"]?.ToString(),
                    BarberLastName = row["BarberLastName"]?.ToString(),
                    BarberEmail = row["BarberEmail"]?.ToString() ?? string.Empty,
                    BarberPhoneNumber = row["BarberPhoneNumber"]?.ToString() ?? string.Empty
                };
                
                barberList.Add(barber);
            }
        }
        
        return barberList;
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