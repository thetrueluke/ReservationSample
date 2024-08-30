using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReservationSample.Model;

namespace UnitTests
{
    internal class ReservationHelper
    {
        public const long ReservationId = 1;
        public const long ReservationIdBad = 2;

        public static Reservation Reservation { get; } = new()
        {
            Id = ReservationId,
            Details = ReservationDetails!
        };

        public static ReservationDetails ReservationDetails { get; } = new()
        {
            ReserverName = "Name",
            ReserverSurname = "Surname"
        };

        public static ReservationDetails NewReservationDetails { get; } = new()
        {
            ReserverName = "New Name",
            ReserverSurname = "New Surname"
        };
    }
}
