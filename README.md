# ParkingLotManagement

This is A web app that manages a parking lot and the flow of cars coming in/out.
In this application uses razor pages and .Net 6 and In this solution, the principles of Clean architecture where bussiness logic and the application domain are at the center of te solution.


<img width="424" alt="image" src="https://user-images.githubusercontent.com/5255854/235308554-e436c067-46be-4d03-bd85-90f38276e367.png">



## Technologies
* ASP.NET 6
* AutoMapper
* ADO.NET
* Razor
* MsTests
# Moq

## Getting Started
The easiest way to get started is clone this repository and run the application. The application has implemented an IHost extension and runs all querys requiered for the application even the database creation. the only thing you need take care is if you have localdb running in your personal laptop, 

If you prefer in another server just go to the appsettings.json and mofify the connection strings.
### Caution
      *InitialConnectionString* should have Initial Catalog "master" this CS is only to create the new database (ParkingDB) 
      
  If you want create the database by yourself, go to appsettings.json and  *CreateInitialDatabase* turn to *0*, with this configuration the app will create the objets requiered to work but no the dabatase
