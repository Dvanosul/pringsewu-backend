FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5144

ENV ASPNETCORE_URLS=http://+:5144

RUN apt-get update && \
    apt-get install -y --no-install-recommends \
    libgdiplus \
    libc6-dev \
    && ln -s /usr/lib/x86_64-linux-gnu/libdl.so.2 /usr/lib/libdl.so \
    && apt-get clean && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["pringsewu.csproj", "./"]
RUN dotnet restore "pringsewu.csproj"

COPY . .
RUN dotnet build "pringsewu.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "pringsewu.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /src/Seed/ ./Seed/

ENTRYPOINT ["dotnet", "pringsewu.dll"]
