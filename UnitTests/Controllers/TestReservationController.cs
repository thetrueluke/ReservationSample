using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ReservationSample.Controllers;
using ReservationSample.Model;
using ReservationSample.Services;

namespace UnitTests.Controllers
{
    [TestClass]
    public class TestReservationController
    {
        private Mock<IRepositoryService<Reservation>> repositoryServiceMock = default!;
        private Mock<ILogger<ReservationController>> loggerMock = default!;
        private ReservationController controller = default!;

        [TestInitialize]
        public void Setup()
        {
            repositoryServiceMock = new Mock<IRepositoryService<Reservation>>();
            loggerMock = new Mock<ILogger<ReservationController>>();
            controller = new ReservationController(repositoryServiceMock.Object, loggerMock.Object);
        }

        [TestMethod]
        public async Task GetReservation_WithExistingId_ShouldReturnOk()
        {
            // Arrange
            var reservationId = 1L;
            var reservation = new Reservation
            {
                Id = reservationId,
                Details = null!
            };
            repositoryServiceMock.Setup(r => r.GetAsync(reservationId)).ReturnsAsync(reservation);

            // Act
            var result = await controller.GetReservation(reservationId);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result.As<OkObjectResult>();
            okResult.Value.Should().BeOfType<Reservation>();
            okResult.Value.As<Reservation>().Id.Should().Be(reservationId);
        }

        [TestMethod]
        public async Task GetReservation_WithNonExistingId_ShouldReturnNotFound()
        {
            // Arrange
            var reservationId = 1L;
            var reservationIdBad = 2L;
            var reservation = new Reservation
            {
                Id = reservationId,
                Details = null!
            };
            repositoryServiceMock.Setup(r => r.GetAsync(reservationId)).ReturnsAsync((Reservation?)null);

            // Act
            var result = await controller.GetReservation(reservationIdBad);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task AddReservation_WithValidDetails_ShouldReturnCreated()
        {
            // Arrange
            var reservationDetails = new ReservationDetails
            {
                ReserverName = string.Empty,
                ReserverSurname = string.Empty
            };

            // Act
            var result = await controller.AddReservation(reservationDetails);

            // Assert
            result.Result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result.Result.As<CreatedAtActionResult>();
            createdResult.Value.Should().BeOfType<Reservation>();
            var returnedReservation = createdResult.Value.As<Reservation>();
            returnedReservation.Id.Should().Be(1);
        }

        [TestMethod]
        public async Task AddReservation_WithReservedRoomId_ShouldReturnBadRequest()
        {
            // Arrange
            var reservationId = 1L;
            var reservationDetails = new ReservationDetails
            {
                ReserverName = string.Empty,
                ReserverSurname = string.Empty
            };
            var reservation = new Reservation
            {
                Id = reservationId,
                Details = reservationDetails
            };
            repositoryServiceMock.Setup(r => r.AddAsync(It.Is<Reservation>(r => r.Details == reservationDetails))).ThrowsAsync(new InvalidOperationException());

            // Act
            var result = await controller.AddReservation(reservationDetails);

            // Assert
            result.Result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public async Task UpdateReservation_WithValidId_ShouldReturnOk()
        {
            // Arrange
            var reservationId = 1L;
            var existingReservationDetails = new ReservationDetails()
            {
                ReserverName = "Old name",
                ReserverSurname = "Old surname"
            };
            var existingReservation = new Reservation()
            {
                Id = reservationId,
                Details = existingReservationDetails
            };
            var newReservationDetails = new ReservationDetails()
            {
                ReserverName = "New Name",
                ReserverSurname = "New Surname"
            };
            repositoryServiceMock.Setup(r => r.UpdateAsync(existingReservation.Id, It.Is<Reservation>(r => r.Details == newReservationDetails))).Verifiable();

            // Act
            var result = await controller.UpdateReservation(existingReservation.Id, newReservationDetails);

            // Assert
            result.Should().BeOfType<OkResult>();
            repositoryServiceMock.Verify(r => r.UpdateAsync(reservationId, It.Is<Reservation>(r =>
                r.Id == existingReservation.Id &&
                r.Details.ReserverName == newReservationDetails.ReserverName &&
                r.Details.ReserverSurname == newReservationDetails.ReserverSurname)), Times.Once);
        }

        [TestMethod]
        public async Task UpdateReservation_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var reservationIdBad = 2;
            var reservationDetails = new ReservationDetails()
            {
                ReserverName = string.Empty,
                ReserverSurname = string.Empty
            };

            // Setup the repository to return null when queried for a non-existent reservation
            repositoryServiceMock.Setup(r => r.UpdateAsync(reservationIdBad, It.Is<Reservation>(r => r.Details == reservationDetails))).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await controller.UpdateReservation(reservationIdBad, reservationDetails);

            // Assert: Verify that the result is a NotFoundResult
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}