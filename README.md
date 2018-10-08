# YesSpa

YesSpa allows adding single page applications (SPA) to ASP.NET Core web projects. Currently supported SPAs are:
- [angular/cli](https://cli.angular.io/), version 6 and up
- [create-react-app](https://github.com/facebook/create-react-app) (tested with [typescript version](https://github.com/wmonk/create-react-app-typescript))

## Benefits using YesSpa

Unlike standard SPA templates coming with ASP.NET Core, YesSpa designed so that the SPAs are located in individual class libraries and embedded as resources in result dll files. A web application can serve multiple SPAs.

During  development you use process and tools 'native' to single page applications, i.e. serve the SPA in development host with angular/cli or create-react-app. When building SPA on a CI server YesSpa launches build tasks for SPAs and embedds result files in dll resources. You do not need to copy package files manually.

## Limitations and conventions

1. Only one SPA per module is allowed. The SPA should be created right inside of the module. It means that SPA's package.json should be located in the same folder as csproj file of the module (check samples for more details).
2. YesSpa assumes that you keep default build folders. Angular/cli puts result files in **dist** folder and create-react-app puts result files in **build** folder.
3. There is no debugger support in Visual Studio but you can use other tools like Chrome debugger and Visual Studio Code.
4. You should use CORS in development evironment for API access (that depends on configuration but typically if API hosted by the same app as SPA you should configure the cross-origin access).

## Running samples

### Command line
```bash
cd .\samples\aspnetcore\AspNetCore.Host.Web\
.\run-dev-embedded.cmd
```

### Visual Studio
1. Open the solution in Visual Studio
2. Set **samples/aspnetcore/AspNetCore.Host.Web** as startup project
3. Run the project

## Using with ASP.Net Core application

In order to use YesSpa in your application you have to create/configure the SPA and configure host project (usually an MVC project in your solution).

### Creating client application
1. Add another .NET standard library to your solution
2. Create client appliation using angular/cli or create-react-app in that folder. **Important**: do not use a subfolder like ClientApp or something like that beause YesSpa's convention is to use the root library folder for building and serving SPA.
3. Reference **YesSpa.Client.Targets** NuGet package in the SPA module.

### Configuring host
1. Add reference to your SPA module in the host app
2. Add reference to **YesSpa.AspNetCore** NuGet package
3. Add the following to **Startup.ConfigureServices()**

```csharp
  services.AddYesSpa();
```
4. Add the following to **Startup.Configure()**
```csharp
  app.UseSpa(builder =>
  {
    builder.AddSpa(Assembly.GetAssembly(typeof(ClientAppModuleReact)), "/", "/.Modules/ClientApp.React/build");
  });
```
This should be the last middleware in ASP.NET Core middleware chain.
The call configures middleware serving SPA and tells YesSpa how to get access to the embedded SPA files:
* **"/"** - means that SPA will be served at root URL
* **"/.Modules/ClientApp.React/build"** - path in resources to embedded package (in this case ClientApp.React is name of assembly containing SPA).

## Using with ASP.NET Boilerplate framework

### Creating client application
1. Add a new ABP module to your solution.
2. Create client appliation using angular/cli or create-react-app in that folder. **Important**: do not use a subfolder like ClientApp or something like that beause YesSpa's convention is to use the root library folder for building and serving SPA.
3. Reference **YesSpa.AbpModule.Targets** NuGet package in the SPA module.
4. Reference **YesSpa.Abp** NuGet package in the SPA module.
5. Add the following in **AbpModule.PreInitialize()**:
```csharp
  public override void PreInitialize()
  {
    string rootPath = "/react/";
    var assembly = Assembly.GetExecutingAssembly();
    var resourceNamespace = "YesSpa.Samples.Abp.ClientApp.React.build";
    Configuration.ConfigureSpa(rootPath, resourceNamespace, assembly);
  }
```
* **"/react/"** - is a root URL for serving SPA
* **"YesSpa.Samples.Abp.ClientApp.React.build"** - path in resources to embedded package (check [ABP docs on embedded resources](https://www.aspnetboilerplate.com/Pages/Documents/Embedded-Resource-Files))

### Configuring host
1. Add reference to your SPA module in the host app
2. Add reference to **YesSpa.Abp** NuGet package
3. Add the following to **Startup.ConfigureServices()**

```csharp
  services.AddYesSpa();
```
4. Add the following to **Startup.Configure()**
```csharp
  app.UseAbpSpa();
```
This should be the last middleware in ASP.NET Core middleware chain.

## Technical details

### SPA Stub pages

If you run your host application in development mode YesSpa displays the internal
stub page when you request an SPA path. This is because by default YesSpa does not
build the SPA during development build (to speed up build and execution). In some cases
you may want to see the actual application when running the host application.
To do that perform the following steps:

1. Make sure you build your SPA manually, for example:
```bash
npx ng build
```
The files in dist folder will be embedded into the SPA class library.

2. Modify **UseStubPage** flag in Startup.Configure():
```csharp
      app.UseSpa(builder =>
      {
        builder.Options.UseStubPage = false;
        builder.AddSpa(...);
        ...
      });
```
