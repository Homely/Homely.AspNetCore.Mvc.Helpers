

# Homely - ASP.NET Core MVC Helpers

This library contains a collection of helpers, models and extension methods that help reduce the time and ceremony to improve an ASP.NET-Core MVC WebApi/RESTful API.

[![Build status](https://ci.appveyor.com/api/projects/status/d4d0iyps9h74kt4s/branch/master?svg=true)](https://ci.appveyor.com/project/Homely/homely-aspnetcore-mvc-helpers/branch/master) [![codecov](https://codecov.io/gh/Homely/Homely.AspNetCore.Mvc.Helpers/branch/master/graph/badge.svg)](https://codecov.io/gh/Homely/Homely.AspNetCore.Mvc.Helpers)

---

## Sample / highlights

- [Consistent Api Error schema.](#Sample1)
- [500 Internal Server Error (unhandled errors) are Json results.](#Sample2) (this is an API after all...)
- [Automatic Model Validation via FluentValidation.](#Sample3)

### <a name="Sample1">Consistent Api Error schema and JSON responses.</a>

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

### <a name="Sample3">500 Internal Server Error (unhandled errors) are Json results.

When an unhandled error is encountered and would then be returned as a 500 Server Error, the data is returned as json instead of the default HTML content.

1. Simple setup.
 - No stacktrace.
 - No CORS.
 - No custom exception handler logic.

```
public void ConfigureServices(IServiceCollection services)
{
    // Common/standard usage: some nice consistent and readable Json settings.
    services.AddMvcCore()
            .AddACommonJsonFormatter(); // So the JSON is nicely formatted.
}

// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    app.UseStaticFiles()
        .UseStatusCodePages()
        .UseJsonExceptionPages()
        .UseMvc();
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
    app.UseJsonExceptionPages(includeStackTrace: env.IsDevelopment())
       .UseStaticFiles() // etc.
       .UseMvc();
}
```

3. Settings a CORS policy.

 Why? If there's an AJAX request which errors, we need to make sure the client which executes the AJAX request can correctly accept the error response. CORS are required when doing AJAX requests.

```
public void ConfigureServices(IServiceCollection services)
{
    // Common/standard usage: some nice consistent and readable Json settings.
    services.AddACommonJsonFormatter();
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
	var corsPolicyName = "Some Cors policy blah blah";
    app.UseJsonExceptionPages(corsPolicyName)
       .UseStaticFiles() // etc.
       .UseMvc();
}
```

4. Some custom exception handling, during the exception. 

This would be if you want to return some specific result for a specific status code. For example, if the status code was an HTTP409 you might have a custom json schema. While for everything else, you have some other result.

```
public void ConfigureServices(IServiceCollection services)
{
    // Common/standard usage: some nice consistent and readable Json settings.
    services.AddACommonJsonFormatter();
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    JsonExceptionPageResult HandleException(Exception result) 
    { 
        // E.g. Real life scenario:
        //      If status code == 401, then return some result
        //      else if status code == 409 then resturn another result
        //      else return some default result or return null (for default handling to kick in).

        // Contrite hardcoded example to return a hardcoded result.
        // Of course you wouldn't *really* do this, in production.
        return new JsonExceptionPageResult
        {
            StatusCode = System.Net.HttpStatusCode.UpgradeRequired,
            ApiErrors = new List<ApiError>
            {
                new ApiError
                {
                    Message = "I'm a little tea pot, short and stout."
                }
            }
        };
    };

    app.UseJsonExceptionPages(customExceptionFunction: HandleException)
       .UseStaticFiles() // etc.
       .UseMvc();
}
```

### <a name="Sample3">Automatic Model Validation via FluentValidation.</a>

```
public void ConfigureServices(IServiceCollection services)
{
    // Reflect through the current assembly looking for FluentValidation Validators 
    services.AddCustomFluentValidation(this.GetType());
}

 -- or --

public void ConfigureServices(IServiceCollection services)
{
    // Reflect through the *all* assemblies looking for any FluentValidation Validators. 
    var types = new [] { typeof(Startup), typeof(AnotherClassFromAnotherAssembly) };
    services.AddCustomFluentValidation(types);
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
