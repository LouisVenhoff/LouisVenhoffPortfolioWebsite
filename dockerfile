FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /src

COPY ["portfolio_backend/portfolio_backend.csproj", "portfolio_backend/"]
RUN dotnet restore "portfolio_backend/portfolio_backend.csproj"

COPY ["portfolio_backend", "portfolio_backend/"]
WORKDIR /src/portfolio_backend
RUN dotnet build "portfolio_backend.csproj" -c Release -o /app/build 

FROM build as publish
RUN dotnet publish "portfolio_backend.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS run
ENV ASPNETCORE_HTTP_PORTS=80
EXPOSE 80
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "portfolio_backend.dll"]

