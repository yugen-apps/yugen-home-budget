## Create User Assigned Managed Identity

id-yugen-home-budget

## Add User Assigned Managed Identity to the Web App

webapp-yugen-home-budget > Security > User Assigned

## Add User Assigned Managed Identity (or Group) to SQL Server Entra admin

sql-server-yugen > Settings > Microsoft Entra Admin > group-sql-admins 

## Add Login to SQL Server

CREATE LOGIN [group-sql-admins] FROM EXTERNAL PROVIDER

## Add User to Database

CREATE USER [group-sql-admins] FOR LOGIN [group-sql-admins]
ALTER ROLE db_datareader ADD MEMBER [group-sql-admins];
ALTER ROLE db_datawriter ADD MEMBER [group-sql-admins];
ALTER ROLE db_ddladmin ADD MEMBER [group-sql-admins];
GO

## GH actions

### AZ Login
Prepare a user-assigned managed identity for Login with OIDC
Create a user-assigned managed identity and assign a role to it
Configure a federated identity credential on a user-assigned managed identity

AZURE_CLIENT_ID: the service principal or user-assigned managed identity client ID
AZURE_SUBSCRIPTION_ID: the subscription ID
AZURE_TENANT_ID: the tenant ID

AZURE_CLIENT_ID: "ddc4c0ff-bab8-4f28-adda-7afa0a086920" (id-yugen-home-budget)
AZURE_SUBSCRIPTION_ID: "ae0647c4-c323-493b-8233-99012b801938"
AZURE_TENANT_ID: "5098fa58-735a-4e2f-b5a4-ea9995b7b00a"

add role (e.g.) contributor to id-yugen-home-budget for all the resources it needs

user_id: the service principal or user-assigned managed identity client ID

server_name: "sql-server-yugen.database.windows.net,1433"
database: "sql-db-yugen-homebudget-dev"
user_id: "ddc4c0ff-bab8-4f28-adda-7afa0a086920"

Server=tcp:sql-server-yugen.database.windows.net,1433;
Database=sql-db-yugen-homebudget;Encrypt=True;
User ID=ddc4c0ff-bab8-4f28-adda-7afa0a086920;
Connection Timeout=10;Authentication=Active Directory Managed Identity;

Server=tcp:sql-server-yugen.database.windows.net,1433;Database=sql-db-yugen-homebudget-dev;Encrypt=True;User ID=ddc4c0ff-bab8-4f28-adda-7afa0a086920;Connection Timeout=10;Authentication=Active Directory Managed Identity;

# Migrations

```
dotnet ef migrations add V3 `
--startup-project ".\Yugen.HomeBudget.Server\Yugen.HomeBudget.Server.csproj" `
--project ".\Yugen.HomeBudget.Data\Yugen.HomeBudget.Data.csproj"`
--verbose
```

```
dotnet ef database update `
--startup-project ".\Yugen.HomeBudget.Server\Yugen.HomeBudget.Server.csproj" `
--project ".\Yugen.HomeBudget.Data\Yugen.HomeBudget.Data.csproj"`
--configuration "debug" `
--verbose
```

```
dotnet build `
".\Yugen.HomeBudget.Server\Yugen.HomeBudget.Server.csproj" `
--configuration Release `
--verbosity detailed
```

```
dotnet ef migrations bundle `
--startup-project ".\Yugen.HomeBudget.Server\Yugen.HomeBudget.Server.csproj" `
--project ".\Yugen.HomeBudget.Data\Yugen.HomeBudget.Data.csproj"`
--configuration Release `
--self-contained `
--verbose
```












# Old

BaseUrl
https://webapp-yugen-home-budget-dev.azurewebsites.net/

fc-yugen-home-budget-dev
Yugen-Apps
yugen-home-budget
DEV


# Yugen.HomeBudget

https://github.com/PlainAdmin/plain-free-bootstrap-admin-template

https://blazestack.blazorforest.com/

AZURE_SQL_CONNECTIONSTRING

Server=yugen-homebudget-sql-server.database.windows.net,1433;Database=yugen-homebudget-sql-db;User ID=pandasharp;Password={pwd};

Server=yugen-homebudget-sql-server.database.windows.net,1433;Database=yugen-homebudget-sql-db;Encrypt=True;Authentication=Active Directory Managed Identity;

Server=tcp:yugen-homebudget-sql-server.database.windows.net,1433;Database=yugen-homebudget-sql-db;Encrypt=True;Authentication=Active Directory Managed Identity;



Server
yugen-homebudget-sql-server.database.windows.net,1433

Database
yugen-homebudget-sql-db


{

}



https://learn.microsoft.com/en-us/azure/app-service/tutorial-dotnetcore-sqldb-app?tabs=copilot&pivots=azure-portal
https://learn.microsoft.com/en-us/azure/app-service/tutorial-connect-msi-sql-database?tabs=windowsclient%2Cefcore%2Cdotnet
https://learn.microsoft.com/en-us/azure/app-service/tutorial-dotnetcore-sqldb-app?tabs=copilot&pivots=azure-portal
https://learn.microsoft.com/en-us/azure/app-service/tutorial-connect-msi-sql-database?tabs=windowsclient%2Cefcore%2Cdotnetcore
https://learn.microsoft.com/en-us/sql/connect/ado-net/sql/azure-active-directory-authentication?view=sql-server-ver16#using-active-directory-managed-identity-authentication

https://techcommunity.microsoft.com/t5/azure-database-support-blog/using-managed-service-identity-msi-to-authenticate-on-azure-sql/ba-p/1288248
https://techcommunity.microsoft.com/t5/apps-on-azure-blog/connect-app-service-with-azure-sql-database-with-managed/ba-p/3288300
https://www.domstamand.com/using-managed-identities-with-sql-azure-database-using-asp-net-core/

# Azure SQL Server

## Grant database access to Microsoft Entra Managed Identity System-assigned

### For Migration via Pipelines

- Add Login to SQL Server

CREATE LOGIN [azdevops-appint-azsub-pandasharp-apps] FROM EXTERNAL PROVIDER

- Add User to Database

CREATE USER [azdevops-appint-azsub-pandasharp-apps] FOR LOGIN [azdevops-appint-azsub-pandasharp-apps]
ALTER ROLE db_datareader ADD MEMBER [azdevops-appint-azsub-pandasharp-apps];
ALTER ROLE db_datawriter ADD MEMBER [azdevops-appint-azsub-pandasharp-apps];
ALTER ROLE db_ddladmin ADD MEMBER [azdevops-appint-azsub-pandasharp-apps];
GO

### For Web App

- Add Login to SQL Server

CREATE LOGIN [yugen-homebudget-app] FROM EXTERNAL PROVIDER

- Add User to Database

CREATE USER [yugen-homebudget-app] FOR LOGIN [yugen-homebudget-app]
ALTER ROLE db_datareader ADD MEMBER [yugen-homebudget-app];
ALTER ROLE db_datawriter ADD MEMBER [yugen-homebudget-app];
ALTER ROLE db_ddladmin ADD MEMBER [yugen-homebudget-app];
GO