using AuthService.API.Utils.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Text.Json;

namespace AuthServices.Tests;

public class GloblalExceptionHandlingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_WhenExceptionThrown_ReturnsInternalServerErrorResponse()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<GloblalExceptionHandlingMiddleware>>();
        var middleware = new GloblalExceptionHandlingMiddleware(loggerMock.Object);

        var context = new DefaultHttpContext();
        var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        // Simula un RequestDelegate que lanza excepción
        RequestDelegate next = (HttpContext ctx) => throw new Exception("Test exception");

        // Act
        await middleware.InvokeAsync(context, next);

        // Assert
        Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
        Assert.Equal("application/json", context.Response.ContentType);

        responseBodyStream.Seek(0, SeekOrigin.Begin);
        string responseBody = new StreamReader(responseBodyStream).ReadToEnd();

        var problem = JsonSerializer.Deserialize<ProblemDetails>(responseBody, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(problem);
        Assert.Equal("Server Error", problem.Type);
        Assert.Equal("Server Error", problem.Title);
        Assert.Equal("An internal server has ocurred.", problem.Detail);
        Assert.Equal((int)HttpStatusCode.InternalServerError, problem.Status);

        // Verifica que LogError fue llamado
        loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true), // Cualquier mensaje
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
}