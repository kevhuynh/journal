# Journal

## Introduction
This is a command line journal application built with .NET Core and Entity Framework Core (EF Core). The application has the ability to:
* Create users
* Create journal entries associated with a user
* List all journal entries
* Search for journal entries based on a filter field
* Get journal entries according to date

## How to run
This application is set up to connect with a SQL database. The .csproj in this contains a package reference for MySQL packages but those can be swapped out with packages for other database providers supported by EF Core if there is a desire to use something other than MySQL. Be sure to update the database connection method if another database provider is used in JournalContext.cs. Also, update the connection string in JournalContext.cs.

After building the application, run the application from the command line. To see a list of commands and options, run the following command-line command in the directory where the build files are:

>*dotnet Journal.dll --help*

This will provide the list of commands for the application. Each command has a --help section of its own to help users understand what options they have.

## Notes
The username functionality is a new addition to the application. The application was originally built without support for multiple users. Not all features support it yet (for example, there is currently no filter option for users).