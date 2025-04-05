# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /src

# Copiamos el proyecto y restauramos dependencias
COPY ./MiProyectoBackend.csproj ./
RUN dotnet restore

# Copiamos el código y publicamos
COPY . ./
RUN dotnet publish -c Release -o /app

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview
WORKDIR /app
COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "BIBLE-API.dll"]