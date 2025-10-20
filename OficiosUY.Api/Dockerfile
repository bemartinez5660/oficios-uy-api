# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["OficiosUY.Api", "OficiosUY.Api.csproj", "./"]
RUN dotnet restore "OficiosUY.Api.csproj"
COPY . .
RUN dotnet publish "OficiosUY.Api.csproj" -c Release

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /OficiosUY.Api/bin/Release/net9.0/publish .
EXPOSE 8080 
ENTRYPOINT ["dotnet", "OficiosUY.Api.dll"]