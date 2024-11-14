using System.Net;
using System.Text;
using FluentAssertions;
using GiantSms.Net;
using GiantSms.Net.Model.Requests;
using GiantSms.Net.Model.Responses;
using GiantSms.Tests;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NSubstitute;

public class GiantSmsServiceTests
{
    private readonly IOptions<GiantSmsConnection> _connection;
    private readonly GiantSmsService _giantSmsService;
    private readonly IHttpClientFactory _httpClientFactoryMock;
    private readonly MockHttpMessageHandler _mockHandler;

    public GiantSmsServiceTests()
    {
        // Create mocks
        _httpClientFactoryMock = Substitute.For<IHttpClientFactory>();
        _connection = Substitute.For<IOptions<GiantSmsConnection>>();
        _mockHandler = new MockHttpMessageHandler();

        // Mock HTTP client
        var httpClient = new HttpClient(_mockHandler);
        _httpClientFactoryMock.CreateClient().Returns(httpClient);

        // Mock GiantSmsConnection options
        var connection = new GiantSmsConnection
        {
            Username = "testuser",
            Password = "testpassword",
            Token = "testtoken",
            SenderId = "testsender"
        };
        _connection.Value.Returns(connection);

        // Initialize the service
        _giantSmsService = new GiantSmsService(_httpClientFactoryMock, _connection);
    }

    #region Configuration

    [Fact]
    public void GiantSmsService_Should_Set_IsReady_When_Valid_Configuration()
    {
        // Arrange & Act (already done in the constructor)

        // Assert
        Assert.True(_giantSmsService.IsReady);
    }

    [Fact]
    public void GiantSmsService_Should_Throw_Exception_When_Options_Are_Null()
    {
        // Arrange
        var connection = Substitute.For<IOptions<GiantSmsConnection>>();
        connection.Value.Returns((GiantSmsConnection)null!);

        // Act & Assert
        Assert.Throws<NullReferenceException>(() =>
            new GiantSmsService(_httpClientFactoryMock, connection));
    }

    #endregion

    #region CheckMessageStatus

    [Fact]
    public async Task CheckMessageStatusShouldReturnSingleSmsResponseWhenMessageIdIsValid()
    {
        // Arrange
        var messageId = "12345";
        var responseContent = JsonConvert.SerializeObject(new SingleSmsResponse { Status = true });

        _mockHandler.SetupResponse(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
        });

        // Act
        var result = await _giantSmsService.CheckMessageStatus(messageId);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(true);
    }

    [Fact]
    public async Task CheckMessageStatusShouldThrowHttpRequestExceptionWhenHostIsUnknown()
    {
        // Arrange
        var messageId = "12345";
        _mockHandler.SetupException(new HttpRequestException("No such host is known."));

        // Act
        Func<Task> act = async () => await _giantSmsService.CheckMessageStatus(messageId);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>().WithMessage("No such host is known.");
    }

    #endregion

    #region GetBalance
    [Fact]
    public async Task GetBalanceShouldReturnBaseResponseWhenRequestIsSuccessful()
    {
        // Arrange
        var responseContent = JsonConvert.SerializeObject(new BaseResponse
        {
            Status = true,
            Message = "Balance retrieved successfully"
        });

        _mockHandler.SetupResponse(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
        });

        // Act
        var result = await _giantSmsService.GetBalance();

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().BeTrue();
        result.Message.Should().Be("Balance retrieved successfully");
    }

    [Fact]
    public async Task GetBalanceShouldThrowHttpRequestExceptionWhenRequestFails()
    {
        // Arrange
        _mockHandler.SetupException(new HttpRequestException("Request failed"));

        // Act
        Func<Task> act = async () => await _giantSmsService.GetBalance();

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>().WithMessage("Request failed");
    }

    #endregion

    #region GetSenderIds
    [Fact]
    public async Task GetSenderIdsShouldReturnSenderIdResponseWhenRequestIsSuccessful()
    {
        // Arrange
        var responseContent = JsonConvert.SerializeObject(new SenderIdResponse
        {
            Status = true,
            Data = new List<SenderIdData>
            {
                new()
                {
                    Name = "Sender1",
                    Purpose = "Testing",
                    Approved = true,
                    Approval_status = "Approved"
                },
                new()
                {
                    Name = "Sender2",
                    Purpose = "Marketing",
                    Approved = false,
                    Approval_status = "Pending"
                }
            }
        });

        _mockHandler.SetupResponse(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
        });

        // Act
        var result = await _giantSmsService.GetSenderIds();

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().BeTrue();
    }

    [Fact]
    public async Task GetSenderIdsShouldThrowHttpRequestExceptionWhenRequestFails()
    {
        // Arrange
        _mockHandler.SetupException(new HttpRequestException("Request failed"));

        // Act
        Func<Task> act = async () => await _giantSmsService.GetSenderIds();

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>().WithMessage("Request failed");
    }

    #endregion

    #region RegisterSenderId
    [Fact]
    public async Task RegisterSenderIdsShouldThrowHttpRequestExceptionWhenRequestFails()
    {
        // Arrange
        var senderIdRequest = new RegisterSenderIdRequest
        {
            Name = "TestSender",
            Purpose = "Marketing"
        };

        _mockHandler.SetupException(new HttpRequestException("Request failed"));

        // Act
        Func<Task> act = async () => await _giantSmsService.RegisterSenderId(senderIdRequest);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>().WithMessage("Request failed");
    }

    #endregion

    #region SendBulkMessages

    [Fact]
    public async Task SendBulkMessagesShouldReturnBaseResponseWhenRequestIsSuccessful()
    {
        // Arrange
        var bulkMessageRequest = new BulkMessageRequest
        {
            Recipients = ["1234567890", "0987654321"],
            Msg = "Test message"
        };

        var responseContent = JsonConvert.SerializeObject(new BaseResponse
        {
            Status = true,
            Message = "Messages sent successfully"
        });

        _mockHandler.SetupResponse(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
        });

        // Act
        var result = await _giantSmsService.SendBulkMessages(bulkMessageRequest);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().BeTrue();
        result.Message.Should().Be("Messages sent successfully");
    }

    [Fact]
    public async Task SendBulkMessagesShouldThrowHttpRequestExceptionWhenRequestFails()
    {
        // Arrange
        var bulkMessageRequest = new BulkMessageRequest
        {
            Recipients = ["1234567890", "0987654321"],
            Msg = "Test message"
        };

        _mockHandler.SetupException(new HttpRequestException("Request failed"));

        // Act
        Func<Task> act = async () => await _giantSmsService.SendBulkMessages(bulkMessageRequest);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>().WithMessage("Request failed");
    }

    #endregion

    #region SendSingleMessage
    [Fact]
    public async Task SendMessageWithTokenShouldReturnSingleSmsResponseWhenRequestIsSuccessful()
    {
        // Arrange
        var messageRequest = new SingleMessageRequest
        {
            To = "1234567890",
            Msg = "Hello, this is a test message"
        };

        var responseContent = JsonConvert.SerializeObject(new SingleSmsResponse
        {
            Status = true,
            Message = "Message sent successfully",
            Data = new MessageStatus { Message_id = "12345" }
        });

        _mockHandler.SetupResponse(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
        });

        // Act
        var result = await _giantSmsService.SendMessageWithToken(messageRequest);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().BeTrue();
        result.Message.Should().Be("Message sent successfully");
        result.Data.Message_id.Should().Be("12345");
    }

    [Fact]
    public async Task SendMessageWithTokenShouldThrowHttpRequestExceptionWhenRequestFails()
    {
        // Arrange
        var messageRequest = new SingleMessageRequest
        {
            To = "1234567890",
            Msg = "Hello, this is a test message"
        };

        _mockHandler.SetupException(new HttpRequestException("Request failed"));

        // Act
        Func<Task> act = async () => await _giantSmsService.SendMessageWithToken(messageRequest);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>().WithMessage("Request failed");
    }

    #endregion

    #region SendSingleMessage
    [Fact]
    public async Task SendSingleMessageShouldReturnSingleSmsResponseWhenRequestIsSuccessful()
    {
        // Arrange
        var to = "1234567890";
        var message = "Hello, this is a test message";

        var responseContent = JsonConvert.SerializeObject(new SingleSmsResponse
        {
            Status = true,
            Message = "Message sent successfully",
            Data = new MessageStatus { Message_id = "12345" }
        });

        _mockHandler.SetupResponse(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
        });

        // Act
        var result = await _giantSmsService.SendSingleMessage(to, message);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().BeTrue();
        result.Message.Should().Be("Message sent successfully");
        result.Data.Message_id.Should().Be("12345");
    }

    [Fact]
    public async Task SendSingleMessageShouldThrowHttpRequestExceptionWhenRequestFails()
    {
        // Arrange
        var to = "1234567890";
        var message = "Hello, this is a test message";

        _mockHandler.SetupException(new HttpRequestException("Request failed"));

        // Act
        Func<Task> act = async () => await _giantSmsService.SendSingleMessage(to, message);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>().WithMessage("Request failed");
    }

    #endregion
}