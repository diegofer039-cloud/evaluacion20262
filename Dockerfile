FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["TecnoGas.Hogar.csproj", "./"]
RUN dotnet restore "TecnoGas.Hogar.csproj"

COPY . .
RUN dotnet publish "TecnoGas.Hogar.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "TecnoGas.Hogar.dll"]
