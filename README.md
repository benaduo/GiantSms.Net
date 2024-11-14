

[![Build Status](https://github.com/benaduo/GiantSms.Net/actions/workflows/dotnet.yml/badge.svg)](https://github.com/benaduo/GiantSms.Net/actions)
[![NuGet](https://img.shields.io/nuget/v/GiantSms.Net.svg)](https://www.nuget.org/packages/GiantSms.Net)
[![Target Framework](https://img.shields.io/badge/Target%20Framework-.NET%208.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![Downloads](https://img.shields.io/nuget/dt/GiantSms.Net.svg)](https://www.nuget.org/packages/GiantSms.Net)

## Table of Contents
- [Getting Started](#getting-started)
- [Prerequisites](#prerequisites)
- [Installing](#installing)
- [Usage](#usage)
  - [Register the Service](#register-the-service)
  - [Inject the Service](#inject-the-service)
  - [Send SMS Messages](#send-sms-messages)
- [Built With](#built-with)
- [Contributing](#contributing)
- [License](#license)


GiantSms.Net is a .NET library that provides a simple and easy-to-use interface for sending SMS messages using the Giant SMS API.


## Getting Started
To use the library, you will need to obtain an API key from Giant SMS. 

You can register for an API key by visiting [Giant SMS Registration](https://app.giantsms.com/register).

## Prerequisites
- .NET 8.0 or later
- A valid API key from Giant SMS

## Installing
You can install GiantSms.Net via NuGet package manager.

### Dotnet CLI:
```bash
dotnet add package GiantSms.Net --version 2.0.0
```

### NuGet Package Manager:
```bash
Install-Package GiantSms.Net -Version 2.0.0
```

## Usage
### Register the Service
To use the `GiantSmsService` class, you need to register it as a service in your application's `IServiceCollection`. You can do this by calling the `AddGiantSms` extension method on `IServiceCollection`:

```csharp

builder.Services.AddGiantSms(configuration)

```

### Inject the Service
In your controller, add the following field:

```csharp
private readonly IGiantSmsService _giantSmsService;
```

In the controller's constructor, inject `IServiceProvider`, and in the constructor body, set `_giantSmsService = serviceProvider.GetService<IGiantSmsService>();`.

```csharp
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
To send an SMS message, call the `SendSingleMessage` method on the `IGiantSmsService` interface:

```csharp
var phoneNumber = "1234567890";
var message = "Hello, World!";
var result = await _giantSmsService.SendSingleMessage(phoneNumber, message);
```

## Built With
- .NET 8.0


## Contributing
Contributions are welcome! Please follow these steps to contribute:

1. Fork the repository.
2. Create a new branch (`git checkout -b feature-branch`).
3. Make your changes.
4. Commit your changes (`git commit -m 'Add some feature'`).
5. Push to the branch (`git push origin feature-branch`).
6. Open a pull request.

Please make sure to update tests as appropriate.

For more details, see the [CONTRIBUTING.md](CONTRIBUTING.md) file.


## License
This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

