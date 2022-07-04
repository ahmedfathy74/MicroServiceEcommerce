#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Basket/BasketApii/BasketApii.csproj", "src/Basket/BasketApii/"]
RUN dotnet restore "src/Basket/BasketApii/BasketApii.csproj"
COPY . .
WORKDIR "/src/src/Basket/BasketApii"
RUN dotnet build "BasketApii.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BasketApii.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BasketApii.dll"]
