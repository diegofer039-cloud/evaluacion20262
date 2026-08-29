# ===== Build stage =====
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

COPY ["TecnoGas.Hogar.csproj", "./"]
RUN dotnet restore "TecnoGas.Hogar.csproj"

COPY . .
RUN dotnet publish "TecnoGas.Hogar.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ===== Runtime stage =====
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS final
WORKDIR /app

# Limitar el consumo de memoria del runtime (.NET) para el tier Free de Render (512 MB).
# gcServer=0 usa el GC de estación de trabajo, que consume menos RAM que el server GC.
# GCHeapHardLimit fija un tope de heap (~384 MB) para evitar que el OOM-killer mate el
# proceso con "Exited with status 139".
ENV DOTNET_gcServer=0
ENV DOTNET_GCHeapHardLimit=0x18000000
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "TecnoGas.Hogar.dll"]
