

# Homely - ASP.NET Core MVC Helpers

This library contains a collection of helpers, models and extension methods that help reduce the time and ceremony to improve an ASP.NET-Core MVC WebApi/RESTful API.

[![Build status](https://ci.appveyor.com/api/projects/status/d4d0iyps9h74kt4s/branch/master?svg=true)](https://ci.appveyor.com/project/Homely/homely-aspnetcore-mvc-helpers/branch/master) [![codecov](https://codecov.io/gh/Homely/Homely.AspNetCore.Mvc.Helpers/branch/master/graph/badge.svg)](https://codecov.io/gh/Homely/Homely.AspNetCore.Mvc.Helpers)

---

## Sample / highlights

- [Automatic Model Validation via FluentValidation.](#Sample1)
- [Consistent Api Error schema.](#Sample2)
- [All Error Responses are in JSON format](#Sample3) (this is an API after all...)

### <a name="Sample1">Automatic Model Validation via FluentValidation.</a>

```
public void ConfigureServices(IServiceCollection services)
{
    // Reflect through the current assembly looking for FluentValidation Validators 
    services.AddCustomFluentValidation(this.GetType());
}

 -- or --

public void ConfigureServices(IServiceCollection services)
{
    // Reflect through the all assemblies looking for any FluentValidation Validators. 
    var types = new [] { typeof(Startup), typeof(AnotherClassFromAnotherAssembly) };
    services.AddCustomFluentValidation(types);
}
```

### <a name="Sample2">Consistent Api Error schema and JSON responses.</a>

Schema is as follows:
- Collection of `errors`
- Optional: Stacktrace of the error.

```
{
    "errors": [
        {
            "key": <some key>,
            "message": <description of the error>
        },
        {
            ... <api error in here> ...
        }
    ],
    "stackTrace": "<blah">
}
```

### <a name="Sample1">All Error Responses are in JSON format</a> (this is an API after all...)

1. Common/standard usage: some nice consistent and readable Json settings.

```
public void ConfigureServices(IServiceCollection services)
{
    // Common/standard usage: some nice consistent and readable Json settings.
    services.AddACommonJsonFormatter();
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    // Standard json exception page for production.
    app.UseJsonExceptionPage();
}
```

2. Standard settings but adding a stacktrace to the output if there's an error.

```
public void ConfigureServices(IServiceCollection services)
{
    // Common/standard usage: some nice consistent and readable Json settings.
    services.AddACommonJsonFormatter();
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    // Json exception page which includes a stack trace for development.
    app.UseJsonExceptionPage(includeStackTrace: env.IsDevelopment());
}
```

3. Settings a CORS policy. Why? If there's an AJAX request which errors, we need to make sure the client which executes the AJAX request can correctly accept the error response. CORS are required when doing AJAX requests.

```

public void ConfigureServices(IServiceCollection services)
{
    // Common/standard usage: some nice consistent and readable Json settings.
    services.AddACommonJsonFormatter();
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    // Json exception page which includes a stack trace for development.
    app.UseJsonExceptionPage(corsPolicyName);
}
```

---

## Contributing

Discussions and pull requests are encouraged :) Please ask all general questions in this repo or pick a specialized repo for specific, targetted issues. We also have a [contributing](https://github.com/OpenRealEstate/Homely/blob/master/CONTRIBUTING.md) document which goes into detail about how to do this.

## Code of Conduct
Yep, we also have a [code of conduct](https://github.com/Homely/Homely/blob/master/CODE_OF_CONDUCT.md) which applies to all repositories in the (GitHub) Homely organisation.

## Feedback
Yep, refer to the [contributing page](https://github.com/Homely/Homely/blob/master/CONTRIBUTING.md) about how best to give feedback - either good or needs-improvement :)

---
