FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
#指定时区
ENV TZ Asia/Shanghai

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/TrustCms.Web.Hosting/TrustCms.Web.Hosting.csproj", "src/TrustCms.Web.Hosting/"]
COPY ["src/TrustCms.Application/TrustCms.Application.csproj", "src/TrustCms.Application/"]
COPY ["src/TrustCms.Domain/TrustCms.Domain.csproj", "src/TrustCms.Domain/"]
COPY ["src/TrustCms.Domain.Shared/TrustCms.Domain.Shared.csproj", "src/TrustCms.Domain.Shared/"]
COPY ["src/TrustCms.Application.Contracts/TrustCms.Application.Contracts.csproj", "src/TrustCms.Application.Contracts/"]
COPY ["src/TrustCms.HttpApi/TrustCms.HttpApi.csproj", "src/TrustCms.HttpApi/"]
COPY ["src/Trust.Abp.WeChat/Trust.Abp.WeChat.csproj", "src/Trust.Abp.WeChat/"]
COPY ["src/TrustCms.EntityFrameworkCore.DbMigrations/TrustCms.EntityFrameworkCore.DbMigrations.csproj", "src/TrustCms.EntityFrameworkCore.DbMigrations/"]
COPY ["src/TrustCms.EntityFrameworkCore/TrustCms.EntityFrameworkCore.csproj", "src/TrustCms.EntityFrameworkCore/"]
RUN dotnet restore "src/TrustCms.Web.Hosting/TrustCms.Web.Hosting.csproj"
COPY . .
WORKDIR "/src/src/TrustCms.Web.Hosting"
RUN dotnet build "TrustCms.Web.Hosting.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TrustCms.Web.Hosting.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TrustCms.Web.Hosting.dll"]