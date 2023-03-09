# GiantSms.Net
GiantSms.Net is a .NET library that provides a simple and easy-to-use interface for sending SMS messages using the Giant SMS API.

## Getting Started
To use the library, you will need to obtain an API key from Giant SMS. 

You can register for an API key by visiting https://app.giantsms.com/register.

Check out API documentation for various requests and reponses structure by visiting https://documenter.getpostman.com/view/16317044/TzeZF6uf. 

## Prerequisites
.NET 5.0 or later

A valid API key from Giant SMS

## Installing
You can install GiantSms.Net via NuGet package manager.

```
dotnet add package GiantSms.Net --version 1.0.0
```

## Usage
### Register the Service

To use the GiantSmsService class, you need to register it as a service in your application's IServiceCollection. You can do this by calling the AddGiantSms extension method on IServiceCollection:
```
    //...
    builder.Services.AddGiantSms();
    //...
```
### Inject the Service
In your controller, add the following field:

```private readonly IGiantSmsService _giantSmsService;```

In the controller's constructor, inject ```IServiceProvider```, and in the constructor body, set ```_giantSmsService = serviceProvider.GetService<IGiantSmsService>();```.

```
public class MyController : ControllerBase
{
    private readonly IGiantSmsService _giantSmsService;

    public MyController(IServiceProvider serviceProvider)
    {
        _giantSmsService = serviceProvider.GetService<IGiantSmsService>();
    }

    // ...
}
```

### Send SMS Messages
To send an SMS message, call the SendSingleMessage method on the IGiantSmsService interface:
```
var phoneNumber = "1234567890";
var message = "Hello, World!";
var result = await _giantSmsService.SendSingleMessage(phoneNumber, message);
```
## Built With
.NET 7.0
RestSharp
## License
This project is licensed under the MIT License - see the LICENSE file for details.
