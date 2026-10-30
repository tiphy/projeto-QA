FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/AlocaSenac.Web/AlocaSenac.Web.csproj src/AlocaSenac.Web/
COPY src/AlocaSenac.Application/AlocaSenac.Application.csproj src/AlocaSenac.Application/
COPY src/AlocaSenac.Domain/AlocaSenac.Domain.csproj src/AlocaSenac.Domain/
COPY src/AlocaSenac.Infrastructure/AlocaSenac.Infrastructure.csproj src/AlocaSenac.Infrastructure/
RUN dotnet restore src/AlocaSenac.Web/AlocaSenac.Web.csproj

COPY . .
RUN dotnet publish src/AlocaSenac.Web/AlocaSenac.Web.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
COPY --from=build /app/publish .
USER $APP_UID
ENTRYPOINT ["dotnet", "AlocaSenac.Web.dll"]