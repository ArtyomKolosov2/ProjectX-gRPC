FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ProjectX-gRPC/ProjectX-gRPC.csproj", "ProjectX-gRPC/"]
RUN dotnet restore "ProjectX-gRPC/ProjectX-gRPC.csproj"
COPY . .
WORKDIR "/src/ProjectX-gRPC"
RUN dotnet build "ProjectX-gRPC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProjectX-gRPC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectX-gRPC.dll"]
