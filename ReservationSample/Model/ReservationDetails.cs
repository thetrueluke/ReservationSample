using System.ComponentModel.DataAnnotations;

namespace ReservationSample.Model
{
    public class ReservationDetails
    {
        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public int RoomNumber { get; set; }

        [Required]
        public required string ReserverName { get; set; }

        [Required]
        public required string ReserverSurname { get; set; }

        public decimal Cost { get; set; }
    }
}
