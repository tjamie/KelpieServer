# KelpieServer

## About

KelpieServer utilizes ASP.NET and Entity Framework to create a server-side application which handles and stores data used in the delineation of Waters of the United States. This application is intended for use with *KelpieLogger* ([see github page](https://github.com/tjamie/KelpieLogger/)). There is no official server for *KelpieLogger*, but users can set up their own instances of *KelpieServer* if desired.

## Setup Steps
1. Install Docker and create a PostgresSQL container.
    - Other databases have not been tested.
1. Clone this repository.
1. Create *appsettings.json* in the *KelpieServer* directory.
    - This should be the nested *KelpieServer* directory that contains controllers and models. Not the one that contains *KelpieServer.Tests*.
1. Place the following in *appsettings.json*:
        
        {
        "ConnectionStrings": {
            "DbContext": "Server=YOUR-SERVER-ADDRESS;Database=YOUR-DATABASE-NAME;Port=YOUR-PORT;User Id=YOUR-USER-ID;Password=YOUR-PASSWORD;"
        },
        "Logging": {
            "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
            }
        },
        "Jwt": {
            "Key": "YOUR-JWT-KEY",
            "Issuer": "ISSUER-ADDRESS",
            "Audience": "AUDIENCE-ADDRESS"
        },
        "AllowedHosts": "*"
        }
   - You will need to replace the CAPITALIZED values above with your own information.     
1. Create a migration and update the database accordingly. This can be done with the following commands in Visual Studio's Package Manager Console:

        add-migration MyFirstMigration
        update-database
1. Build and run the application.
