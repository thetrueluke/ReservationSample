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
            throw new NotImplementedException();
        }

        [HttpPost("add")]
        public async Task<ActionResult<Reservation>> AddReservation([FromBody] ReservationDetails reservationDetails)
        {
            throw new NotImplementedException();
        }

        [HttpPost("update/{reservationId}")]
        public async Task<IActionResult> UpdateReservation(long reservationId, [FromBody] ReservationDetails reservationDetails)
        {
            throw new NotImplementedException();
        }
    }
}
