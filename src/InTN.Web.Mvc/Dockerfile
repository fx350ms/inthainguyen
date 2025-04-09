FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

RUN dotnet tool install -g Microsoft.Web.LibraryManager.Cli
RUN ln -s ~/.dotnet/tools/libman /usr/local/bin/libman

WORKDIR /src
COPY ["src/InTN.Web.Mvc/InTN.Web.Mvc.csproj", "src/InTN.Web.Mvc/"]
COPY ["src/InTN.Web.Core/InTN.Web.Core.csproj", "src/InTN.Web.Core/"]
COPY ["src/InTN.Application/InTN.Application.csproj", "src/InTN.Application/"]
COPY ["src/InTN.Core/InTN.Core.csproj", "src/InTN.Core/"]
COPY ["src/InTN.EntityFrameworkCore/InTN.EntityFrameworkCore.csproj", "src/InTN.EntityFrameworkCore/"]
WORKDIR "/src/src/InTN.Web.Mvc"
RUN dotnet restore 

WORKDIR /src
COPY ["src/InTN.Web.Mvc", "src/InTN.Web.Mvc"]
COPY ["src/InTN.Web.Core", "src/InTN.Web.Core"]
COPY ["src/InTN.Application", "src/InTN.Application"]
COPY ["src/InTN.Core", "src/InTN.Core"]
COPY ["src/InTN.EntityFrameworkCore", "src/InTN.EntityFrameworkCore"]
WORKDIR "/src/src/InTN.Web.Mvc"
RUN libman restore
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "InTN.Web.Mvc.dll"]
