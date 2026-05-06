using irrigation_system.Controllers;
using irrigation_system.Data;
using irrigation_system.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;

namespace Tests
{
    public class DataControllerTests
    {
        private static AppDbContext CreateDbContext(SqliteConnection connection)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            return context;
        }

        [Fact]
        public async Task Post_WithValidData_ReturnsOk_AndSavesReading()
        {
            using var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            await using var context = CreateDbContext(connection);
            var controller = new DataController(context);

            var request = new TemperatureDto
            {
                Temperature = "6.5",
                ReadAtUnix = 1778085385,
                ReadAt = "19:36:25"
            };

            var result = await controller.Post(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var savedReading = await context.TemperatureReadings.FirstOrDefaultAsync();

            Assert.NotNull(savedReading);
            Assert.Equal(6.5, savedReading.Temperature);
            Assert.Equal(
                DateTimeOffset.FromUnixTimeSeconds(1778085385).LocalDateTime,
                savedReading.ReadAt
            );
        }

        [Fact]
        public async Task Post_WithInvalidTemperature_ReturnsBadRequest()
        {
            using var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            await using var context = CreateDbContext(connection);
            var controller = new DataController(context);

            var request = new TemperatureDto
            {
                Temperature = "abc",
                ReadAtUnix = 1778085385,
                ReadAt = "19:36:25"
            };

            var result = await controller.Post(request);

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Empty(context.TemperatureReadings);
        }

        [Fact]
        public async Task Post_WithoutTemperature_ReturnsBadRequest()
        {
            using var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            await using var context = CreateDbContext(connection);
            var controller = new DataController(context);

            var request = new TemperatureDto
            {
                ReadAtUnix = 1778085385,
                ReadAt = "19:36:25"
            };

            var result = await controller.Post(request);

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Empty(context.TemperatureReadings);
        }
    }
}