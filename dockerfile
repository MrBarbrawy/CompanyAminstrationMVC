# Use the official .NET image as a build environment
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CompanyAdminstrationMVC/CompanyAdminstrationMVC.csproj", "CompanyAdminstrationMVC/"]
RUN dotnet restore "CompanyAdminstrationMVC/CompanyAdminstrationMVC.csproj"
COPY . .
WORKDIR "/src/CompanyAdminstrationMVC"
RUN dotnet build "CompanyAdminstrationMVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CompanyAdminstrationMVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CompanyAdminstrationMVC.dll"]
