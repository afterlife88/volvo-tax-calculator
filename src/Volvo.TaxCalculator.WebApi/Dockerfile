FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Volvo.TaxCalculator.WebApi/Volvo.Tax.WebApi.csproj", "Volvo.Tax.WebApi/"]
RUN dotnet restore "Volvo.TaxCalculator.WebApi/Volvo.Tax.WebApi.csproj"
COPY . .
WORKDIR "/src/Volvo.TaxCalculator.WebApi"
RUN dotnet build "Volvo.Tax.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Volvo.TaxCalculator.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Volvo.TaxCalculator.WebApi.dll"]
