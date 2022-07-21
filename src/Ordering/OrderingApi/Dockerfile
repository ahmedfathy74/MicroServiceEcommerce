#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Ordering/OrderingApi/OrderingApi.csproj", "src/Ordering/OrderingApi/"]
COPY ["src/Ordering/Ordering.Application/Ordering.Application.csproj", "src/Ordering/Ordering.Application/"]
COPY ["src/Ordering/Ordering.Core/Ordering.Core.csproj", "src/Ordering/Ordering.Core/"]
COPY ["src/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj", "src/Ordering/Ordering.Infrastructure/"]
COPY ["src/Common/EventBusRabbitMQ/EventBusRabbitMQ.csproj", "src/Common/EventBusRabbitMQ/"]
RUN dotnet restore "src/Ordering/OrderingApi/OrderingApi.csproj"
COPY . .
WORKDIR "/src/src/Ordering/OrderingApi"
RUN dotnet build "OrderingApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderingApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderingApi.dll"]
