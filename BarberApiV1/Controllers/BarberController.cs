using BarberApiV1.CustomExceptions;
using BarberApiV1.Interfaces;
using BarberApiV1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberApiV1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BarberController : ControllerBase
    {
        private readonly IBarber _barber;

        public BarberController(IBarber barber)
        {
            _barber = barber;
        }

        /// <summary>
        /// Obtiene todos los barberos activos
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Barber>>> GetAllBarbers()
        {
            try
            {
                var barbers = await _barber.GetAllActiveBarbersAsync();
                return Ok(barbers);
            }
            catch (CustomHandledException cEx)
            {
                CustomHandledExceptionResponse exceptionResponse = new CustomHandledExceptionResponse()
                {
                    Function = cEx.Function,
                    Class = cEx.Class,
                    FunctionArguments = cEx.FunctionArguments,
                    Line = cEx.Line,
                    Message = cEx.Message
                };
                
                return StatusCode(499, exceptionResponse);
            }
            catch (Exception e)
            {
                CustomHandledException cEx = new CustomHandledException(e)
                {
                    Function = "GetAllBarbers",
                    Class = "BarberController"
                };
                
                CustomHandledExceptionResponse exceptionResponse = new CustomHandledExceptionResponse()
                {
                    Function = cEx.Function,
                    Class = cEx.Class,
                    FunctionArguments = cEx.FunctionArguments,
                    Line = cEx.Line,
                    Message = cEx.Message
                };
                
                return StatusCode(500, exceptionResponse);
            }
        }

        /// <summary>
        /// Obtiene un barbero por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Barber>> GetBarberById(int id)
        {
            try
            {
                var barber = await _barber.GetBarberByIdAsync(id);
                if (barber == null)
                    return NotFound($"Barbero con ID {id} no encontrado");

                return Ok(barber);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Crea un nuevo barbero
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Barber>> CreateBarber([FromBody] BarberRequest request)
        {
            try
            {
                var createdBarber = await _barber.CreateBarberAsync(request);
                return CreatedAtAction(nameof(GetBarberById), new { id = createdBarber.BarberId }, createdBarber);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Actualiza un barbero existente
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Barber>> UpdateBarber(int id, [FromBody] BarberRequest request)
        {
            try
            {
                var updatedBarber = await _barber.UpdateBarberAsync(id, request);
                if (updatedBarber == null)
                    return NotFound($"Barbero con ID {id} no encontrado");

                return Ok(updatedBarber);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Desactiva un barbero (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeactivateBarber(int id)
        {
            try
            {
                var result = await _barber.DeactivateBarberAsync(id);
                if (!result)
                    return NotFound($"Barbero con ID {id} no encontrado");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene los servicios que ofrece un barbero específico
        /// </summary>
        [HttpGet("{id}/services")]
        public async Task<ActionResult<IEnumerable<ServiceModel>>> GetBarberServices(int id)
        {
            try
            {
                var services = await _barber.GetBarberServicesAsync(id);
                return Ok(services);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene la disponibilidad de un barbero en una fecha específica
        /// </summary>
        [HttpGet("{id}/availability")]
        public async Task<ActionResult<IEnumerable<TimeSlot>>> GetBarberAvailability(int id,
            [FromQuery] DateTime date)
        {
            try
            {
                var availability = await _barber.GetBarberAvailabilityAsync(id, date);
                return Ok(availability);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene las citas de un barbero en un rango de fechas
        /// </summary>
        [HttpGet("{id}/appointments")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetBarberAppointments(
            int id, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var appointments = await _barber.GetBarberAppointmentsAsync(id, startDate, endDate);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}