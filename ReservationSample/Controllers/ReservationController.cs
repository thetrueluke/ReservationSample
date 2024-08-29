using Microsoft.AspNetCore.Mvc;
using ReservationSample.Model;

namespace ReservationSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController(ILogger<ReservationController> logger) : ControllerBase
    {
        private readonly ILogger<ReservationController> logger = logger;

        [HttpGet("get/{reservationId}")]
        public IActionResult GetReservation(long reservationId)
        {
            throw new NotImplementedException();
        }

        [HttpPost("add")]
        public ActionResult<Reservation> AddReservation([FromBody] ReservationDetails reservationDetails)
        {
            throw new NotImplementedException();
        }

        [HttpPost("update/{reservationId}")]
        public IActionResult UpdateReservation(long reservationId)
        {
            throw new NotImplementedException();
        }
    }
}
