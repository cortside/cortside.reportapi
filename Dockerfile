FROM mcr.microsoft.com/dotnet/runtime:6.0-alpine AS runtime
ENV configpath /app/appsettings.json

RUN apk update && apk add jq bash 

COPY startup.sh /
RUN dos2unix /startup.sh
RUN chmod a+x /startup.sh

WORKDIR /app
COPY publish/linux-musl-x64/ /app
RUN rm -f /app/appsettings.local.json
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Deployment

EXPOSE 5000/tcp
CMD ["/startup.sh"]
