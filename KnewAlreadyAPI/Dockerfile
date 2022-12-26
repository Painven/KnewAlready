#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["KnewAlreadyAPI/KnewAlreadyAPI.csproj", "KnewAlreadyAPI/"]
COPY ["KnewAlreadyCore/KnewAlreadyCore.csproj", "KnewAlreadyCore/"]
RUN dotnet restore "KnewAlreadyAPI/KnewAlreadyAPI.csproj"
COPY . .
WORKDIR "/src/KnewAlreadyAPI"
RUN dotnet build "KnewAlreadyAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KnewAlreadyAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KnewAlreadyAPI.dll"]