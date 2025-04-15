FROM mcr.microsoft.com/dotnet/aspnet:2.1

# Set environment variables for non-interactive apt installation
ENV DEBIAN_FRONTEND=noninteractive

VOLUME /root/.citychain

WORKDIR /usr/local/app/

# Update apt sources to use archive.debian.org instead
RUN echo "deb http://archive.debian.org/debian stretch main" > /etc/apt/sources.list \
    && echo "deb http://archive.debian.org/debian-security stretch/updates main" >> /etc/apt/sources.list \
    && apt-get -o Acquire::Check-Valid-Until=false update \
    && apt-get install -y --no-install-recommends curl \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

RUN curl -Ls https://github.com/CityChainFoundation/city-chain/releases/download/v1.0.38/City.Chain-1.0.38-linux-x64.tar.gz \
    | tar -xvz -C .

COPY city.conf.docker /root/.citychain/city/CityMain/city.conf

EXPOSE 4333 4334 4335 4336

ENTRYPOINT ["dotnet", "City.Chain.dll"]