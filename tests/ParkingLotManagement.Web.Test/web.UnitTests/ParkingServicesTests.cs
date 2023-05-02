using AutoMapper;
using Moq;
using ParkingLotManagement.Application.DTOs;
using ParkingLotManagement.Application.Interfaces;
using ParkingLotManagement.Application.Mappings;
using ParkingLotManagement.Application.Services;
using ParkingLotManagement.Application.Validators;
using ParkingLotManagement.Core.Entities;
using ParkingLotManagement.Infrastructure.Repositories;

namespace ParkingLotManagement.Web.Test.web.UnitTests
{
    [TestClass]
    public class ParkingServicesTests
    {
        private Mock<IParkingRepository> _parkingRepositoryMock;
        private Mock<ICustomConfigureServices> _customConfigureServicesMock;
        private Mock<IMapper> _mapper;
        private ParkingServices _parkingService;

        [TestInitialize]
        public void Initialize()
        {
            _parkingRepositoryMock = new Mock<IParkingRepository>();
            _customConfigureServicesMock = new Mock<ICustomConfigureServices>();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = new Mock<IMapper>();
            _parkingService = new ParkingServices(_parkingRepositoryMock.Object, _mapper.Object, _customConfigureServicesMock.Object);
        }

        [TestMethod]
        public async Task Add_WhenTagNumberIsNull_ReturnsInvalidOperationResult()
        {
            // Arrange
            var parking = new InOutParkingDTO
            {
                TagNumber = null
            };

            // Act
            var result = await _parkingService.Add(parking);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Tag number is required", result.Message);
            _parkingRepositoryMock.Verify(x => x.GetLastParkingByTag(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task Add_WhenVehicleIsAlreadyParked_ReturnsInvalidOperationResult()
        {
            // Arrange
            var tagNumber = "ABC123";
            var parking = new InOutParkingDTO
            {
                TagNumber = tagNumber
            };
            var lastParking = new Parking
            {
                TagNumber = tagNumber,
                EntryTime = DateTime.Now
            };
            _parkingRepositoryMock.Setup(x => x.GetLastParkingByTag(tagNumber)).ReturnsAsync(lastParking);

            // Act
            var result = await _parkingService.Add(parking);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The vehicle is already in the parking", result.Message);
            _parkingRepositoryMock.Verify(x => x.GetLastParkingByTag(tagNumber), Times.Once);
        }

        [TestMethod]
        public async Task Add_WhenNoAvailableSpots_ReturnsInvalidOperationResult()
        {
            // Arrange
            var tagNumber = "ABC123";
            var parking = new InOutParkingDTO
            {
                TagNumber = tagNumber
            };
            _customConfigureServicesMock.Setup(x => x.CapacitySpots()).Returns(10);
            _parkingRepositoryMock.Setup(x => x.CountCurrentParkedAsync()).ReturnsAsync(10);

            // Act
            var result = await _parkingService.Add(parking);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("No available spots", result.Message);
            _customConfigureServicesMock.Verify(x => x.CapacitySpots(), Times.Once);
            _parkingRepositoryMock.Verify(x => x.CountCurrentParkedAsync(), Times.Once);
        }


        [TestMethod]
        public async Task Add_WhenValidInput_ReturnsValidOperationResult()
        {
            // Arrange
            var parkingRepositoryMock = new Mock<IParkingRepository>();
            var customConfigureServicesMock = new Mock<ICustomConfigureServices>();
            var mapperMock = new Mock<IMapper>();

            var parking = new InOutParkingDTO
            {
                TagNumber = "ABC123"
            };

            var result = new Parking
            {
                TagNumber = "ABC123",
                EntryTime = DateTime.Now
            };



            _parkingRepositoryMock.Setup(x => x.GetLastParkingByTag(It.IsAny<string>()))
                                 .ReturnsAsync((Parking)null);

            _parkingRepositoryMock.Setup(x => x.CountCurrentParkedAsync())
                                 .ReturnsAsync(1);

            _customConfigureServicesMock.Setup(x => x.CapacitySpots())
                                    .Returns(100);

            _parkingRepositoryMock.Setup(x => x.Add(It.IsAny<Parking>()))
                                 .ReturnsAsync(true);



            _mapper.Setup(x => x.Map<Parking>(It.IsAny<InOutParkingDTO>()))
                      .Returns(result);
           


            // Act
            var operationResult = await _parkingService.Add(parking);

            // Assert
            Assert.IsNotNull(operationResult);
            Assert.IsTrue(operationResult.IsValid);
            Assert.AreEqual("Successful", operationResult.Message);

            _customConfigureServicesMock.Verify(x => x.CapacitySpots(), Times.Once);
            _parkingRepositoryMock.Verify(x => x.GetLastParkingByTag(It.IsAny<string>()), Times.Once);
            _parkingRepositoryMock.Verify(x => x.CountCurrentParkedAsync(), Times.Once);
        }

    }
}