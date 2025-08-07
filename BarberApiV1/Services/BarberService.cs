using System.Data;
using BarberApiV1.CustomExceptions;
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
        List<Barber> barberList = new List<Barber>();

        try
        {
            var dataSet
                = await _repository.ExecuteStoredProcedureAsync<DataSet>("getActiveBarbers");

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
        }
        catch (CustomHandledException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new CustomHandledException(e)
            {
                Function = "GetAllActiveBarbersAsync",
                Class = "BarberService"
            };
        }
        
        return barberList;
    }

    public async Task<Barber?> GetBarberByIdAsync(int id)
    {
        try
        {
            var dataSet = await _repository.ExecuteStoredProcedureAsync<DataSet>("getBarber", new { id = id });

            if (dataSet?.Tables?.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                var row = dataSet.Tables[0].Rows[0];

                return new Barber
                {
                    BarberId = Convert.ToInt32(row["BarberId"]),
                    BarberFirstName = row["BarberFirstName"].ToString(),
                    BarberLastName = row["BarberLastName"].ToString(),
                    BarberEmail = row["BarberEmail"].ToString(),
                    BarberPhoneNumber = row["BarberPhoneNumber"].ToString(),
                    BarberHireDate = Convert.ToDateTime(row["BarberHireDate"]),
                    IsActive = Convert.ToBoolean(row["IsActive"])
                };
            }
        }
        catch (CustomHandledException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new CustomHandledException(e)
            {
                Function = "GetBarberByIdAsync",
                Class = "BarberService"
            };
        }
        
        return null;
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