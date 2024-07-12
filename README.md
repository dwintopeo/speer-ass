# Speer Software Engineer Assessment
## Stack
The application is developed using .net 8 framework and Microsoft SQL Server. 
The IDE is Microsoft Visual Studio 2022.

## Installation
Please follow the steps below to setup the code on your dev environment;

1. Checkout from repo and open with Visual Studio 2022
2. Setup the database;
     * Create a new database and tables by running the scripts located in {SolutionDirectory}\Deploy\DbScripts.sql
	 * Please change connection string in the appsettings.json accordingly. 

Please build the code for the IDE to download the dependencies for Nuget.

## Dependencies
Please install the lastest version of following dependencies from Nuget if not automatically downloaded when the application is built.
1.  Automapper - This is a third-party library. It is used for mapping database entity models to api response model
2.  Serilog.AspNetCore - For file logging
3.  Swashbuckle.AspNetCore
4.  System.IdentityModel.Tokens.Jwt
5.  Microsoft.AspNetCore.Authentication.JwtBearer
6.  Microsoft.EntityFrameworkCore.SqlServer
7.  Microsoft.EntityFrameworkCore

## Swagger Link
https://speer-assessment.azurewebsites.net/swagger/index.html