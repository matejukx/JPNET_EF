FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["JPNET_EF/JPNET_EF.csproj", "JPNET_EF/"]
RUN dotnet restore "JPNET_EF/JPNET_EF.csproj"
COPY . .
WORKDIR "/src/JPNET_EF"
RUN dotnet build "JPNET_EF.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JPNET_EF.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JPNET_EF.dll"]
