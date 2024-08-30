using Microsoft.AspNetCore.Mvc;
using ReservationSample.Model;
using ReservationSample.Services;

namespace ReservationSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController(IRepositoryService<Reservation> repositoryService, ILogger<ReservationController> logger) : ControllerBase
    {
        private readonly IRepositoryService<Reservation> repositoryService = repositoryService;
        private readonly ILogger<ReservationController> logger = logger;

        [HttpGet("get/{reservationId}")]
        public async Task<ActionResult<Reservation>> GetReservation(long reservationId)
        {
            var reservation = await repositoryService.GetAsync(reservationId);
            return reservation is not null ? Ok(reservation) : NotFound();
        }

        [HttpPost("add")]
        public async Task<ActionResult<Reservation>> AddReservation([FromBody] ReservationDetails reservationDetails)
        {
            var reservation = new Reservation()
            {
                Id = await repositoryService.GetCountAsync() + 1,
                Details = reservationDetails
            };
            try
            {
                await repositoryService.AddAsync(reservation);
                return CreatedAtAction(null, reservation);
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
        }

        [HttpPost("update/{reservationId}")]
        public async Task<IActionResult> UpdateReservation(long reservationId, [FromBody] ReservationDetails reservationDetails)
        {
            try
            {
                var newReservation = new Reservation()
                {
                    Id = reservationId,
                    Details = reservationDetails
                };
                await repositoryService.UpdateAsync(reservationId, newReservation);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
