# Ensek Technical Test
## Overview
This project is an implementation of the Ensek Remote Technical Excercise. It is a Web API which exposes an endpoint that accepts a Meter Reading CSV (as outlined in the brief), and processes it.

## Prerequisites
This project was written in .NET 6, so in order to run it you will need .NET 6 installed on your system. Head [here](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) for the required SDKs/Runtime.

I suggest for testing the API, you use cURL to upload the file. However, if you have a prefered tool you can use that instead.

SQL Server was used in this example (running in a Docker container), but you can change this if you want. You need to ensure though that the database has a user called testapplication with the password present in the file (which is a test password and should be changed if this is used in a larger context), and all the correct permissions to manage databases.

## Getting Started
To run the project, simply change into the MeterReadingWebApi folder, and run the command **dotnet run**.

Once it's running, you should see a console window appear. On this window, a https and http address will appear. Choose the http address for your interactions here. 

In order to upload a file, send a POST request to the endpoint /meter-reading-uploads. This will parse the file, remove records which don't match the rules defined in the brief and then save the results.

An example cURL command you can use to do this is:

curl -Lv -X POST -F 'MeterReadings=@./Meter_Reading.csv' http://localhost:5169/meter-reading-upload

As seen in this example, your file should be assigned to a property called MeterReadings.

To run tests, simply change into the MeterReadingTests directory and run **dotnet test**.

To apply migrations to the database, and seed the appropriate data, change into the MeterReadingDatabase directory and run the command **dotnet ef database update --startup-project ../MeterReadingWebApi**.




