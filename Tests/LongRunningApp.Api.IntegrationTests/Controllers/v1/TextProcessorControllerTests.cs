﻿using LongRunningApp.Api.Controllers.v1;
using LongRunningApp.Api.Hubs.v1;
using LongRunningApp.Api.Models.v1;
using LongRunningApp.Api.Properties;
using LongRunningApp.Api.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace LongRunningApp.Api.IntegrationTests.Controllers.v1;

[TestClass]
public sealed class TextProcessorControllerTests
{
    private static string ProcessTextRequestUrl = @$"/api/v1/{ApiEndpointNames.Controllers.TextProcessor}/{ApiEndpointNames.ControllersActions.ProcessText}";
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;
    private Mock<ITextProcessorService> _textProcessorServiceMock = new Mock<ITextProcessorService>();

    [TestInitialize]
    public void TestInitialize()
    {
        _textProcessorServiceMock = new Mock<ITextProcessorService>();
        _factory = new WebApplicationFactory<Program>()
        .WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_textProcessorServiceMock.Object);
            });
        });
        _client = _factory.CreateClient();
    }

    [TestCleanup]
    public void TestCleanup()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [TestMethod]
    public async Task ProcessText_RequestWithConnectionId_ReturnBadRequest()
    {
        //Arrange
        var request = new ProcessTextRequest() { ConnectionId = "", Text = "TestText" };
        var requestBody = ProcessTextCreateRequestBody(request);

        //Act
        var response = await _client.PostAsync(ProcessTextRequestUrl, requestBody);

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        var responseBody = await ProcessTextGetResponseBody(response);
        Assert.IsNotNull(responseBody);
        Assert.AreEqual(Resource.ConectionIdForProcessingIsEmpty, responseBody.ErrorMessage);
    }

    [TestMethod]
    public async Task ProcessText_RequestWithEmptyText_ReturnBadRequest()
    {
        //Arrange
        var request = new ProcessTextRequest() { ConnectionId = "ConnectionId", Text = "" };
        var requestBody = ProcessTextCreateRequestBody(request);

        //Act
        var response = await _client.PostAsync(ProcessTextRequestUrl, requestBody);

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        var responseBody = await ProcessTextGetResponseBody(response);
        Assert.IsNotNull(responseBody);
        Assert.AreEqual(Resource.TextForProcessingIsEmpty, responseBody.ErrorMessage);
    }

    [TestMethod]
    public async Task ProcessText_ProcessingTextException_ReturnInternalServerError()
    {
        //Arrange
        var request = new ProcessTextRequest() { ConnectionId = "ConnectionId", Text = "TestText" };
        var requestBody = ProcessTextCreateRequestBody(request);

        _textProcessorServiceMock.Setup(x => x.PerformProcessing(It.IsAny<string>(), "TestText", It.IsAny<CancellationToken>()))
            .Throws(new Exception());

        //Act
        var response = await _client.PostAsync(ProcessTextRequestUrl, requestBody);

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);
        var responseBody = await ProcessTextGetResponseBody(response);
        Assert.IsNotNull(responseBody);
        Assert.AreEqual(Resource.ProcessingTextError, responseBody.ErrorMessage);
    }

    [TestMethod]
    public async Task ProcessText_WithDefaults_ReturnOk()
    {
        //Arrange
        var request = new ProcessTextRequest() { ConnectionId = "ConnectionId", Text = "TestText" };
        var requestBody = ProcessTextCreateRequestBody(request);

        //Act
        var response = await _client.PostAsync(ProcessTextRequestUrl, requestBody);

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

    }

    private static StringContent ProcessTextCreateRequestBody(ProcessTextRequest request)
        => new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

    private static async Task<ProcessTextResponse?> ProcessTextGetResponseBody(HttpResponseMessage httpResponse)
        => await httpResponse.Content.ReadFromJsonAsync<ProcessTextResponse>();
}