using Developist.Core.Api.MvcFilters;
using Developist.Samples.Infrastructure;
using Developist.Samples.Infrastructure.Persistence;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

void ConfigureServices(IServiceCollection services)
{
    services.AddOptions<ApiExceptionFilterOptions>().Configure(options =>
    {
        var defaultOnSerializingAction = options.OnSerializingProblemDetails;
        options.OnSerializingProblemDetails = (problem, exception, context) =>
        {
            defaultOnSerializingAction(problem, exception, context);
            if (exception.InnerException is { } innerException)
            {
                problem.Extensions["innerExceptionMessage"] = innerException.Message;
            }
        };
    });
    services.AddScoped<ApiExceptionFilterAttribute>();

    services.AddControllers(options =>
    {
        options.Filters.Add<ApiExceptionFilterAttribute>();
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressMapClientErrors = true;
    });

    services.AddRouting(options =>
    {
        options.LowercaseUrls = true;
    });

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Sample Web API",
            Version = "v1"
        });
        options.DescribeAllParametersInCamelCase();
    });

    services.AddScoped<IEventAggregator, EventAggregator>();
    services.AddUnitOfWork().WrapUnitOfWork((uow, provider) => ActivatorUtilities.CreateInstance<CustomUnitOfWork>(provider, uow));

    services.AddCqrs(cfg => cfg
        .AddDispatchers()
        .AddHandlersFromAssembly(Assembly.Load("Developist.Samples"))
    );
}

void ConfigureApplication(IApplicationBuilder app, IHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("v1/swagger.json", "Sample Web API v1");
        });
    }

    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}

#region Bootstrap the application

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services);

var app = builder.Build();
ConfigureApplication(app, app.Environment);

app.Run();

#endregion