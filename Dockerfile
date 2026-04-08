FROM mcr.microsoft.com/dotnet/sdk:9.0

WORKDIR /app

COPY . .

RUN dotnet build Bynder/Sample/Bynder.Sample.csproj

# Simple entrypoint to keep container running
ENTRYPOINT ["tail", "-f", "/dev/null"]