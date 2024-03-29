# TemplateRESTfulAPI

A starting web template that manages registrations and logins for multiple users. It includes different mechanisms such as using SSO login for Google accounts, and OTP authentication using an Authenticator app device, such as Microsoft or Google Auth.
<br>

https://github.com/andresmgomez/UserManagementDemo/assets/88804430/c0f5696a-24a1-476c-82c8-1711ecae9ab5

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
  update-database -context IdentityContext
  update-database -context ApplicationDbContext
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

## Current Features

### 1. Users:

- **1. User registrations** - Multiple users can register an account at the same or different times.
- **2. User confirmations** - Each user can login and confirm their account by clicking a secure link.
- **3. User profile** - Each user can access, and change personal information on their profile.
- **4. User credentials** - Each user can securely reset their password if they forgot their passkey.
- **5. User SSO login** - Some users can login to the application using their Google account.
- **6. User enable 2FA** - Each user can enable two factor authentication by scanning a QR code.
- **7. User 2FA Access** - Some users can login using a OTP access code provided by Authenticator.
- **8. User recovery access** - Each user can access recovery tokens when unable to login using 2FA.

### 2. Admins:

- **1. Admin authentication** - Admin user can login to application by using Email generated access code.
- **2. User registrations** - Admin user can see User registration information, such as active status
- **3. User login attempts** - Admin can see User failed login attempts logs, such as Id and login time.
- **4. User privileges** - Admin can grant or deny access to User accounts that violated login policy.

## Current Endpoints

### 1. Users

| API                                                           | Description                            | HTTPS(GET) | HTTPS(POST) |
| ------------------------------------------------------------- | -------------------------------------- | ---------- | ----------- |
| [RegisterUser](https://localhost:44313/api/users/register)    | Public user can sign-up for an account | No         | Yes         |
| [LoginUser](https://localhost:44313/api/users/login)          | Account user can make a login request  | No         | Yes         |
| [LogoutUser](https://localhost:44313/users/logout)            | Public user can end current session    | Yes        | No          |
| [ResetPassword](https://localhost:44313/users/reset-password) | Public user can change their password  | Yes        | Yes         |

### 2. Accounts

| API                                                                    | Description                                   | HTTPS(GET) | HTTPS(POST) |
| ---------------------------------------------------------------------- | --------------------------------------------- | ---------- | ----------- |
| [ConfirmAccount](https://localhost:44313/api/accounts/confirm-account) | Account user can confirm their account        | Yes        | Yes         |
| [VerifyAccount](https://localhost:44313/api/accounts/verify-account)   | Account user can use contact number to verify | No         | Yes         |

### 3. Profiles

| API                                                                            | Description                                    | HTTPS(GET) | HTTPS(POST) |
| ------------------------------------------------------------------------------ | ---------------------------------------------- | ---------- | ----------- |
| [ProfileAccounts](https://localhost:44313/api/accounts/profiles)               | Admin user can see a list of account profiles  | Yes        | No          |
| [ProfileAccount](https://localhost:44313/api/accounts/profiles/single-profile) | Admin user can see account profile information | Yes        | No          |

### 4. Admins

| API                                                                         | Description                                       | HTTPS(GET) | HTTPS(POST) |
| --------------------------------------------------------------------------- | ------------------------------------------------- | ---------- | ----------- |
| [AuthorizeAdmins](https://localhost:44313/api/accounts/admins/send-auth)    | Admin user can request authorization access code  | No         | Yes         |
| [AuthenticateAdmins](https://localhost:44313/api/accounts/admins/send-auth) | Admin user can validate authorization access code | No         | Yes         |

<br>

## Acknowledgements

This project has been built using [AdminLTE](https://adminlte.io/docs/3.0) UI template, to launch a quick demo, and contains some custom styles.

## License

Distributed under the MIT License. See `LICENSE` for more information.
