namespace BarberApiV1.Models;

//Representa un bloque de tiempo disponible para reservar
public class TimeSlot
{
    public TimeOnly StartTime { get; set; }
    
    public TimeOnly EndTime { get; set; }
    
    public DateOnly Date { get; set; }
    
    public bool IsAvailable { get; set; }
    
    public int BarberId { get; set; }

    public string BarberFullName { get; set; }
    
    public string? UnavailableReason { get; set; } // Motivo por el cual no está disponible (si aplica)
    
    public int? AppointmentId { get; set; } // ID de la cita si está ocupado

}
