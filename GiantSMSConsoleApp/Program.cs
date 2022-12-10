// See https://aka.ms/new-console-template for more information
using GiantSMS.Net;
using GiantSMS.Net.Model;
using Microsoft.Extensions.Configuration;
//Console.WriteLine("Hello, World!");

IConfigurationBuilder builder = new ConfigurationBuilder();

builder.AddJsonFile("appsettings.json");

IConfiguration config = builder.Build();

GiantSMSClient client = new GiantSMSClient("VWUXQgmq", "GFDtwrKnag", "VldVWFFnbXE6R0ZEdHdyS25hZw==", "GPCL");

//var response = await client.CheckBalance();

//Console.WriteLine("status: {0}, message:{1}",response.Status, response.Message);

//var response = await client.SendSingleMessage("0545140251", "From .net client");
//var response = await client.GetSenderIds();

//BulkMessageRequest request = new();

//request.Recipients = new string[] { "0205468168", "0545140251" };
//request.Msg = "Bulk with token";

//var response = await client.SendBulkMessagesWithToken(request);
//var response = await client.CheckMessageStatus("4FB63509-30C8-4A69-9C85-A084FA50E54C");
//RegisterSenderIdRequest r = new RegisterSenderIdRequest() { Name = "GPCL", Purpose = "Testing" };
//var response = await client.RegisterSenderIds(r);
//Console.WriteLine("status: {0}, message:{1}, message_id:{2}", response.Status, response.Message,response?.Data?.Message_id);

var response = await client.GetSenderIds();

Console.WriteLine("status: {0}, message:{1}", response.Status, response.Message);
foreach (var item in response.Data)
{
    Console.WriteLine(item.Name);
    Console.WriteLine(item.Purpose);
}
