FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["BeatFlowApi.csproj", "./"]
RUN dotnet restore "BeatFlowApi.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "BeatFlowApi.csproj" -c Release -o /app/build
RUN dotnet publish "BeatFlowApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BeatFlowApi.dll"]
