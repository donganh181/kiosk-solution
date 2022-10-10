#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["kiosk-solution/kiosk-solution.API.csproj", "kiosk-solution/"]
COPY ["kiosk-solution.Data/kiosk-solution.Data.csproj", "kiosk-solution.Data/"]
COPY ["kiosk-solution.Business/kiosk-solution.Business.csproj", "kiosk-solution.Business/"]
RUN dotnet restore "kiosk-solution/kiosk-solution.API.csproj"
COPY . .
WORKDIR "/src/kiosk-solution"
RUN dotnet clean "kiosk-solution.API.csproj"
RUN dotnet build "kiosk-solution.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "kiosk-solution.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "kiosk-solution.API.dll"]