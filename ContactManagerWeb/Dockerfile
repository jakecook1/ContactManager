FROM mcr.microsoft.com/dotnet/core/aspnet:3.0

WORKDIR /app

COPY . .

CMD ASPNETCORE_URLS=http://*:$PORT dotnet ContactManagerWeb.dll