# Yugen.HomeBudget

https://github.com/PlainAdmin/plain-free-bootstrap-admin-template

AZURE_SQL_CONNECTIONSTRING

Server=yugen-homebudget-sql-server.database.windows.net,1433;Database=yugen-homebudget-sql-db;User ID=pandasharp;Password={pwd};

Server=yugen-homebudget-sql-server.database.windows.net,1433;Database=yugen-homebudget-sql-db;Encrypt=True;Authentication=Active Directory Managed Identity;

Server=tcp:yugen-homebudget-sql-server.database.windows.net,1433;Database=yugen-homebudget-sql-db;Encrypt=True;Authentication=Active Directory Managed Identity;


Server
yugen-homebudget-sql-server.database.windows.net,1433

Database
yugen-homebudget-sql-db


{
    "clientId":  "dd6d9c2e-2873-4747-8cc2-bd0de50ef3b6",
    "clientSecret":  "",
    "subscriptionId":  "fb367263-c9a5-4f73-b7d3-731c457e4b07",
    "tenantId":  "27da2c29-87f6-4b70-837f-d14be1a6f265"
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