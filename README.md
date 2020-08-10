
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

- [Simple HomeController [HTTP-GET /] which can show a banner + assembly/build info.](#Sample3)
- [Json response-output to default with some common JsonSerializerOptions settings.](#Sample5)
- [Custom Swagger wired up](#Sample6)

### <a name="Sample3">Simple HomeController [HTTP-GET /] which can show a banner + assembly/build info.</a>

Great for API's, this will create the default "root/home" route => `HTTP GET /` with:
- Optional banner - some text (like ASCII ART)
- Build information about the an assembly.

```
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers()
            .AddAHomeController(services, SomeASCIIArt);
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
Build Date : Sunday, 7 June 2020 2:41:53 PM
Application Started: Monday, 8 June 2020 12:02:37 PM
Server name: PUREKROME-PC

```

### <a name="Sample5">Json output default to use the common JsonSerializerSettings.</a>

All responses are JSON and formatted using the common JSON settings:
:white_check_mark: CamelCase property names.
:white_check_mark: Indented formatting.
:white_check_mark: Ignore null properties which have values.
:white_check_mark: Enums are rendered as `string`'s ... not their backing number-value. 

```
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers()
            .AddDefaultJsonOptions();
}
```

Sample Model/Domain object:
```
new FakeVehicle
{
    Id = 1,
    Name = "Name1",
    RegistrationNumber = "RegistrationNumber1",
    Colour = ColourType.Grey,
    VIN = null
});
```

Result JSON text:
```
{
  "id": 1,
  "name": "Name1",
  "registrationNumber": "RegistrationNumber1",
  "colour": "Grey"
}
```

### <a name="sample6">Custom Swagger wired up</a>

Swagger (using the [Swashbuckle library](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)) has been wired up and allows for you to define the `title` and `version` of this API and also a custom `route prefix` (which is good for a gateway API with multiple swagger endpoints because each microservice has their own swagger doc).

```
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers()
            .AddCustomSwagger("Test API", "v2");
}

public void Configure(IApplicationBuilder app)
{
    app.UseCustomSwagger("accounts/swagger", "Test API", "v2")
       .UseRouting()
       .UseEndpoints(endpoints => endpoints.MapControllers());
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
