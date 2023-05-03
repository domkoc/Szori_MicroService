FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ./MovieStream_FIBRPN.csproj .
RUN dotnet restore MovieStream_FIBRPN.csproj
COPY . .
RUN dotnet build MovieStream_FIBRPN.csproj -c Release -o /app/build
FROM build AS publish
RUN dotnet publish MovieStream_FIBRPN.csproj -c Release -o /app/publish
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MovieStream_FIBRPN.dll"]