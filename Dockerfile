# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos el csproj y restauramos
COPY ./MiProyectoBackend.csproj ./
RUN dotnet restore

# Copiamos el resto del código
COPY . ./
RUN dotnet publish -c Release -o /app --no-restore

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "MiProyectoBackend.dll"]