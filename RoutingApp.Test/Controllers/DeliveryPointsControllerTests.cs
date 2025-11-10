using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RoutingApp.API.Controllers;
using RoutingApp.API.Models;
using RoutingApp.API.Models.DTO;
using RoutingApp.API.Models.Responses.DeliveryPoints;
using RoutingApp.API.Services.Interfaces;
using RoutingApp.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutingApp.Test.Controllers
{
	public class DeliveryPointsControllerTests
	{
		private readonly Mock<IDeliveryPointService> _mockService;
		private readonly Mock<ILogger<DeliveryPointsController>> _mockLogger;
		private readonly DeliveryPointsController _controller;

		public DeliveryPointsControllerTests()
		{
			_mockService = new Mock<IDeliveryPointService>();
			_mockLogger = new Mock<ILogger<DeliveryPointsController>>();
			_controller = new DeliveryPointsController(_mockService.Object, _mockLogger.Object);
		}

		[Fact]
		public async Task GetAll_ReturnsOkResult_WithData()
		{
			var filters = new QueryParametersModel();
			var expected = new API.Models.Responses.PaginatedResponseDTO<DeliveryPointResponseDTO>
			{
				Items = new List<DeliveryPointResponseDTO>(),
				TotalCount = 0
			};

			_mockService.Setup(s => s.GetAllPointsAsync(filters)).ReturnsAsync(expected);

			var result = await _controller.GetAll(filters);

			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(expected, okResult.Value);
		}

		[Fact]
		public async Task GetAll_LogsErrorAndReturnsBadRequest_OnException()
		{
			var filters = new QueryParametersModel();
			_mockService.Setup(s => s.GetAllPointsAsync(filters)).ThrowsAsync(new Exception("fail"));

			var result = await _controller.GetAll(filters);

			var badRequest = Assert.IsType<BadRequestObjectResult>(result);
			Assert.Equal("fail", badRequest.Value);
		}

		[Fact]
		public async Task GetByID_ReturnsOkResult()
		{
			var expected = new DeliveryPointDetailsResponseDTO
			{
				Id = 1,
				Name = "Main Depot",
				Address = "123 Logistics Ln",
				Longitude = 30.123m,
				Latitude = 50.456m,
				Weight = 12.5m
			};

			_mockService.Setup(s => s.GetPointByIDAsync(1)).ReturnsAsync(expected);

			var result = await _controller.GetByID(1);

			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(expected, okResult.Value);
		}

		[Fact]
		public async Task Create_ReturnsOkResult()
		{
			var dto = new CreateDeliveryPointRequestDTO
			{
				Name = "Depot A",
				Address = "123 Logistics Blvd",
				Longitude = 30.123m,
				Latitude = 50.456m,
				Weight = 15.0m
			};
			var expected = new DeliveryPointResponseDTO
			{
				Name = "Depot A",
				Address = "123 Logistics Blvd",
				Longitude = 30.123m,
				Latitude = 50.456m,
				Weight = 15.0m
			};

			_mockService.Setup(s => s.CreatePointAsync(dto)).ReturnsAsync(expected);

			var result = await _controller.Create(dto);

			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(expected, okResult.Value);
		}

		[Fact]
		public async Task ImportCSV_ReturnsOkResult()
		{
			var fileMock = new Mock<IFormFile>();
			var expected = new FileUploadResult
			{
				BlobName = "deliverypoints.csv",
				Size = 2048,
				ContentType = "text/csv",
				Version = "v1"
			};

			_mockService.Setup(s => s.SaveRawFileAsync(fileMock.Object)).ReturnsAsync(expected);

			var result = await _controller.ImportCSV(fileMock.Object);

			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(expected, okResult.Value);
		}

		[Fact]
		public async Task DownloadFile_ReturnsFileResult()
		{
			var stream = new MemoryStream();
			var contentType = "text/csv";
			var fileName = "data.csv";

			_mockService.Setup(s => s.GetRawFileAsync("blob")).ReturnsAsync((stream, contentType, fileName));

			var result = await _controller.DownloadFile("blob");

			var fileResult = Assert.IsType<FileStreamResult>(result);
			Assert.Equal(contentType, fileResult.ContentType);
		}

		[Fact]
		public async Task Delete_ReturnsOkResult()
		{
			_mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

			var result = await _controller.Delete(1);

			Assert.IsType<OkResult>(result);
		}

		[Fact]
		public async Task Edit_ReturnsOkResult()
		{
			var request = new EditDeliveryPointRequestDTO
			{
				Name = "Depot A",
				Address = "123 Logistics Blvd",
				Longitude = 30.123m,
				Latitude = 50.456m,
				Weight = 15.0m
			};
			var expected = new DeliveryPointResponseDTO
			{
				Name = "Depot A",
				Address = "123 Logistics Blvd",
				Longitude = 30.123m,
				Latitude = 50.456m,
				Weight = 15.0m
			};


			_mockService.Setup(s => s.EditAsync(request)).ReturnsAsync(expected);

			var result = await _controller.Edit(request);

			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(expected, okResult.Value);
		}
	}
}
