# TemplateRESTfulAPI

A Web based application that orchestrates authorization and authentication for online users, by calling separately a Web based API and User interface.
<br>

## System Requirements

- [Visual Studio 2019 or later](https://visualstudio.microsoft.com/downloads/)
- [Microsoft .NET SDK v5.0.400](https://dotnet.microsoft.com/en-us/download/dotnet/5.0)
- [SQL Server 2019 or later](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [SQL Server Management Studio](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16)

> Make sure you **select** <em>x64</em> version of the architecture

## Getting Started

<details>
  <summary>Open the Solution in Visual Studio</summary>

  <div align="left">
     <img src="https://github.com/andresmgomez/NETCoreAppTemplate/blob/main/screenshots/open-project.png" alt="Open the project in Visual Studio" width="500px" />
  </div>
</details>

<details>
  <summary>Configure local path for Database</summary>

Expand the <em>TemplateRESTful.API</em> and <em>TemplateRESTful.Web</em> folders

  <div align="left">
   <img src="https://github.com/andresmgomez/NETCoreAppTemplate/blob/main/screenshots/expand-projects.png" alt="Expand the Api or Web project" width="400px" />
  </div>

2. Open the <em>appsettings.json</em> file

 <div align="left">
     <img src="https://github.com/andresmgomez/NETCoreAppTemplate/blob/main/screenshots/db-project.png" alt="Configure the path for database" width="500px" />
</div>

3. Replace the following database settings

```SQL
Server=myServerAddress;Database=myDataBase;
```

with the correct settings for your Database Server

```SQL
  Data Source=SQL_SERVER\\SQL_DATABASE;Initial Catalog=DATABASE_NAME;
```

</details>

<details>
  <summary>Populate the Database with initial data</summary>

1. Click on **Tools** in the program menu bar
2. Then go to <em>NuGet Package Manager</em> and click on **Package Manager Console**

3. Run the following <em>command</em> to seed database

```cmd
  update-database -context IdentityContext -o OutputFolder
  update-database -context ApplicationDbContext -o OutputFolder
```

> Note: Make sure to select, **TemplateRESTful.Persistence** option to avoid errors.

</details>

<details>
  <summary>Select a Project to load the <strong>API or Web</strong></summary>

  <div align="left">
    <img src="https://github.com/andresmgomez/NETCoreAppTemplate/blob/main/screenshots/select-project.png" alt="Select and load the project" width="400px" />
  </div>

Right click on the <em>project solution</em>, and select <strong><em>Set as a Startup Project</em></strong>

</details>

<details>
  <summary>Run and build the <strong>API or Web</strong></summary>

Select the <em>TemplateRESTful.API</em> or <em>TemplateRESTful.Web</em>, then click on **IIS Express**

  <div align="left">
    <img src="https://github.com/andresmgomez/NETCoreAppTemplate/blob/main/screenshots/run-project.png" alt="Run and build the project" width="500px" />
  </div>

</details>
<br>

## Core Services

### Generate a passcode for your App

1. Enable [2-step verification](https://support.google.com/accounts/answer/10956730?hl=en) in your gmail settings

2. After clicking on <em>App Password Options</em>, set a password and click on **Generate button** to get the sign in passcode

3. Inside the **appsettings.json** file, replace email settings

```json
  "EmailConfiguration": {
    "From": "business.email@gmail.com",
    "SmtpServer": "smtp.gmail.com",
    "Port": 465,
    "Username": "business.email@gmail.com",
    "Password": "xxxx xxxx xxxx xxxx"
  },
```

> Google has disabled support for third-party apps using just credentials after May 30 2022. This is the way to authorize your app

### Set Google authentication in NET Core

1. Create a new **app** in Google Cloud Platform, and install the <strong>External Identity Provider</strong>NuGet package

2. Inside the **appsettings.json** file, replace auth settings

```json
"Authentication": {
    "Google": {
      "ClientId": "0000000000000-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx.apps.googleusercontent.com",
      "ClientSecret": "GXXXXX-XXXX_XXXXXXXXXXXXXXXXX_GXXXX"
    }
  },
```

Click for [step by step](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-5.0) for instructions, how to generate your <strong><em>ClientId and ClientSecret</em></strong>
<br>

## UI Screenshots (Auth)

|                                          Registration Page                                          |                                          Confirm Registration Page                                          |                                            Login Page                                            |
| :-------------------------------------------------------------------------------------------------: | :---------------------------------------------------------------------------------------------------------: | :----------------------------------------------------------------------------------------------: |
| <img src="https://github.com/andresmgomez/NETCoreAppTemplate/blob/main/screenshots/register.gif" /> | <img src="https://github.com/andresmgomez/NETCoreAppTemplate/blob/main/screenshots/register-confirm.gif" /> | <img src="https://github.com/andresmgomez/NETCoreAppTemplate/blob/main/screenshots/login.gif" /> |

|                                            Reset Password                                             |                                            Change Password                                             |                                            Lockout Page                                            |
| :---------------------------------------------------------------------------------------------------: | :----------------------------------------------------------------------------------------------------: | :------------------------------------------------------------------------------------------------: |
| <img src="https://github.com/andresmgomez/NETCoreAppTemplate/blob/main/screenshots/reset-pass.gif" /> | <img src="https://github.com/andresmgomez/NETCoreAppTemplate/blob/main/screenshots/change-pass.gif" /> | <img src="https://github.com/andresmgomez/NETCoreAppTemplate/blob/main/screenshots/lockout.gif" /> |

## Project Features

<details>
  <summary>User interactions with Web UI</summary>

- Guest Users

  - [x] User can login using social account

- Users

  - [x] User can sign up a new account
  - [x] User can confirm their account by email address
  - [x] User can change their password securely
  - [x] User can set their contact number info
  - [x] User can see quick notification errors

- Admins
  - [x] Admin can see most recent User registrations
  </details>

<details>
  <summary>User interactions with Web API</summary>

`https://localhost:44313/swagger/index.html`

  <summary>Register [POST]</summary>

`https://localhost:44313/api/accounts/register`

  <summary>Confirm Account [GET]</summary>

`https://localhost:44313/api/accounts/confirm-account`

  <summary>Confirm Account [POST]</summary>

`https://localhost:44313/api/accounts/confirm-account`

  <summary>Login [POST]</summary>

`https://localhost:44313/api/accounts/login`

  <summary>Reset Password [GET]</summary>

`https://localhost:44313/api/accounts/reset-password`

  <summary>Reset Password [POST]</summary>

`https://localhost:44313/api/accounts/reset-password`

</details>

## License

Distributed under the MIT License. See `LICENSE` for more information.
