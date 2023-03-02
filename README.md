# GiaoHangNhanh project 
## Technologies
- ASP.NET Core MVC 5.0
- Entity Framework core 5.0
## Install Tools
- .NET Core SDK 3.1
- Git client
- Visual Studio 2019
- SQL Server 2019
## Youtube tutorial
- Video list: https://www.youtube.com/playlist?list=PLRhlTlpDUWsyN_FiVQrDWMtHix_E2A_UD
- TEDU Course: https://tedu.com.vn/khoa-hoc/lam-du-an-voi-aspnet-core-31-34.html
## How to configure and run
- Clone code
- Open solution GiaoHangNhanh.sln in Visual Studio 2019
- Set startup project is GiaoHangNhanh.DAL
- Change connection string in Appsetting.json in GiaoHangNhanh.DAL project
- Open Tools --> Nuget Package Manager -->  Package Manager Console in Visual Studio
- Run "ADD-migration db1"  and Enter.
- Run "Update-database" and Enter.
- After migrate database successful, set Startup Project is GiaoHangNhanh.WebApp
- Change database connection in appsettings.Development.json in GiaoHangNhanh.WebApp project.
- You need to change 3 projects to self-host profile.
- Set multiple run project: Right click to Solution and choose Properties and set Multiple Project, choose Start for 3 Projects: BackendApi, WebApp and AdminApp.
- Choose profile to run or press F5
## Admin template: metronic_v8, Https://preview.keenthemes.com/metronic8
## I18N (Internalization)
- References: https://medium.com/swlh/step-by-step-tutorial-to-build-multi-cultural-asp-net-core-web-app-3fac9a960c43
- Source code: https://github.com/LazZiya/ExpressLocalizationSampleCore3
## Install packages for GiaoHangNhanh.Services
- Microsoft.AspNetCore.Identity

## Install packages for GiaoHangNhanh.BackendApi

## Install packages for GiaoHangNhanh.AdminApp

## Install packages for GiaoHangNhanh.DAL
- microsoft.entityframeworkcore.sqlserver 5.0.0
- microsoft.entityframeworkcore.design 5.0.0
- microsoft.entityframeworkcore.tools 5.0.0
- Microsoft.AspNetCore.Identity.EntityFramework 5.0.0
- Microsoft.Extensions.Configuration.Json 5.0.0
## Install packages for GiaoHangNhanh.Utilities

## Install packages for GiaoHangNhanh.ApiIntegration
