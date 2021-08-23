FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["GameStore.csproj", "."]
RUN dotnet restore "GameStore.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "GameStore.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameStore.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameStore.dll"]
