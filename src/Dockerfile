FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

COPY ["Telefe.ServiceHooks.Api/Telefe.ServiceHooks.Api.csproj", "Telefe.ServiceHooks.Api/"]
RUN dotnet restore "Telefe.ServiceHooks.Api/Telefe.ServiceHooks.Api.csproj"

COPY . .
WORKDIR "/src/Telefe.ServiceHooks.Api"
RUN dotnet build "Telefe.ServiceHooks.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Telefe.ServiceHooks.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Telefe.ServiceHooks.Api.dll"] 