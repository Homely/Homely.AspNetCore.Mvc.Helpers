

# Homely - ASP.NET Core MVC Helpers

This library contains a collection of helpers, models and extension methods that help reduce the time and ceremony to improve an ASP.NET-Core MVC WebApi/RESTful API.

[![Build status](https://ci.appveyor.com/api/projects/status/d4d0iyps9h74kt4s/branch/master?svg=true)](https://ci.appveyor.com/project/Homely/homely-aspnetcore-mvc-helpers/branch/master) [![codecov](https://codecov.io/gh/Homely/Homely.AspNetCore.Mvc.Helpers/branch/master/graph/badge.svg)](https://codecov.io/gh/Homely/Homely.AspNetCore.Mvc.Helpers)

---

## Samples / highlights

- [Uses ProblemDetails for consistent error models](#Sample1) [Shoutout to [@khellang Middleware library](https://github.com/khellang/Middleware)]
- [ModelStates that fail validation will use ProblemDetails are the error model](#Sample2)
- [Graceful handling of interupted/cut/cancelled Requests, mid flight.](#Sample3)
- [Simple HomeController [HTTP-GET /] which can show a banner + assembly/build info.](#Sample4)
- [Common JsonSerializerSettings.](#Sample5)
- [Json output default to use the common JsonSerializerSettings.](#Sample6)

### <a name="Sample1">Uses ProblemDetails for consistent error models.</a>

This is based off:
- ProblemDetails inclusion into ASP.NET Core 2.1.
- @KHellang's Middleware which simplifies the ceremony to leverage ProblemDetails. ASP.NET Core 2.2 _should_ bake this ceremony in, which should then mean we can remove this middleware and use the official one.

Step 1: Add the service.

```
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvcCore( ... )
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

    services.AddProblemDetails(options => options.IncludeExceptionDetails = _ => 
                                              _hostingEnvironment.IsDevelopment());
}
```

Step 2: Configure the HTTP Pipeline to use these ProblemDetails.

```
public void Configure(IApplicationBuilder app)
{
    app.UseProblemDetails()
       .UseMvc();
}
```

### <a name="Sample1">ModelStates that fail validation will use ProblemDetails are the error model</a>
If a ModelState fails validation during the start of handling the request, the framework responds with an `HTTP Status - 400 BAD REQUEST` but uses a simple key/value "error" model.

Instead, this uses `ProblemDetails` as the error model.

```
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvcCore( ... )
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

    services.ConfigureInvalidModelStateProblemDetails();
}
```

### <a name="Sample3">Graceful handling of interupted/cut/cancelled Requests, mid flight.</a>

If the request is cancelled (either from the user, response is taking too long or some technical hicup between client & server) then stop processing the request. Of course, your own code needs to have smarts to react/handle to `CancellationToken`'s.

```
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvcCore(options =>
                        {
                            options.WithGlobalCancelledRequestHandler();
                        })
```

### <a name="Sample4">Simple HomeController [HTTP-GET /] which can show a banner + assembly/build info.</a>

Great for API's, this will create the default "root/home" route => `HTTP GET /` with:
- Optional banner - some text (like ASCII ART)
- Build information about the an assembly.

```
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvcCore( ... )
            .AddAHomeController(services, typeof(Startup), SomeASCIIArt);
}
```
E.g. output


```
      ___           ___           ___           ___           ___           ___                    ___           ___                 
     /\__\         /\  \         /\  \         /\__\         /\  \         /\  \                  /\  \         /\  \          ___   
    /:/  /        /::\  \       /::\  \       /::|  |       /::\  \        \:\  \                /::\  \       /::\  \        /\  \  
   /:/__/        /:/\:\  \     /:/\:\  \     /:|:|  |      /:/\:\  \        \:\  \              /:/\:\  \     /:/\:\  \       \:\  \ 
  /::\  \ ___   /:/  \:\  \   /::\~\:\  \   /:/|:|  |__   /::\~\:\  \       /::\  \            /::\~\:\  \   /::\~\:\  \      /::\__\
 /:/\:\  /\__\ /:/__/ \:\__\ /:/\:\ \:\__\ /:/ |:| /\__\ /:/\:\ \:\__\     /:/\:\__\          /:/\:\ \:\__\ /:/\:\ \:\__\  __/:/\/__/
 \/__\:\/:/  / \:\  \ /:/  / \/_|::\/:/  / \/__|:|/:/  / \:\~\:\ \/__/    /:/  \/__/          \/__\:\/:/  / \/__\:\/:/  / /\/:/  /   
      \::/  /   \:\  /:/  /     |:|::/  /      |:/:/  /   \:\ \:\__\     /:/  /                    \::/  /       \::/  /  \::/__/    
      /:/  /     \:\/:/  /      |:|\/__/       |::/  /     \:\ \/__/     \/__/                     /:/  /         \/__/    \:\__\    
     /:/  /       \::/  /       |:|  |         /:/  /       \:\__\                                /:/  /                    \/__/    
     \/__/         \/__/         \|__|         \/__/         \/__/                                \/__/                              

                                                                                                        A P I    G A T E W A Y  -  W E B

Name: ApiGateway.Web
Version: 3.1.0.0
Date: 10-October-2018 05:53:36

```

### <a name="Sample5">Common JsonSerializerSettings.</a>

Some common JSON settings. This keeps things consistent across projects.
- CamelCase property names.
- Indented formatting.
- Ignore null properties which have values.
- DateTimes are ISO formatted.
- Enums are rendered as `string`'s ... not their backing number-value. 

### <a name="Sample6">Json output default to use the common JsonSerializerSettings.</a>

All responses are JSON and formatted using the common JSON settings (above).

```
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvcCore( ... )
            .AddACommonJsonFormatter();
}
```

---

## Contributing

Discussions and pull requests are encouraged :) Please ask all general questions in this repo or pick a specialized repo for specific, targetted issues. We also have a [contributing](https://github.com/Homely/Homely/blob/master/CONTRIBUTING.md) document which goes into detail about how to do this.

## Code of Conduct
Yep, we also have a [code of conduct](https://github.com/Homely/Homely/blob/master/CODE_OF_CONDUCT.md) which applies to all repositories in the (GitHub) Homely organisation.

## Feedback
Yep, refer to the [contributing page](https://github.com/Homely/Homely/blob/master/CONTRIBUTING.md) about how best to give feedback - either good or needs-improvement :)

---
