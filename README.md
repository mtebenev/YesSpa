# YesSpa

YesSpa allows adding single page applications (SPA) to ASP.NET Core web projects. Currently supported SPAs are:
- [angular/cli](https://cli.angular.io/), version 6 and up
- [create-react-app](https://github.com/facebook/create-react-app) (tested with [typescript version](https://github.com/wmonk/create-react-app-typescript))

## YesSpa vs ASP.NET Core JavaScript Services

The main difference is the approach for SPA tooling. JavaScript services provide sophisticated tooling over SPA. Internally they are setting up proxies to transfer data between the web server and a CLI tool (create react app or angular-cli). Effectively JavaScript services almost completely hide the fact that you are running another development server on the SPA level. Thanks to that you have client code debugging right in Visual Studio and smooth experience. Still the 'native' CLIs provide the same functionality and I believe that JavaScript services make things over-complicated. 

YesSpa is different: basically it assumes that you have the knowledge of SPA CLIs and using them for development. YesSpa comes into play only when you need to deploy the SPA along with the ASP.Net host application. It performs the SPA build and embedds the result files into .Net Core assembly. Then it serves HTTP requests for that files from ASP.Net Core pipeline.

## Benefits using YesSpa

Unlike standard SPA templates coming with ASP.NET Core, YesSpa designed so that the SPAs are located in individual class libraries and embedded as resources in result dll files. A web application can serve multiple SPAs.

During  development you use process and tools 'native' to single page applications, i.e. serve the SPA in development host with angular/cli or create-react-app. When building SPA on a CI server YesSpa launches build tasks for SPAs and embedds result files in dll resources. You do not need to copy package files manually.

## Limitations and conventions

1. Only one SPA per module is allowed. The SPA should be created right inside of the module. It means that SPA's package.json should be located in the same folder as csproj file of the module (check samples for more details).
2. YesSpa assumes that you keep default build folders. Angular/cli puts result files in **dist** folder and create-react-app puts result files in **build** folder.
3. There is no debugger support in Visual Studio but you can use other tools like Chrome debugger and Visual Studio Code.
4. You should use CORS in development evironment for API access (that depends on configuration but typically if API hosted by the same app as SPA you should configure the cross-origin access).
5. No support for server-side rendering (check JavaScript services if you need that).

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

The following needed in order to use YesSpa in your application:
1. Create/configure the SPA module
2. Configure host project (usually an MVC project in your solution)
3. Configure SPA build

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

### Configuring SPA build

When you reference **YesSpa.Client.Targets** in an SPA module it adjusts the build process so that MSBuild executes tasks for building the SPA and embedding result files into the SPA assembly. You enable the SPA build by passing **YesSpaEnabled=true** to MSBuild.

You can perform that in two ways: set the property explicitly in csproj files (like in the sample application) and pass the property in dotnet build command line.
Common SPA build settings:

**YesSpaEnabled=true**  
Should be set to set to true to enable whole YesSpa during the build.

**YesSpaSkipSpaBuild=true**  
Pass to skip SPA build. Use with caution because if there's no SPA build files they will not be embedded in assembly.
You have to build SPA manually when using this option (e.g. 'ng biuld').

**YesSpaSkipNpmInstall=true**  
Pass to skip npm install call during the build. Use with caution because if there's no npm packages installed SPA build will fail.
You have to update NPM packages manually when using this option (e.g. 'npm install').


There are also framework-specific settings that should be set in order to build the SPA.

### Angular SPA

**YesSpaBuildTools=angular**  
Should be set to enable angular toolchain.

**YesSpaAngularBaseHref=_base_href_**  
Example: YesSpaAngularBaseHref='/angular/'  
This is basically the value to be set as base href in index.html.
Check details at [angular cli wiki](https://github.com/angular/angular-cli/wiki/build#base-tag-handling-in-indexhtml)

**YesSpaAngularDeployUrl=_embedded_path_**  
Example: YesSpaAngularDeployUrl='/.Modules/AspNetCore.ClientApp.Angular/dist/aspnetcore-clientapp-angular/'
this is angular-cli deploy-url option.
The path convention is the following: '.Modules/<SPA_MODULE_NAMESPACE>/<EMBEDDED_PATH>'

**YesSpaAngularConfiguration=_configuration_name_**  
Example: YesSpaAngularConfiguration='dev_embedded'
this is angular-cli configuration option (FKA environment).
Check details at [angular cli wiki](https://github.com/angular/angular-cli/wiki/build)

### React SPA

**YesSpaBuildTools=react**  
Should be set to enable react toolchain.

For React application you have to configure PUBLIC_URL in .env file
For example:  
PUBLIC_URL=/.Modules/AspNetCore.ClientApp.React/build  
Check details at [create react app user guide](https://github.com/facebook/create-react-app/blob/master/packages/react-scripts/template/README.md#advanced-configuration)

**YesSpaEnvCmdEnvironment=_env_cmd_environment_**  
Sets environment name for react apps using .env-cmdrc

Check details at [env-cmd repository](https://github.com/toddbluhm/env-cmd)

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
