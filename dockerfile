FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /src

COPY ["portfolio_backend/portfolio_backend.csproj", "portfolio_backend/"]
RUN dotnet restore "portfolio_backend/portfolio_backend.csproj"

COPY ["portfolio_backend", "portfolio_backend/"]
WORKDIR /src/portfolio_backend
RUN dotnet build "portfolio_backend.csproj" -c Release -o /app/build 

#Build frontend
FROM node:20 AS reactbuild

WORKDIR /app

COPY ["louis_venhoff_portfolio/package*.json", "./"]
RUN npm install

ENV VITE_API_URL=http://venhoff.org
ENV VITE_GITHUB_PAT=

COPY ["louis_venhoff_portfolio", "./"]
RUN npm run build

FROM build as publish
COPY --from=reactbuild /app/dist /src/portfolio_backend/wwwroot
RUN dotnet publish "portfolio_backend.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS run

ENV ASPNETCORE_HTTP_PORTS=80
ENV GITHUB_ACCESS_TOKEN=

EXPOSE 80
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "portfolio_backend.dll"]

