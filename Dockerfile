FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder

ARG VERSION_INFORMATIONAL
ARG VERSION_ASSEMBLY
ARG NUGET_SOURCE_PUBLIC="https://api.nuget.org/v3/index.json"

WORKDIR /src
COPY ["src/", "./"]

RUN dotnet build \
	--source "$NUGET_SOURCE_PUBLIC" \
	-p:AssemblyVersion="$VERSION_ASSEMBLY" \
	-p:Version="$VERSION_INFORMATIONAL" \
	"config.map.test.csproj" \
	-c Release \
	-o /app

RUN dotnet publish \
	--no-restore \
	-p:AssemblyVersion="$VERSION_ASSEMBLY" \
	-p:Version="$VERSION_INFORMATIONAL" \
	"config.map.test.csproj" \
	-c Release \
	-o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=builder /app .
ENTRYPOINT ["dotnet", "config.map.test.dll"]