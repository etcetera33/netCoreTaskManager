FROM mcr.microsoft.com/dotnet/core/runtime:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src

COPY ["NotificationService/NotificationService.csproj", "NotificationService/"]
COPY ["Contracts/Contracts.csproj", "Contracts/"]
COPY ["Models/Models.csproj", "Models/"]
COPY ["Core/Core.csproj", "Core/"]

RUN dotnet restore "NotificationService/NotificationService.csproj"

COPY . ./
WORKDIR "/src/NotificationService"
RUN dotnet build "NotificationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NotificationService.csproj" -c Release -o /app/publish

FROM build AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificationService.dll"]