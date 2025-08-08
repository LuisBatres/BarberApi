using BarberApiV1.Models;

namespace BarberApiV1.Utils;

public class RequestValidator
{
    public static void ValidateBarberRequest(BarberRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "La solicitud del barbero no puede ser nula");
        }

        if (string.IsNullOrWhiteSpace(request.BarberFirstName.Trim()))
        {
            throw new ArgumentException("El nombre del barbero es obligatorio", nameof(request.BarberFirstName));
        }

        if (!IsValidName(request.BarberFirstName.Trim().ToUpper()))
        {
            
        }

        if (string.IsNullOrWhiteSpace(request.BarberLastName.Trim()))
        {
            throw new ArgumentException("El apellido del barbero es obligatorio", nameof(request.BarberLastName));
        }
        
        if (!IsValidName(request.BarberLastName.Trim().ToUpper()))
        {
            
        }

        if (string.IsNullOrWhiteSpace(request.BarberEmail.Trim()))
        {
            throw new ArgumentException("El email del barbero es obligatorio", nameof(request.BarberEmail));
        }
        
        if (!IsValidEmail(request.BarberEmail.Trim()))
        {
            
        }

        if (string.IsNullOrWhiteSpace(request.BarberPhoneNumber.Trim()))
        {
            throw new ArgumentException("El teléfono del barbero es obligatorio", nameof(request.BarberPhoneNumber));
        }

        if (!IsValidPhone(request.BarberEmail.Trim()))
        {
            
        }
    }

    private static bool IsValidPhone(string phone)
    {
        throw new NotImplementedException();
    }

    private static bool IsValidEmail(string email)
    {
        throw new NotImplementedException();
    }

    private static bool IsValidName(string name)
    {
        throw new NotImplementedException();
    }
}