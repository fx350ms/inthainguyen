FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY ["src/InTN.Web.Host/InTN.Web.Host.csproj", "src/InTN.Web.Host/"]
COPY ["src/InTN.Web.Core/InTN.Web.Core.csproj", "src/InTN.Web.Core/"]
COPY ["src/InTN.Application/InTN.Application.csproj", "src/InTN.Application/"]
COPY ["src/InTN.Core/InTN.Core.csproj", "src/InTN.Core/"]
COPY ["src/InTN.EntityFrameworkCore/InTN.EntityFrameworkCore.csproj", "src/InTN.EntityFrameworkCore/"]
WORKDIR "/src/src/InTN.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/InTN.Web.Host", "src/InTN.Web.Host"]
COPY ["src/InTN.Web.Core", "src/InTN.Web.Core"]
COPY ["src/InTN.Application", "src/InTN.Application"]
COPY ["src/InTN.Core", "src/InTN.Core"]
COPY ["src/InTN.EntityFrameworkCore", "src/InTN.EntityFrameworkCore"]
WORKDIR "/src/src/InTN.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "InTN.Web.Host.dll"]
