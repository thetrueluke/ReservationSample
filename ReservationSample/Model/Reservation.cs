using System.ComponentModel.DataAnnotations;

namespace ReservationSample.Model
{
    public class Reservation
    {
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        [Required]
        public required ReservationDetails Details { get; set; }
    }
}
