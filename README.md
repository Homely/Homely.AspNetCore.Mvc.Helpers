
<div>
    <p align="center">
    <img src="https://imgur.com/9E8hN79.png" alt="Homely - ASP.NET Core MVC Helpers" />
    </p>
</div>

# Homely - ASP.NET Core MVC Helpers

This library contains a collection of helpers, models and extension methods that help reduce the time and ceremony to improve an ASP.NET-Core MVC WebApi/RESTful API.

[![Build status](https://ci.appveyor.com/api/projects/status/d4d0iyps9h74kt4s/branch/master?svg=true)](https://ci.appveyor.com/project/Homely/homely-aspnetcore-mvc-helpers/branch/master) [![codecov](https://codecov.io/gh/Homely/Homely.AspNetCore.Mvc.Helpers/branch/master/graph/badge.svg)](https://codecov.io/gh/Homely/Homely.AspNetCore.Mvc.Helpers)

---

## Samples / highlights

- [ProblemDetails for all 4xx/5xx HTTP Errors](#Sample1)
- [Graceful handling of interupted/cut/cancelled Requests, mid flight.](#Sample2)
- [Simple HomeController [HTTP-GET /] which can show a banner + assembly/build info.](#Sample3)
- [Common JsonSerializerSettings.](#Sample4)
- [Json output default to use the common JsonSerializerSettings.](#Sample5)
- [Custom Swagger wired up](#Sample6)


### <a name="Sample1">ProblemDetails for all 4xx/5xx HTTP Errors</a>
When any 4xx/5xx error occurs, then this will be expressed as a" [Problem Detail](https://tools.ietf.org/html/rfc7807)". This includes the following example scenarios:
- Model binding fails.
- Route doesn't exist.
- Credentials required but failed/missing.
- Manual 4XX responses.
- Manual 5XX responses.

```
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvcCore( ... )
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

    services.AddProblemDetails(options => options.IncludeExceptionDetails = _ => _hostingEnvironment.IsDevelopment());
}
```

### <a name="Sample2">Graceful handling of interupted/cut/cancelled Requests, mid flight.</a>

If the request is cancelled (either from the user, response is taking too long or some technical hicup between client & server) then stop processing the request. Of course, your own code needs to have smarts to react/handle to `CancellationToken`'s.

```
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvcCore(options =>
                        {
                            options.WithGlobalCancelledRequestHandler();
                        })
```

### <a name="Sample3">Simple HomeController [HTTP-GET /] which can show a banner + assembly/build info.</a>

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

                                                                                                      S E R V I C E  ->  A C C O U N T S

Name: ApiGateway.Web
Version: 3.1.0.0
Date: 10-October-2018 05:53:36

```

### <a name="Sample4">Common JsonSerializerSettings.</a>

Some common JSON settings. This keeps things consistent across projects.
- CamelCase property names.
- Indented formatting.
- Ignore null properties which have values.
- DateTimes are ISO formatted.
- Enums are rendered as `string`'s ... not their backing number-value. 

### <a name="Sample5">Json output default to use the common JsonSerializerSettings.</a>

All responses are JSON and formatted using the common JSON settings (above).

```
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvcCore( ... )
            .AddACommonJsonFormatter();
}
```

### <a name="sample6">Custom Swagger wired up</a>

Swagger (using the [Swashbuckle library](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)) has been wired up and allows for you to define the `title` and `version` of this API and also a custom `route prefix` (which is good for a gateway API with multiple swagger endpoints because each microservice has their own swagger doc).

```
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvcCore( ... )
            .AddCustomSwagger("Test API", "v2");
}

public void Configure(IApplicationBuilder app)
{
    app.UseProblemDetails()
       .UseCustomSwagger("accounts/swagger", "Test API", "v2")
       .UseMvc();
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
