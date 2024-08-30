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
            repositoryServiceMock.Setup(r => r.GetAsync(ReservationHelper.ReservationId)).ReturnsAsync(ReservationHelper.Reservation);

            // Act
            var result = await controller.GetReservation(ReservationHelper.ReservationId);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result.As<OkObjectResult>();
            okResult.Value.Should().BeOfType<Reservation>();
            okResult.Value.As<Reservation>().Id.Should().Be(ReservationHelper.ReservationId);
        }

        [TestMethod]
        public async Task GetReservation_WithNonExistingId_ShouldReturnNotFound()
        {
            // Arrange
            repositoryServiceMock.Setup(r => r.GetAsync(ReservationHelper.ReservationId)).ReturnsAsync((Reservation?)null);

            // Act
            var result = await controller.GetReservation(ReservationHelper.ReservationIdBad);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task AddReservation_WithValidDetails_ShouldReturnCreated()
        {
            // Arrange

            // Act
            var result = await controller.AddReservation(ReservationHelper.ReservationDetails);

            // Assert
            result.Result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result.Result.As<CreatedAtActionResult>();
            createdResult.Value.Should().BeOfType<Reservation>();
            var returnedReservation = createdResult.Value.As<Reservation>();
            returnedReservation.Id.Should().Be(1);
        }

        [TestMethod]
        public async Task AddReservation_WithAlreadyReservedRoomId_ShouldReturnBadRequest()
        {
            // Arrange
            repositoryServiceMock.Setup(r => r.AddAsync(It.Is<Reservation>(r => r.Details == ReservationHelper.ReservationDetails))).ThrowsAsync(new InvalidOperationException());

            // Act
            var result = await controller.AddReservation(ReservationHelper.ReservationDetails);

            // Assert
            result.Result.Should().BeOfType<BadRequestResult>();
        }

        [TestMethod]
        public async Task UpdateReservation_WithValidId_ShouldReturnOk()
        {
            // Arrange
            repositoryServiceMock.Setup(r => r.UpdateAsync(ReservationHelper.ReservationId, It.Is<Reservation>(r => r.Details == ReservationHelper.NewReservationDetails))).Verifiable();

            // Act
            var result = await controller.UpdateReservation(ReservationHelper.ReservationId, ReservationHelper.NewReservationDetails);

            // Assert
            result.Should().BeOfType<OkResult>();
            repositoryServiceMock.Verify(r => r.UpdateAsync(ReservationHelper.ReservationId, It.Is<Reservation>(r =>
                r.Id == ReservationHelper.ReservationId &&
                r.Details.ReserverName == ReservationHelper.NewReservationDetails.ReserverName &&
                r.Details.ReserverSurname == ReservationHelper.NewReservationDetails.ReserverSurname)), Times.Once);
        }

        [TestMethod]
        public async Task UpdateReservation_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            repositoryServiceMock.Setup(r => r.UpdateAsync(ReservationHelper.ReservationIdBad, It.Is<Reservation>(r => r.Details == ReservationHelper.ReservationDetails))).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await controller.UpdateReservation(ReservationHelper.ReservationIdBad, ReservationHelper.ReservationDetails);

            // Assert: Verify that the result is a NotFoundResult
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}