FROM mcr.microsoft.com/dotnet/sdk:8.0

# Install Node.js for TeMML
RUN curl -fsSL https://deb.nodesource.com/setup_18.x | bash - && \
    apt-get install -y nodejs

# Install development tools
RUN apt-get update && apt-get install -y \
    git \
    curl \
    wget \
    vim \
    sqlite3 \
    && rm -rf /var/lib/apt/lists/*

# Set working directory
WORKDIR /workspace

# Install .NET tools
RUN dotnet tool install --global dotnet-ef
RUN dotnet tool install --global dotnet-format

ENV PATH="${PATH}:/root/.dotnet/tools"


# Pre-restore NuGet packages
#COPY MathMLParser/src/MathMLParser.Web/MathMLParser.Web.csproj /workspace/src/MathMLParser.Web/
#COPY MathMLParser/src/MathMLParser.Tests/MathMLParser.Tests.csproj /workspace/src/MathMLParser.Tests/
#COPY MathMLParser.sln /workspace/
#RUN dotnet restore

# Copy project files
#COPY . /workspace/