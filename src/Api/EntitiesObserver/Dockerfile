FROM mcr.microsoft.com/dotnet/core/runtime:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src

COPY "EntitiesObserver/EntitiesObserver.csproj" "EntitiesObserver/"
COPY ["Api/Api.csproj", "Api/"]
COPY ["Models/Models.csproj", "Models/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["Contracts/Contracts.csproj", "Contracts/"]
COPY ["Data/Data.csproj", "Data/"]

RUN dotnet restore "EntitiesObserver/EntitiesObserver.csproj"

COPY . ./
WORKDIR "/src/EntitiesObserver"
RUN dotnet build "EntitiesObserver.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EntitiesObserver.csproj" -c Release -o /app/publish

FROM build AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EntitiesObserver.dll"]