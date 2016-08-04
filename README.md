# Outbox Sender Core

Application is designed to process outbox messages and send them in scheduled way.
REST service is provided to add outbox messages, which will be processed in a queue.

### Application contains two major parts:
- REST api - to bring messages which have to be send;
- Scheduled Sender - service which is responsible to process outbox messages;

## Framework:
- netcoreapp1.0

## Dependencies:
- Microsoft.NETCore.App
- Microsoft.AspNetCore.Mvc
- Microsoft.EntityFrameworkCore.Sqlite
- Serilog.Extensions.Logging
- Swashbuckle (Swagger UI)
- Chroniton.NetCore (scheduling tasks)

## Get Started:
There are several simple steps which let you get this project running on your development instance.
0. Install [.net core] SDK. 
1. Download sources with zip or sourcecode from a github;
2a. Open Solution in VisualStudio, run F5.
2b. Open .\src\Iquality.Shared.OutboxSender.Core\ folder and run command
```sh
dotnet restore
dotnet run
```
3. Application is up and running.

## How to use it:
- Start url navigate to the list of outbox messages: http://localhost:5000/api/outboxmessages
- Open Swagger UI to make service requests:  http://localhost:5000/swagger/ui
- Prepare outbox message JSON to submit to the outbox sender
example:
```json
{
  "fromAddress": "string",
  "toAddress": "string",
  "ccAddress": "string",
  "bccAddress": "string",
  "subject": "string",
  "body": "string"
}
````
- Make PUT request to provide new message to be sent: http://localhost:5000/swagger/ui/index.html#!/OutboxMessages/ApiOutboxMessagesPut
- Open list of message and check that message is submitted:
http://localhost:5000/api/outboxmessages
- Email should be sent with provided data with a minute.

### Setting up Scheduling:
Add Startup call of OutboxProcessor from the Configure method in Startup.cs:
```c#
OutboxProcessor.Startup(new OutboxProcessorSettings
            {
                ItemsPerShot = 10,
                StartTime = DateTime.UtcNow.AddSeconds(10),
                FinishTime = DateTime.UtcNow.AddMinutes(2),
                Frequency = TimeSpan.FromMinutes(1)
            });
````
here you can define settings.
TODO: Move it to eh configs. Expected in next versions.

### Setting up SMTP:
Now settings are in sourcecode:
SmtpEmailSender.cs
````C#
client.Connect("iprint", 25, SecureSocketOptions.None);
````
Adjust it to required smtp server.

## To pay attention
- SqLite is used as a db storage for outbox message. default path/name is: `c:\temp\OuboxMailer\OutboxSQLiteData.db`
- Default Logging location: `c:\Logs\Api`, errors and info are located in different folders.

## Deployment:
IIS deployment (learn more: https://docs.asp.net/en/latest/publishing/iis.html)
- .net core Windows Server Hosting (https://go.microsoft.com/fwlink/?LinkId=817246)
- define folder where service will be located (f.e. c:\inetpub\OutboxSender)
- Add website in IIS, assign chosen folder, hostname (f.e. localhost), port (f.e. 5000)
- Change .NET CLR Version of AppPool to "No Managed Code"
- run dotnet publish command to publish sources
```sh
dotnet publish --framework netcoreapp1.0 --output c:\inetpub\OutboxSender\ --configuration Debug
````
- Check service: (f.e. localhost:5000/api/outboxmessages)

## TODOs
- Add UnitTest project with different usages examples (framework 4.0, 4.5, .net core, etc...);
- Add support of Attachments;
- Extract SMTP settings to the config files;
- Extract Scheduler to the config files;
- Add support templating engine;
- Balanced email sending process; (for disbalanced and huge amount of email to be send)
- to be added.