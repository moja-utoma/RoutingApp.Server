using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RoutingApp.API.Controllers;
using RoutingApp.API.Enumerations;
using RoutingApp.API.Models.DTO;
using RoutingApp.API.Services.Interfaces;

namespace RoutingApp.Test;

public class PointsControllerTests
{
    private readonly Mock<IPointService> _pointServiceMock;
    private readonly PointsController _controller;
    public PointsControllerTests()
    {
        _pointServiceMock = new Mock<IPointService>();
        _controller = new PointsController(_pointServiceMock.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnPointsAsync()
    {
        // Arrange
        var points = new List<PointResponse>
            {
                new PointResponse { Id = 1, Name = "Point A", Address = "Addr 1", Latitude = 10, Longitude = 20, Type = PointType.DeliveryPoint, Weight = 12.5m },
                new PointResponse { Id = 2, Name = "Point B", Address = "Addr 2", Latitude = 30, Longitude = 40, Type = PointType.Warehouse, Weight = null }
            };

        _pointServiceMock
            .Setup(service => service.GetAllPointsAsync(SortOrder.AlphabetAscending, "", 1, 10, null))
            .ReturnsAsync(points);

        // Act
        var result = await _controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<PointResponse>>(okResult.Value);
        Assert.Equal(2, returnValue.Count());
        Assert.Equal("Point A", returnValue.First().Name);
    }

    [Fact]
    public async Task GetByID_ShouldReturnPoint()
    {
        // Arrange
        var point = new PointResponse { Id = 1, Name = "Test", Address = "Addr", Longitude = 10, Latitude = 20, Type = PointType.Warehouse };
        _pointServiceMock
            .Setup(s => s.GetPointByIDAsync(1))
            .ReturnsAsync(point);

        // Act
        var result = await _controller.GetByID(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<PointResponse>(okResult.Value);
        Assert.Equal(1, returnValue.Id);
    }

    [Fact]
    public async Task CreateFromCSV_InvalidFile_ShouldReturnBadRequest()
    {
        // Arrange
        IFormFile? file = new FormFile(
            baseStream: new MemoryStream(Encoding.UTF8.GetBytes("")),
            baseStreamOffset: 0,
            length: 0,
            name: "file",
            fileName: "test.csv"
        );
        _pointServiceMock
            .Setup(s => s.ImportCSV(It.IsAny<IFormFile>()))
            .ThrowsAsync(new Exception("File is empty"));

        // Act
        var result = await _controller.CreateFromCSV(file);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("File is empty", badRequestResult.Value);
    }
}
