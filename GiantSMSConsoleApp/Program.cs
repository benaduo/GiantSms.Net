// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var connection = configuration.GetSection("GiantSmsConnection");

