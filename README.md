# Login To SQL Server with User Assigned Managed Identity (or Service Principal)

## Create User Assigned Managed Identity
id-yugen-home-budget

## Add User Assigned Managed Identity to the Web App
webapp-yugen-home-budget > Security > User Assigned

## Add User(s) Assigned Managed Identity (or Group) to SQL Server Entra admin
sql-server-yugen > Settings > Microsoft Entra Admin > group-sql-admins 

## Add Login to SQL Server
CREATE LOGIN [group-sql-admins] FROM EXTERNAL PROVIDER

## Add User to Database
CREATE USER [group-sql-admins] FOR LOGIN [group-sql-admins]
ALTER ROLE db_datareader ADD MEMBER [group-sql-admins];
ALTER ROLE db_datawriter ADD MEMBER [group-sql-admins];
ALTER ROLE db_ddladmin ADD MEMBER [group-sql-admins];
GO

## Note:
Istead of a group, for instance you can use directly the 

pipeline service prinicpal
CREATE LOGIN [azdevops-azsub-yugen] FROM EXTERNAL PROVIDER

app service prinicapl
CREATE LOGIN [app-yugenp] FROM EXTERNAL PROVIDER

User Assigned Managed Identity
CREATE LOGIN [id-yugen] FROM EXTERNAL PROVIDER

## Connection String
azure_user_id: the service principal or user-assigned managed identity client ID
server_name: e.g. "xxx.database.windows.net,1433"
database: the database name

Server=tcp:{server_name};Database={database};Encrypt=True;User ID={azure_user_id};Connection Timeout=10;Authentication=Active Directory Managed Identity;

# GH actions AZ Login With OpenID Connect (OIDC)
https://github.com/yugen-apps/yugen-home-budget

## Prepare a user-assigned managed identity for Login with OIDC

### Create a user-assigned managed identity and assign a role to it
add role (e.g.) contributor to the user-assigned managed identity for all the resources it needs

### Configure a federated identity credential on a user-assigned managed identity

## GH actions AZ Login parameters
AZURE_CLIENT_ID: the service principal or user-assigned managed identity client ID
AZURE_SUBSCRIPTION_ID: the subscription ID
AZURE_TENANT_ID: the tenant ID

# CLI cheat sheet

## Create Migrations

```
dotnet ef migrations add V1 `
--startup-project ".\Yugen.Home.Budget.Server\Yugen.Home.Budget.Server.csproj" `
--project ".\Yugen.Home.Budget.Data\Yugen.Home.Budget.Data.csproj"`
--verbose
```

## Apply Migrations

```
dotnet ef database update `
--startup-project ".\Yugen.Home.Budget.Server\Yugen.Home.Budget.Server.csproj" `
--project ".\Yugen.Home.Budget.Data\Yugen.Home.Budget.Data.csproj"`
--configuration "debug" `
--verbose
```

## Build App

```
dotnet build `
".\Yugen.Home.Budget.Server\Yugen.Home.Budget.Server.csproj" `
--configuration Release `
--verbosity detailed
```

## Create Migrations Bundle

```
dotnet ef migrations bundle `
--startup-project ".\Yugen.Home.Budget.Server\Yugen.Home.Budget.Server.csproj" `
--project ".\Yugen.Home.Budget.Data\Yugen.Home.Budget.Data.csproj"`
--configuration Release `
--self-contained `
--verbose
```

# Resoruces

https://github.com/PlainAdmin/plain-free-bootstrap-admin-template
https://blazestack.blazorforest.com/

https://learn.microsoft.com/en-us/azure/app-service/tutorial-dotnetcore-sqldb-app?tabs=copilot&pivots=azure-portal
https://learn.microsoft.com/en-us/azure/app-service/tutorial-connect-msi-sql-database?tabs=windowsclient%2Cefcore%2Cdotnet
https://learn.microsoft.com/en-us/azure/app-service/tutorial-dotnetcore-sqldb-app?tabs=copilot&pivots=azure-portal
https://learn.microsoft.com/en-us/azure/app-service/tutorial-connect-msi-sql-database?tabs=windowsclient%2Cefcore%2Cdotnetcore
https://learn.microsoft.com/en-us/sql/connect/ado-net/sql/azure-active-directory-authentication?view=sql-server-ver16#using-active-directory-managed-identity-authentication

https://techcommunity.microsoft.com/t5/azure-database-support-blog/using-managed-service-identity-msi-to-authenticate-on-azure-sql/ba-p/1288248
https://techcommunity.microsoft.com/t5/apps-on-azure-blog/connect-app-service-with-azure-sql-database-with-managed/ba-p/3288300
https://www.domstamand.com/using-managed-identities-with-sql-azure-database-using-asp-net-core/
