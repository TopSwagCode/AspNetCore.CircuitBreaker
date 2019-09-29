FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["ExternalService/ExternalService.csproj", "ExternalService/"]
RUN dotnet restore "ExternalService/ExternalService.csproj"
COPY . .
WORKDIR "/src/ExternalService"
RUN dotnet build "ExternalService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExternalService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExternalService.dll"]