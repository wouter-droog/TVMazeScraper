#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/azure-functions/dotnet:3.0 AS base
WORKDIR /home/site/wwwroot
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Rtl.TVMazeService.Functions/Rtl.TVMazeService.Functions.csproj", "Rtl.TVMazeService.Functions/"]
COPY ["Rtl.TVMazeService.Infrastructure.Sql/Rtl.TVMazeService.Infrastructure.Sql.csproj", "Rtl.TVMazeService.Infrastructure.Sql/"]
COPY ["Rtl.TVMazeService.Domain/Rtl.TVMazeService.Domain.csproj", "Rtl.TVMazeService.Domain/"]
RUN dotnet restore "Rtl.TVMazeService.Functions/Rtl.TVMazeService.Functions.csproj"
COPY . .
WORKDIR "/src/Rtl.TVMazeService.Functions"
RUN dotnet build "Rtl.TVMazeService.Functions.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Rtl.TVMazeService.Functions.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true
ENTRYPOINT ["dotnet", "Rtl.TVMazeService.Functions.dll"]