Steps to deploy solution into Docker container.
1. Installed Docker to local environment.
2. You have to have installed .NET SDK 8
3. Run from the solution root folder using PowerShell: docker-compose up --build 
4. Open 'http://localhost:7145/swagger/index.html' where you have running Swagger from API.
5. Open 'http://localhost:4200/' where you have running Angular application.