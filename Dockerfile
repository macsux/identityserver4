FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 1146
EXPOSE 44300

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["IdentityServer4.csproj", ""]
RUN dotnet restore
COPY . .
WORKDIR "/src/"
RUN dotnet build "IdentityServer4.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "IdentityServer4.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "IdentityServerDemo.dll"]
